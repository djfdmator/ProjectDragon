// ==============================================================
// Cracked FSM_NormalEnemy
//
//  AUTHOR: Yang SeEun
// CREATED:
// UPDATED: 2020-01-04
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NormalEnemyState { Idle, Walk, Attack, Wait}
public class FSM_NormalEnemy : Enemy
{

    [Header("[NormalEnemy Attribute]")]
    public float readyTime;          //Idle->Walk time
    public float cooltime;          //Idle ->Attack time
    public int attackCount;
    protected float Current_readyTime = 0;
    protected float Current_cooltime = 0;
    protected bool isAttackActive;

    [Header("[Enemy State]")]
    [SerializeField] protected NormalEnemyState normalEnemyState;
    public NormalEnemyState NEState
    {
        get { return normalEnemyState; }
        set
        {
            normalEnemyState = value;
            SetState(normalEnemyState);

        }
    }

    public override IEnumerator Start_On()
    {
        StartCoroutine(base.Start_On());
        
        //1초후 추적
        yield return new WaitForSeconds(1.0f);
        isIdle = false;
        NEState = NormalEnemyState.Walk;

        //공격감지 체크
        StartCoroutine(AttackRangeCheck());
        yield return null;
    }

  




    protected virtual IEnumerator Idle()
    {
        isIdle = true;
        StartCoroutine(CalcCooltime());
        yield return null;
    }

    public virtual IEnumerator CalcCooltime()
    {
        while (NEState == NormalEnemyState.Idle&&!isDead)
        {
            if (isHit)
            {
                yield return null;
                continue;
            }

            //[조건] cooltime > readyTime
            if (Current_cooltime < cooltime)                    //cooltime 전
            {
                if (Current_readyTime < readyTime)                 //readyTime 전
                {
                    Current_readyTime += Time.deltaTime;
                    Current_cooltime = Current_readyTime;
                }
                else                                             //readyTime 후
                {
                    //공격범위에 플레이어가 없다면 추적
                    if (!inAtkDetectionRange)
                    {
                        isAttackActive = false;
                        isIdle = false;
                        NEState = NormalEnemyState.Walk;   //Idle->Walk
                        yield break;
                    }
                    else  //공격범위에 플레이어가 있다면 대기
                    {
                        Current_cooltime += Time.deltaTime;
                    }
                }
            }
            else                                                 //cooltime 후
            {
                if (NEState == NormalEnemyState.Idle)    
                {
                    isAttackActive = true;
                    isIdle = false;
                    NEState = NormalEnemyState.Attack;          //Idle->Attack
                    yield break;
                }
            }
            yield return null;
        }
    }

    protected virtual IEnumerator Walk()
    {
        //Walk Animation parameters
        objectAnimator.SetBool("Walk", true);

        float currentWalkTime = 0;
        float walkTime = Random.Range(2.0f, 6.0f);

        while (NEState == NormalEnemyState.Walk && !isDead)
        {
            if (isHit)
            {
                yield return null;
                continue;
            }
            //공격감지범위에 들어오면 Attack
            if (inAtkDetectionRange)
            {
                isWalk = false;
                NEState = NormalEnemyState.Attack;
                objectAnimator.SetBool("Walk", false);
                yield break;
            }
            if (rb2d.velocity != Vector2.zero)
            {
                PushStopCor = PushStop();
                StartCoroutine(PushStopCor);
            }
            else
            {
                //move
                if (!collisionPlayer)
                {
                    isWalk = true;
                    currentWalkTime += Time.deltaTime;
                    if (walkTime < currentWalkTime)
                    {
                        //Wait
                        isWalk = false;
                        NEState = NormalEnemyState.Wait;
                        objectAnimator.SetBool("Walk", false);
                        yield break;
                    }
                    //AStar
                    GetComponent<Tracking>().FindPathManager(rb2d, MoveSpeed);

                    //rb2d.velocity = direction * MoveSpeed * 10.0f * Time.deltaTime;
                    //transform.position = Vector3.MoveTowards(transform.position, other.transform.position, MoveSpeed * Time.deltaTime);
                }
                else { isWalk = false; }
            }
            yield return null;
        }
    }
   
    protected IEnumerator Wait()
    {
        //Walk Animation parameters
        objectAnimator.SetBool("Wait", true);

        float waitTime = Random.Range(1.0f, 2.0f);
        float current_waitTime = 0;

        isIdle = true;
        //rb2d.velocity = Vector2.zero;
        while (NEState == NormalEnemyState.Wait && !isDead)
        {
            if (isHit)
            {
                yield return null;
                continue;
            }
            //공격감지범위에 들어오면 Attack
            if (inAtkDetectionRange)
            {
                isIdle = false;
                NEState = NormalEnemyState.Attack;           //Wait -> Attack
                objectAnimator.SetBool("Wait", false);
                yield break;
            }
            current_waitTime += Time.deltaTime;
            if (waitTime < current_waitTime)
            {
                isIdle = false;
                NEState = NormalEnemyState.Walk;             //Wait -> Walk
                objectAnimator.SetBool("Wait", false);
                yield break;
            }
            rb2d.velocity = Vector2.zero;
            yield return null;
        }
    }

    protected virtual IEnumerator Attack()
    {
        AttackStart();

        isAttacking = true;
        yield return null;

        StartCoroutine(AttackEnd());
    }

    protected void AttackStart()
    {
        //Attack Animation parameters
        objectAnimator.SetBool("Attack", true);
        objectAnimator.SetBool("isAttackActive", isAttackActive);

        //Cooltime Initialize
        Current_readyTime = 0;
        Current_cooltime = 0;
    }

    //애니메이션 n번 돌리고 -> Idle로
    #region Attack 공격 횟수 관리
    protected virtual IEnumerator AttackEnd()
    {
        int count = 0;
        while (isAttacking&&!isDead)
        {
            if (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
            {
                yield return null;
                continue;
            }
            AnimatorClipInfo[] clipInfo = objectAnimator.GetCurrentAnimatorClipInfo(0);
            //Debug.Log(clipInfo[0].clip.name);

            float cliptime = clipInfo[0].clip.length;
            yield return new WaitForSeconds(cliptime / objectAnimator.GetCurrentAnimatorStateInfo(0).speed);

            count++;
            if (attackCount == count)
            {
                isAttacking = false;
                //Attack Animation parameters
                objectAnimator.SetBool("Attack", isAttacking);
                NEState = NormalEnemyState.Idle;
                break;
            }

            yield return null;
        }
    }
    #endregion


   


    

}

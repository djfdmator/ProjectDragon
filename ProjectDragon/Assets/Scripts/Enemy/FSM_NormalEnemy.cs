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

public enum NormalEnemyState { Idle, Walk, Attack, Wait, Dead}
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




    public override IEnumerator Start_On()
    {
        StartCoroutine(base.Start_On());

        //1초 대기
        yield return new WaitForSeconds(1.0f);

        //추적
        isIdle = false;
        normalEnemyState = NormalEnemyState.Walk;
        ChangeState<NormalEnemyState>(normalEnemyState);

        //공격감지 체크
        StartCoroutine(AttackRangeCheck());
        yield return null;
    }

    public override void Dead()
    {
        normalEnemyState = NormalEnemyState.Dead;
        base.Dead();
    }




    #region State Routine


    protected virtual IEnumerator IdleState()
    {
        //OnEnter
        isIdle = true;
        //시간 변수 초기화
        Current_readyTime = 0;
        Current_cooltime = 0;

        StartCoroutine(CalcCooltime());
        yield return null;
    }

    public IEnumerator CalcCooltime()
    {
        //Execute
        while (normalEnemyState == NormalEnemyState.Idle)
        {
            //타격시 넉백(이동 고정하지 않기)
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
                        normalEnemyState = NormalEnemyState.Walk;   //Idle->Walk
                        break;
                    }
                    else  //공격범위에 플레이어가 있다면 대기
                    {
                        Current_cooltime += Time.deltaTime;
                    }
                }
            }
            else                                                      //cooltime 후
            {
                isAttackActive = true;
                isIdle = false;
                normalEnemyState = NormalEnemyState.Attack;          //Idle->Attack
                break;
            }
            yield return null;
        }

        //OnExit
        ChangeState<NormalEnemyState>(normalEnemyState);
    }

    protected virtual IEnumerator WalkState()
    {
        //OnEnter
        objectAnimator.SetBool("Walk", true);
        float currentWalkTime = 0;
        float walkTime = Random.Range(2.0f, 6.0f);

        //Execute
        while (normalEnemyState == NormalEnemyState.Walk)
        {
            //타격시 넉백(추적하지 않기)
            if (isHit)
            {
                yield return null;
                continue;
            }
            //공격감지범위에 들어오면 공격
            if (inAtkDetectionRange)
            {
                normalEnemyState = NormalEnemyState.Attack;
                break;
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
                        normalEnemyState = NormalEnemyState.Wait;
                        break;
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

        //OnExit
        isWalk = false;
        objectAnimator.SetBool("Walk", false);
        ChangeState<NormalEnemyState>(normalEnemyState);
    }

    protected IEnumerator WaitState()
    {
        //OnEnter
        float waitTime = Random.Range(1.0f, 2.0f);
        float current_waitTime = 0;
        isIdle = true;
        objectAnimator.SetBool("Wait", true);
        rb2d.velocity = Vector2.zero;

        //Execute
        while (normalEnemyState == NormalEnemyState.Wait)
        {
            //타격시 넉백(이동 고정하지 않기)
            if (isHit)
            {
                yield return null;
                continue;
            }
            //공격감지범위에 들어오면 공격
            if (inAtkDetectionRange)
            {
                normalEnemyState = NormalEnemyState.Attack;           //Wait -> Attack
                break;
            }
            current_waitTime += Time.deltaTime;
            if (waitTime < current_waitTime)
            {
                normalEnemyState = NormalEnemyState.Walk;             //Wait -> Walk
                break;
            }
            rb2d.velocity = Vector2.zero;
            yield return null;
        }

        //OnExit
        isIdle = false;
        objectAnimator.SetBool("Wait", false);
        ChangeState<NormalEnemyState>(normalEnemyState);
    }


    protected IEnumerator AttackEndCor = null;

    protected virtual IEnumerator AttackState()
    {
        //OnEnter
        AttackStart();
        yield return null;

        AttackEndCor = AttackEnd();
        StartCoroutine(AttackEndCor);
    }

    protected void AttackStart()
    {
        isAttacking = true;
        objectAnimator.SetBool("Attack", isAttacking);
        objectAnimator.SetBool("isAttackActive", isAttackActive);
    }


    //애니메이션 n번 돌리고 -> Idle로
    protected virtual IEnumerator AttackEnd()
    {
        int count = 0;

        //Execute
        while (normalEnemyState == NormalEnemyState.Attack)
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
                normalEnemyState = NormalEnemyState.Idle;
                break;
            }
            yield return null;
        }

        //Exit
        AttackEndCor = null;
        isAttacking = false;
        objectAnimator.SetBool("Attack", isAttacking);
        ChangeState<NormalEnemyState>(normalEnemyState);
    }


    protected virtual IEnumerator DeadState()
    {
        yield return null;
    }

    #endregion



}

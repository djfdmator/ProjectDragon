using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_NormalEnemy_t : BaseFSM_Enemy
{
    protected float Current_readyTime;
    protected float Current_cooltime;

    [Header("[NormalEnemy Attribute]")]
    public float readyTime;                     //Idle->Walk time
    public float cooltime;                      //Idle ->Attack time
    public int attackCount;
    public bool isAttackActive;              //Idle-> (Walk,Attack)에서 선택하는 플래그

    [Header("[Enemy State]")]
    [SerializeField] protected NormalEnemyState currentState;

    //상태 머신 작동
    public override IEnumerator Start_On()
    {
        yield return StartCoroutine(base.Start_On());

        //추적 상태
        ChangeState(NormalEnemyState.Walk);
        yield return null;
    }

    public override void Dead()
    {
        base.Dead();
        ChangeState(NormalEnemyState.Dead);
    }




    #region FSM
    protected override void InitStateDatas()
    {
    }

    //상태 변경
    public void ChangeState(NormalEnemyState newState)
    {
        currentState = newState;
        stateMachine.ChangeState(stateMachine.states[newState]);

        HandleInput();
    }

    protected void HandleInput()
    {
        switch (currentState)
        {
            case NormalEnemyState.Idle:
                StartCoroutine(CalcCooltime());
                break;
            case NormalEnemyState.Walk:
                StartCoroutine(CheckWalkState());
                break;
            case NormalEnemyState.Attack:
                StartCoroutine(CheckAttackState());
                break;
            case NormalEnemyState.Wait:
                StartCoroutine(CheckWaitState());
                break;
        }

    }








    #region HandleInput Function

    protected virtual IEnumerator CheckAttackState()
    {
        yield return null;
    }

    public IEnumerator CalcCooltime()
    {
        //시간 변수 초기화
        Current_readyTime = 0;
        Current_cooltime = 0;

        while (currentState == NormalEnemyState.Idle)
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
                        ChangeState(NormalEnemyState.Walk);             //Idle->Walk
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
                ChangeState(NormalEnemyState.Attack);                 //Idle->Attack
                break;
            }
            yield return null;
        }
            yield return null;
    }

    protected IEnumerator CheckWalkState()
    {
        float currentWalkTime = 0f;
        float walkTime = Random.Range(2.0f, 6.0f);

        while (currentState == NormalEnemyState.Walk)
        {
            //공격감지범위에 들어오면 공격
            if (inAtkDetectionRange)
            {
                ChangeState(NormalEnemyState.Attack);
                break;
            }
            if (!collisionPlayer)
            {
                currentWalkTime += Time.deltaTime;
                if (walkTime < currentWalkTime)
                {
                    //Wait
                    ChangeState(NormalEnemyState.Wait);
                    break;
                }
            }
            yield return null;
        }
        yield return null;
    }


    protected IEnumerator CheckWaitState()
    {
        float current_waitTime = 0f;
        float waitTime = Random.Range(1.0f, 2.0f);

        while (currentState == NormalEnemyState.Wait)
        {
            //공격감지범위에 들어오면 공격
            if (inAtkDetectionRange)
            {
                ChangeState(NormalEnemyState.Attack);
                break;
            }
            current_waitTime += Time.deltaTime;
            if (waitTime < current_waitTime)
            {
                ChangeState(NormalEnemyState.Walk);
                break;
            }
            yield return null;
        }
            yield return null;
    }

    #endregion

    #endregion

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doldori_t : FSM_NormalEnemy_t
{
    private CircleCollider2D circleCol;
    private Vector3 attackDirection;

    protected override void Awake()
    {
        circleCol = GetComponents<CircleCollider2D>()[0];
        triggerCol = GetComponents<CircleCollider2D>()[1];
        col = circleCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall", "Cliff"); // 근거리는 Cliff 추가
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }




    protected override RaycastHit2D[] GetRaycastType()
    {
        //CapsuleCas
        return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction, AtkRange - originOffset, m_viewTargetMask);
    }


    #region FSM Override
    protected override void InitStateDatas()
    {
        stateMachine.states.Add(NormalEnemyState.Idle, new Enemy_IdleState());
        stateMachine.states.Add(NormalEnemyState.Walk, new Enemy_WalkState());
        stateMachine.states.Add(NormalEnemyState.Attack, new RollAndAttack());
        stateMachine.states.Add(NormalEnemyState.Wait, new Enemy_WaitState());
        stateMachine.states.Add(NormalEnemyState.Dead, new Enemy_DeadState());

        //초기 상태 설정
        stateMachine.Initial_Setting(gameObject.GetComponent<BaseFSM_Enemy>(), stateMachine.states[NormalEnemyState.Idle]);
    }
    #endregion






    public override void Attack_On()
    {
        if (!isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE, false, 0);
        }
    }


    //벽과 플레이어 부딪히면 그로기 상태
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        //연속으로 될경우 방지 (한번만 돌리게)
        if (currentState == NormalEnemyState.Attack)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall")
                || collision.gameObject.CompareTag("Cliff") || collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Enemy"))
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    Attack_On();
                }
                else if (collision.gameObject.GetComponent<MapObject>() != null)
                {
                    collision.gameObject.GetComponent<MapObject>().HpChanged(50);
                    if (collision.gameObject.GetComponent<Box>() != null)       //박스는 뚫고 가기
                    {
                        return;
                    }
                }
                ChangeState(NormalEnemyState.Idle);
            }
        }
    }



    #region [이전버전] 밀림 방지용 충돌처리
    //몬스터끼리의 충돌이 가능할때 사용한 함수들

    //부모함수 부르지않기 위해서 (지우면안됨)
    protected override void OnCollisionExit2D(Collision2D collision)
    {
    }
    //protected override void OnTriggerStay2D(Collider2D collision)
    //{
    //    base.OnTriggerStay2D(collision);

    //    if (isAttacking)
    //    {
    //        if (collision.gameObject.CompareTag("Enemy"))
    //        {
    //            //충돌할때 Attack이면 콜라이더끄기
    //            Physics2D.IgnoreCollision(collision, col);

    //        }
    //    }
    //}
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobulhwa_t : FSM_NormalEnemy_t
{
    private CircleCollider2D circleCol;

    //object
    private Projectile projectile;
    public RuntimeAnimatorController projectileAnimator;

    protected override void Awake()
    {
        circleCol = GetComponents<CircleCollider2D>()[0];
        triggerCol = GetComponents<CircleCollider2D>()[1];
        col = circleCol;
        childDustParticle = transform.Find("DustParticle").gameObject;
        projectile = new Projectile();
        base.Awake();
        //projectile = this.gameObject.AddComponent<Projectile>();

    }

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        DustParticleCheck();
    }







    protected override RaycastHit2D[] GetRaycastType()
    {
        //CircleCast
        return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction, AtkRange - originOffset, m_viewTargetMask);
    }

    #region FSM Override
    protected override void InitStateDatas()
    {
        stateMachine.states.Add(NormalEnemyState.Idle, new Enemy_IdleState());
        stateMachine.states.Add(NormalEnemyState.Walk,   new Enemy_WalkState());
        stateMachine.states.Add(NormalEnemyState.Attack, new StoneAttack());
        stateMachine.states.Add(NormalEnemyState.Wait, new Enemy_WaitState());
        stateMachine.states.Add(NormalEnemyState.Dead, new Enemy_DeadState());

        //초기 상태 설정
        stateMachine.Initial_Setting(gameObject.GetComponent<BaseFSM_Enemy>(), stateMachine.states[NormalEnemyState.Idle]);
    }

    protected override IEnumerator CheckAttackState()
    {
        while (currentState == NormalEnemyState.Attack)
        {
            if (attackCount == curAttackCount)
            {
                ChangeState(NormalEnemyState.Idle);
                break;
            }
            yield return null;
        }
        yield return null;
    }
    #endregion




    /// <summary>
    /// 탄환 생성 (애니메이션 프레임에 넣기)
    /// </summary>
    ///  In this case you choose event based on the clip weight
    public override void Attack_On()
    {
        Vector2 offset = new Vector2(0.0f, 0.0f);
        float radius = 0.06f;

        projectile.Create(projectileTargetList, offset, radius, Angle - 30, 3.0f, ATTACKDAMAGE, projectileAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle, 3.0f, ATTACKDAMAGE, projectileAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle + 30, 3.0f, ATTACKDAMAGE, projectileAnimator, false, transform.position);
    }
}

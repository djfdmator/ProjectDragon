using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime_t : FSM_NormalEnemy_t
{
    [SerializeField] CircleCollider2D circleCol;

    protected override void Awake()
    {
        circleCol = GetComponents<CircleCollider2D>()[0];
        triggerCol = GetComponents<CircleCollider2D>()[1];
        col = circleCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall", "Cliff"); // 근거리는 Cliff 추가
        childDustParticle = transform.Find("DustParticle").gameObject;
        childDeadParticle = transform.Find("SlimeDead_Particle").gameObject;
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        DustParticleCheck();
    }




    protected override RaycastHit2D[] GetRaycastType()
    {
        //CircleCast
        return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction, AtkRange - originOffset - circleCol.radius, m_viewTargetMask);
    }

    #region FSM Override
    protected override void InitStateDatas()
    {
        stateMachine.states.Add(NormalEnemyState.Idle, new Enemy_IdleState());
        stateMachine.states.Add(NormalEnemyState.Walk, new Enemy_WalkState());
        stateMachine.states.Add(NormalEnemyState.Attack, new TackleAttack());
        stateMachine.states.Add(NormalEnemyState.Wait, new Enemy_WaitState());
        stateMachine.states.Add(NormalEnemyState.Dead, new MeltAndDead());

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




    // In this case you choose event based on the clip weight
    /// <summary>
    /// 공격 (애니메이션 프레임에 넣기)
    /// </summary>
    public override void Attack_On()
    {

        isNuckback = true;

        if (inAtkDetectionRange && !isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE, false, 0);
        }

    }
}

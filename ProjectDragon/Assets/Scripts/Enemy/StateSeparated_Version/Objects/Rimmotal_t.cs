using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rimmotal_t : BaseFSM_Enemy
{
    private CapsuleCollider2D capsuleCol;

    [Header("[Rare Enemy Attribute]")]
    [SerializeField] protected float skillCooltime;     //상태2->상태1 time [Rare Type만]
    public int skillDamage;

    //object
    private Projectile projectile;
    [HideInInspector] public TargetPoint thornPoint;
    public RuntimeAnimatorController LeafAnimator;
    public RuntimeAnimatorController ThornAnimator;
    public bool _thorn_attacking = true;

    [Header("[Enemy State]")]
    [SerializeField] protected RimmotalEnemyState currentState;

    protected override void Awake()
    {
        capsuleCol = GetComponents<CapsuleCollider2D>()[0];
        triggerCol = GetComponents<CapsuleCollider2D>()[1];
        col = capsuleCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall", "Cliff"); // 근거리는 Cliff 추가
        childDustParticle = transform.Find("DustParticle").gameObject;

        //thornTargeting = new ThornTargeting();
        projectile = new Projectile();
        thornPoint = new TargetPoint();
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



    //상태 머신 작동
    public override IEnumerator Start_On()
    {
        yield return StartCoroutine(base.Start_On());

        //추적 상태
        ChangeState(RimmotalEnemyState.Walk);
        yield return null;
    }

    public override void Dead()
    {
        base.Dead();
        ChangeState(RimmotalEnemyState.Dead);
        GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().DropItem_Rimmotal(transform.position);
    }







    #region FSM
    //상태 변경
    public void ChangeState(RimmotalEnemyState newState)
    {
        currentState = newState;
        stateMachine.ChangeState(stateMachine.states[newState]);

        HandleInput();
    }

    protected void HandleInput()
    {
        switch (currentState)
        {
            case RimmotalEnemyState.Idle:
                StartCoroutine(CheckIdleState());
                break;
            case RimmotalEnemyState.Walk:
                StartCoroutine(CheckWalkState());
                break;
            case RimmotalEnemyState.Attack1:
                StartCoroutine(CheckAttack1State());
                break;
            case RimmotalEnemyState.Attack2:
                StartCoroutine(CheckAttack2State());
                break;
        }

    }

    #region HandleInput Function

    private IEnumerator CheckIdleState()
    {
        yield return new WaitForSeconds(1.0f);

        if (inAtkDetectionRange)
        {
            ChangeState(RimmotalEnemyState.Attack1);
        }
        else
        {
            ChangeState(RimmotalEnemyState.Walk);
        }
    }

    private IEnumerator CheckWalkState()
    {
        while (currentState == RimmotalEnemyState.Walk)
        {
            //공격감지범위에 들어오면 공격
            if (inAtkDetectionRange)
            {
                ChangeState(RimmotalEnemyState.Attack1);
                break;
            }
            yield return null;
        }
    }

    private IEnumerator CheckAttack1State()
    {
        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            yield return null;
        }
        //AnimatorClipInfo[] clipInfo = objectAnimator.GetCurrentAnimatorClipInfo(0);
        //float cliptime = clipInfo[0].clip.length;

        float cliptime = objectAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(cliptime / objectAnimator.GetCurrentAnimatorStateInfo(0).speed);

        ChangeState(RimmotalEnemyState.Attack2);
    }

    private IEnumerator CheckAttack2State()
    {
        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rimmotal_Burrow"))
        {
            yield return null;
        }
        SoundManager.Inst.EffectPlayerDB(27, this.gameObject);
        AnimationMatching();

        //가시공격 쿨타임 검사
        yield return StartCoroutine(CoolTimeCheck());
        objectAnimator.SetBool("Attack2", false);

        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rimmotal_Restore"))
        {
            yield return null;
        }

        AnimationMatching();
       
        ChangeState(RimmotalEnemyState.Idle);        
    }

    /// <summary>
    /// 가시공격(공격2) 쿨타임 검사
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoolTimeCheck()
    {
        _thorn_attacking = true;
        yield return new WaitForSeconds(skillCooltime);
        _thorn_attacking = false;

    }

    //해당 애니메이션이 끝날때까지 대기
    private IEnumerator AnimationMatching()
    {
        float cliptime = objectAnimator.GetCurrentAnimatorStateInfo(0).length;
        float deleyTime = cliptime / objectAnimator.GetCurrentAnimatorStateInfo(0).speed;
        yield return new WaitForSeconds(deleyTime);
    }

    #endregion

    #endregion







    #region 림모탈
    protected override RaycastHit2D[] GetRaycastType()
    {
        //float maxSizeAxis = capsuleCol.size.x < capsuleCol.size.y ? capsuleCol.size.y : capsuleCol.size.x;
        //CapsuleCast
        return Physics2D.CapsuleCastAll(startingPosition, capsuleCol.size, CapsuleDirection2D.Vertical, 0, direction, AtkRange - originOffset/*- (maxSizeAxis*0.5f)*/, m_viewTargetMask);
    }

    #region FSM Override
    protected override void InitStateDatas()
    {
        stateMachine.states.Add(RimmotalEnemyState.Idle, new Enemy_IdleState());
        stateMachine.states.Add(RimmotalEnemyState.Walk, new Enemy_WalkState());
        stateMachine.states.Add(RimmotalEnemyState.Attack1, new LeafAttack());
        stateMachine.states.Add(RimmotalEnemyState.Attack2, new ThornAttack());
        stateMachine.states.Add(RimmotalEnemyState.Dead, new Enemy_DeadState());

        //초기 상태 설정
        stateMachine.Initial_Setting(gameObject.GetComponent<BaseFSM_Enemy>(), stateMachine.states[RimmotalEnemyState.Idle]);
    }

    #endregion


    /// <summary>
    /// 나뭇잎 생성 (애니메이션 프레임에 넣기)
    /// </summary>
    public void Attack1_On()
    {
        Vector2 offset = new Vector2(-0.01f, -0.1f);
        float radius = 0.1f;

        SoundManager.Inst.EffectPlayerDB(26, this.gameObject);

        projectile.Create(projectileTargetList, offset, radius, Angle - 20.0f, 4.0f, ATTACKDAMAGE, LeafAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle - 15.0f, 4.0f, ATTACKDAMAGE, LeafAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle, 4.0f, ATTACKDAMAGE, LeafAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle + 15.0f, 4.0f, ATTACKDAMAGE, LeafAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle + 20.0f, 4.0f, ATTACKDAMAGE, LeafAnimator, false, transform.position);

    }


    #endregion

}

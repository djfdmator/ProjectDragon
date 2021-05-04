// ==============================================================
// Cracked Rimmotal
//
//  AUTHOR: Yang SeEun
// CREATED: 2019-12-20
// UPDATED: 2020-01-08
// ==============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RimmotalEnemyState { Idle, Walk, Attack1, Attack2, Dead}
public class Rimmotal : Enemy
{
    [Header("[Enemy State]")]
    [SerializeField] protected RimmotalEnemyState rimmotalEnemyState;
    CapsuleCollider2D capsuleCol;

    //object
    Projectile projectile;
    TargetPoint thornPoint;
    public RuntimeAnimatorController LeafAnimator;
    public RuntimeAnimatorController ThornAnimator;
    [SerializeField] bool _thorn_attacking = true;


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

    public override void Dead()
    {
        rimmotalEnemyState = RimmotalEnemyState.Dead;
        base.Dead();
    }

    protected override RaycastHit2D[] GetRaycastType()
    {
        //float maxSizeAxis = capsuleCol.size.x < capsuleCol.size.y ? capsuleCol.size.y : capsuleCol.size.x;

        //CapsuleCast
        return Physics2D.CapsuleCastAll(startingPosition, capsuleCol.size, CapsuleDirection2D.Vertical, 0, direction, AtkRange - originOffset/*- (maxSizeAxis*0.5f)*/, m_viewTargetMask);
    }


    public override IEnumerator Start_On()
    {
        StartCoroutine(base.Start_On());

        //1초 대기
        yield return new WaitForSeconds(1.0f);
        
        //추적
        isIdle = false;
        rimmotalEnemyState = RimmotalEnemyState.Walk;
        ChangeState<RimmotalEnemyState>(rimmotalEnemyState);

        //공격감지 체크
        StartCoroutine(AttackRangeCheck());
        yield return null;
    }


    /********************************************************************/
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
        projectile.Create(projectileTargetList, offset, radius, Angle , 4.0f, ATTACKDAMAGE, LeafAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle + 15.0f, 4.0f, ATTACKDAMAGE, LeafAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle + 20.0f, 4.0f, ATTACKDAMAGE, LeafAnimator, false, transform.position);

    }





    #region State Routine


    private IEnumerator IdleState()
    {
        //OnEnter
        isIdle = true;

        // Excute
        yield return new WaitForSeconds(1.0f);
        if (inAtkDetectionRange)
        {
            rimmotalEnemyState = RimmotalEnemyState.Attack1;
        }
        else
        {
            rimmotalEnemyState = RimmotalEnemyState.Walk;
        }

        //OnExit
        isIdle = false;
        ChangeState<RimmotalEnemyState>(rimmotalEnemyState);
    }

    private IEnumerator WalkState()
    {
        //OnEnter
        objectAnimator.SetBool("Walk", true);


        // Excute
        while (rimmotalEnemyState == RimmotalEnemyState.Walk)
        {
            if (isHit)
            {
                yield return null;
                continue;
            }

            //공격감지범위에 들어오면 공격
            if (inAtkDetectionRange)
            {
                rimmotalEnemyState = RimmotalEnemyState.Attack1;
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
                    //AStar
                    GetComponent<Tracking>().FindPathManager(rb2d, MoveSpeed);
                }
                else { isWalk = false; }
            }
            yield return null;
        }


        //OnExit
        isWalk = false;
        objectAnimator.SetBool("Walk", false);
        ChangeState<RimmotalEnemyState>(rimmotalEnemyState);
    }


    private IEnumerator Attack1State()
    {
        //OnEnter
        objectAnimator.SetBool("Attack1", true);
        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            yield return null;
        }
        isAttacking = true;


        //Execute
        AnimatorClipInfo[] clipInfo = objectAnimator.GetCurrentAnimatorClipInfo(0);
        float cliptime = clipInfo[0].clip.length;
        yield return new WaitForSeconds(cliptime / objectAnimator.GetCurrentAnimatorStateInfo(0).speed);
        rimmotalEnemyState = RimmotalEnemyState.Attack2;


        //OnExit
        objectAnimator.SetBool("Attack1", false);
        ChangeState<RimmotalEnemyState>(rimmotalEnemyState);
    }

    private IEnumerator Attack2State()
    {
        //OnEnter
        objectAnimator.SetBool("Attack2", true);
        IsFix = true;
        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rimmotal_Burrow"))
        {
            yield return null;
        }
        SoundManager.Inst.EffectPlayerDB(27, this.gameObject);



        //Execute
        float cliptime = objectAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(cliptime/ objectAnimator.GetCurrentAnimatorStateInfo(0).speed);

        //가시공격 쿨타임 검사
        StartCoroutine(CoolTimeCheck());
        yield return StartCoroutine(ThornAttack());
        objectAnimator.SetBool("Attack2", false);

        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rimmotal_Restore"))
        {
            yield return null;
        }

        float cliptime1 = objectAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(cliptime1 / objectAnimator.GetCurrentAnimatorStateInfo(0).speed);
        rimmotalEnemyState = RimmotalEnemyState.Idle;



        //OnExit
        IsFix = false;
        isAttacking = false;
        ChangeState<RimmotalEnemyState>(rimmotalEnemyState);
    }


    private IEnumerator DeadState()
    {
        GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().DropItem_Rimmotal(transform.position);
        yield return null;
    }


    /// <summary>
    /// 가시 공격
    /// </summary>
    private IEnumerator ThornAttack()
    {
        _thorn_attacking = true;

        while (_thorn_attacking && !isDead)
        {
            if (inAtkDetectionRange)
            {
                SoundManager.Inst.EffectPlayerDB(28, this.gameObject);

                //가시 생성
                thornPoint.Create(projectileTargetList, Vector2.zero, new Vector2(0.7f, 0.7f), skillDamage, ThornAnimator, false, other.position - new Vector3(0.0f, 0.5f, 0.0f));
                //thornTargeting.Create(skillDamage, "ThornTargeting", other.position);
                yield return new WaitForSeconds(2.5f);
            }
            yield return null;
        }
    }

    /// <summary>
    /// 가시공격(공격2) 쿨타임 검사
    /// </summary>
    /// <returns></returns>
    private IEnumerator CoolTimeCheck()
    {
        yield return new WaitForSeconds(skillCooltime);
        _thorn_attacking = false;

    }
    #endregion


}

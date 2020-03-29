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

public enum RimmotalEnemyState { Idle, Walk, Attack1, Attack2, }
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
    bool _thorn_attacking = true;


    public RimmotalEnemyState REState
    {
        get { return rimmotalEnemyState; }
        set
        {
            rimmotalEnemyState = value;
            SetState(rimmotalEnemyState);

        }
    }
    protected override void Awake()
    {
        base.Awake();
        

        capsuleCol = GetComponent<CapsuleCollider2D>();
        col = capsuleCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall", "Cliff"); // 근거리는 Cliff 추가
        childDustParticle = transform.Find("DustParticle").gameObject;

        //thornTargeting = new ThornTargeting();
        projectile = new Projectile();
        thornPoint = new TargetPoint();
    }
    protected override RaycastHit2D[] GetRaycastType()
    {
        //float maxSizeAxis = capsuleCol.size.x < capsuleCol.size.y ? capsuleCol.size.y : capsuleCol.size.x;

        //CapsuleCas
        return Physics2D.CapsuleCastAll(startingPosition, capsuleCol.size, CapsuleDirection2D.Vertical, 0, direction, AtkRange - originOffset/*- (maxSizeAxis*0.5f)*/, m_viewTargetMask);
    }
    public void Update()
    {
        DustParticleCheck();
    }


    protected override void Start()
    {
        base.Start();
    }

    public override IEnumerator Start_On()
    {
        StartCoroutine(base.Start_On());

        //1초후 추적
        yield return new WaitForSeconds(1.0f);
        isIdle = false;
        REState = RimmotalEnemyState.Walk;

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

        projectile.Create(projectileTargetList, offset, radius, Angle - 20.0f, 5.0f, 2, LeafAnimator, "ProjectileObj", false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle - 10.0f, 5.0f, 2, LeafAnimator, "ProjectileObj", false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle - 5.0f, 5.0f, 2, LeafAnimator, "ProjectileObj", false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle, 5.0f, 2, LeafAnimator, "ProjectileObj", false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle + 5.0f, 5.0f, 2, LeafAnimator, "ProjectileObj", false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle + 10.0f, 5.0f, 2, LeafAnimator, "ProjectileObj", false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle + 20.0f, 5.0f, 2, LeafAnimator, "ProjectileObj", false, transform.position);

    }



    IEnumerator Idle()
    { 
        isIdle = true;
        yield return new WaitForSeconds(1.0f);
        if (inAtkDetectionRange)
        {
            REState = RimmotalEnemyState.Attack1;
        }
        else
        {
            REState = RimmotalEnemyState.Walk;
        }
        isIdle = false;

    }

    IEnumerator Walk()
    {
        //Walk Animation parameters
        objectAnimator.SetBool("Walk", true);
        while (REState == RimmotalEnemyState.Walk && !isDead)
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
                REState = RimmotalEnemyState.Attack1;
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
                    //AStar
                    GetComponent<Tracking>().FindPathManager(rb2d, MoveSpeed);
                }
                else { isWalk = false; }
            }
            yield return null;
        }
    }

  
    IEnumerator Attack1()
    {
        //Attack Animation parameters
        objectAnimator.SetBool("Attack1", true);

        while(!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            yield return null;
        }

        isAttacking = true;

        AnimatorClipInfo[] clipInfo = objectAnimator.GetCurrentAnimatorClipInfo(0);
        float cliptime = clipInfo[0].clip.length;
        yield return new WaitForSeconds(cliptime / objectAnimator.GetCurrentAnimatorStateInfo(0).speed);

        REState = RimmotalEnemyState.Attack2;
        objectAnimator.SetBool("Attack1", false);
    }

    IEnumerator Attack2()
    {
        //Attack Animation parameters
        objectAnimator.SetBool("Attack2", true);
        IsFix = true;
      
        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rimmotal_Burrow"))
        {
            yield return null;
        }

        float cliptime = objectAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(cliptime/ objectAnimator.GetCurrentAnimatorStateInfo(0).speed);

        StartCoroutine(CoolTimeCheck());
        yield return StartCoroutine(ThornAttack());
        objectAnimator.SetBool("Attack2", false);

        while (!objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Rimmotal_Restore"))
        {
            yield return null;
        }

        float cliptime1 = objectAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(cliptime1 / objectAnimator.GetCurrentAnimatorStateInfo(0).speed);
        IsFix = false;
        isAttacking = false;
        REState = RimmotalEnemyState.Idle;

    }

    /// <summary>
    /// 가시생성
    /// </summary>
    IEnumerator ThornAttack()
    {
        _thorn_attacking = true;

        while (_thorn_attacking&&!isDead)
        {
            if (inAtkDetectionRange)
            {
                thornPoint.Create(projectileTargetList,Vector2.zero,0.35f,ATTACKDAMAGE, ThornAnimator, "TargetPoint", other.position - new Vector3(0.0f,0.5f,0.0f));
                //thornTargeting.Create(skillDamage, "ThornTargeting", other.position);
                yield return new WaitForSeconds(2.0f);
            }
            yield return null;
        }
    }
    /// <summary>
    /// 가시공격(공격2) 쿨타임 검사
    /// </summary>
    /// <returns></returns>
    IEnumerator CoolTimeCheck()
    {
        yield return new WaitForSeconds(6.0f);
        _thorn_attacking = false;

    }

}

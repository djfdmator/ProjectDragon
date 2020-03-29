// ==============================================================
// Cracked Enemy
//
//  AUTHOR: Yang SeEun
// CREATED: 
// UPDATED: 2020-01-04
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Monster
{
    [SerializeField]
    protected bool isIdle = true;
    protected Rigidbody2D rb2d;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected LayerMask m_viewTargetMask; // 인식 가능한 타켓의 마스크
    protected Collider2D col;
    [SerializeField] protected bool collisionPlayer = false;  // 플레이어와 충돌하였는지
    public bool isNuckback=true;                          //넉백할수있는지
    private bool isFix = false;
    protected bool IsFix                              //고정 
    {
        get { return isFix; }
        set
        {
            isFix = value;
            rb2d.constraints =
                isFix ? rb2d.constraints = RigidbodyConstraints2D.FreezeAll : rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        }
    } 
    



    protected float knockPower = 0.2f;
    protected const float knockTime = 0.3f;

    [Header("[Enemy Attribute]")]
    protected int knock_Resist;
    protected int dropMana_Min;
    protected int dropMana_Max;


    [Header("[Rare Enemy Attribute]")]
    [SerializeField] protected float skillCooltime;     //상태2->상태1 time [Rare Type만]
    public int skillDamage;


    //Distance calculation
    [Header("[Distance calculation]")]
    [SerializeField] protected bool m_bDebugMode = false;      //테스트용
    public bool inAtkDetectionRange;
    protected Vector3 direction;
    protected RaycastHit2D[] hit;
    [SerializeField] protected float originOffset;
    protected Vector2 startingPosition;
    protected Vector3 directionOriginOffset;


    //Effect
    protected GameObject childDustParticle;
    protected GameObject childDeadParticle;
    bool DustParticle_Actuation = false;

    public float Angle
    {
        get { return current_angle; }
        set
        {
            current_angle = value;
            objectAnimator.SetFloat("Angle", current_angle);
        }
    }


    protected override void Awake()
    {
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall");

        //col = GetComponent<BoxCollider2D>();
        objectAnimator = gameObject.GetComponentInParent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();
        other = GameObject.FindGameObjectWithTag("Player").transform;
        spriteRenderer = GetComponent<SpriteRenderer>();

        base.Awake();
    }

    protected void FixedUpdate()
    {
        direction = (other.transform.position - gameObject.transform.position).normalized;

        //플레이어를 바라보는 방향에 대한 각도체크
        if(isAttacking || isIdle)
        {
            Angle = Quaternion.FromToRotation(Vector3.up, transform.position - other.transform.position).eulerAngles.z;
        }
        else if(isWalk)
        {
            Angle = Quaternion.FromToRotation(Vector3.up, transform.position - GetComponent<Tracking>().currentWaypoint).eulerAngles.z;
        }
    }

    /// <summary>
    ///개체의 상태가 바뀔때마다 실행
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="newState"></param>
    protected override void SetState<T>(T newState)
    {
        StartCoroutine(newState.ToString());
    }


    public override IEnumerator Start_On()
    {
        //Grid 생성
        GetComponent<Tracking>().pathFinding.Create(col, transform.GetComponentInParent<t_Grid>());
        yield return null;
    }

    public override void Dead()
    {
        base.Dead();
        StartCoroutine(EnemyDead());
    }

    protected virtual IEnumerator EnemyDead()
    {
        //Dead Animation parameters
        objectAnimator.SetTrigger("Dead");

        col.enabled = false;

        Destroy(gameObject, 5.0f);

        yield return null;
    }

    //raycast
    protected IEnumerator AttackRangeCheck()
    {
        //살아있을때만 raycast 체크
        while (!isDead)
        {
            inAtkDetectionRange = CheckRaycast();
            yield return null;
        }
    }

    protected virtual RaycastHit2D[] GetRaycastType()
    {
        ////BoxCast
        //return Physics2D.BoxCastAll(startingPosition, boxCol.size, 0, direction, AtkRange - originOffset, m_viewTargetMask);
        ////CircleCast
        //return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction, AtkRange - originOffset, m_viewTargetMask);
        ////CapsuleCas
        //return Physics2D.CapsuleCastAll(startingPosition, capsuleCol.size, CapsuleDirection2D.Vertical, 0, direction, AtkRange - originOffset, m_viewTargetMask);

        return Physics2D.RaycastAll(startingPosition, direction, AtkRange - originOffset, m_viewTargetMask);
    }

    bool isRayHit = false;
    RaycastHit2D HitRay;
    protected bool CheckRaycast()
    {
        inAtkDetectionRange = false;

        directionOriginOffset = originOffset * new Vector3(direction.x, direction.y, transform.position.z);
        startingPosition = transform.position + directionOriginOffset;

        #region int layerMask 숫자로 변환 해두기..

        ////layerMask = ~layerMask;   //이런것도 있다. 
        ////int layerMask = (1 << ;player_Layer_num; | 1 << ;player_Layer_num; );
        ////int layerMask = (1 << 8) | (1 << 13) | (1 << 12);

        //int layerMask = (1 << LayerMask.NameToLayer("Player") | 1 << LayerMask.NameToLayer("Wall"));

        #endregion

       
        hit = GetRaycastType();
        //hit = Physics2D.RaycastAll(startingPosition, direction, AtkRange - originOffset, m_viewTargetMask);

        isRayHit = false;
        foreach (RaycastHit2D _hit in hit)
        {
            if (_hit.collider != null)
            {
                HitRay = _hit;
                //Debug.Log("hit name :" + _hit.collider.gameObject.name);
                if (_hit.collider.gameObject.CompareTag("Wall") || _hit.collider.gameObject.CompareTag("Cliff"))
                {
                    isRayHit = true;
                    break;
                }
                if (_hit.collider.gameObject.CompareTag("Player"))
                {
                    isRayHit = true;
                    inAtkDetectionRange = true;
                    break;
                }
                
            }
        }
        return inAtkDetectionRange;
    }

    #region Hit
    public override int HPChanged(int ATK, bool isCritical, int NukBack)
    {
        //살아 있을때 + 무적이 아닐때
        if (!isDead && !invincible)
        {
            Hit(NukBack, isCritical);
            return base.HPChanged(ATK,isCritical, NukBack);
        }

        return 0;
    }

    IEnumerator KnockBackCor;
    protected void Hit(int NukBack, bool isCritical)
    {
        isHit = true;
        isWalk = false;

        //White Shader
        IEnumerator FlashWhiteCor = flashWhite.Flash();
        StopCoroutine(FlashWhiteCor);
        StartCoroutine(FlashWhiteCor);

        if (!IsFix|| isNuckback)
        {
            //넉백수치조정
            if (isCritical)
            {
                knockPower = NukBack;    //기본 1.0f;
            }
            else   //움찔
            {
                knockPower = 0.2f;
            }

            KnockBackCor = KnockBack();
            StopCoroutine(KnockBackCor);
            StartCoroutine(KnockBackCor);
        }
    }
    IEnumerator KnockBack()
    {
        rb2d.AddForce(-direction * knockPower, ForceMode2D.Impulse);
        yield return new WaitForSeconds(knockTime);

        rb2d.velocity = Vector2.zero;
        isHit = false;
    }

    #endregion



    //Draw!!!! 테스트용
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (m_bDebugMode)
        {
            directionOriginOffset = originOffset * new Vector3(direction.x, direction.y, transform.position.z);
            startingPosition = transform.position + directionOriginOffset;

            //범위
            Gizmos.DrawWireSphere(transform.position, AtkRange);

            //RayCast
            if (isRayHit)
            {
                //Debug.DrawRay(startingPosition, direction * (HitRay.distance-0.3f), Color.red);
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(new Vector3(startingPosition.x, startingPosition.y,1) + direction * HitRay.distance, 0.3f);  //0.3f는 임시로 콜라이더 radius
            }
            else
            {
                Debug.DrawRay(startingPosition, direction * (AtkRange - originOffset), Color.red);
            }
        }
#endif
    }
    //+ Vector2.Dot(directionOriginOffset, direction)


    #region Player에게 다가가는 무리들에 대한 이동조정
    protected virtual void OnCollisionStay2D(Collision2D collision)
    {
        //Player에게 다가가는 무리들에 대한 이동조정.. (walk)

        if (collision.gameObject.CompareTag("Player") ||
             (collision.gameObject.CompareTag("Enemy") && (collision.gameObject.GetComponent<Enemy>().collisionPlayer)))
        {
            collisionPlayer = true;
            if (collisionPlayer)
            {
                rb2d.isKinematic = true;
                rb2d.velocity = Vector2.zero;
            }
        }

    }
    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("Enemy"))
        {
            rb2d.isKinematic = false;
            collisionPlayer = false;
        }
    }
    #endregion

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //콜라이더 꺼져있을때 Hit되면 콜라이더 켜기 (혹시모를 검사 한번더하기)
            if (isHit)
            {
                Physics2D.IgnoreCollision(collision, col);
            }
        }
    }
    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Wall")||collision.gameObject.CompareTag("Cliff"))
        {
            //충돌할때 walk이면 콜라이더끄기
            if (isWalk)
            {
                Physics2D.IgnoreCollision(collision, col);
            }
            //콜라이더 꺼져있을때 Hit되면 콜라이더 켜기
            if (isHit)
            {
                Physics2D.IgnoreCollision(collision, col, false);
            }
        }

        //Hit중이면 Enemy 충돌무시
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (isHit)
            {
                Physics2D.IgnoreCollision(collision, col);
            }
            else
            {
                Physics2D.IgnoreCollision(collision, col, false);
            }
        }
    }
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Wall") || collision.gameObject.CompareTag("Cliff"))
        {
            Physics2D.IgnoreCollision(collision, col, false);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Physics2D.IgnoreCollision(collision, col,false);
        }
    }
    protected IEnumerator PushStopCor;
    /// <summary>
    /// 밀리는 것을 방지
    /// </summary>
    /// <returns></returns>
    protected IEnumerator PushStop() 
    {
        yield return new WaitForSeconds(1.0f);
        rb2d.velocity = Vector2.zero;
    }


    //Dust Particle
    protected void DustParticleCheck()
    {
        if (!isDead && childDustParticle != null)
        {
            DustParticle_Actuation = /*isHit ||*/ isWalk ? true : false;
            childDustParticle.SetActive(DustParticle_Actuation);
        }
        else
        {
            childDustParticle.SetActive(false);
        }
    }


}

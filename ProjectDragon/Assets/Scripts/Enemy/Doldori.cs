/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-18////////////////////
//////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doldori : FSM_NormalEnemy
{
    CapsuleCollider2D capsuleCol;
    Vector3 attackDirection;
    IEnumerator AttackEndCor = null;

[SerializeField] float attackSpeed=0.2f;
    protected override void Awake()
    {
        base.Awake();
        capsuleCol = GetComponent<CapsuleCollider2D>();
        col = capsuleCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall" , "Cliff"); // 근거리는 Cliff 추가
        //childDustParticle = transform.Find("DustParticle").gameObject;
    }

    protected override RaycastHit2D[] GetRaycastType()
    {
        //CapsuleCas
        return Physics2D.CapsuleCastAll(startingPosition, capsuleCol.size, CapsuleDirection2D.Vertical,0, direction,AtkRange - originOffset, m_viewTargetMask);
    }

    protected override void Start()
    {
        base.Start();
    }
    void Update()
    {
       
    }

    public void Attack_On()
    {
        if (!isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE,false,0);
        }
    }

    protected override IEnumerator Attack()
    {
        AttackStart();
        invincible = true;                             //무적
        yield return new WaitForSeconds(1.5f);         //대기

        //Attacking
        isAttacking = true;
        objectAnimator.Play("Attacking");
        attackDirection = direction;

        while (isAttacking)
        {
            rb2d.AddForce(attackDirection * attackSpeed, ForceMode2D.Impulse);
            yield return null;
        }
    }


    //부딪히면 Attack-> Idle로
    protected override IEnumerator AttackEnd()
    {
        isAttacking = false;
        invincible = false;

        rb2d.velocity = Vector2.zero;

        //반동
        rb2d.AddForce(-attackDirection * 1.5f, ForceMode2D.Impulse);
        //Attack Animation parameters
        objectAnimator.SetBool("Attack", isAttacking);
        NEState = NormalEnemyState.Idle;


        yield return null;

        AttackEndCor = null;
    }



    //벽과 플레이어 부딪히면 그로기 상태
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        //연속으로 될경우 방지 (한번만 돌리게)
        if (NEState == NormalEnemyState.Attack && AttackEndCor == null)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall")
                || collision.gameObject.CompareTag("Cliff")|| collision.gameObject.CompareTag("Object"))
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    Attack_On();
                }
  
                if(collision.gameObject.GetComponent<MapObject>() !=null)
                {
                    collision.gameObject.GetComponent<MapObject>().HpChanged(50);
                    if( collision.gameObject.GetComponent<Box>() != null)
                    {
                        return;
                    }
                }
                AttackEndCor = AttackEnd();
                StartCoroutine(AttackEnd());
            }
        }
    }
    //부모함수 부르지않기 위해서 (지우면안됨)
    protected override void OnCollisionExit2D(Collision2D collision)
    {
    } 
    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);

        if (isAttacking)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                //충돌할때 Attack이면 콜라이더끄기
                Physics2D.IgnoreCollision(collision, col);
            }
        }
    }
}

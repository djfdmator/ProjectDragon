﻿// ==============================================================
// 몬스터 돌진형인 돌돌이
//
//  AUTHOR: Yang SeEun
// CREATED: 2019-09-18
// UPDATED: 2020-11-13
// ==============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doldori : FSM_NormalEnemy
{
    CircleCollider2D circleCol;
    Vector3 attackDirection;

    protected override void Awake()
    {
        circleCol = GetComponents<CircleCollider2D>()[0];
        triggerCol = GetComponents<CircleCollider2D>()[1];
        col = circleCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall" , "Cliff"); // 근거리는 Cliff 추가
        base.Awake();
        //childDustParticle = transform.Find("DustParticle").gameObject;
    }

    protected override RaycastHit2D[] GetRaycastType()
    {
        //CapsuleCas
        return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction,AtkRange - originOffset, m_viewTargetMask);
    }

    protected override void Start()
    {
        base.Start();
    }

    public override void Attack_On()
    {
        if (!isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE,false,0);
        }
    }

    protected override IEnumerator AttackState()
    {
        //OnEnter
        AttackStart();
        yield return new WaitForSeconds(1.0f);         //대기


        if (!isDead)
        {
            invincible = true;                             //무적

            //Attacking
            isAttacking = true;
            objectAnimator.Play("Attacking");
            attackDirection = direction;

            //Execute
            while (normalEnemyState == NormalEnemyState.Attack)
            {
                rb2d.AddForce(attackDirection * ATTACKSPEED, ForceMode2D.Impulse);
                yield return new WaitForSeconds(Time.deltaTime);
                yield return null;
            }
        }
    }


    //부딪히면 Attack-> Idle로
    protected override IEnumerator AttackEnd()
    {
        //OnExit
        normalEnemyState = NormalEnemyState.Idle;
        isAttacking = false;
        invincible = false;
        objectAnimator.SetBool("Attack", isAttacking);

        //반동
        rb2d.velocity = Vector2.zero;
        rb2d.AddForce(-attackDirection * 1.5f, ForceMode2D.Impulse);

        yield return null;

        AttackEndCor = null;
        ChangeState<NormalEnemyState>(normalEnemyState);
    }



    //벽과 플레이어 부딪히면 그로기 상태
    protected override void OnCollisionStay2D(Collision2D collision)
    {
        //연속으로 될경우 방지 (한번만 돌리게)
        if (normalEnemyState == NormalEnemyState.Attack && AttackEndCor == null)
        {
            if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Wall")
                || collision.gameObject.CompareTag("Cliff") || collision.gameObject.CompareTag("Object") || collision.gameObject.CompareTag("Enemy"))
            {

                AttackEndCor = AttackEnd();
                if (collision.gameObject.CompareTag("Player"))
                {
                    Attack_On();
                }
                else if(collision.gameObject.GetComponent<MapObject>() !=null)
                {
                    collision.gameObject.GetComponent<MapObject>().HpChanged(50);
                    if( collision.gameObject.GetComponent<Box>() != null)       //박스는 뚫고 가기
                    {

                        AttackEndCor = null;
                        return;
                    }
                }
                StartCoroutine(AttackEndCor);
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

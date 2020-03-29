/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-18////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FSM_NormalEnemy
{
    CircleCollider2D circleCol;
    protected override void Awake()
    {

        base.Awake();
        circleCol = GetComponent<CircleCollider2D>();
        col = circleCol;
        m_viewTargetMask = LayerMask.GetMask("Player", "Wall", "Cliff"); // 근거리는 Cliff 추가
        childDustParticle = transform.Find("DustParticle").gameObject;
        childDeadParticle = transform.Find("SlimeDead_Particle").gameObject;
    }

    protected override RaycastHit2D[] GetRaycastType()
    {
        //CircleCast
        return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction, AtkRange - originOffset- circleCol.radius, m_viewTargetMask);
    }

    protected override void Start()
    {
        base.Start();
    }

    public void Update()
    {
        DustParticleCheck();
    }


    // In this case you choose event based on the clip weight
    /// <summary>
    /// 공격 (애니메이션 프레임에 넣기)
    /// </summary>
    public void Attack_On()
    {

        isNuckback = true;

        if (inAtkDetectionRange && !isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE,false,0);
        }

    }

   

    protected override IEnumerator Attack()
    {
        isNuckback = false;

        StartCoroutine(base.Attack());
        yield return null;
    }


    //임시
    protected override IEnumerator EnemyDead()
    {
        DeadParticle();
        StartCoroutine(base.EnemyDead());
        yield return null;
    }


   
    void DeadParticle()
    {
        childDeadParticle.SetActive(true);
        childDeadParticle.GetComponent<ParticleSystem>().Play();
    }

}
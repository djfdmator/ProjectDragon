﻿/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-18////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : FSM_NormalEnemy
{
    [SerializeField] CircleCollider2D circleCol;
    protected GameObject childDeadParticle;

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
    public override void Attack_On()
    {

        isNuckback = true;

        if (inAtkDetectionRange && !isDead)
        {
            //Player hit
            other.gameObject.GetComponent<Character>().HPChanged(ATTACKDAMAGE,false,0);
        }

    }

    protected override IEnumerator AttackState()
    {
        isNuckback = false;
        StartCoroutine(base.AttackState());
        yield return null;
    }

    protected override IEnumerator DeadState()
    {
        StartCoroutine(base.DeadState());
        DeadParticle();
        yield return null;

    }





    private void DeadParticle()
    {
        childDeadParticle.SetActive(true);
        childDeadParticle.GetComponent<ParticleSystem>().Play();
    }

}
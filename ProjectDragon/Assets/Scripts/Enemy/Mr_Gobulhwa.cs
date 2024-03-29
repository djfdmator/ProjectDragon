﻿// ==============================================================
// Cracked Mr_Gobulhwa
//
//  AUTHOR: Yang SeEun
// CREATED: 
// UPDATED: 2019-12-18
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobulhwa : FSM_NormalEnemy
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

    protected override RaycastHit2D[] GetRaycastType()
    {
        //CircleCast
        return Physics2D.CircleCastAll(startingPosition, circleCol.radius, direction, AtkRange - originOffset, m_viewTargetMask);
    }

    private void Update()
    {
        DustParticleCheck();
    }

    /// <summary>
    /// 탄환 생성 (애니메이션 프레임에 넣기)
    /// </summary>
    ///  In this case you choose event based on the clip weight
    public override void Attack_On()
    {
        Vector2 offset = new Vector2(0.0f,0.0f);
        float radius = 0.06f;

        projectile.Create(projectileTargetList,offset, radius, Angle - 30, 3.0f, ATTACKDAMAGE, projectileAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle, 3.0f, ATTACKDAMAGE, projectileAnimator, false, transform.position);
        projectile.Create(projectileTargetList, offset, radius, Angle + 30, 3.0f, ATTACKDAMAGE, projectileAnimator, false, transform.position);

    }
   
}

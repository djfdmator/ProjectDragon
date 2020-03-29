/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-18////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mr_Gobunin : FSM_NormalEnemy
{
    CircleCollider2D circleCol;
    Projectile projectile;
    public RuntimeAnimatorController projectileAnimator;

    protected override void Awake()
    {
        base.Awake();
        circleCol = GetComponent<CircleCollider2D>();
        col = circleCol;
        childDustParticle = transform.Find("DustParticle").gameObject;
        projectile = new Projectile();
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
    /// 탄환 공격 (애니메이션 프레임에 넣기)
    /// </summary>
    public void Attack_On()
    {
        Vector2 offset = new Vector2(0.0f, 0.0f);
        float radius = 0.06f;

        projectile.Create(projectileTargetList, offset, radius, Angle, 3, ATTACKDAMAGE, projectileAnimator, "ProjectileObj", false, transform.position);
    }
    
}


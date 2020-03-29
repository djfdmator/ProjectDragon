/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-13////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Monster : Character
{

  [SerializeField] protected bool invincible = false;       //무적상태인지

    protected Animator objectAnimator;
    //Effect
    protected FlashWhite flashWhite;
    IEnumerator StartOnCor;


    protected override void Awake()
    {
        base.Awake();
        flashWhite = GetComponent<FlashWhite>();
        StartOnCor = Start_On();

        projectileTargetList.Add("Player");

     //   Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Player"), true);
    }
    public override int HPChanged(int ATK, bool isCritical, int NukBack)
    {
        if (!isDead)
        {
            //데미지 띄우기
            damagePopup.Create(transform.position + new Vector3(0.0f, 0.5f, 0.0f), ATK, isCritical, invincible);
            return base.HPChanged(ATK, isCritical, NukBack);
        }
        return 0;
    }

    public virtual IEnumerator Start_On()
    {
        yield return null;
    }

    public void StartOn()
    {
        StartCoroutine(StartOnCor);
    }


    //삭제할것
    //플레이어와 적과의 거리 캐스팅

    public float distanceOfPlayer;
    [HideInInspector]
    public float angleOfPlayer;
    [HideInInspector]
    public float moveDistance;

}

// ==============================================================
// Skill Button
//
//  AUTHOR: Yang SeEun
// CREATED: 2020-11-03
// UPDATED: 2020-11-05
// ==============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    private Player player;
    private GameObject normalObj;
    private GameObject activationObj;
    private GameObject inactivationObj;

    private BoxCollider col;
    private UISprite icon_sprite;
    private UISprite yellowRing;
    private UILabel timeLabel;

    private float coolTime;
    private int mpCost;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        normalObj = transform.Find("Basic").gameObject;
        activationObj = transform.Find("Activation").gameObject;
        inactivationObj = transform.Find("Inactivation").gameObject;

        icon_sprite = transform.Find("Mask").GetComponentInChildren<UISprite>();
        yellowRing = activationObj.transform.Find("YellowRing").GetComponent<UISprite>();
        timeLabel = activationObj.transform.Find("TimeLabel").GetComponent<UILabel>();
        col = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        OnBasic();
    }


    /// <summary>
    /// 스킬이미지,쿨타임,마나소모량 초기화
    /// </summary>
    public void Init(float _coolTime, int _mpCost , string _imageName)
    {
        coolTime = _coolTime;
        mpCost = _mpCost;

        string str_ICON = string.Format("SkillIcon_{0}_Ingame", _imageName);
        //str_ICON = string.Format("SkillIcon_Thunderbolt_Ingame");

        icon_sprite.spriteName = str_ICON;
        timeLabel.text = Mathf.FloorToInt(coolTime).ToString();

    }


    private void OnClick()
    {
        if(player.inAttackTarget)
        {
            if (player.MP - mpCost >= 0)
            {
                player.OnSkillActive();

                player.MP -= mpCost;
                StartCoroutine(CalcCoolTime());
                OnActive();
                SoundManager.Inst.Ds_EffectPlayerDB(12);
            }
        }
        else
        {
            Debug.Log("공격범위에 대상이 없음");
            //띠릭 Sound
        }
    }


    
    //스킬사용할때
    private void OnActive()
    {
        activationObj.SetActive(true);
        normalObj.SetActive(false);
        //inactivationObj.SetActive(false);

        col.enabled = false;
    }
    //기본일때
    private void OnBasic()
    {
        normalObj.SetActive(true);
        activationObj.SetActive(false);
        inactivationObj.SetActive(false);

        col.enabled = true;
        yellowRing.fillAmount = 1f;
    }

    //뭔가 버벅거림..그래서 코루틴사용
    //private void OnInactive()
    //{
    //    inactivationObj.SetActive(true);
    //    activationObj.SetActive(false);

    //    col.enabled = false;
    //}


    //쿨타임 계산
    private IEnumerator CalcCoolTime()
    {
        float _time = coolTime;
        while (_time > 0)
        {
            yellowRing.fillAmount = _time / coolTime;
            timeLabel.text = Mathf.CeilToInt(_time).ToString();
            _time -= Time.deltaTime;
            yield return null;
        }

        if (player.MP - mpCost < 0)
        {
            //OnInactive();
            inactivationObj.SetActive(true);
            col.enabled = false;
            yield return null;

            activationObj.SetActive(false);

            yield return StartCoroutine(CheckIsSkill());
        }

        OnBasic();
        yield return null;
    }

    /// <summary>
    /// 현재 스킬 사용할 수 있는지 체크하여 버튼을 활성화/비활성화한다.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CheckIsSkill()
    {
        timeLabel.gameObject.SetActive(false);
        while (player.MP - mpCost < 0)
        {
            yield return null;
        }
        timeLabel.gameObject.SetActive(true);
 
    }


    ///// <summary>
    ///// Skill Projectile
    ///// Animation Event Function (애니메이션 이벤트)
    ///// </summary>
    ///// <param name="type"></param>
    //public void CreateProjectile()
    //{
    //    //투사체 앵글 변수
    //    float attackAngle = (player.EnemyArray.Count == 0) ? player.current_angle : player.enemy_angle;

    //    Vector2 offset = Vector2.zero;
    //    float radius = 0.2f;
    //    if (player.weaponType == Player.WeaponType.NormalSword)
    //    {
    //        projectile.Create(player.projectileTargetList, offset, radius, attackAngle, 3.0f, 17, player.projectileAnimator[0], true, player.transform.position);
    //    }
    //    else if (player.weaponType == Player.WeaponType.Excalibur)
    //    {
    //        projectile.Create(player.projectileTargetList, offset, radius, attackAngle, 0, 17, player.projectileAnimator[7], true, player.transform.position);
    //    }
    //    else if (player.weaponType == Player.WeaponType.Nereides)
    //    {
    //        projectile.Create(player.projectileTargetList, offset, radius, attackAngle, 1.5f, 17, player.projectileAnimator[5], true, player.transform.position, true);

    //    }
    //    else if (player.weaponType == Player.WeaponType.NormalStaff)
    //    {
    //        if (player.TempEnemy != null)
    //        {
    //            Time.timeScale = 0.5f;
    //            offset = new Vector2(0.0f, 0.3f);
    //            targetProjectile.Create(player.projectileTargetList, offset, 0.4f, 17, player.projectileAnimator[1], true, player.TempEnemy.transform.position);
    //        }
    //    }
    //    else if (player.weaponType == Player.WeaponType.Nyx)
    //    {
    //        if (player.TempEnemy != null)
    //        {
    //            offset = new Vector2(0.0f, 0.3f);
    //            targetProjectile.Create(player.projectileTargetList, offset, 0.7f, 17, player.projectileAnimator[3], true, player.TempEnemy.transform.position);
    //        }
    //    }


    //    //if (player.attackType == AttackType.ShortRange)
    //    //{
    //    //    //Create Projectile 
    //    //    Vector2 offset = new Vector2(0.0f, 0.0f);
    //    //    float radius = 0.2f;
    //    //    projectile.Create(player.projectileTargetList, offset, radius, attackAngle, 3.0f, 17, player.projectileAnimator[0], "ProjectileObj", true, player.transform.position);
    //    //}
    //    //else if (player.attackType == AttackType.LongRange)
    //    //{
    //    //    Vector2 offset = new Vector2(0.0f, 0.5f);
    //    //    pointProjectile.Create(player.projectileTargetList, offset, 0.7f, 17, player.projectileAnimator[1], "TargetPoint", player.TempEnemy.transform.position);
    //    //    //Create Projectile 
    //    //    //projectile.Create(player.projectileTargetList, offset, 0.2f, _swordAttackangle, 3.0f, 10, player.projectileAnimator[1], "ProjectileObj", true, player.transform.position);
    //    //}
    //}

}

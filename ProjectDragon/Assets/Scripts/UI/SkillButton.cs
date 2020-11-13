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

    private UIButton button;
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
        button = GetComponent<UIButton>();
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
        button.normalSprite = str_ICON;
        timeLabel.text = Mathf.FloorToInt(coolTime).ToString();

    }


    private void OnClick()
    {
        if (player.inSkillRange)
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
#if UNITY_EDITOR
            Debug.Log("공격범위에 대상이 없음");
#endif
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
}

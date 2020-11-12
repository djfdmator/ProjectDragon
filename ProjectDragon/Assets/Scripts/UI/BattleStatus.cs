// ==============================================================
// BattleMone UI 캐릭터 능력치
//
//  AUTHOR: Yang SeEun
// CREATED: 2020-11-03
// UPDATED: 2020-11-05
// ==============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStatus : MonoBehaviour
{
    private UISprite hp_foreGround;
    private UILabel hpLabel;
    private UILabel mpLabel;

    //현재 스테이지의 Hp, Mp
    private float m_curHP;
    private float m_maxHP;
    private float m_curMP;

    // 카운팅에 걸리는 시간
    [SerializeField] private float duration = 0.5f;
    private IEnumerator Co_MpCount;
    private IEnumerator Co_HpCount;

    private void Awake()
    {
        hp_foreGround = transform.Find("HPBar").transform.Find("ForeGround").GetComponent<UISprite>();
        hpLabel = transform.Find("HPBar").GetComponentInChildren<UILabel>();
        mpLabel = transform.Find("MPBar").GetComponentInChildren<UILabel>();
    }


    //스테이지 넘어갈때 (장비교체 후) Hp와 Mp초기화해주기
    public void Init(float _curHp, float _maxHp, int _mp)
    {
        m_curHP = _curHp;
        m_maxHP = _maxHp;
        m_curMP = _mp;


        hp_foreGround.fillAmount = m_curHP / m_maxHP;
        hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(m_curHP), Mathf.Floor(m_maxHP));
        mpLabel.text = string.Format("{0:#,###}", m_curMP);
    }

    public void ChangeHpBar(float _curHp, float _nextHp)
    {
        //m_curHP = Mathf.FloorToInt(_curHp);
        //hpLabel.text = string.Format("{0}/{1}", m_curHP, m_maxHP);
        //hp_foreGround.fillAmount = _curHp/m_maxHP;

        //숫자카운팅,프로그래스바추가
        if (Co_HpCount != null)
        {
            StopCoroutine(Co_HpCount);
        }
        else
        {
            m_curHP = _curHp;
        }
        Co_HpCount = HpCount( _nextHp);
        StartCoroutine(Co_HpCount);
       

    }
    public void ChangeMpBar(int _curMp, int nextMp)
    {
        //숫자카운팅추가
        if (Co_MpCount != null)
        {
            StopCoroutine(Co_MpCount);
        }
        else
        {
            m_curMP = _curMp;
        }
        Co_MpCount = MpCount(nextMp);
        StartCoroutine(Co_MpCount);
    }


    private IEnumerator MpCount (float target)
    {
        //Label
        float offest = (target - m_curMP) / duration;

        float tempTarget = target;
        tempTarget = Mathf.Clamp(tempTarget, 0, 999999999);

        if (target - m_curMP > 0)                              //증가
        {
            while (m_curMP < tempTarget)
            {
                mpLabel.text = string.Format("{0:#,##0}", Mathf.Floor(m_curMP));
                m_curMP += offest * Time.deltaTime;
                yield return null;
            }
        }
        else                                                   //감소
        {
            while (m_curMP > tempTarget)
            {
                mpLabel.text = string.Format("{0:#,##0}", Mathf.Floor(m_curMP));
                m_curMP += offest * Time.deltaTime;
                yield return null;
            }
        }
        mpLabel.text = string.Format("{0:#,##0}", Mathf.Floor(tempTarget));

        Co_MpCount = null;
        yield return null;
    }

    private IEnumerator HpCount(float target)
    {
        float offest = (target - m_curHP) / duration;

        float tempTarget = target;
        tempTarget = Mathf.Clamp(tempTarget, 0, m_maxHP);

        if (target - m_curHP > 0)                            //증가
        {
            while (m_curHP <tempTarget)
            {
                //progress bar
                hp_foreGround.fillAmount = m_curHP / m_maxHP;

                //Label
                hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(m_curHP), Mathf.Floor(m_maxHP));
                m_curHP += offest * Time.deltaTime;
                yield return null;
            }
            hp_foreGround.fillAmount = m_curHP / m_maxHP;
        }
        else                                               //감소
        {
            while (m_curHP > tempTarget)
            {
                //progress bar
                hp_foreGround.fillAmount = m_curHP / m_maxHP;

                //Label
                hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(m_curHP), Mathf.Floor(m_maxHP));
                m_curHP += offest * Time.deltaTime;
                yield return null;
            }
            hp_foreGround.fillAmount = m_curHP / m_maxHP;
        }
        hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(tempTarget), m_maxHP);

        Co_HpCount = null;
        yield return null;
    }
}

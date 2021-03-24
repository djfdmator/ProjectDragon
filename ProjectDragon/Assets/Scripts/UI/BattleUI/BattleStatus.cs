// ==============================================================
// 플레이어 능력치 UI
// (배틀화면의 좌측 상단 정보창)
//
//  AUTHOR: Yang SeEun
// CREATED: 2020-11-03
// UPDATED: 2020-11-12
// ==============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStatus : MonoBehaviour
{
    private UISprite hp_foreGround;
    private UILabel hpLabel;
    private UILabel curMpLabel;
    public UILabel addMpLabel;

    //현재 스테이지의 Hp, Mp
    private float m_curHP;
    private float m_maxHP;
    private float m_curMP;
    private float m_addMP;

    // 카운팅에 걸리는 시간
    [SerializeField] private float duration = 0.5f;
    private IEnumerator Co_MpCount;
    private IEnumerator Co_addMpCount;
    private IEnumerator Co_HpCount;

    private void Awake()
    {
        hp_foreGround = transform.Find("HPBar").transform.Find("ForeGround").GetComponent<UISprite>();
        hpLabel = transform.Find("HPBar").GetComponentInChildren<UILabel>();
        curMpLabel = transform.Find("MPBar").transform.Find("CurrentMPLabel").GetComponent<UILabel>();
        addMpLabel = transform.Find("MPBar").transform.Find("AddMPLabel").GetComponent<UILabel>();
    }


    //스테이지 넘어갈때 (장비교체 후) Hp와 Mp초기화해주기
    public void Init(float _curHp, float _maxHp, int _mp)
    {
        m_curHP = _curHp;
        m_maxHP = _maxHp;
        m_curMP = _mp;
        m_addMP = 0;


        hp_foreGround.fillAmount = m_curHP / m_maxHP;
        hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(m_curHP), Mathf.Floor(m_maxHP));
        curMpLabel.text = string.Format("{0:#,##0}", m_curMP);
        addMpLabel.text = string.Format("+{0:#,##0}", m_addMP);
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
        Co_HpCount = HpCount(_nextHp);
        StartCoroutine(Co_HpCount);


    }
    /// <summary>
    /// 현재마나의 값을 변경
    /// </summary>
    /// <param name="_mp"></param>
    /// <param name="_targetMp"></param>
    public void ChangeMpLabel(int _mp, int _targetMp)
    {
        //숫자카운팅추가
        if (Co_MpCount != null)
        {
            StopCoroutine(Co_MpCount);
        }
        else
        {
            m_curMP = _mp;
        }
        Co_MpCount = MpCount(_targetMp);
        StartCoroutine(Co_MpCount);
    }

    /// <summary>
    /// 추가마나의 값을 변경
    /// </summary>
    /// <param name="_mp"></param>
    /// <param name="_targetMp"></param>
    public void ChangeAddMpLabel(int _targetMp)
    {
        //증가만 있기때문에 cur변수가 필요없음.
        //숫자카운팅추가
        if (Co_addMpCount != null)
        {
            StopCoroutine(Co_addMpCount);
        }

        Co_addMpCount = AddMpCount(_targetMp);
        StartCoroutine(Co_addMpCount);
    }

    #region Count

    private IEnumerator MpCount(float target)
    {
        //Label
        float offest = (target - m_curMP) / duration;

        float tempTarget = target;
        tempTarget = Mathf.Clamp(tempTarget, 0, 999999);

        if (target - m_curMP > 0)                              //증가
        {
            while (m_curMP < tempTarget)
            {
                curMpLabel.text = string.Format("{0:#,##0}", Mathf.Floor(m_curMP));
                m_curMP += offest * Time.deltaTime;
                yield return null;
            }
        }
        else                                                   //감소
        {
            while (m_curMP > tempTarget)
            {
                curMpLabel.text = string.Format("{0:#,##0}", Mathf.Floor(m_curMP));
                m_curMP += offest * Time.deltaTime;
                yield return null;
            }
        }
        curMpLabel.text = string.Format("{0:#,##0}", Mathf.Floor(tempTarget));
        m_curMP = tempTarget;

        Co_MpCount = null;
        yield return null;
    }

    private IEnumerator AddMpCount(float target)
    {
        //Label
        float offest = (target - m_addMP) / duration;

        float tempTarget = target;
        tempTarget = Mathf.Clamp(tempTarget, 0, 999999);

        if (target - m_addMP > 0)                              //증가
        {
            while (m_addMP < tempTarget)
            {
                addMpLabel.text = string.Format("+{0:#,##0}", Mathf.Floor(m_addMP));
                m_addMP += offest * Time.deltaTime;
                yield return null;
            }
        }

        addMpLabel.text = string.Format("+{0:#,##0}", Mathf.Floor(tempTarget));

        m_addMP = tempTarget;

        Co_addMpCount = null;
        yield return null;
    }


    private IEnumerator HpCount(float target)
    {
        float offest = (target - m_curHP) / duration;

        float tempTarget = target;
        tempTarget = Mathf.Clamp(tempTarget, 0, m_maxHP);

        if (target - m_curHP > 0)                            //증가
        {
            while (m_curHP < tempTarget)
            {
                //progress bar
                hp_foreGround.fillAmount = m_curHP / m_maxHP;

                //Label
                hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(m_curHP), Mathf.Floor(m_maxHP));
                m_curHP += offest * Time.deltaTime;
                yield return null;
            }
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
        }

        hp_foreGround.fillAmount = tempTarget / m_maxHP;
        hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(tempTarget), m_maxHP);

        m_curHP = tempTarget;

        Co_HpCount = null;
        yield return null;
    }
    #endregion
}

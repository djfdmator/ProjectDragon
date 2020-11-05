// ==============================================================
// BattleMone UI Stat
//
//  AUTHOR: Yang SeEun
// CREATED: 2020-11-03
// UPDATED: 2020-11-03
// ==============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStat : MonoBehaviour
{

    private UISprite hp_foreGround;
    private UILabel hpLabel;
    private UILabel mpLabel;

    //현재 스테이지의 Hp, Mp
    private float m_curHP;
    private float m_maxHP;
    private int m_curMP;

    // 카운팅에 걸리는 시간
    [SerializeField] private float duration = 0.5f;
    private IEnumerator Co_MpCount;
    private IEnumerator Co_HpCount;

    private void Awake()
    {
        hp_foreGround = transform.Find("HPBar").transform.Find("ForeGround").GetComponent<UISprite>();
        hpLabel = transform.Find("HPBar").GetComponentInChildren<UILabel>();
        mpLabel = transform.Find("MPBar").GetComponentInChildren<UILabel>();

        //파라미터 무엇으로 넣어야할지 고민해보기~
        Init(GameManager.Inst.CurrentHp, GameManager.Inst.MaxHp , GameManager.Inst.Mp);
    }

    //test용 나중에 꼭 지울것!
    private void Start()
    {
        ChangeHpBar(GameManager.Inst.CurrentHp, GameManager.Inst.CurrentHp + 50);
        ChangeMpBar(GameManager.Inst.Mp);
    }

    //스테이지 넘어갈때 (장비교체 후) 초기화해주기
    public void Init(float _curHp, float _maxHp , int _mp)
    {
        //m_curHP = Mathf.FloorToInt(_curHp);
        //m_maxHP = Mathf.FloorToInt(_maxHp);
        m_curHP = _curHp;
        m_maxHP = _maxHp;
        m_curMP = _mp;


        hp_foreGround.fillAmount = m_curHP / m_maxHP;
        hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(m_curHP), Mathf.Floor(m_maxHP));
        mpLabel.text = string.Format("{0:#,###}", m_curMP);
    }

    public void ChangeHpBar(float _curHp , float _nextHp)
    {
        //m_curHP = Mathf.FloorToInt(_curHp);
        //hpLabel.text = string.Format("{0}/{1}", m_curHP, m_maxHP);
        //hp_foreGround.fillAmount = _curHp/m_maxHP;

        //숫자카운팅,프로그래스바추가
        if (Co_HpCount != null)
        {
            StopCoroutine(Co_HpCount);
        }
        Co_HpCount = HpCount(_curHp, _nextHp);
        StartCoroutine(Co_HpCount);

    }
    public void ChangeMpBar(int _mp)
    {
        m_curMP = _mp;
        //mpLabel.text = string.Format("{0:#,###}", m_curMP);


        //숫자카운팅추가
        if (Co_MpCount != null)
        {
            StopCoroutine(Co_MpCount);
        }
        Co_MpCount = MpCount(m_curMP, 3344);
        StartCoroutine(Co_MpCount);
    }



    private IEnumerator MpCount(float current, float target)
    {
        //Label
        float offest = (target - current) / duration;
        if (target - current > 0)                              //증가
        {
            while (current < target)
            {
                mpLabel.text = string.Format("{0:#,###}", Mathf.Floor(current));
                current += offest * Time.deltaTime;
                yield return null;
            }
        }
        else                                                   //감소
        {
            while (current > target)
            {
                mpLabel.text = string.Format("{0:#,###}", Mathf.Floor(current));
                current += offest * Time.deltaTime;
                yield return null;
            }
        }
        mpLabel.text = string.Format("{0:#,###}",Mathf.Floor(target));
    }

    private IEnumerator HpCount(float current, float target)
    {
        float offest = (target - current) / duration;


        if (target - current > 0)                            //증가
        {
            while (current < target)
            {
                //progress bar
                hp_foreGround.fillAmount =current / m_maxHP;

                //Label
                hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(current), Mathf.Floor(m_maxHP));
                current += offest * Time.deltaTime;
                yield return null;
            }
        }
        else                                               //감소
        {
            while (current > target)
            {
                //progress bar
                hp_foreGround.fillAmount = 1f-(current / m_maxHP);

                //Label
                hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(current), Mathf.Floor(m_maxHP));
                current += offest * Time.deltaTime;
                yield return null;
            }
        }
        
        hpLabel.text = string.Format("{0}/{1}", Mathf.Floor(target), m_maxHP);
    }
}

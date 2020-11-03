// ==============================================================
// BattleMone UI Stat
//
//  AUTHOR: Yang SeEun
// CREATED:
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
    private float m_curMP;

    private void Awake()
    {
        hp_foreGround = transform.Find("HPBar").GetComponentInChildren<UISprite>();
        hpLabel = transform.Find("HPBar").GetComponentInChildren<UILabel>();
        mpLabel = transform.Find("MPBar").GetComponentInChildren<UILabel>();


        Init(GameManager.Inst.MaxHp, GameManager.Inst.MaxHp);
       
    }

    //스테이지 넘어갈때 (장비교체 후) 초기화해주기
    private void Init(float _curHp, float _maxHp)
    {
        m_curHP = _curHp;
        m_maxHP = _curHp;
        m_curMP = _maxHp;

        hpLabel.text = string.Format("{0}/{0}", m_curMP, m_maxHP);
        mpLabel.text = string.Format("{0}", m_curMP);
    }

    public void ChangeHpBar(float _curHp)
    {
        hpLabel.text = string.Format("{0}/{1}", _curHp, m_maxHP);
        hp_foreGround.fillAmount = _curHp/m_maxHP;
    }
    public void ChangeMpBar(int mana)
    {
        mpLabel.text = string.Format("{0}", mana);
    }
}

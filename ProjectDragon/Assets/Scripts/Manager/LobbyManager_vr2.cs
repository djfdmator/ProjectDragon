using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager_vr2 : MonoBehaviour
{
    public List<GUITestScrollView> gUITestScrollViews;


    public Maintenance maintenance = null;
    public OptionWindow optionWindow = null;

    void Start()
    {
        #region Maintenance
        if (maintenance == null) maintenance = transform.Find(string.Format("Maintenance")).GetComponent<Maintenance>();
        maintenance.gameObject.SetActive(false);
        #endregion

        #region OptionPanel
        if (optionWindow == null) optionWindow = transform.Find(string.Format("PopupWindow/OptionWindow")).GetComponent<OptionWindow>();
        optionWindow.gameObject.SetActive(false);
        #endregion
    }

    void Update()
    {

    }

    
    public void RefreshCharactorData()
    {

    }



    /// <summary>
    /// 배틀씬으로 가기
    /// </summary>
    public void GotoBattle()
    {
        GameManager.Inst.Loading(true);
        #region kks

        //GameManager.Inst.PlayData.currentStage = (int)developerStageSetting + 1;
        #endregion
        //GameObject.Find()
    }

    #region 정비창 정렬 기능
    /// <summary>
    /// 인벤토리를 수집한 순서로 정렬 후 스크롤뷰 갱신
    /// </summary>
    public void inventoryAcquisitionorder()
    {
        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
        {

            if (a.num > b.num)
            {
                return 1;
            }
            else
            {
                return -1;
            }
        });
        UpdateAllScrollview(0);
    }
    /// <summary>
    /// 인베노리를 희귀도순으로 정렬 후 스크롤뷰갱신
    /// </summary>
    public void inventoryClassorder()
    {
        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
        {
            if (a.rarity > b.rarity)
            {
                return 1;
            }
            else if (a.rarity.Equals(b.rarity))
            {
                if (a.Class > b.Class)
                {
                    return 1;
                }
                else if (a.Class.Equals(b.Class))
                {
                    return a.num.CompareTo(b.num);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        });
        UpdateAllScrollview(0);
    }
    /// <summary>
    /// 인벤토리를 클래스별로 정렬후 갱신
    /// </summary>
    public void inventorytypeorder()
    {
        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
        {
            if (a.Class < b.Class)
            {
                return 1;
            }
            else if (a.Class.Equals(b.Class))
            {
                if (a.rarity > b.rarity)
                {
                    return 1;
                }
                else if (a.rarity.Equals(b.rarity))
                {
                    return a.num.CompareTo(b.num);
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                return -1;
            }
        });
        UpdateAllScrollview(0);
    }

    /// <summary>
    /// 여러개의 스크롤뷰가 모두갱신되어야 할때 사용
    /// </summary>
    public void UpdateAllScrollview(int num)
    {
        foreach (GUITestScrollView scrollView in gUITestScrollViews)
        {
            scrollView.EV_UpdateAll();
            scrollView.Setposition(num);
        }

    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maintenance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OpenMaintenance()
    {
        gameObject.SetActive(true);
    }
    public void CloseMaintenance()
    {
        gameObject.SetActive(false);
    }

    //정비 cs에 넣을 거임
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
        //foreach (GUITestScrollView scrollView in gUITestScrollViews)
        //{
        //    scrollView.EV_UpdateAll();
        //    scrollView.Setposition(num);
        //}

    }
    #endregion
}

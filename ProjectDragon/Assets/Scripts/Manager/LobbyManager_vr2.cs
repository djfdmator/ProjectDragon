using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager_vr2 : MonoBehaviour
{
    public List<GUITestScrollView> gUITestScrollViews;

    public Maintenance maintenance = null;
    public OptionWindow optionWindow = null;

    public GameObject equipArmor;
    public GameObject equipWeapon;
    public GameObject equipSkill;
    private GameObject curPopInformation;
    private const int hashCode_Armor = 22446227;
    private const int hashCode_Weapon = -224219278;
    private const int hashCode_Skill = 1312877309;

    void Start()
    {

        #region Maintenance
        if (maintenance == null) maintenance = transform.Find("Maintenance").GetComponent<Maintenance>();
        maintenance.gameObject.SetActive(false);
        #endregion

        #region OptionPanel
        if (optionWindow == null) optionWindow = transform.Find("PopupWindow").Find("OptionWindow").GetComponent<OptionWindow>();
        optionWindow.gameObject.SetActive(false);
        #endregion

        #region EquipItem
        Transform equip = transform.Find("LobbyPanel/TopUI/EquipItem");
        equipArmor = equip.Find("Armor").gameObject;
        equipWeapon = equip.Find("Weapon").gameObject;
        equipSkill = equip.Find("Skill").gameObject;
        #endregion
    }

    public void RefreshCharactorData()
    {

    }

    #region Button
    public void PopupItemInformation(GameObject obj)
    {
        GameObject optionBGI = obj.transform.Find("OptionBGI").gameObject;
        ButtonSound1();

        if (optionBGI.activeSelf)
        {
            optionBGI.SetActive(false);
            curPopInformation = null;
        }
        else
        {
            CloseCurPopInformation();
            curPopInformation = optionBGI;
            curPopInformation.SetActive(true);
            switch (obj.name.GetHashCode())
            {
                case hashCode_Armor:
                    break;
                case hashCode_Weapon:
                    break;
                case hashCode_Skill:
                    break;
            }
        }

    }

    public void CloseCurPopInformation()
    {
        if (curPopInformation != null)
        {
            curPopInformation.SetActive(false);
            curPopInformation = null;
        }
    }

    public void OptionButton()
    {
        ButtonSound1();
        optionWindow.OpenOptionWindow();
    }

    public void MaintenanceButton()
    {
        ButtonSound1();
        maintenance.OpenMaintenance();
    }

    //배틀로 간다.
    public void BattleButton()
    {
        ButtonSound1();
        GameManager.Inst.Loading(true);
    }
    #endregion


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
        foreach (GUITestScrollView scrollView in gUITestScrollViews)
        {
            scrollView.EV_UpdateAll();
            scrollView.Setposition(num);
        }

    }
    #endregion

    #region sound
    public void ButtonSound1()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
    }
    #endregion
}

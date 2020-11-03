using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager_vr2 : MonoBehaviour
{
    public List<GUITestScrollView> gUITestScrollViews;

    public Maintenance maintenance = null;
    public OptionWindow optionWindow = null;
    public StatPanel statPanel = null;

    #region LobbyMain Component
    public UISprite hpProgress;
    public UILabel hpLabel;
    public UILabel mpLabel;
    public UISprite equipArmorImage = null;
    public UISprite equipWeaponImage = null;
    public UISprite equipSkillImage = null;
    private GameObject curPopInformation = null;
    #endregion

    private const int hashCode_Armor = 22446227;
    private const int hashCode_Weapon = -224219278;
    private const int hashCode_Skill = 1312877309;

    void Start()
    {
        if (maintenance == null) maintenance = transform.Find("Maintenance").GetComponent<Maintenance>();
        maintenance.gameObject.SetActive(false);

        if (optionWindow == null) optionWindow = transform.Find("PopupWindow").Find("OptionWindow").GetComponent<OptionWindow>();
        optionWindow.gameObject.SetActive(false);

        if (statPanel == null) statPanel = transform.Find("LobbyPanel/TopUI/Statpanel").GetComponent<StatPanel>();

        #region EquipItem
        Transform equip = transform.Find("LobbyPanel/TopUI/EquipItem");
        if (equipArmorImage == null) equipArmorImage = equip.Find("Armor/Image").GetComponent<UISprite>();
        if (equipWeaponImage == null) equipWeaponImage = equip.Find("Weapon/Image").GetComponent<UISprite>();
        if (equipSkillImage == null) equipSkillImage = equip.Find("Skill/Panel/Image").GetComponent<UISprite>();
        #endregion

        RefreshCharactorData();
    }

    public void RefreshCharactorData()
    {
        //체력 - 현재 체력 초기화 버그 있음
        hpProgress.fillAmount = (float)GameManager.Inst.CurrentHp / GameManager.Inst.MaxHp;
        hpLabel.text = GameManager.Inst.CurrentHp + "/" + GameManager.Inst.MaxHp;
        //마나
        mpLabel.text = GameManager.Inst.Mp.ToString();
        //끼고 있는 장착 아이템 이미지
        equipArmorImage.spriteName = "ArmorIcon_" + GameManager.Inst.CurrentEquipArmor.imageName;
        equipWeaponImage.spriteName = "WeaponIcon_" + GameManager.Inst.CurrentEquipWeapon.imageName;
        equipSkillImage.spriteName = "SkillIcon_" + GameManager.Inst.CurrentSkill.imageName;

        //캐릭터 창 이미지 변경하기
        statPanel.RefreshStatData();
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

            UILabel name = curPopInformation.transform.Find("Name").GetComponent<UILabel>();
            //if(name.text == )
            UILabel value = curPopInformation.transform.Find("Value").GetComponent<UILabel>();
            UILabel rank = curPopInformation.transform.Find("Rank").GetComponent<UILabel>();
            switch (obj.name.GetHashCode())
            {
                case hashCode_Armor:
                    name.text = GameManager.Inst.CurrentEquipArmor.name;
                    value.text = GameManager.Inst.CurrentEquipArmor.description;
                    rank.text = GameManager.Inst.CurrentEquipArmor.rarity_Text;
                    break;
                case hashCode_Weapon:
                    name.text = GameManager.Inst.CurrentEquipWeapon.name;
                    value.text = GameManager.Inst.CurrentEquipWeapon.description;
                    rank.text = GameManager.Inst.CurrentEquipWeapon.rarity_Text;
                    break;
                case hashCode_Skill:
                    name.text = GameManager.Inst.CurrentSkill.name;
                    value.text = GameManager.Inst.CurrentSkill.description;
                    rank.text = GameManager.Inst.CurrentSkill.skillType.ToString();
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

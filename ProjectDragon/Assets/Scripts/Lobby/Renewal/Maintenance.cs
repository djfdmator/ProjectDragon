using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maintenance : MonoBehaviour
{
    public Inventory inventory;

    public UILabel manaCount;

    public GameObject popupWeapon;
    public UILabel weaponName;
    public UILabel weaponRank;
    public UILabel weaponDescription;
    public UILabel weaponAtkMin;
    public UILabel weaponAtkMax;
    public UILabel weaponAtkSpeed;
    public UILabel weaponNuckback;

    public GameObject popupSkill;
    public UILabel skillName;
    public UILabel skillRank;
    public UILabel skillDescription;
    public UILabel skillAtk;
    public UILabel skillMpCost;
    public UILabel skillCoolTime;

    void Start()
    {
        if (inventory == null) inventory = transform.Find("Inventory").GetComponent<Inventory>();
        if (manaCount == null) manaCount = transform.Find("TopUI/ManaCount/Label").GetComponent<UILabel>();

        if (popupWeapon == null) popupWeapon = transform.Find("Popup/Weapon").gameObject;
        if (weaponName == null) weaponName = popupWeapon.transform.Find("Name").GetComponent<UILabel>();
        if (weaponRank == null) weaponRank = popupWeapon.transform.Find("Rank").GetComponent<UILabel>();
        if (weaponDescription == null) weaponDescription = popupWeapon.transform.Find("Description").GetComponent<UILabel>();
        if (weaponAtkMin == null) weaponAtkMin = popupWeapon.transform.Find("AtkMin/Label").GetComponent<UILabel>();
        if (weaponAtkMax == null) weaponAtkMax = popupWeapon.transform.Find("AtkMax/Label").GetComponent<UILabel>();
        if (weaponAtkSpeed == null) weaponAtkSpeed = popupWeapon.transform.Find("AtkSpeed/Label").GetComponent<UILabel>();
        if (weaponNuckback == null) weaponNuckback = popupWeapon.transform.Find("Nuckback/Label").GetComponent<UILabel>();

        if (popupSkill == null) popupSkill = transform.Find("Popup/Skill").gameObject;
        if (skillName == null) skillName = popupSkill.transform.Find("Name").GetComponent<UILabel>();
        if (skillRank == null) skillRank = popupSkill.transform.Find("Rank").GetComponent<UILabel>();
        if (skillDescription == null) skillDescription = popupSkill.transform.Find("Description").GetComponent<UILabel>();
        if (skillAtk == null) skillAtk = popupSkill.transform.Find("Damage/Label").GetComponent<UILabel>();
        if (skillMpCost == null) skillMpCost = popupSkill.transform.Find("Mana/Label").GetComponent<UILabel>();
        if (skillCoolTime == null) skillCoolTime = popupSkill.transform.Find("CoolTime/Label").GetComponent<UILabel>();
    }

    #region Button
    public void OpenMaintenance()
    {
        gameObject.SetActive(true);
        RefreshManaCount();
    }

    public void CloseMaintenance()
    {
        gameObject.SetActive(false);
    }

    public void CurWeaponPopup()
    {
        WeaponPopup(GameManager.Inst.CurrentEquipWeapon);
    }

    public void CurSkillPopup()
    {
        SkillPopup(GameManager.Inst.CurrentSkill);
    }

    public void WeaponPopup(Database.Weapon weapon)
    {
        weaponName.text = weapon.name;
        weaponRank.text = weapon.rarity.ToString();
        weaponDescription.text = weapon.description;
        weaponAtkMin.text = weapon.atk_Min.ToString();
        weaponAtkMax.text = weapon.atk_Max.ToString();
        weaponAtkSpeed.text = weapon.atk_Speed.ToString();
        weaponNuckback.text = weapon.nuckback_Percentage.ToString();

        popupWeapon.SetActive(true);
    }

    public void SkillPopup(Database.Skill skill)
    {
        skillName.text = skill.name;
        skillRank.text = skill.skillType.ToString();
        skillDescription.text = skill.description;
        skillAtk.text = skill.atk.ToString();
        skillMpCost.text = skill.mpCost.ToString();
        skillCoolTime.text = skill.coolTime.ToString();

        popupSkill.SetActive(true);
    }

    public void CloseWeaponPopup()
    {
        popupWeapon.SetActive(false);
    }

    public void CloseSkillPopup()
    {
        popupSkill.SetActive(false);
    }
    #endregion

    public void RefreshManaCount()
    {
        manaCount.text = GameManager.Inst.Mp.ToString();
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

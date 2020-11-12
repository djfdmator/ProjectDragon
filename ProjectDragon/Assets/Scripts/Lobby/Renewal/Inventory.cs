using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public UIGrid grid;
    public GameObject itemCell;

    public NGUIAtlas weaponAtlas;
    public NGUIAtlas ArmorAtlas;

    private UISprite itemImage;
    private UISprite skillImage;
    private Transform atkIcon;
    private UILabel damage;
    private UILabel itemLabel;
    private UILabel skillLabel;
    private Transform equip;
    private Transform _new;

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

    public UIButton equipButton;
    public int curChoiceItme = -1;

    [System.Serializable]
    public struct ItemBtnData
    {
        public GameObject obj;
        public int inventory_index;
        public int skill_index;
        public int weapon_index;
    }

    public List<ItemBtnData> itemBtnDatas = new List<ItemBtnData>();

    void Awake()
    {
        if (grid == null) grid = transform.Find("ItemWindow/Scroll View/Grid").GetComponent<UIGrid>();
        if (itemCell == null) itemCell = Resources.Load("UI/ItemCell") as GameObject;
        itemImage = itemCell.transform.Find("ItemImage/Sprite").GetComponent<UISprite>();
        skillImage = itemCell.transform.Find("SkillImage/Sprite").GetComponent<UISprite>();
        atkIcon = itemCell.transform.Find("AttackIcon");
        damage = itemCell.transform.Find("Damage").GetComponent<UILabel>();
        itemLabel = itemCell.transform.Find("ItemLabel").GetComponent<UILabel>();
        skillLabel = itemCell.transform.Find("SkillLabel").GetComponent<UILabel>();
        equip = itemCell.transform.Find("Equip");
        _new = itemCell.transform.Find("ItemImage/New");

        if (equipButton == null) equipButton = transform.Find("ItemWindow/EquipButton").GetComponent<UIButton>();

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

        SettingItem();

        weaponAtlas = Resources.Load<NGUIAtlas>("UI/WeaponIconAtlas");
        ArmorAtlas = Resources.Load<NGUIAtlas>("UI/ArmorIconAtlas");

        equipButton.isEnabled = false;
    }

    public void SettingItem()
    {
        List<Database.Inventory> inventories = GameManager.Inst.PlayData.inventory;

        if (itemBtnDatas.Count > 0)
        {
            for (int i = 0; i < itemBtnDatas.Count; i++)
            {
                Destroy(itemBtnDatas[i].obj);
            }
            itemBtnDatas.Clear();
        }

        for (int i = 0; i < inventories.Count; i++)
        {
            if (inventories[i].Class == CLASS.갑옷) continue;

            if (Database.Inst.playData.equiWeapon_InventoryNum == i)
            {
                equip.gameObject.SetActive(true);
            }
            else
            {
                equip.gameObject.SetActive(false);
            }


            if (inventories[i].isNew)
            {
                _new.gameObject.SetActive(true);
            }
            else
            {
                _new.gameObject.SetActive(false);
            }

            itemLabel.text = inventories[i].name;

            Database.Weapon weapon = Database.Inst.weapons[inventories[i].DB_Num];
            Database.Skill skill = Database.Inst.skill[weapon.skill_Index];
            //itemImage.atlas = weaponAtlas;
            itemImage.spriteName = "WeaponIcon_" + inventories[i].imageName;
            damage.text = weapon.atk_Min.ToString();
            skillImage.spriteName = "SkillIcon_" + skill.imageName;
            skillLabel.text = skill.name;

            GameObject obj = Instantiate(itemCell, grid.transform);

            UIButton itemImageBtn = obj.transform.Find("ItemImage").GetComponent<UIButton>();
            UIButton skillImageBtn = obj.transform.Find("SkillImage").GetComponent<UIButton>();
            UIButton itemCellBtn = obj.GetComponent<UIButton>();


            itemImageBtn.onClick.Clear();
            skillImageBtn.onClick.Clear();
            itemCellBtn.onClick.Clear();

            EventDelegate itemEvent = new EventDelegate(this, "Event_PopupItem");
            itemEvent.parameters[0].value = weapon.num;
            itemEvent.parameters[1].value = i;

            EventDelegate skillEvent = new EventDelegate(this, "Event_PopupSkill");
            skillEvent.parameters[0].value = weapon.skill_Index;
            skillEvent.parameters[1].value = i;

            EventDelegate itemChoiceBtn = new EventDelegate(this, "Event_ChoiceEquipItem");
            itemChoiceBtn.parameters[0].value = i;

            EventDelegate.Set(itemImageBtn.onClick, itemEvent);
            EventDelegate.Set(skillImageBtn.onClick, skillEvent);
            EventDelegate.Set(itemCellBtn.onClick, itemChoiceBtn);

            ItemBtnData btnData = new ItemBtnData
            {
                inventory_index = i,
                weapon_index = weapon.num,
                skill_index = weapon.skill_Index,
                obj = obj
            };

            itemBtnDatas.Add(btnData);
        }
   
        SortByInventoryNum();
    }

    public void Event_PopupItem(int weaponNum, int inventoryNum)
    {
        WeaponPopup(Database.Inst.weapons[weaponNum]);
        RefreshNewLabel(inventoryNum);
    }

    public void Event_PopupSkill(int skill_Index, int inventoryNum)
    {
        SkillPopup(Database.Inst.skill[skill_Index]);
        RefreshNewLabel(inventoryNum);
    }

    public void Event_ChoiceEquipItem(int inventoryNum)
    {
        curChoiceItme = inventoryNum;
        RefreshNewLabel(inventoryNum);
        if (GameManager.Inst.PlayData.equiWeapon_InventoryNum != inventoryNum) equipButton.isEnabled = true;
        else equipButton.isEnabled = false;
    }

    public void AllRefreshNewLabel()
    {
        for (int i = 0; i < GameManager.Inst.PlayData.inventory.Count; i++)
        {
            GameManager.Inst.PlayData.inventory[i].isNew = false;
        }
    }

    private void RefreshNewLabel(int inventoryNum)
    {
        if (GameManager.Inst.PlayData.inventory[inventoryNum].isNew)
        {
            GameManager.Inst.PlayData.inventory[inventoryNum].isNew = false;
            for (int i = 0; i < itemBtnDatas.Count; i++)
            {
                if (itemBtnDatas[i].inventory_index == inventoryNum)
                {
                    itemBtnDatas[i].obj.transform.Find("ItemImage/New").gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    public void EquipButton()
    {
        GameManager.Inst.PlayerEquipWeapon = GameManager.Inst.PlayData.inventory[curChoiceItme];
        RefreshEquipItem();
        curChoiceItme = -1;
        equipButton.isEnabled = false;
        SortByInventoryNum();
    }

    private void RefreshEquipItem()
    {
        for (int i = 0; i < itemBtnDatas.Count; i++)
        {
            if (GameManager.Inst.PlayData.equiWeapon_InventoryNum == itemBtnDatas[i].inventory_index)
            {
                itemBtnDatas[i].obj.transform.Find("Equip").gameObject.SetActive(true);
                itemBtnDatas[i].obj.GetComponent<UIToggle>().Set(false);
            }
            else
            {
                itemBtnDatas[i].obj.transform.Find("Equip").gameObject.SetActive(false);
            }
        }
    }

    public void SortByInventoryNum()
    {
        for (int i = 0; i < itemBtnDatas.Count; i++)
        {
            if (itemBtnDatas[i].inventory_index == GameManager.Inst.PlayData.equiWeapon_InventoryNum)
            {
                itemBtnDatas[i].obj.transform.localPosition = new Vector3(0.0f, 10.0f, 0.0f);
            }
            else
            {
                itemBtnDatas[i].obj.transform.localPosition = new Vector3(0.0f, -5.0f * (itemBtnDatas[i].inventory_index + 1), 0.0f);
            }
        }
        grid.Reposition();
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

}

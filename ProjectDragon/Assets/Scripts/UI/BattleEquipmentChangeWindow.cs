using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEquipmentChangeWindow : MonoBehaviour
{

    public UIGrid grid;
    public GameObject itemCell;

    #region Current Equip Item
    public UISprite weaponIcon;
    public UISprite skillIcon;
    public UILabel weaponLabel;
    public UILabel atkLabel;
    public UILabel skillNameLabel;
    public UILabel damageLabel;
    public UILabel rankLabel;
    public UILabel coolTimeLabel;
    public UILabel manaLabel;
    #endregion

    #region Choice Item
    public UISprite weaponIcon2;
    public UISprite skillIcon2;
    public UILabel weaponLabel2;
    public UILabel atkLabel2;
    public UILabel skillNameLabel2;
    public UILabel damageLabel2;
    public UILabel rankLabel2;
    public UILabel coolTimeLabel2;
    public UILabel manaLabel2;
    #endregion

    #region Arrow
    private bool isOnArrow = false;
    public Sprite arrowEnableImage;
    public Sprite arrowDisableImage;
    public GameObject arrow;
    #endregion

    public UIButton equipButton;
    private int curChoiceItme = -1;

    #region ItemCell
    private UISprite itemImage;
    private UISprite skillImage;
    private Transform atkIcon;
    private UILabel damage;
    private UILabel itemLabel;
    private UILabel skillLabel;
    private Transform equip;
    private Transform newLabel;
    #endregion

    #region Popup
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
    #endregion

    [System.Serializable]
    public struct ItemBtnData
    {
        public GameObject obj;
        public int inventory_index;
        public int skill_index;
        public int weapon_index;
    }

    public List<ItemBtnData> itemBtnDatas = new List<ItemBtnData>();

    void Start()
    {
        if (grid == null) grid = transform.Find("Inventory/ItemWindow/Scroll View/Grid").GetComponent<UIGrid>();
        if (itemCell == null) itemCell = Resources.Load("UI/ItemCell") as GameObject;
        itemImage = itemCell.transform.Find("ItemImage/Sprite").GetComponent<UISprite>();
        skillImage = itemCell.transform.Find("SkillImage/Sprite").GetComponent<UISprite>();
        atkIcon = itemCell.transform.Find("AttackIcon");
        damage = itemCell.transform.Find("Damage").GetComponent<UILabel>();
        itemLabel = itemCell.transform.Find("ItemLabel").GetComponent<UILabel>();
        skillLabel = itemCell.transform.Find("SkillLabel").GetComponent<UILabel>();
        equip = itemCell.transform.Find("Equip");
        newLabel = itemCell.transform.Find("ItemImage/New");

        if (equipButton == null) equipButton = transform.Find("EquipButton").GetComponent<UIButton>();

        if (popupWeapon == null) popupWeapon = transform.Find("Inventory/Popup/Weapon").gameObject;
        if (weaponName == null) weaponName = popupWeapon.transform.Find("Name").GetComponent<UILabel>();
        if (weaponRank == null) weaponRank = popupWeapon.transform.Find("Rank").GetComponent<UILabel>();
        if (weaponDescription == null) weaponDescription = popupWeapon.transform.Find("Description").GetComponent<UILabel>();
        if (weaponAtkMin == null) weaponAtkMin = popupWeapon.transform.Find("AtkMin/Label").GetComponent<UILabel>();
        if (weaponAtkMax == null) weaponAtkMax = popupWeapon.transform.Find("AtkMax/Label").GetComponent<UILabel>();
        if (weaponAtkSpeed == null) weaponAtkSpeed = popupWeapon.transform.Find("AtkSpeed/Label").GetComponent<UILabel>();
        if (weaponNuckback == null) weaponNuckback = popupWeapon.transform.Find("Nuckback/Label").GetComponent<UILabel>();

        if (popupSkill == null) popupSkill = transform.Find("Inventory/Popup/Skill").gameObject;
        if (skillName == null) skillName = popupSkill.transform.Find("Name").GetComponent<UILabel>();
        if (skillRank == null) skillRank = popupSkill.transform.Find("Rank").GetComponent<UILabel>();
        if (skillDescription == null) skillDescription = popupSkill.transform.Find("Description").GetComponent<UILabel>();
        if (skillAtk == null) skillAtk = popupSkill.transform.Find("Damage/Label").GetComponent<UILabel>();
        if (skillMpCost == null) skillMpCost = popupSkill.transform.Find("Mana/Label").GetComponent<UILabel>();
        if (skillCoolTime == null) skillCoolTime = popupSkill.transform.Find("CoolTime/Label").GetComponent<UILabel>();

        equipButton.isEnabled = false;
    }

    public void SettingItem()
    {
        List<Database.Inventory> inventories = GameManager.Inst.PlayData.inventory;

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

            EventDelegate skillEvent = new EventDelegate(this, "Event_PopupSkill");
            skillEvent.parameters[0].value = weapon.skill_Index;

            EventDelegate itemChoiceBtn = new EventDelegate(this, "Event_ChoiceEquipItem");
            itemChoiceBtn.parameters[0].value = i;

            EventDelegate.Set(itemImageBtn.onClick, itemEvent);
            EventDelegate.Set(skillImageBtn.onClick, skillEvent);
            EventDelegate.Set(itemCellBtn.onClick, itemChoiceBtn);

            ItemBtnData btnData = new ItemBtnData();
            btnData.inventory_index = i;
            btnData.weapon_index = weapon.num;
            btnData.skill_index = weapon.skill_Index;
            btnData.obj = obj;

            itemBtnDatas.Add(btnData);
        }

        SortByInventoryNum();
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

    private void ToggleArrow()
    {
        isOnArrow = !isOnArrow;

        arrow.GetComponent<UI2DSprite>().sprite2D = isOnArrow ? arrowEnableImage : arrowDisableImage;
        arrow.GetComponent<UI2DSpriteAnimation>().enabled = isOnArrow;
    }

    private void RefreshCurEquipItem()
    {
        Database.Weapon weapon = GameManager.Inst.CurrentEquipWeapon;
        Database.Skill skill = GameManager.Inst.CurrentSkill;

        weaponIcon.spriteName = "WeaponIcon_" + weapon.imageName;
        skillIcon.spriteName = "SkillIcon_" + skill.imageName;
        weaponLabel.text = weapon.name;
        atkLabel.text = "공격력 : " + weapon.atk_Min + " ~ " + weapon.atk_Max;
        skillNameLabel.text = skill.name;
        damageLabel.text = "데미지 : " + skill.atk;
        rankLabel.text = weapon.rarity_Text;
        coolTimeLabel.text = skill.coolTime + "s";
        manaLabel.text = skill.mpCost.ToString();
    }

    private void RefreshChoiceItem()
    {
        Database.Weapon weapon = GameManager.Inst.CurrentEquipWeapon;
        Database.Skill skill = GameManager.Inst.CurrentSkill;

        weaponIcon2.spriteName = "WeaponIcon_" + weapon.imageName;
        skillIcon2.spriteName = "SkillIcon_" + skill.imageName;
        weaponLabel2.text = weapon.name;
        atkLabel2.text = "공격력 : " + weapon.atk_Min + " ~ " + weapon.atk_Max;
        skillNameLabel2.text = skill.name;
        damageLabel2.text = "데미지 : " + skill.atk;
        rankLabel2.text = weapon.rarity_Text;
        coolTimeLabel2.text = skill.coolTime + "s";
        manaLabel2.text = skill.mpCost.ToString();
    }
}

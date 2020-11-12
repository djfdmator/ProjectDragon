using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEquipmentChangeWindow : MonoBehaviour
{
    public Inventory inventory;

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
    public GameObject ChoiceItemPanel;
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

    void Start()
    {
        if (inventory == null) inventory = transform.Find("Inventory").GetComponent<Inventory>();

        if (weaponIcon == null) weaponIcon = transform.Find("CurItemPanel/Item/Icon").GetComponent<UISprite>();
        if (skillIcon == null) skillIcon = transform.Find("CurItemPanel/Skill/Icon").GetComponent<UISprite>();
        if (weaponLabel == null) weaponLabel = transform.Find("CurItemPanel/ItemName").GetComponent<UILabel>();
        if (atkLabel == null) atkLabel = transform.Find("CurItemPanel/Atk").GetComponent<UILabel>();
        if (skillNameLabel == null) skillNameLabel = transform.Find("CurItemPanel/SkillName").GetComponent<UILabel>();
        if (damageLabel == null) damageLabel = transform.Find("CurItemPanel/Damage").GetComponent<UILabel>();
        if (rankLabel == null) rankLabel = transform.Find("CurItemPanel/Rank").GetComponent<UILabel>();
        if (coolTimeLabel == null) coolTimeLabel = transform.Find("CurItemPanel/CoolTime/Label").GetComponent<UILabel>();
        if (manaLabel == null) manaLabel = transform.Find("CurItemPanel/Mana/Label").GetComponent<UILabel>();

        if (ChoiceItemPanel == null) ChoiceItemPanel = transform.Find("ChoiceItemPanel/ItemPanel").gameObject;
        if (weaponIcon2 == null) weaponIcon2 = ChoiceItemPanel.transform.Find("Item/Icon").GetComponent<UISprite>();
        if (skillIcon2 == null) skillIcon2 = ChoiceItemPanel.transform.Find("Skill/Icon").GetComponent<UISprite>();
        if (weaponLabel2 == null) weaponLabel2 = ChoiceItemPanel.transform.Find("ItemName").GetComponent<UILabel>();
        if (atkLabel2 == null) atkLabel2 = ChoiceItemPanel.transform.Find("Atk").GetComponent<UILabel>();
        if (skillNameLabel2 == null) skillNameLabel2 = ChoiceItemPanel.transform.Find("SkillName").GetComponent<UILabel>();
        if (damageLabel2 == null) damageLabel2 = ChoiceItemPanel.transform.Find("Damage").GetComponent<UILabel>();
        if (rankLabel2 == null) rankLabel2 = ChoiceItemPanel.transform.Find("Rank").GetComponent<UILabel>();
        if (coolTimeLabel2 == null) coolTimeLabel2 = ChoiceItemPanel.transform.Find("CoolTime/Label").GetComponent<UILabel>();
        if (manaLabel2 == null) manaLabel2 = ChoiceItemPanel.transform.Find("Mana/Label").GetComponent<UILabel>();

        if (arrowEnableImage == null) arrowEnableImage = Resources.Load<Sprite>("UI/EquipmentChange_Enable");
        if (arrowDisableImage == null) arrowDisableImage = Resources.Load<Sprite>("UI/EquipmentChange_Disable");
        if (arrow == null) arrow = transform.Find("Arrow").gameObject;

        ChoiceItemPanel.SetActive(false);
        arrow.GetComponent<UI2DSprite>().sprite2D = isOnArrow ? arrowEnableImage : arrowDisableImage;
        arrow.GetComponent<UI2DSpriteAnimation>().enabled = isOnArrow;

        gameObject.SetActive(false);
    }

    public void Init()
    {
        inventory.SettingItem();
        SettingEvent();
    }

    private void SettingEvent()
    {
        EventDelegate toggleArrow = new EventDelegate(this, "ToggleArrow");
        EventDelegate refreshChoiceItem = new EventDelegate(this, "RefreshChoiceItem");

        Debug.Log(inventory.itemBtnDatas.Count);
        for (int i = 0; i < inventory.itemBtnDatas.Count; i++)
        {
            UIButton button = inventory.itemBtnDatas[i].obj.GetComponent<UIButton>();
            button.onClick.Add(toggleArrow);
            button.onClick.Add(refreshChoiceItem);
        }
    }

    public void ToggleArrow()
    {
        if (GameManager.Inst.PlayData.equiWeapon_InventoryNum == inventory.curChoiceItme)
        {
            isOnArrow = false;
        }
        else
        {
            isOnArrow = true;
        }

        arrow.GetComponent<UI2DSprite>().sprite2D = isOnArrow ? arrowEnableImage : arrowDisableImage;
        arrow.GetComponent<UI2DSpriteAnimation>().enabled = isOnArrow;
    }

    public void RefreshCurEquipItem()
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

        isOnArrow = false;
        arrow.GetComponent<UI2DSprite>().sprite2D = arrowDisableImage;
        arrow.GetComponent<UI2DSpriteAnimation>().enabled = isOnArrow;
        ChoiceItemPanel.SetActive(false);
    }

    public void RefreshChoiceItem()
    {
        Database.Weapon weapon = Database.Inst.weapons[GameManager.Inst.PlayData.inventory[inventory.curChoiceItme].DB_Num];
        Database.Skill skill = Database.Inst.skill[GameManager.Inst.PlayData.inventory[inventory.curChoiceItme].skill_Index];

        weaponIcon2.spriteName = "WeaponIcon_" + weapon.imageName;
        skillIcon2.spriteName = "SkillIcon_" + skill.imageName;
        weaponLabel2.text = weapon.name;
        atkLabel2.text = "공격력 : " + weapon.atk_Min + " ~ " + weapon.atk_Max;
        skillNameLabel2.text = skill.name;
        damageLabel2.text = "데미지 : " + skill.atk;
        rankLabel2.text = weapon.rarity_Text;
        coolTimeLabel2.text = skill.coolTime + "s";
        manaLabel2.text = skill.mpCost.ToString();
        ChoiceItemPanel.SetActive(true);

        //if (GameManager.Inst.PlayData.equiWeapon_InventoryNum == inventory.curChoiceItme)
        //{
        //    ChoiceItemPanel.SetActive(false);
        //}
        //else
        //{
        //    Database.Weapon weapon = Database.Inst.weapons[GameManager.Inst.PlayData.inventory[inventory.curChoiceItme].DB_Num];
        //    Database.Skill skill = Database.Inst.skill[GameManager.Inst.PlayData.inventory[inventory.curChoiceItme].skill_Index];

        //    weaponIcon2.spriteName = "WeaponIcon_" + weapon.imageName;
        //    skillIcon2.spriteName = "SkillIcon_" + skill.imageName;
        //    weaponLabel2.text = weapon.name;
        //    atkLabel2.text = "공격력 : " + weapon.atk_Min + " ~ " + weapon.atk_Max;
        //    skillNameLabel2.text = skill.name;
        //    damageLabel2.text = "데미지 : " + skill.atk;
        //    rankLabel2.text = weapon.rarity_Text;
        //    coolTimeLabel2.text = skill.coolTime + "s";
        //    manaLabel2.text = skill.mpCost.ToString();
        //    ChoiceItemPanel.SetActive(true);
        //}
    }

    public void CurrentWeaponPopup()
    {
        inventory.WeaponPopup(GameManager.Inst.CurrentEquipWeapon);
    }

    public void CurrentSkillPopup()
    {
        inventory.SkillPopup(GameManager.Inst.CurrentSkill);
    }

    public void ChoiceWeaponPopup()
    {
        inventory.WeaponPopup(Database.Inst.weapons[GameManager.Inst.PlayData.inventory[inventory.curChoiceItme].DB_Num]);
    }

    public void ChoiceSkillPopup()
    {
        inventory.SkillPopup(Database.Inst.skill[GameManager.Inst.PlayData.inventory[inventory.curChoiceItme].skill_Index]);
    }

    public void NextStageButton()
    {
        Time.timeScale = 1.0f;
        GameManager.Inst.Loading(true);
    }
}

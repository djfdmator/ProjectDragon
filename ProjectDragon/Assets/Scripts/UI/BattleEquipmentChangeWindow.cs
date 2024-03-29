﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEquipmentChangeWindow : MonoBehaviour
{
    public Inventory inventory;

    public UIButton leftButton;
    public UIButton rightButton;
    public Transform title_EquipmentChange;
    public Transform title_Enhancement;

    private GameObject uiRoot;
    private BattleStatus battleStatus;

    #region Enhancement
    public GameObject enhancementObj;

    public UILabel enhance_ItemName;
    public UISprite enhance_ItemImage;
    public UISprite enhance_Dialogue;
    public UIButton enhance_Button;
    public UILabel enhance_Cost;

    public GameObject enhance_ItemInfo;
    public UILabel enhance_CurItemAtk;
    public UILabel enhance_CurSkillAtk;
    public UILabel enhance_NextItemAtk;
    public UILabel enhance_NextSkillAtk;

    public BoxCollider doingEnhance;
    public UISprite enhance_effectSprite;
    public ParticleSystem parti_Effect;
    public float playTime = 2.0f;
    public AnimationCurve effectCurve;

    public UIPanel EnhanceSuccess;

    #endregion

    #region Equipment Change
    public GameObject equipmentChangeObj;

    #region Current Equip Item
    public Transform curItemPanel;
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

    #endregion

    void Start()
    {
        if (uiRoot == null) uiRoot = GameObject.Find("UI Root").gameObject;
        if (battleStatus == null) battleStatus = uiRoot.transform.Find("Status").GetComponent<BattleStatus>();

        if (inventory == null) inventory = transform.Find("Inventory").GetComponent<Inventory>();

        if (leftButton == null) leftButton = transform.Find("ToggleButton/Left").GetComponent<UIButton>();
        if (rightButton == null) rightButton = transform.Find("ToggleButton/Right").GetComponent<UIButton>();
        if (title_EquipmentChange == null) title_EquipmentChange = transform.Find("Title_EquipmentChange");
        if (title_Enhancement == null) title_Enhancement = transform.Find("Title_Enhancement");

        #region Enhancement
        if (enhancementObj == null) enhancementObj = transform.Find("Enhancement").gameObject;

        if (enhance_ItemName == null) enhance_ItemName = enhancementObj.transform.Find("ItemNameLabel").GetComponent<UILabel>();
        if (enhance_Dialogue == null) enhance_Dialogue = enhancementObj.transform.Find("Dialogue").GetComponent<UISprite>();

        if (enhance_ItemImage == null) enhance_ItemImage = enhancementObj.transform.Find("ItemSlot/ItemSlot/ItemImage").GetComponent<UISprite>();
        if (enhance_effectSprite == null) enhance_effectSprite = enhancementObj.transform.Find("MagicCircleEffect").GetComponent<UISprite>();
        if (parti_Effect == null) parti_Effect = enhance_effectSprite.transform.Find("Panel/Effect").GetComponent<ParticleSystem>();

        if (enhance_Button == null) enhance_Button = enhancementObj.transform.Find("EnhanceButton").GetComponent<UIButton>();
        if (enhance_Cost == null) enhance_Cost = enhance_Button.transform.Find("CostLabel").GetComponent<UILabel>();

        if (enhance_ItemInfo == null) enhance_ItemInfo = enhancementObj.transform.Find("ItemInfo").gameObject;
        if (enhance_CurItemAtk == null) enhance_CurItemAtk = enhance_ItemInfo.transform.Find("Before/Atk").GetComponent<UILabel>();
        if (enhance_CurSkillAtk == null) enhance_CurSkillAtk = enhance_ItemInfo.transform.Find("Before/SkillAtk").GetComponent<UILabel>();
        if (enhance_NextItemAtk == null) enhance_NextItemAtk = enhance_ItemInfo.transform.Find("After/Atk").GetComponent<UILabel>();
        if (enhance_NextSkillAtk == null) enhance_NextSkillAtk = enhance_ItemInfo.transform.Find("After/SkillAtk").GetComponent<UILabel>();

        if (doingEnhance == null) doingEnhance = enhancementObj.transform.Find("DoingEnhance").GetComponent<BoxCollider>();
        if (EnhanceSuccess == null) EnhanceSuccess = enhancementObj.transform.Find("EnhanceSuccess").GetComponent<UIPanel>();

        #endregion

        #region Equipment Change
        if (equipmentChangeObj == null) equipmentChangeObj = transform.Find("EquipmentChange").gameObject;

        if (curItemPanel == null) curItemPanel = equipmentChangeObj.transform.Find("CurItemPanel");
        if (weaponIcon == null) weaponIcon = curItemPanel.Find("Icon").GetComponent<UISprite>();
        if (skillIcon == null) skillIcon = curItemPanel.Find("Icon").GetComponent<UISprite>();
        if (weaponLabel == null) weaponLabel = curItemPanel.Find("ItemName").GetComponent<UILabel>();
        if (atkLabel == null) atkLabel = curItemPanel.Find("Atk").GetComponent<UILabel>();
        if (skillNameLabel == null) skillNameLabel = curItemPanel.Find("SkillName").GetComponent<UILabel>();
        if (damageLabel == null) damageLabel = curItemPanel.Find("Damage").GetComponent<UILabel>();
        if (rankLabel == null) rankLabel = curItemPanel.Find("Rank").GetComponent<UILabel>();
        if (coolTimeLabel == null) coolTimeLabel = curItemPanel.Find("CoolTime/Label").GetComponent<UILabel>();
        if (manaLabel == null) manaLabel = curItemPanel.Find("Mana/Label").GetComponent<UILabel>();

        if (ChoiceItemPanel == null) ChoiceItemPanel = equipmentChangeObj.transform.Find("ChoiceItemPanel/ItemPanel").gameObject;
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
        #endregion

        leftButton.isEnabled = false;
        enhancementObj.SetActive(false);
        title_Enhancement.GetComponent<UISprite>().alpha = 0.5f;

        gameObject.SetActive(false);

        parti_Effect.gameObject.SetActive(false);
        EnhanceSuccess.gameObject.SetActive(false);
    }

    public void Init(int acheiveMP)
    {
        SoundManager.Inst.Ds_BGMPlayerDB(3);
        inventory.SettingItem();
        RefreshCurEquipItem();
        SettingEquipChangeEvent();
        gameObject.SetActive(true);

        battleStatus.ChangeMpLabel(GameManager.Inst.Mp, GameManager.Inst.Mp + acheiveMP);
        battleStatus.addMpLabel.text = "+0";
        GameManager.Inst.Mp += acheiveMP;
    }

    private void SettingEquipChangeEvent()
    {
        EventDelegate toggleArrow = new EventDelegate(this, "ToggleArrow");
        EventDelegate refreshChoiceItem = new EventDelegate(this, "RefreshChoiceItem");

        for (int i = 0; i < inventory.itemBtnDatas.Count; i++)
        {
            UIButton button = inventory.itemBtnDatas[i].obj.GetComponent<UIButton>();
            if (button.onClick.Count >= 2) button.onClick.RemoveAt(1);
            button.onClick.Add(toggleArrow);
            button.onClick.Add(refreshChoiceItem);
        }
    }

    public void ToggleArrow()
    {
        if (GameManager.Inst.PlayData.equiWeapon_InventoryNum == inventory.curChoiceItem)
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
        Database.Inventory item = GameManager.Inst.PlayerEquipWeapon;

        weaponIcon.spriteName = "WeaponIcon_" + weapon.imageName;
        skillIcon.spriteName = "SkillIcon_" + skill.imageName;
        weaponLabel.text = weapon.name + (item.enhanceLevel == 0 ? "" : " +" + item.enhanceLevel.ToString());
        atkLabel.text = "공격력 : " + (weapon.atk_Min + weapon.enhanceValue * item.enhanceLevel) + " ~ " + (weapon.atk_Max + weapon.enhanceValue * item.enhanceLevel);
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
        Database.Weapon weapon = Database.Inst.weapons[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].DB_Num];
        Database.Skill skill = Database.Inst.skill[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].skill_Index];
        Database.Inventory item = GameManager.Inst.PlayData.inventory[inventory.curChoiceItem];

        weaponIcon2.spriteName = "WeaponIcon_" + weapon.imageName;
        skillIcon2.spriteName = "SkillIcon_" + skill.imageName;
        weaponLabel2.text = weapon.name + (item.enhanceLevel == 0 ? "" : " +" + item.enhanceLevel.ToString());
        atkLabel2.text = "공격력 : " + (weapon.atk_Min + weapon.enhanceValue * item.enhanceLevel) + " ~ " + (weapon.atk_Max + weapon.enhanceValue * item.enhanceLevel);
        skillNameLabel2.text = skill.name;
        damageLabel2.text = "데미지 : " + (skill.atk + skill.enhanceValue * item.enhanceLevel);
        rankLabel2.text = weapon.rarity_Text;
        coolTimeLabel2.text = skill.coolTime + "s";
        manaLabel2.text = skill.mpCost.ToString();
        ChoiceItemPanel.SetActive(true);
    }

    public void CurrentWeaponPopup()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        inventory.WeaponPopup(GameManager.Inst.CurrentEquipWeapon, GameManager.Inst.PlayerEquipWeapon);
    }

    public void CurrentSkillPopup()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        inventory.SkillPopup(GameManager.Inst.CurrentSkill, GameManager.Inst.PlayerEquipWeapon);
    }

    public void ChoiceWeaponPopup()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        inventory.WeaponPopup(Database.Inst.weapons[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].DB_Num], GameManager.Inst.PlayData.inventory[inventory.curChoiceItem]);
    }

    public void ChoiceSkillPopup()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        inventory.SkillPopup(Database.Inst.skill[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].skill_Index], GameManager.Inst.PlayData.inventory[inventory.curChoiceItem]);
    }

    public void NextStageButton()
    {
        //도감,업적 저장
        GameManager.Inst.Save_Encyclopedia_Monster_Table();
        GameManager.Inst.Save_Encyclopedia_Weapon_Table();
        GameManager.Inst.Save_Achievement_Table();

        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);

        GameManager.Inst.Loading(true);
    }

    public void RightButton()
    {
        StopAllCoroutines();
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        StartCoroutine(TweenPosition(0.2f, title_EquipmentChange.gameObject, new Vector2(-400.0f, 450.0f)));
        StartCoroutine(TweenPosition(0.2f, title_Enhancement.gameObject, new Vector2(0.0f, 450.0f)));
        StartCoroutine(TweenScale(0.2f, title_EquipmentChange.gameObject, new Vector2(0.5f, 0.5f)));
        StartCoroutine(TweenScale(0.2f, title_Enhancement.gameObject, new Vector2(1.0f, 1.0f)));
        title_EquipmentChange.GetComponent<UISprite>().alpha = 0.5f;
        title_Enhancement.GetComponent<UISprite>().alpha = 1.0f;
        leftButton.isEnabled = true;
        rightButton.isEnabled = false;

        inventory.EquipButtonSetActive(false);
        InitEnhancement();
        inventory.ReleaseItemChoiceEffect();

        //인벤토리 아이템 선택 이벤트 변경
        SettingEnhanceEvent();

        equipmentChangeObj.SetActive(false);
        enhancementObj.SetActive(true);
    }

    public void LeftButton()
    {
        StopAllCoroutines();
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        StartCoroutine(TweenPosition(0.2f, title_EquipmentChange.gameObject, new Vector2(0.0f, 450.0f)));
        StartCoroutine(TweenPosition(0.2f, title_Enhancement.gameObject, new Vector2(400.0f, 450.0f)));
        StartCoroutine(TweenScale(0.2f, title_EquipmentChange.gameObject, new Vector2(1.0f, 1.0f)));
        StartCoroutine(TweenScale(0.2f, title_Enhancement.gameObject, new Vector2(0.5f, 0.5f)));
        title_EquipmentChange.GetComponent<UISprite>().alpha = 1.0f;
        title_Enhancement.GetComponent<UISprite>().alpha = 0.5f;
        leftButton.isEnabled = false;
        rightButton.isEnabled = true;

        inventory.EquipButtonSetActive(true);
        inventory.equipButton.isEnabled = false;
        RefreshCurEquipItem();
        inventory.ReleaseItemChoiceEffect();

        //인벤토리 아이템 선택 이벤트 변경
        SettingEquipChangeEvent();

        equipmentChangeObj.SetActive(true);
        enhancementObj.SetActive(false);
    }

    public void InitEnhancement()
    {
        enhance_ItemName.text = "";
        enhance_ItemImage.spriteName = " ";
        enhance_Dialogue.spriteName = "Descriptionwindow_Deactivate";
        enhance_Dialogue.transform.localPosition = new Vector3(-445.0f, -220.0f, 0.0f);
        enhance_ItemInfo.SetActive(false);
        enhance_Cost.text = "";
        enhance_Button.isEnabled = false;
        doingEnhance.enabled = false;
    }

    public void SettingEnhanceEvent()
    {
        EventDelegate clickEvent = new EventDelegate(this, "ItemChoiceEvent");

        for (int i = 0; i < inventory.itemBtnDatas.Count; i++)
        {
            UIButton button = inventory.itemBtnDatas[i].obj.GetComponent<UIButton>();
            clickEvent.parameters[0].value = inventory.itemBtnDatas[i].inventory_index;
            button.onClick.RemoveAt(2);
            button.onClick.RemoveAt(1);
            button.onClick.Add(clickEvent);
        }
    }

    public void ItemChoiceEvent(int inventoyNum)
    {
        Database.Weapon weapon = Database.Inst.weapons[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].DB_Num];
        Database.Skill skill = Database.Inst.skill[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].skill_Index];
        Database.Inventory choiceItem = GameManager.Inst.PlayData.inventory[inventory.curChoiceItem];

        enhance_ItemName.text = weapon.name + (choiceItem.enhanceLevel == 0 ? "" : " +" + choiceItem.enhanceLevel.ToString());
        enhance_ItemImage.spriteName = "WeaponIcon_" + weapon.imageName;
        //소유 마나량에 따라 처리 다르게 하기
        enhance_Cost.text = (400 + choiceItem.enhanceLevel * 50).ToString();
        if (GameManager.Inst.Mp >= 400 + choiceItem.enhanceLevel * 50)
        {
            enhance_Dialogue.spriteName = "Descriptionwindow_Activate";
            enhance_Button.isEnabled = true;
        }
        else
        {
            enhance_Dialogue.spriteName = "Descriptionwindow_Error";
            enhance_Button.isEnabled = false;
        }
        enhance_Dialogue.transform.localPosition = new Vector3(-445.0f, -350.0f, 0.0f);


        enhance_CurItemAtk.text = (weapon.atk_Min + weapon.enhanceValue * choiceItem.enhanceLevel) + " ~ " + (weapon.atk_Max + weapon.enhanceValue * choiceItem.enhanceLevel);
        enhance_CurSkillAtk.text = (skill.atk + skill.enhanceValue * choiceItem.enhanceLevel).ToString();
        enhance_NextItemAtk.text = (weapon.atk_Min + weapon.enhanceValue * (choiceItem.enhanceLevel + 1)) + " ~ " + (weapon.atk_Max + weapon.enhanceValue * (choiceItem.enhanceLevel + 1));
        enhance_NextSkillAtk.text = (skill.atk + skill.enhanceValue * (choiceItem.enhanceLevel + 1)).ToString();
        enhance_ItemInfo.SetActive(true);
    }

    private IEnumerator TweenPosition(float playTime, GameObject obj, Vector2 targetPos)
    {
        Vector2 from = obj.transform.localPosition;
        float time = 0.0f;
        while (time <= playTime)
        {
            time += Time.deltaTime;
            obj.transform.localPosition = Vector2.Lerp(from, targetPos, time / playTime);
            yield return null;
        }
    }

    private IEnumerator TweenScale(float playTime, GameObject obj, Vector2 targetSize)
    {
        Vector2 from = obj.transform.localScale;
        float time = 0.0f;
        while (time <= playTime)
        {
            time += Time.deltaTime;
            obj.transform.localScale = Vector2.Lerp(from, targetSize, time / playTime);
            yield return null;
        }
    }

    public void EnhanceButton()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        StartCoroutine(TweenFillRotationAC());
    }

    private IEnumerator TweenFillRotationAC()
    {
        float time = 0.0f;
        doingEnhance.enabled = true;
        enhance_effectSprite.fillAmount = 0.0f;
        parti_Effect.gameObject.SetActive(true);
        parti_Effect.Play();
        SoundManager.Inst.EffectPlayerDB(36, this.gameObject);
        while (time <= playTime)
        {
            enhance_effectSprite.fillAmount = effectCurve.Evaluate(time / playTime);
            time += Time.deltaTime;
            yield return null;
        }

        Database.Inventory choiceItem = GameManager.Inst.PlayData.inventory[inventory.curChoiceItem];
        int enhanceCost = 400 + choiceItem.enhanceLevel * 50;
        battleStatus.ChangeMpLabel(GameManager.Inst.Mp, GameManager.Inst.Mp - enhanceCost);
        GameManager.Inst.Mp -= enhanceCost;
        choiceItem.enhanceLevel++;
        if (GameManager.Inst.PlayData.equiWeapon_InventoryNum == inventory.curChoiceItem)
        {
            GameManager.Inst.PlayerEquipWeapon = choiceItem;
        }

        //[강화] 업적 달성
        switch(choiceItem.enhanceLevel)
        {
            case 1:
                GameManager.Inst.PlayData.collection.AchievementCollection(9);
                break;
            case 5:
                GameManager.Inst.PlayData.collection.AchievementCollection(10);
                break;
            case 10:
                GameManager.Inst.PlayData.collection.AchievementCollection(11);
                break;
        }

        ItemChoiceEvent(inventory.curChoiceItem);
        inventory.RefreshEnhanceCellData();

        SoundManager.Inst.EffectPlayerDB(38, this.gameObject);
        yield return new WaitForSeconds(0.5f);

        enhance_effectSprite.fillAmount = 0.0f;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(SuccessEvent());
    }

    private IEnumerator SuccessEvent()
    {
        UISprite image = EnhanceSuccess.transform.Find("Sprite").GetComponent<UISprite>();
        image.alpha = 1.0f;
        EnhanceSuccess.SetRect(0.0f, 0.0f, 638.0f, 1.0f);
        EnhanceSuccess.gameObject.SetActive(true);
        SoundManager.Inst.EffectPlayerDB(39, this.gameObject);

        float temp = 255.0f;
        float time = 0.0f;
        float playTime = 1.0f;
        while (time <= playTime)
        {
            time += Time.deltaTime;
            EnhanceSuccess.SetRect(0.0f, 0.0f, 638.0f, 1.0f + temp * time / playTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        time = 0.0f;
        playTime = 0.5f;
        while (time <= playTime)
        {
            time += Time.deltaTime;
            image.alpha = 1.0f - time / playTime;
            yield return null;
        }

        EnhanceSuccess.gameObject.SetActive(false);
        parti_Effect.gameObject.SetActive(false);
        doingEnhance.enabled = false;
    }
}
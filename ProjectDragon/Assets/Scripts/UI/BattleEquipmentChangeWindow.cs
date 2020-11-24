using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleEquipmentChangeWindow : MonoBehaviour
{
    public Inventory inventory;

    public UIButton leftButton;
    public UIButton rightButton;
    public Transform title_EquipmentChange;
    public Transform title_Enhancement;

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


    public UISprite enhance_effectSprite;
    public float playTime = 2.0f;
    public AnimationCurve effectCurve;

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
        if (inventory == null) inventory = transform.Find("Inventory").GetComponent<Inventory>();

        if (leftButton == null) leftButton = transform.Find("ToggleButton/Left").GetComponent<UIButton>();
        if (rightButton == null) rightButton = transform.Find("ToggleButton/Right").GetComponent<UIButton>();
        if (title_EquipmentChange == null) title_EquipmentChange = transform.Find("Title_EquipmentChange");
        if (title_Enhancement == null) title_Enhancement = transform.Find("Title_Enhancement");

        #region Enhancement
        if (enhancementObj == null) enhancementObj = transform.Find("Enhancement").gameObject;

        if (enhance_ItemName == null) enhance_ItemName = enhancementObj.transform.Find("ItemNameLabel").GetComponent<UILabel>();
        if (enhance_Dialogue == null) enhance_Dialogue = enhancementObj.transform.Find("Dialogue").GetComponent<UISprite>();

        if (enhance_ItemImage == null) enhance_ItemImage = enhancementObj.transform.Find("MagicCircle/ItemSlot/ItemImage").GetComponent<UISprite>();
        if (enhance_effectSprite == null) enhance_effectSprite = enhancementObj.transform.Find("MagicCircle/MagicCircleEffect").GetComponent<UISprite>();

        if (enhance_Button == null) enhance_Button = enhancementObj.transform.Find("EnhanceButton").GetComponent<UIButton>();
        if (enhance_Cost == null) enhance_Cost = enhance_Button.transform.Find("CostLabel").GetComponent<UILabel>();

        if (enhance_ItemInfo == null) enhance_ItemInfo = enhancementObj.transform.Find("ItemInfo").gameObject;
        if (enhance_CurItemAtk == null) enhance_CurItemAtk = enhance_ItemInfo.transform.Find("Before/Atk").GetComponent<UILabel>();
        if (enhance_CurSkillAtk == null) enhance_CurSkillAtk = enhance_ItemInfo.transform.Find("Before/SkillAtk").GetComponent<UILabel>();
        if (enhance_CurItemAtk == null) enhance_CurItemAtk = enhance_ItemInfo.transform.Find("After/Atk").GetComponent<UILabel>();
        if (enhance_CurSkillAtk == null) enhance_CurSkillAtk = enhance_ItemInfo.transform.Find("After/SkillAtk").GetComponent<UILabel>();
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
    }

    public void Init()
    {
        SoundManager.Inst.Ds_BGMPlayerDB(3);
        inventory.SettingItem();
        RefreshCurEquipItem();
        SettingEquipChangeEvent();
        gameObject.SetActive(true);
    }

    private void SettingEquipChangeEvent()
    {
        EventDelegate toggleArrow = new EventDelegate(this, "ToggleArrow");
        EventDelegate refreshChoiceItem = new EventDelegate(this, "RefreshChoiceItem");

        for (int i = 0; i < inventory.itemBtnDatas.Count; i++)
        {
            UIButton button = inventory.itemBtnDatas[i].obj.GetComponent<UIButton>();
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
        Database.Weapon weapon = Database.Inst.weapons[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].DB_Num];
        Database.Skill skill = Database.Inst.skill[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].skill_Index];

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
    }

    public void CurrentWeaponPopup()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        inventory.WeaponPopup(GameManager.Inst.CurrentEquipWeapon);
    }

    public void CurrentSkillPopup()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        inventory.SkillPopup(GameManager.Inst.CurrentSkill);
    }

    public void ChoiceWeaponPopup()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        inventory.WeaponPopup(Database.Inst.weapons[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].DB_Num]);
    }

    public void ChoiceSkillPopup()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        inventory.SkillPopup(Database.Inst.skill[GameManager.Inst.PlayData.inventory[inventory.curChoiceItem].skill_Index]);
    }

    public void NextStageButton()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        GameManager.Inst.Loading(true);
    }

    public void RightButton()
    {
        StopAllCoroutines();
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

        equipmentChangeObj.SetActive(false);
        enhancementObj.SetActive(true);
    }

    public void LeftButton()
    {
        StopAllCoroutines();
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

        enhance_ItemName.text = weapon.name;
        enhance_ItemImage.spriteName = weapon.imageName;
        //소유 마나량에 따라 처리 다르게 하기
        enhance_Cost.text = "0";
        if(GameManager.Inst.Mp >= 0)
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

        enhance_CurItemAtk.text = weapon.atk_Min + " ~ " + weapon.atk_Max;
        enhance_CurSkillAtk.text = skill.atk.ToString();
        enhance_NextItemAtk.text = weapon.atk_Min + " ~ " + weapon.atk_Max;
        enhance_NextSkillAtk.text = skill.atk.ToString();
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
        StartCoroutine(TweenFillRotationAC());
    }

    private IEnumerator TweenFillRotationAC()
    {
        float time = 0.0f;
        enhance_effectSprite.fillAmount = 0.0f;
        while (time <= playTime)
        {
            enhance_effectSprite.fillAmount = effectCurve.Evaluate(time / playTime);
            time += Time.deltaTime;
            yield return null;
        }
    }
}
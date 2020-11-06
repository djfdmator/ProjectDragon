using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maintenance : MonoBehaviour
{
    public Inventory inventory;

    public UILabel manaCount;

    public UISprite curArmorImage;
    public UISprite curWeaponImage;
    public UISprite curSkillImage;

    public LobbyManager_vr2 lobbyManager;
    public StatPanel statPanel;

    void Start()
    {
        if (inventory == null) inventory = transform.Find("Inventory").GetComponent<Inventory>();
        if (manaCount == null) manaCount = transform.Find("TopUI/ManaCount/Label").GetComponent<UILabel>();

        if (curArmorImage == null) curArmorImage = transform.Find("CurEquipItem/Armor/Image").GetComponent<UISprite>();
        if (curWeaponImage == null) curWeaponImage = transform.Find("CurEquipItem/Weapon/Image").GetComponent<UISprite>();
        if (curSkillImage == null) curSkillImage = transform.Find("CurEquipItem/Skill/Image").GetComponent<UISprite>();

        lobbyManager = transform.parent.GetComponent<LobbyManager_vr2>();
        statPanel = transform.Find("Statpanel").GetComponent<StatPanel>();
    }

    #region Button
    public void OpenMaintenance()
    {
        gameObject.SetActive(true);
        RefreshManaCount();
        RefreshEquipItem();
    }

    public void CloseMaintenance()
    {
        gameObject.SetActive(false);
    }

    public void CurWeaponPopup()
    {
        inventory.WeaponPopup(GameManager.Inst.CurrentEquipWeapon);
    }

    public void CurSkillPopup()
    {
        inventory.SkillPopup(GameManager.Inst.CurrentSkill);
    }

    public void EquipButton()
    {
        inventory.EquipButton();
        RefreshEquipItem();
        statPanel.RefreshStatData();
        lobbyManager.RefreshCharactorData();
    }

    #endregion

    public void RefreshEquipItem()
    {
        curArmorImage.spriteName = "ArmorIcon_" + GameManager.Inst.CurrentEquipArmor.imageName;
        curWeaponImage.spriteName = "WeaponIcon_" + GameManager.Inst.CurrentEquipWeapon.imageName;
        curSkillImage.spriteName = "SkillIcon_" + GameManager.Inst.CurrentSkill.imageName;
    }
    public void RefreshManaCount()
    {
        manaCount.text = GameManager.Inst.Mp.ToString();
    }

}

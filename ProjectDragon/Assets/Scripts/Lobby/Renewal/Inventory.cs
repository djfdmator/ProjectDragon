﻿using System.Collections;
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

    void Start()
    {
        if (grid == null) grid = transform.Find("ItemWindow/Scroll View/Grid").GetComponent<UIGrid>();
        if (itemCell == null) itemCell = Resources.Load("UI/ItemCell") as GameObject;
        itemImage = itemCell.transform.Find("ItemImage/Sprite").GetComponent<UISprite>();
        skillImage = itemCell.transform.Find("SkillImage/Panel/Sprite").GetComponent<UISprite>();
        atkIcon = itemCell.transform.Find("AttackIcon");
        damage = itemCell.transform.Find("Damage").GetComponent<UILabel>();
        itemLabel = itemCell.transform.Find("ItemLabel").GetComponent<UILabel>();
        skillLabel = itemCell.transform.Find("SkillLabel").GetComponent<UILabel>();
        equip = itemCell.transform.Find("Equip");

        weaponAtlas = Resources.Load<NGUIAtlas>("UI/WeaponIconAtlas");
        ArmorAtlas = Resources.Load<NGUIAtlas>("UI/ArmorIconAtlas");

        SettingItem();
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

            itemLabel.text = inventories[i].name;

            Database.Weapon weapon = Database.Inst.weapons[inventories[i].DB_Num];
            Database.Skill skill = Database.Inst.skill[weapon.skill_Index];
            //itemImage.atlas = weaponAtlas;
            itemImage.spriteName = "WeaponIcon_" + inventories[i].imageName;
            damage.text = weapon.atk_Min.ToString();
            skillImage.spriteName = "SkillIcon_" + skill.imageName;
            skillLabel.text = skill.name;

            Instantiate(itemCell, grid.transform);
        }
    }
}

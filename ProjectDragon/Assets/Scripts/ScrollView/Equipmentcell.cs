using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class Equipmentcell : UIReuseScrollViewCell
{
    public UISprite equipIcon, itemIcon;
    public UISprite rarity, activeIcon;
    public UILabel Itemname, Itemvalue;
    public EuipmentcellData cell;
    public bool change = false;
    GameObject click;
    float pressyposition;
    private void Start()
    {
        UIEventListener.Get(gameObject).onPress += Buttonpress;
    }
    public override void UpdateData(IReuseCellData _CellData)
    {

        EuipmentcellData item = _CellData as EuipmentcellData;
        if (item == null)
            return;
        cell = item;

        if (cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum) || cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
        {
            equipIcon.gameObject.SetActive(true);
        }
        else
        {
            equipIcon.gameObject.SetActive(false);
        }
        Itemname.text = cell.name;
        itemIcon.spriteName = cell.imageName;
        if (!item.Class.Equals(CLASS.갑옷))
        {
            Itemvalue.text = "공격력:" + cell.stat.ToString();
            activeIcon.gameObject.SetActive(true);
        }
        else
        {
            Itemvalue.text = "체력:\t" + cell.stat.ToString();
            activeIcon.gameObject.SetActive(false);
        }
        switch (LobbyManager.inst.lobbystate)
        {
            case LobbyState.Nomal:
                if (LobbyManager.inst.selectData.Equals(cell.inventoryNum))
                {
                    gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "정비 UI 방어구선택 터치선택";
                    gameObject.transform.Find("StatBGI/ActiveBGI").GetComponent<UISprite>().spriteName = "정비 UI 무기선택 스킬칸 터치선택";
                }
                else
                {
                    gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "정비 UI 방어구선택 기본상태";
                    gameObject.transform.Find("StatBGI/ActiveBGI").GetComponent<UISprite>().spriteName = "정비 UI 무기선택 스킬칸 기본상태";
                }

                break;
            case LobbyState.Decomposition:
                bool checkselect = false;
                for (int i = 0; i < LobbyManager.inst.Selecteditem.Count; i++)
                {
                    if (LobbyManager.inst.Selecteditem[i].Equals(cell.inventoryNum))
                    {
                        gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "정비 UI 방어구선택 터치선택";
                        gameObject.transform.Find("StatBGI/ActiveBGI").GetComponent<UISprite>().spriteName = "정비 UI 무기선택 스킬칸 기본상태";
                        checkselect = true;
                        break;
                    }
                }
                if (!checkselect)
                {
                    gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "정비 UI 방어구선택 기본상태";
                    gameObject.transform.Find("StatBGI/ActiveBGI").GetComponent<UISprite>().spriteName = "정비 UI 무기선택 스킬칸 기본상태";
                }
                break;
            default:
                break;
        }
    }
    public void ItemActive()
    {

        Database.Inventory inventory = Database.Inst.playData.inventory[cell.inventoryNum];

        LobbyManager.inst.equipBGI.SetActive(true);
        LobbyManager.inst.ItemInfo.SetActive(true);
        UIRect.AnchorPoint anchorPoint = LobbyManager.inst.ItemInfo.GetComponent<UISprite>().bottomAnchor;

        for (int i = 0; i < LobbyManager.inst.ItemInfo.transform.childCount; i++)
        {
            //ItemInfo.GetComponentsInChildren<UILabel>()[i].UpdateAnchors();
            //Debug.Log(i + "(" + LobbyManager.inst.ItemInfo.transform.childCount + ")");
            LobbyManager.inst.ItemInfo.transform.GetChild(i).gameObject.SetActive(false);
            //ItemInfo.GetComponentsInChildren<UILabel>()[i].gameObject.SetActive(false);
        }
        Color rarecolor = Color.white;
        if (inventory.Class.Equals(CLASS.갑옷))
        {
            switch (Database.Inst.armors[inventory.DB_Num].rarity)
            {
                case RARITY.노말:
                    rarecolor = Color.white;
                    break;
                case RARITY.유니크:
                    rarecolor = Color.blue;
                    break;
                case RARITY.레전드:
                    rarecolor = Color.yellow;
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (Database.Inst.weapons[inventory.DB_Num].rarity)
            {
                case RARITY.노말:
                    rarecolor = Color.white;
                    break;
                case RARITY.유니크:
                    rarecolor = Color.blue;
                    break;
                case RARITY.레전드:
                    rarecolor = Color.yellow;
                    break;
                default:
                    break;
            }
        }
        LobbyManager.inst.ItemInfo.transform.Find("name").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().color = rarecolor;

        LobbyManager.inst.ItemInfo.transform.Find("discription").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("discription").gameObject.GetComponent<UILabel>().text = "";

        LobbyManager.inst.ItemInfo.transform.Find("rare").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().color = rarecolor;

        if (!inventory.Class.Equals(CLASS.갑옷))
        {

            LobbyManager.inst.ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].name.ToString();

            LobbyManager.inst.ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].rarity.ToString();

            LobbyManager.inst.ItemInfo.transform.Find("mindamage").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("mindamage").gameObject.GetComponent<UILabel>().text = "최소 데미지";
            LobbyManager.inst.ItemInfo.transform.Find("mindamagenum").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("mindamagenum").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].atk_Min.ToString();

            LobbyManager.inst.ItemInfo.transform.Find("maxdamage").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("maxdamage").GetComponent<UILabel>().text = "최대 데미지";
            LobbyManager.inst.ItemInfo.transform.Find("maxdamagenum").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("maxdamagenum").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].atk_Max.ToString();

            LobbyManager.inst.ItemInfo.transform.Find("attackspeed").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("attackspeed").GetComponent<UILabel>().text = "공격 속도";
            LobbyManager.inst.ItemInfo.transform.Find("attackspeednum").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("attackspeednum").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].atk_Speed.ToString();

            LobbyManager.inst.ItemInfo.transform.Find("knockback").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("knockback").GetComponent<UILabel>().text = "넉백 확률";
            LobbyManager.inst.ItemInfo.transform.Find("knockbacknum").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("knockbacknum").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].nuckback_Percentage.ToString();

            LobbyManager.inst.ItemInfo.transform.Find("optiondiscription").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("optiondiscription").gameObject.GetComponent<UILabel>().text = "";


            anchorPoint.absolute = 250;
        }
        else
        {
            LobbyManager.inst.ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().text = Database.Inst.armors[inventory.DB_Num].name.ToString();

            LobbyManager.inst.ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().text = Database.Inst.armors[inventory.DB_Num].rarity.ToString();

            LobbyManager.inst.ItemInfo.transform.Find("mindamage").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("mindamage").gameObject.GetComponent<UILabel>().text = "추가 체력";
            LobbyManager.inst.ItemInfo.transform.Find("mindamagenum").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("mindamagenum").gameObject.GetComponent<UILabel>().text = Database.Inst.armors[inventory.DB_Num].hp.ToString();

            LobbyManager.inst.ItemInfo.transform.Find("optiondiscription").gameObject.SetActive(true);
            LobbyManager.inst.ItemInfo.transform.Find("optiondiscription").gameObject.GetComponent<UILabel>().text = "";
            anchorPoint.absolute = 300;
        }
        LobbyManager.inst.ItemInfo.GetComponent<UISprite>().bottomAnchor = anchorPoint;
        LobbyManager.inst.ItemInfo.GetComponent<UISprite>().UpdateAnchors();
        for (int i = 0; i < LobbyManager.inst.ItemInfo.GetComponentsInChildren<UILabel>().Length; i++)
        {
            //ItemInfo.transform.GetChild(i).GetComponent<UILabel>().UpdateAnchors();
            //ItemInfo.GetComponentsInChildren<UILabel>()[i].gameObject.SetActive(false);
        }

    }
    public void SkillActive()
    {
        LobbyManager.inst.equipBGI.SetActive(true);
        Database.Skill Skill = Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index];
        Color rarecolor = Color.white;
        LobbyManager.inst.ItemInfo.SetActive(true);
        UIRect.AnchorPoint anchorPoint = LobbyManager.inst.ItemInfo.GetComponent<UISprite>().bottomAnchor;

        for (int i = 0; i < LobbyManager.inst.ItemInfo.transform.childCount; i++)
        {
            LobbyManager.inst.ItemInfo.transform.GetChild(i).gameObject.SetActive(false);
        }

        LobbyManager.inst.ItemInfo.transform.Find("name").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().text = Skill.name;
        LobbyManager.inst.ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().color = rarecolor;

        LobbyManager.inst.ItemInfo.transform.Find("discription").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("discription").gameObject.GetComponent<UILabel>().text = Skill.skill_Duration.ToString();

        LobbyManager.inst.ItemInfo.transform.Find("rare").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().text = Skill.skillType.ToString();
        LobbyManager.inst.ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().color = rarecolor;

        LobbyManager.inst.ItemInfo.transform.Find("mindamage").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("mindamage").gameObject.GetComponent<UILabel>().text = "데미지";
        LobbyManager.inst.ItemInfo.transform.Find("mindamagenum").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("mindamagenum").gameObject.GetComponent<UILabel>().text = Skill.atk.ToString();

        LobbyManager.inst.ItemInfo.transform.Find("maxdamage").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("maxdamage").GetComponent<UILabel>().text = "쿨타임";
        LobbyManager.inst.ItemInfo.transform.Find("maxdamagenum").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("maxdamagenum").gameObject.GetComponent<UILabel>().text = Skill.coolTime.ToString();

        LobbyManager.inst.ItemInfo.transform.Find("attackspeed").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("attackspeed").GetComponent<UILabel>().text = "소모 마나";
        LobbyManager.inst.ItemInfo.transform.Find("attackspeednum").gameObject.SetActive(true);
        LobbyManager.inst.ItemInfo.transform.Find("attackspeednum").gameObject.GetComponent<UILabel>().text = Skill.mpCost.ToString();


        anchorPoint.absolute = 250;
        LobbyManager.inst.ItemInfo.GetComponent<UISprite>().bottomAnchor = anchorPoint;
        LobbyManager.inst.ItemInfo.GetComponent<UISprite>().UpdateAnchors();
    }
    //public void ChangeEquip(GameObject panel, Database.Inventory data, float stat)
    //{
    //    EquipWeaponIcon(panel.transform.Find("EquipBGI").gameObject, data);
    //    panel.transform.Find("EquipItemname").GetComponent<UILabel>().text = data.name;
    //    panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().text = data.rarity.ToString();
    //    panel.transform.Find("EquipItemclass").GetComponent<UILabel>().text = string.Format("종류: {0}", data.Class.ToString());
    //    panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}", Database.Inst.weapons[data.DB_Num].atk_Min);
    //    if (!data.Class.Equals(CLASS.갑옷))
    //    {
    //        panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}", Database.Inst.weapons[data.DB_Num].atk_Min);
    //        GameObject ActiveSkill;
    //        ActiveSkill = panel.transform.Find("ActiveSkill").gameObject;
    //        ActiveSkill.SetActive(true);
    //        ActiveSkill.transform.Find("Activename").GetComponent<UILabel>().text = string.Format("액티브: {0}", Database.Inst.skill[data.skill_Index].name);
    //        ActiveSkill.transform.Find("ActiveDamage").GetComponent<UILabel>().text = string.Format("공격력: {0}%", Database.Inst.skill[data.skill_Index].atk);
    //        ActiveSkill.transform.Find("ActiveRange").GetComponent<UILabel>().text = string.Format("범위: {0}", Database.Inst.skill[data.skill_Index].skill_Range);
    //        ActiveSkill.transform.Find("Activemana").GetComponent<UILabel>().text = string.Format("마나: {0}", Database.Inst.skill[data.skill_Index].skill_Duration);
    //        ActiveSkill.transform.Find("Activecooltime").GetComponent<UILabel>().text = string.Format("쿨타임: {0}", Database.Inst.skill[data.skill_Index].coolTime);
    //        ActiveSkill.transform.Find("ActiveBGI").Find("ActiveIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[data.skill_Index].imageName;
    //    }
    //    else
    //    {
    //        panel.transform.Find("AttackDamage").GetComponent<UILabel>().text = string.Format("체력: {0}", Database.Inst.armors[data.DB_Num].hp);
    //        GameObject ActiveSkill;
    //        ActiveSkill = panel.transform.Find("ActiveSkill").gameObject;
    //        ActiveSkill.SetActive(false);
    //    }
    //    if (!stat.Equals(-1))
    //    {
    //        panel.transform.Find("AttackDamage").Find("StatGap").gameObject.SetActive(true);
    //        if (Database.Inst.weapons[data.DB_Num].atk_Min > stat)
    //        {
    //            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.blue;
    //            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = string.Format("(+{0})", Mathf.Abs(Database.Inst.weapons[data.DB_Num].atk_Min - stat).ToString());

    //        }
    //        else if (Database.Inst.weapons[data.DB_Num].atk_Min.Equals(stat))
    //        {
    //            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.gray;
    //            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = "0";
    //        }
    //        else
    //        {
    //            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().color = Color.red;
    //            panel.transform.Find("AttackDamage").Find("StatGap").GetComponent<UILabel>().text = string.Format("(-{0})", Mathf.Abs(Database.Inst.weapons[data.DB_Num].atk_Min - stat).ToString());
    //        }
    //    }
    //    else
    //    {
    //        panel.transform.Find("AttackDamage").Find("StatGap").gameObject.SetActive(false);
    //    }
    //    switch (data.rarity)
    //    {
    //        case RARITY.노말:
    //            panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = Color.gray;
    //            break;
    //        case RARITY.유니크:
    //            panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = new Color(156, 91, 025);
    //            break;
    //        case RARITY.레전드:
    //            panel.transform.Find("EquipItem").Find("EquipItemrare").GetComponent<UILabel>().color = Color.yellow;
    //            break;
    //        default:
    //            break;
    //    }
    //}
    //public void EquipWeaponIcon(GameObject IconObject, Database.Inventory data)
    //{
    //    LobbyManager.inst.ChangeItemIcon(IconObject, data);
    //}
    public void Buttonpress(GameObject sender, bool state)
    {
        if (sender.Equals(gameObject) && !state)
        {
            //Debug.Log(Mathf.Abs(pressyposition - gameObject.transform.position.y));
            if (Mathf.Abs(pressyposition -gameObject.transform.position.y)<0.1f)
            {
                //Debug.Log("cellclick");
                SoundManager.Inst.Ds_EffectPlayerDB(1);
                switch (LobbyManager.inst.lobbystate)
                {
                    case LobbyState.Nomal:
                        LobbyManager.inst.selectData = cell.inventoryNum;
                        if (cell.inventoryNum.Equals(Database.Inst.playData.equiArmor_InventoryNum) || cell.inventoryNum.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
                        {
                            LobbyManager.inst.equipanel.transform.Find("ItemWindow/FilterButtonBGI").GetComponent<UIButton>().isEnabled = false;
                            gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "정비 UI 방어구선택 기본상태";
                        }
                        else
                        {
                            LobbyManager.inst.equipanel.transform.Find("ItemWindow/FilterButtonBGI").GetComponent<UIButton>().isEnabled = true;
                            gameObject.transform.Find("StatBGI").GetComponent<UISprite>().spriteName = "정비 UI 방어구선택 터치선택";

                        }
                        break;
                    case LobbyState.Enchant:

                        break;
                    case LobbyState.Decomposition:
                        bool check = true;
                        int manavalue = 0;
                        LobbyManager.inst.equipanel.transform.Find("ItemWindow/FilterButtonBGI").GetComponent<UIButton>().isEnabled = false;
                        if (LobbyManager.inst.Selecteditem.Count < 7)
                        {

                            for (int i = 0; i < LobbyManager.inst.Selecteditem.Count; i++)
                            {
                                if (LobbyManager.inst.Selecteditem[i].Equals(cell.inventoryNum))
                                {
                                    if (LobbyManager.inst.Selecteditem.Count.Equals(1))
                                    {
                                        LobbyManager.inst.Selecteditem.Clear();

                                    }
                                    else
                                    {
                                        LobbyManager.inst.Selecteditem.Remove(LobbyManager.inst.Selecteditem[i]);
                                    }
                                    check = false;
                                    break;
                                }
                            }
                            if (check)
                            {
                                LobbyManager.inst.Selecteditem.Add(cell.inventoryNum);

                            }
                            for (int i = 0; i < LobbyManager.inst.Selecteditem.Count; i++)
                            {
                                //Debug.Log(string.Format(LobbyManager.inst.Selecteditem.Count + "," + check));
                                LobbyManager.inst.DecompositionPanel.transform.Find(string.Format("DecompositionInfo/Itemplace{0}/ItemIcon", i)).GetComponent<UISprite>().spriteName = GameManager.Inst.PlayData.inventory[LobbyManager.inst.Selecteditem[i]].imageName;
                                manavalue += GameManager.Inst.PlayData.inventory[LobbyManager.inst.Selecteditem[i]].itemValue;
                                LobbyManager.inst.equipanel.transform.Find("ItemWindow/FilterButtonBGI").GetComponent<UIButton>().isEnabled = true;
                            }
                            LobbyManager.inst.DecompositionCountLabel.GetComponent<UILabel>().text = manavalue.ToString();
                            if(LobbyManager.inst.Selecteditem.Count<7)
                            LobbyManager.inst.DecompositionPanel.transform.Find(string.Format("DecompositionInfo/Itemplace{0}/ItemIcon", LobbyManager.inst.Selecteditem.Count)).GetComponent<UISprite>().spriteName = "크리스탈";
                        }
                        break;
                    default:
                        break;
                }
                if (cell.Index > 1)
                {
                    LobbyManager.inst.UpdateAllScrollview(cell.Index - 1);

                }
                else
                {
                    LobbyManager.inst.UpdateAllScrollview(0);
                }
            }
        }
        else if (sender.Equals(gameObject) && state)
        {
            pressyposition = Mathf.Abs(gameObject.transform.position.y);
        }
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
// 메인 클래스. 여기서 grid에 데이터를 추가시켜준다.
public class GUITestScrollView : MonoBehaviour
{
    public int count;
    private UIReuseGrid grid;
    public List<Database.Inventory> inventories;
    public UIReuseGrid Grid
    {
        get
        {
            return grid;
        }
    }

    void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
    }

    void Start()
    {
        LobbyManager.inst.gUITestScrollViews.Add(gameObject.GetComponent<GUITestScrollView>());

        inventories = Database.Inst.playData.inventory;
        // 임의의 데이터가 생성해서 gird에 추가시켜둔다.
        // ItemCellData 는 IReuseCellData 상속받아서 구현된 데이터 클래스다.
        count = inventories.Count;
        for (int i = 0; i < count; ++i)
        {
            EuipmentcellData cell = new EuipmentcellData();
            if (!inventories[i].Class.ToString().Equals(CLASS.갑옷.ToString()))
            {
                cell.DB_Num = inventories[i].DB_Num;
                cell.imageName = inventories[i].imageName;
                cell.itemValue = inventories[i].itemValue;
                cell.Class = inventories[i].Class;
                cell.name = inventories[i].name;
                cell.inventoryNum = inventories[i].num;
                cell.rarity = inventories[i].rarity;
                //cell.isLock = inventories[i].isLock;
                if (!cell.Class.Equals(CLASS.갑옷))
                {
                    cell.stat = Database.Inst.weapons[cell.DB_Num].atk_Min;
                    cell.optionnum = Database.Inst.playData.inventory[cell.DB_Num].option_Index;
                }
                else
                {
                    cell.stat = Database.Inst.armors[cell.DB_Num].hp;
                }
                grid.AddItem(cell, false);
                Debug.Log(grid.DataList.Count+cell.Class.ToString());
            }
        }
        grid.UpdateAllCellData();
        EV_UpdateAll();
    }
    public void EV_UpdateAll()
    {
        grid.ClearItem(true);
        count = inventories.Count;
        for (int i = 0; i < count; ++i)
        {
            EuipmentcellData cell = new EuipmentcellData();
            switch (LobbyManager.inst.lobbystate)
            {
                case LobbyState.Nomal:
                    if ((!LobbyManager.inst.itemclassselect.Equals(ItemState.갑옷) && !inventories[i].Class.Equals(CLASS.갑옷)) || (LobbyManager.inst.itemclassselect.Equals(ItemState.갑옷) && inventories[i].Class.Equals(CLASS.갑옷)))
                    {

                        cell.DB_Num = inventories[i].DB_Num;
                        cell.imageName = inventories[i].imageName;
                        cell.itemValue = inventories[i].itemValue;
                        cell.Class = inventories[i].Class;
                        cell.name = inventories[i].name;
                        cell.inventoryNum = inventories[i].num;
                        cell.rarity = inventories[i].rarity;
                        if (!cell.Class.Equals(CLASS.갑옷))
                        {
                            cell.discription = Database.Inst.weapons[inventories[i].DB_Num].description;
                        }
                        //cell.isLock = inventories[i].isLock;
                        if (!cell.Class.Equals(CLASS.갑옷))
                        {
                            cell.stat = Database.Inst.weapons[cell.DB_Num].atk_Min;
                        }
                        else
                        {
                            cell.stat = Database.Inst.armors[cell.DB_Num].hp;
                        }
                        grid.AddItem(cell, false);
                    }
                    break;
                case LobbyState.Enchant:
                    break;
                case LobbyState.Decomposition:
                    if (!inventories[i].num.Equals(Database.Inst.playData.equiArmor_InventoryNum) && !inventories[i].num.Equals(Database.Inst.playData.equiWeapon_InventoryNum))
                    {
                        if ((!LobbyManager.inst.itemclassselect.Equals(ItemState.갑옷) && !inventories[i].Class.Equals(CLASS.갑옷)) || (LobbyManager.inst.itemclassselect.Equals(ItemState.갑옷) && inventories[i].Class.Equals(CLASS.갑옷)))
                        {

                            cell.DB_Num = inventories[i].DB_Num;
                            cell.imageName = inventories[i].imageName;
                            cell.itemValue = inventories[i].itemValue;
                            cell.Class = inventories[i].Class;
                            cell.name = inventories[i].name;
                            cell.inventoryNum = inventories[i].num;
                            cell.rarity = inventories[i].rarity;
                            if (!cell.Class.Equals(CLASS.갑옷))
                            {
                                cell.discription = Database.Inst.weapons[inventories[i].DB_Num].description;
                            }
                            //cell.isLock = inventories[i].isLock;
                            if (!cell.Class.Equals(CLASS.갑옷))
                            {
                                cell.stat = Database.Inst.weapons[cell.DB_Num].atk_Min;
                            }
                            else
                            {
                                cell.stat = Database.Inst.armors[cell.DB_Num].hp;
                            }
                            grid.AddItem(cell, false);
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        grid.UpdateAllCellData();
    }
    #region Event
    public void EV_Add()
    {
        EuipmentcellData cell = new EuipmentcellData();
        cell.DB_Num = grid.MaxCellData;
        cell.imageName = string.Format("name:{0}", cell.DB_Num);
        grid.AddItem(cell, true);
    }

    public void EV_Remove()
    {
        grid.RemoveItem(grid.GetCellData(0), true);
    }

    public void EV_RemoveAll()
    {
        grid.ClearItem(true);
    }
    public void Setposition(int num)
    {
        grid.SetPostion(num);
    }
    #endregion
}

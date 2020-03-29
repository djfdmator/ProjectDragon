using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIItemScrollView : MonoBehaviour
{
    ///public int countItem;
    public List<Database.Inventory> AcheiveItemList;
    UIReuseGrid grid;
    public UIReuseGrid Grid
    {
        get
        {
            return grid;
        }
    }

    private void Awake()
    {
        grid = GetComponentInChildren<UIReuseGrid>();
        AcheiveItemList= Database.Inst.playData.inventory;

    }
    private void Start()
    {
        GetResultItem();
    }
    public void GetResultItem()
    {
        for(int i = 0; i < AcheiveItemList.Count; ++i)
        {

            EuipmentcellData cell = new EuipmentcellData();
            cell.DB_Num = AcheiveItemList[i].DB_Num;
            cell.imageName = AcheiveItemList[i].imageName;
            cell.itemValue = AcheiveItemList[i].itemValue;
            cell.Class = AcheiveItemList[i].Class;
            cell.name = AcheiveItemList[i].name;
            cell.inventoryNum = AcheiveItemList[i].num;
            cell.rarity = AcheiveItemList[i].rarity;
            //// 임의의 데이터가 생성해서 gird에 추가시켜둔다.
            //// ItemCellData 는 IReuseCellData 상속받아서 구현된 데이터 클래스다.

           // EuipmentcellData cell = new EuipmentcellData();
            grid.AddItem(cell, false);

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
    #endregion
}

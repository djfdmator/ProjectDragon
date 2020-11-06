using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIItemScrollView : MonoBehaviour
{
    ///public int countItem;
    public List<Database.Inventory> ResulttemList;
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

    }
    public void GetResultItem()
    {
        ResulttemList = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().items;

        for(int i = 0; i < ResulttemList.Count; ++i)
        {

            EuipmentcellData cell = new EuipmentcellData();
            cell.DB_Num = ResulttemList[i].DB_Num;
            cell.imageName = ResulttemList[i].imageName;
            cell.itemValue = ResulttemList[i].itemValue;
            cell.Class = ResulttemList[i].Class;
            cell.name = ResulttemList[i].name;
            cell.inventoryNum = ResulttemList[i].num;
            cell.rarity = ResulttemList[i].rarity;
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

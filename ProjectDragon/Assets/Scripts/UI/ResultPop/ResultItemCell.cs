using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultItemCell : UIReuseScrollViewCell
{
    public UISprite itemIcon;


    public override void UpdateData(IReuseCellData CellData)
    {
        EuipmentcellData item = CellData as EuipmentcellData;
        if (item == null)
            return;
        itemIcon.spriteName = item.imageName;
    }
}

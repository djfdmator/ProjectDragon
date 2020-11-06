using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWarpItemContent : UIWrapContent
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void UpdateItem(Transform item, int index)
    {
        base.UpdateItem(item, index);
    }
    public void AddItem(Transform item, int index)
    {
        UpdateItem(item, index);
    }
}

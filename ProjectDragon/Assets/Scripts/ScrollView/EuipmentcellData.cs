using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class EuipmentcellData : IReuseCellData
{
    #region CellData
    private int m_index;
    public int Index { get { return m_index; } set { m_index = value; } }
    private int m_inventoryNum;
    public int inventoryNum { get { return m_inventoryNum; } set { m_inventoryNum = value; } }
    private int m_DB_Num;
    public int DB_Num { get { return m_DB_Num; } set { m_DB_Num = value; } }
    private string m_name;
    public string name { get { return m_name; } set { m_name = value; } }
    private float m_stat;
    public float stat{ get { return m_stat; } set { m_stat = value; } }
    private bool m_isLock;
    public bool isLock { get { return m_isLock; }set { m_isLock = value; } }
    private int m_itemValue;
    public int itemValue { get { return m_itemValue; } set { m_itemValue = value; } }
    private RARITY m_rarity;
    public RARITY rarity { get { return m_rarity; } set { m_rarity = value; } }
    private CLASS m_Class;
    public CLASS Class { get { return m_Class; } set { m_Class = value; } }
    private string m_imageName;
    public string imageName { get { return m_imageName; } set { m_imageName = value; } }
    private int m_skill_index;
    public int skill_index { get { return m_skill_index; } set { m_skill_index = value; } }
    private string m_discription;
    public string discription { get { return m_discription; } set { m_discription = value; } }
    private int m_optionnum;
    public int optionnum { get { return m_optionnum; } set { m_optionnum = value; } }
    #endregion
}

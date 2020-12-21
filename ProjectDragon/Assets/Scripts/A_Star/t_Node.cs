using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class t_Node : IHeapItem<t_Node>
{

    public int gridX;
    public int gridY;

    public bool Walkable;
    public bool IsObject = false;
    public Vector3 Pos;

    public t_Node Parent;

    public int gCost;
    public int hCost;

    public int FCost { get { return gCost + hCost; } }

    public t_Node(bool _walkable, Vector3 _pos,int _gridX,int _gridY)
    {
        Walkable = _walkable;
        Pos = _pos;
        gridX = _gridX;
        gridY = _gridY;
    }


    //Heap
    int heapIndex;
    public int HeapIndex
    {
        get
        {
            return heapIndex;
        }
        set
        {
            heapIndex = value;
        }
    }
    public int CompareTo(t_Node nodeToCompare)
    {
        int compare = FCost.CompareTo(nodeToCompare.FCost);
        if (compare == 0)
        {
            compare = hCost.CompareTo(nodeToCompare.hCost);
        }
        return -compare;
    }
}

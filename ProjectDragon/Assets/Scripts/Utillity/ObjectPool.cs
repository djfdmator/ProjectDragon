

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool Instance
    {
        get { return instance; }
        set { }
    }
    static ObjectPool instance = null;


    public List<PooledObject> objectPool = new List<PooledObject>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        for (int i= 0; i < objectPool.Count; ++i)
        {
            objectPool[i].Initialize(transform);
        }
    }

    public bool PushToPool(string itemName, GameObject item, Transform parent = null)
    {
        PooledObject pool = GetPoolItem(itemName);
        if (pool == null)
        {
            return false;
        }

        pool.PushToPool(item, parent == null ? transform : parent);
        return true;
    }

    PooledObject GetPoolItem(string itemName)
    {
        for (int i= 0;i<objectPool.Count;++i)
        {
            if(objectPool[i].poolItemName.Equals(itemName))
            {
                return objectPool[i];
            }
        }
        return null;
    }

    public GameObject PopFromPool(string itemName, Transform parent = null)
    {
        PooledObject pool = GetPoolItem(itemName);
        if(pool==null)
        {
            return null;
        }

        return pool.PopFromPool(parent);
    }



}

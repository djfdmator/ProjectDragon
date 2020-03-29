
// ==============================================================
// Map Objects parent class
// The parent of the interactive map object
// 
//  AUTHOR: Kim Dong Ha
// CREATED: 2019-12-31
// UPDATED: 2020-01-02
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    protected virtual int Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0)
            {
                //astar.RescanPath(GetComponent<BoxCollider2D>());
                StartCoroutine(vfx);
            }
        }
    }
    protected int hp = 1;

    public string objName = string.Empty;
    protected IEnumerator vfx;

    protected virtual void Awake()
    {
        vfx = Effect();
    }

    public virtual void HpChanged(int _damage)
    {
        Hp -= _damage;
    }

    protected virtual IEnumerator Effect()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<PolygonCollider2D>().enabled = false;

        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }
}

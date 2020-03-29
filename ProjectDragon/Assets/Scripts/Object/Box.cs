
// ==============================================================
// Box Object
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2020-01-02
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MapObject
{
    protected override int Hp
    {
        get => base.Hp;
        set
        {
            base.Hp = value;
            if(hp <= 0)
            {
                DropItem();
            }
        }
    }
    public float itemDropPercentage = 0.0f;
    public GameObject particle;

    protected override void Awake()
    {
        base.Awake();
        objName = "Box";

        particle = GetComponentInChildren<ParticleSystem>().gameObject;
        particle.SetActive(false);
    }

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("Player"))
        {
            Hp--;
        }
    }

    protected override IEnumerator Effect()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        particle.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        Destroy(gameObject);
    }

    void DropItem()
    {
        GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().DropItem(transform.position);
    }
}

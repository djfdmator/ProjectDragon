
// ==============================================================
// Bush Object
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2020-01-02
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bush : MapObject
{

    protected override void Awake()
    {
        base.Awake();
        objName = "Bush";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Skill"))
        {
            StartCoroutine(vfx);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.CompareTag("Enemy") || obj.CompareTag("Player"))
        {
            //데미지 주기
            //obj.GetComponent<Character>().HPChanged(1, 1, true);
        }
    }
}

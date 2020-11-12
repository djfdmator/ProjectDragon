
// ==============================================================
// Root Object
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2020-01-02
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Root : MapObject
{
    public enum Size
    {
        Small,
        Middle
    }
    public enum State
    {
        Phase1,
        Phase2,
        Destroy
    }
    public Size rootSize = Size.Small;
    public State rootState = State.Phase1;
    public Sprite crackedRoot;

    private bool isSpriteChange = false;
    private int halfHP;

    protected override void Awake()
    {
        base.Awake();
        objName = "Root";
        hp = 100;
        halfHP = hp / 2;
        if (crackedRoot == null)
        {
            ResourceLoad();
        }
    }

    //깨졌을때의 이미지 로드
    private void ResourceLoad()
    {
        string resourceName = "Object/Sprite/";
        switch (rootSize)
        {
            case Size.Small:
                resourceName += "crackedRoot_Small";
                break;
            case Size.Middle:
                resourceName += "crackedRoot_Middle";
                break;
        }
        crackedRoot = Resources.Load<Sprite>(resourceName);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;

        if (obj.CompareTag("Skill"))
        {
            hp -= 50;

            if (hp <= halfHP && rootState.Equals(State.Phase1))
            {
                ChangeSprite();
            }
            else if (hp <= 0.0f && rootState.Equals(State.Phase2))
            {
                rootState = State.Destroy;
                StartCoroutine(vfx);
            }
        }
    }

    // 밑동이 부서지는 연출
    private void ChangeSprite()
    {
        GetComponent<SpriteRenderer>().sprite = crackedRoot;
        isSpriteChange = true;
        rootState = State.Phase2;
    }
}

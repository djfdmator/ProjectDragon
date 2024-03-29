﻿
// ==============================================================
// Portal Object
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-16
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public bool IsPortalOn
    {
        get { return isPortalOn; }
        set
        {
            isPortalOn = value;
            if(isPortalOn == true)
            {
                PortalActivation();
            }
        }
    }

    //private Sprite image_Activate;
    private RoomManager RoomManager;
    private bool isPortalOn = false;
    private Sprite portalOnImage;

    public bool isPortalActivate = false;

    private void Start()
    {
        RoomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        portalOnImage = Resources.Load<Sprite>("Object/Sprite/Activate");
    }

    //포탈에 부딪혔을 경우 미니맵 확대
    private void OnTriggerEnter2D(Collider2D collision)
    {
#if UNITY_EDITOR
        Debug.Log(isPortalOn);
#endif
        if(collision.CompareTag("Player") && isPortalOn)
        {
            isPortalActivate = true;
            RoomManager.MiniMapMaximalize();
        }
    }
    //포탈 범위에서 나가면 미니맵 축소
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isPortalOn)
        {
            isPortalActivate = false;
            RoomManager.MiniMapMinimalize();
        }
    }

    private void PortalActivation()
    {
        GetComponent<SpriteRenderer>().sprite = portalOnImage;
    }
}

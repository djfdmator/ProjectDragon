
// ==============================================================
// Async Loader
// Load the map asynchronously.
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-16
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Stair : MonoBehaviour
{
    public bool IsOpen
    {
        get { return isOpen; }
        set
        {
            isOpen = value;
            if (isOpen)
            {
                StartCoroutine(OpenStair());
            }
        }
    }

    public RoomManager roomManager;
    public bool isOpen = false;
    private Player player;

    private void Awake()
    {
        roomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        GetComponent<BoxCollider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen && collision.gameObject.CompareTag("Player"))
        {
            //게임 매니저에게 스테이지 지나갔다고 전달
            //게임 현재 위치 저장
            //다음 스테이지 시작
            roomManager.OpenResultPop(false);
        }
    }

    IEnumerator OpenStair()
    {
        #region 문 열리는 연출
        gameObject.transform.Find("OpenSprite").gameObject.SetActive(true);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;
        #endregion
        yield return null;
    }
}

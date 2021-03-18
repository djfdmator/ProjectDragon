
// ==============================================================
// Door Object
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-16
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorName
{
    North, South, West, East
}

public class Door : MonoBehaviour
{
    public DoorName Name;
    private RoomManager RoomManager;
    public Animator animator;
    private bool isOpened = false;

    private void Awake()
    {
        RoomManager = GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>();
        animator = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        animator.SetBool("isClear", isOpened);
    }

    public void OpenDoor()
    {
        isOpened = true;
        animator.SetBool("isClear", isOpened);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RoomManager.Fade());
            //이동 구현
            int playerPosX = RoomManager.player_PosX;
            int playerPosY = RoomManager.player_PosY;
            int gridSizeX_Cen = RoomManager.gridSizeX_Cen;
            int gridSizeY_Cen = RoomManager.gridSizeY_Cen;

            float aspect_MoveRangeX = 0.0f;
            float aspect_MoveRangeY = 0.0f;

            RoomManager.Map_Data[playerPosX + gridSizeX_Cen, playerPosY + gridSizeY_Cen].SetActive(false);

            //문 방향에 따른 이동 방향, 거리 계산
            switch (Name)
            {
                case DoorName.North:
                    playerPosY += 1;
                    aspect_MoveRangeY += 4.5f;
                    break;
                case DoorName.South:
                    playerPosY -= 1;
                    aspect_MoveRangeY -= 4.5f;
                    break;
                case DoorName.West:
                    playerPosX -= 1;
                    aspect_MoveRangeX -= 6.5f;
                    break;
                case DoorName.East:
                    playerPosX += 1;
                    aspect_MoveRangeX += 6.5f;
                    break;
            }

            GameObject room = RoomManager.Map_Data[playerPosX + gridSizeX_Cen, playerPosY + gridSizeY_Cen];
            if (room != null)
            {
                room.gameObject.SetActive(true);
                aspect_MoveRangeX += transform.position.x;
                aspect_MoveRangeY += transform.position.y;
                collision.gameObject.transform.SetPositionAndRotation(new Vector3(aspect_MoveRangeX, aspect_MoveRangeY, 0.0f), Quaternion.identity);
                collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                RoomManager.SetPlayerPos(playerPosX, playerPosY);
                RoomManager.MiniMapMinimalize();
            }
        }
    }
}

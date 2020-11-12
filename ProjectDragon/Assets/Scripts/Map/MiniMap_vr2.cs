using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap_vr2 : MonoBehaviour
{
    public RoomManager RoomManager;
    private Transform roomRoot;
    public UIPanel panel;

    public UIButton button;
    public UISprite background;
    public UISprite playerPosSprite;


    public bool isMini = true;

    public GameObject mini;
    public GameObject maxi;

    private UISprite[] mini_Room;
    private UISprite[] maxi_Room;

    private void Awake()
    {
        mini = transform.Find("Mini").gameObject;
        maxi = transform.Find("Maxi").gameObject;

        mini_Room = mini.transform.Find("Panel").GetComponentsInChildren<UISprite>();
        maxi_Room = maxi.transform.Find("Panel").GetComponentsInChildren<UISprite>();




        //room = GameObject.FindGameObjectsWithTag("MiniMapRoom");
        roomRoot = transform.Find("RoomRoot");
        panel = GetComponent<UIPanel>();

        button = transform.parent.GetComponent<UIButton>();
        background = transform.parent.GetComponent<UISprite>();
        playerPosSprite = transform.Find("PlayerPos").GetComponent<UISprite>();
    }

    private void Start()
    {
        if(isMini)
        {
            maxi.SetActive(false);
        }
        else
        {
            mini.SetActive(false);
        }
    }

    //미니맵 업데이트
    public void UpdateMiniMap()
    {
        int x = RoomManager.player_PosX;
        int y = RoomManager.player_PosY;
        roomRoot.localPosition = new Vector3(-x * 60.0f, -y * 60.0f, 0.0f);

        //이동한 방이 클리어되지 않았다면 미니맵을 끈다.
        if (!RoomManager.Map_Data[x + RoomManager.gridSizeX_Cen, y + RoomManager.gridSizeY_Cen].GetComponent<Room>().roomState.Equals(RoomState.Clear))
        {
            gameObject.SetActive(false);
        }

        foreach (Room obj in RoomManager.PlayerLocationAroundRoomInMap())
        {
            if (obj == null || obj.roomType.Equals(RoomType.Hidden)) continue;

            //클리어가 안된 방들 미니맵에서 표시
            if (!obj.roomState.Equals(RoomState.Clear))
            {
                mini_Room[obj.miniMap_Index].gameObject.SetActive(true);
                maxi_Room[obj.miniMap_Index].gameObject.SetActive(true);

                //히든방은 숨깁니다.
                if (obj.roomType.Equals(RoomType.Hidden))
                {
                    mini_Room[obj.miniMap_Index].gameObject.SetActive(false);
                    maxi_Room[obj.miniMap_Index].gameObject.SetActive(false);
                }
            }
        }
    }

    //미니맵 초기화
    public void InitMiniMap()
    {
        GameObject[,] map_Data = RoomManager.Map_Data;

        int i = 0;
        foreach (GameObject obj in map_Data)
        {
            if (obj == null) continue;

            //포탈 이미지 로드
            //방의 정보
            Room temp_room = obj.GetComponent<Room>();

            //setting room position
            int x = (int)temp_room.gridPos.x;
            int y = (int)temp_room.gridPos.y;
            mini_Room[i].transform.localPosition = new Vector3(x * 60.0f, y * 60.0f);
            maxi_Room[i].transform.localPosition = new Vector3(x * 90.0f, y * 90.0f - 60.0f);

            temp_room.miniMap_Index = i;

            //처음 시작시 시작방을 제외하고 미니맵에서 지웁니다.
            if (x != 0.0f || y != 0.0f)
            {
                mini_Room[i].gameObject.SetActive(false);
                maxi_Room[i].gameObject.SetActive(false);
                mini_Room[i].spriteName = "Ingame_MiniMap_UnCheckedIcon";
                maxi_Room[i].spriteName = "Ingame_Map_UnCheckedIcon";
            }
            else
            {
                mini_Room[i].spriteName = "Ingame_MiniMap_NowIcon";
                maxi_Room[i].spriteName = "Ingame_Map_NowIcon";
            }

            i++;
        }

        //안쓰는 미니맵 오브젝트들 전부 끄기
        for (int j = i; j < mini_Room.Length; j++)
        {
            mini_Room[j].gameObject.SetActive(false);
            maxi_Room[i].gameObject.SetActive(false);
        }
    }

    //미니맵 확장
    public void Maximalize()
    {
        maxi.SetActive(true);
        mini.SetActive(false);
    }

    //미니맵 축소
    public void Minimalize()
    {
        maxi.SetActive(false);
        mini.SetActive(true);
    }

    //플레이어 teleport
    public void Teleport(int _x, int _y)
    {
        RoomManager.PlayerTeleportation(_x, _y);
    }
}

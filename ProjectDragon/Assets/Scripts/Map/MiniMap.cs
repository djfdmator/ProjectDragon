using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public bool isMini = true;
    public int curRoomIndex;

    public RoomManager RoomManager;

    public GameObject mini;
    public GameObject maxi;

    private UISprite[] mini_Room;
    private UISprite[] maxi_Room;

    private Transform miniRoomRoot;

    private void Awake()
    {
        mini = transform.Find("Mini").gameObject;
        maxi = transform.Find("Maxi").gameObject;

        mini_Room = mini.transform.Find("Panel/Root").GetComponentsInChildren<UISprite>();
        maxi_Room = maxi.transform.Find("Panel").GetComponentsInChildren<UISprite>();

        miniRoomRoot = mini.transform.Find("Panel/Root");
    }

    private void Start()
    {
        if (isMini)
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
        miniRoomRoot.localPosition = new Vector3(-x * 60.0f, -y * 60.0f, 0.0f);

        //이동한 방이 클리어되지 않았다면 미니맵을 끈다.
        if (!RoomManager.Map_Data[x + RoomManager.gridSizeX_Cen, y + RoomManager.gridSizeY_Cen].GetComponent<Room>().roomState.Equals(RoomState.Clear))
        {
            gameObject.SetActive(false);
        }

        foreach (GameObject obj in RoomManager.Map_Data)
        {
            if (obj == null) continue;

            Room temp_room = obj.GetComponent<Room>();

            if (temp_room.roomState == RoomState.Clear)
            {
                switch (temp_room.roomType)
                {
                    case RoomType.Begin:
                        mini_Room[temp_room.miniMap_Index].color = Color.yellow;
                        maxi_Room[temp_room.miniMap_Index].color = Color.yellow;
                        break;
                    case RoomType.Normal:
                        mini_Room[temp_room.miniMap_Index].spriteName = "Ingame_MiniMap_CheckedIcon";
                        maxi_Room[temp_room.miniMap_Index].spriteName = "Ingame_Map_CheckedIcon";
                        break;
                    case RoomType.Stair:
                        mini_Room[temp_room.miniMap_Index].spriteName = "Ingame_MiniMap_StairsIcon";
                        maxi_Room[temp_room.miniMap_Index].spriteName = "Ingame_Map_StairsIcon";
                        break;
                    case RoomType.NPC:
                        if (RoomManager.PortalRoomClearCount() >= 2)
                        {
                            mini_Room[temp_room.miniMap_Index].spriteName = "Ingame_MiniMap_PortalIcon";
                            maxi_Room[temp_room.miniMap_Index].spriteName = "Ingame_Map_PortalIcon";
                        }
                        else
                        {
                            mini_Room[temp_room.miniMap_Index].spriteName = "Ingame_MiniMap_UnPortalIcon";
                            maxi_Room[temp_room.miniMap_Index].spriteName = "Ingame_Map_UnPortalIcon";
                        }
                        break;
                    case RoomType.Hidden:
                        mini_Room[temp_room.miniMap_Index].spriteName = "Ingame_MiniMap_SecretIcon";
                        maxi_Room[temp_room.miniMap_Index].spriteName = "Ingame_Map_SecretIcon";
                        break;
                    case RoomType.Boss:
                        mini_Room[temp_room.miniMap_Index].spriteName = "Ingame_MiniMap_CheckedIcon";
                        maxi_Room[temp_room.miniMap_Index].spriteName = "Ingame_Map_CheckedIcon";
                        break;
                }
            }
        }

        foreach (Room obj in RoomManager.PlayerLocationAroundRoomInMap())
        {
            if (obj == null) continue;

            //클리어가 안된 방들 미니맵에서 표시
            if (!obj.roomState.Equals(RoomState.Clear))
            {
                mini_Room[obj.miniMap_Index].gameObject.SetActive(true);
                maxi_Room[obj.miniMap_Index].gameObject.SetActive(true);
            }

            //히든방은 숨깁니다.
            if (obj.roomType.Equals(RoomType.Hidden) && obj.roomState == RoomState.DeActivate)
            {
                mini_Room[obj.miniMap_Index].gameObject.SetActive(false);
                maxi_Room[obj.miniMap_Index].gameObject.SetActive(false);
            }
        }

        curRoomIndex = RoomManager.PlayerLocationInMap().miniMap_Index;
        mini_Room[RoomManager.PlayerLocationInMap().miniMap_Index].spriteName = "Ingame_MiniMap_NowIcon";
        maxi_Room[RoomManager.PlayerLocationInMap().miniMap_Index].spriteName = "Ingame_Map_NowIcon";

        if (RoomManager.PlayerLocationInMap().roomType == RoomType.Begin)
        {
            mini_Room[RoomManager.PlayerLocationInMap().miniMap_Index].color = Color.white;
            maxi_Room[RoomManager.PlayerLocationInMap().miniMap_Index].color = Color.white;
        }
        else if (RoomManager.PlayerLocationInMap().roomType == RoomType.Hidden)
        {
            mini_Room[RoomManager.PlayerLocationInMap().miniMap_Index].gameObject.SetActive(true);
            maxi_Room[RoomManager.PlayerLocationInMap().miniMap_Index].gameObject.SetActive(true);
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
            mini_Room[i].transform.localPosition = new Vector3(x * 52.5f, y * 52.5f);
            maxi_Room[i].transform.localPosition = new Vector3(x * 74.0f, y * 74.0f - 60.0f);

            temp_room.miniMap_Index = i;

            if (!temp_room.roomType.Equals(RoomType.Normal))
            {
                maxi_Room[i].gameObject.AddComponent<UIButton>();
                maxi_Room[i].gameObject.AddComponent<BoxCollider>();
                maxi_Room[i].gameObject.GetComponent<BoxCollider>().size = maxi_Room[i].localSize;
                maxi_Room[i].gameObject.GetComponent<BoxCollider>().isTrigger = true;
                EventDelegate teleport = new EventDelegate(this, "Teleport");
                teleport.parameters[0] = new EventDelegate.Parameter(x);
                teleport.parameters[1] = new EventDelegate.Parameter(y);
                maxi_Room[i].gameObject.GetComponent<UIButton>().onClick.Add(teleport);
            }

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
                curRoomIndex = i;
            }

            i++;
        }

        //안쓰는 미니맵 오브젝트들 전부 끄기
        for (int j = i; j < mini_Room.Length; j++)
        {
            mini_Room[j].gameObject.SetActive(false);
            maxi_Room[j].gameObject.SetActive(false);
        }
    }

    //미니맵 확장
    public void Maximalize()
    {
        isMini = false;
        maxi.SetActive(true);
        mini.SetActive(false);
    }

    //미니맵 축소
    public void Minimalize()
    {
        isMini = true;
        maxi.SetActive(false);
        mini.SetActive(true);
    }

    //플레이어 teleport
    public void Teleport(int _x, int _y)
    {
        if (RoomManager.PlayerLocationInMap().roomType == RoomType.Normal || !RoomManager.PlayerLocationInMap().portal.GetComponent<Portal>().isPortalActivate)
        {
            return;
        }

        RoomManager.PlayerLocationInMap().portal.GetComponent<Portal>().isPortalActivate = false;
        RoomManager.PlayerTeleportation(_x, _y);
    }
}

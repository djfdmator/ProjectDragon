
// ==============================================================
// Minimap Object
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-16
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour
{
    public RoomManager RoomManager;
    private GameObject[] room;
    private Transform roomRoot;
    public UIPanel panel;

    public UIButton button;
    public EventDelegate mini = new EventDelegate();
    public EventDelegate maxi = new EventDelegate();
    //public EventDelegate teleport = new EventDelegate();

    private void Awake()
    {
        room = GameObject.FindGameObjectsWithTag("MiniMapRoom");
        roomRoot = transform.Find("RoomRoot");
        panel = GetComponentInParent<UIPanel>();

        button = transform.Find("Button").GetComponent<UIButton>();
    }

    private void Start()
    {
        //미니맵 버튼 이벤트 설정
        mini = new EventDelegate(GetComponent<MiniMap>(), "Minimalize");
        maxi = new EventDelegate(GetComponent<MiniMap>(), "Maximalize");
        //teleport = new EventDelegate(GetComponent<MiniMap>(), "Teleport");

        button.onClick.Add(maxi);
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

        foreach(Room obj in RoomManager.PlayerLocationAroundRoomInMap())
        {
            if (obj == null) continue;

            //클리어가 안된 방들 미니맵에서 연하게 표시
            if(!obj.roomState.Equals(RoomState.Clear))
            {
                obj.MiniMapPos.SetActive(true);
                obj.MiniMapPos.GetComponent<UISprite>().alpha = 0.5f;

                //히든방은 숨깁니다.
                if (obj.roomType.Equals(RoomType.Hidden)) obj.MiniMapPos.GetComponent<UISprite>().alpha = 0.0f;
            }
        }
    }

    //미니맵 초기화
    public void InitMiniMap()
    {
        List<GameObject> temp = new List<GameObject>();
        GameObject[,] map_Data = RoomManager.Map_Data;
        GameObject portalImage = Resources.Load("Map/MiniMap/portal_Image") as GameObject;

        int i = 0;
        foreach (GameObject obj in map_Data)
        {
            if (obj == null) continue;

            //포탈 이미지 로드
            //방의 정보
            Room temp_room = obj.GetComponent<Room>();
            int x = (int)temp_room.gridPos.x;
            int y = (int)temp_room.gridPos.y;

            //미니맵 위치 세팅
            room[i].transform.localPosition = new Vector3(x * 60.0f, y * 60.0f, 0.0f);
            //방이 일반 방이 아니면 포탈 이미지 세팅
            if (!temp_room.roomType.Equals(RoomType.Normal))
            {
                //room[i].AddComponent<BoxCollider2D>();
                //room[i].GetComponent<BoxCollider2D>().enabled = false;
                //포탈 그림 붙이고, 안보이게 하기
                GameObject portal = Instantiate(portalImage, room[i].transform.localPosition, Quaternion.identity, room[i].transform);
                portal.transform.localPosition = Vector3.zero;
                portal.GetComponent<UISprite>().enabled = false;
                portal.name = "Portal";
                UIButton button = portal.GetComponent<UIButton>();
                EventDelegate teleport = new EventDelegate(GetComponent<MiniMap>(), "Teleport");
                teleport.parameters[0] = new EventDelegate.Parameter(x);
                teleport.parameters[1] = new EventDelegate.Parameter(y);
                button.onClick.Add(teleport);
            }

            temp_room.MiniMapPos = room[i];

            //처음 시작시 시작방을 제외하고 미니맵에서 지웁니다.
            if (x != 0.0f || y != 0.0f)
            {
                room[i].SetActive(false);
            }

            i++;
        }

        //안쓰는 미니맵 오브젝트들 전부 끄기
        for(int j = i; j < room.Length; j++)
        {
            room[j].SetActive(false);
        }
    }

    //미니맵 확장
    public void Maximalize()
    {
        //panel의 위치와 크기 조절
        panel.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        panel.baseClipRegion = new Vector4(0.0f, 0.0f, 725.0f, 725.0f);
        panel.clipOffset = new Vector2(0.0f, 12.0f);

        //미니맵 확대
        GetComponent<UISprite>().SetDimensions(1000, 1000);
        button.GetComponent<UISprite>().SetDimensions(800, 800);
        transform.Find("PlayerPos").GetComponent<UISprite>().SetDimensions(120, 120);
        roomRoot.localScale = new Vector3(2.0f, 2.0f, 1.0f);
        roomRoot.localPosition *= 2.0f;

        button.onClick.RemoveAt(0);
        StartCoroutine(AddButton(mini));
    }

    //미니맵 축소
    public void Minimalize()
    {
        //panel의 위치와 크기 조절
        panel.transform.localPosition = new Vector3(-800.0f, 200.0f, 0.0f);
        panel.baseClipRegion = new Vector4(0.0f, 0.0f, 255.0f, 255.0f);
        panel.clipOffset = new Vector2(0.0f, 4.0f);

        //미니맵 축소
        GetComponent<UISprite>().SetDimensions(350, 350);
        button.GetComponent<UISprite>().SetDimensions(300, 300);
        transform.Find("PlayerPos").GetComponent<UISprite>().SetDimensions(60, 60);
        roomRoot.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        roomRoot.localPosition /= 2.0f;

        button.onClick.RemoveAt(0);
        StartCoroutine(AddButton(maxi));
    }

    //클릭 이벤트 바꾸기
    IEnumerator AddButton(EventDelegate _Event)
    {
        yield return new WaitForSeconds(0.3f);
        button.onClick.Add(_Event);
    }

    //플레이어 teleport
    public void Teleport(int _x, int _y)
    {
        RoomManager.PlayerTeleportation(_x, _y);
    }
}

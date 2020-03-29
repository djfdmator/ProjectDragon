
// ==============================================================
// Room Object
//
// 2019-12-27: Responding to the boss
// 2019-12-31: change Boss monster counting and add plus monster method  
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-31
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    Begin,
    Normal,
    Stair,
    NPC,
    Hidden,
    Boss
}

public enum RoomState
{
    DeActivate,
    Activate,
    Clear
}

public class Room : MonoBehaviour
{
    public Vector2 gridPos;     //방의 위치를 나타냅니다.
    public RoomType roomType;   //어떤 방인지를 나타냅니다.
    public RoomState roomState = RoomState.DeActivate; //현재 방의 상태
    public int depth; //방이 시작방에서 얼마나 먼 곳에 있는지

    public bool doorTop, doorBot, doorLeft, doorRight; //문이 해당 방향에 있는지 없는지 나타냅니다.
    public GameObject[] door_All = new GameObject[4] { null, null, null, null }; //문 오브젝트

    public GameObject portal; //포탈 오브젝트
    public List<GameObject> Enemies = new List<GameObject>();
    public List<GameObject> monsters = new List<GameObject>(); //방의 몬스터를 관리하기 위한 리스트
    public List<GameObject> items = new List<GameObject>();

    public RoomManager roomManager;

    public Player playerSet;

    private IEnumerator gathering;

    public GameObject MiniMapPos //미니맵 상에서의 방 오브젝트
    {
        get { return miniMapPos; }
        set
        {
            miniMapPos = value;
        }
    }
    private GameObject miniMapPos;

    private void Awake()
    {
        //데이터 초기화
        InitRoom();
        gathering = Gathering(2.0f);
    }

    private void Start() {
        //gathering = Gathering(2.0f);    
    }

    private void OnEnable()
    {
        switch(roomType)
        {
            case RoomType.Hidden:
            SoundManager.Inst.Ds_BGMPlayerDB(6);
            break;
            case RoomType.NPC:
            SoundManager.Inst.Ds_BGMPlayerDB(5);
            break;
            case RoomType.Boss:
            SoundManager.Inst.Ds_BGMPlayerDB(7);
            break;
            default:
            SoundManager.Inst.Ds_BGMPlayerDB(4);
            break;
        }

        StartCoroutine(gathering);
    }
    private void OnDisable()
    {
        StopCoroutine(gathering);
    }
    void Update()
    {
        //룸의 상태 관리
        CheckRoomState();
    }

    //방의 데이터를 초기화 - 방 생성시 바로 작동합니다.
    private void InitRoom()
    {
        playerSet = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Monster[] temp_monsters = transform.GetComponentsInChildren<Monster>();
        foreach (Monster obj in temp_monsters)
        {

            monsters.Add(obj.gameObject);
            if (obj.GetComponent<Enemy>() != null)
            {
                Enemies.Add(obj.gameObject);
            }
            
        }
    }

    //룸의 상태를 확인
    void CheckRoomState()
    {
        if (!roomState.Equals(RoomState.Clear))
        {
            //몬스터 리스트 관리
            MonsterCounting();

            if (monsters.Count == 0 && roomState.Equals(RoomState.Activate))
            {
                //몬스터가 한 마리도 없다면 클리어입니다.
                IsClear();
            }
            else if (!roomState.Equals(RoomState.Activate)) CheckPlayerPos();

        }
    }

    //몬스터 리스트 관리
    void MonsterCounting()
    {
        List<GameObject> temp_monsters = new List<GameObject>();
        List<GameObject> temp_enemies = new List<GameObject>();
        foreach (GameObject obj in monsters)
        {
            if (obj.GetComponent<Monster>().isDead) continue;
            else
            {
                temp_monsters.Add(obj);
                if (obj.CompareTag("Enemy"))
                {
                    temp_enemies.Add(obj);
                }
            }
        }

        //리스트 값 재지정
        monsters.Clear();
        monsters.AddRange(temp_monsters);
        Enemies.Clear();
        Enemies.AddRange(temp_enemies);

        //몬스터가 없으면 플레이어에게 Null을 세팅
        if (monsters.Count == 0)
        {
            playerSet.TempNullSet();
        }
    }

    //몬스터 추가되는 기능이 있으면 필요함
    #region 공사중
    /// <summary>
    /// 전투 도중에 몬스터를 추가 하기 위한 함수입니다. 예시) 보스
    /// </summary>
    /// <param name="_monsters"></param>
    public void AddMonsters(List<GameObject> _monsters)
    {
        if (!monsters.Count.Equals(0) && roomState.Equals(RoomState.Activate))
        {
            monsters.AddRange(_monsters);
            Enemies.AddRange(_monsters);
            //몬스터와 플레이어 계산 다시 해야 하나? 
        }
        StartCoroutine(playerSet.CalculateDistanceWithPlayer());
    }
    #endregion
    /// <summary>
    /// 전투 도중에 몬스터를 추가 하기 위한 함수입니다. 예시) 보스
    /// </summary>
    /// <param name="_monsters"></param>
    public void AddMonsters(GameObject _monster)
    {
        if (!monsters.Count.Equals(0) && roomState.Equals(RoomState.Activate))
        {
            monsters.Add(_monster);

            if (_monster.GetComponent<Enemy>() != null)
            {
                Enemies.Add(_monster);
                _monster.GetComponent<Monster>().StartOn();
            }
        }
    }

    public void AddMonster(GameObject _monster)
    {
        if (!monsters.Count.Equals(0) && roomState.Equals(RoomState.Activate))
        {
            Enemies.Add(_monster);
            monsters.Add(_monster);
            StartCoroutine(playerSet.CalculateDistanceWithPlayer());
        }
    }

    //플레이어가 현재 방에 있는게 맞다면 배틀 시작
    void CheckPlayerPos()
    {
        Vector2 PlayerPos = new Vector2(roomManager.player_PosX, roomManager.player_PosY);
        if (gridPos == PlayerPos)
        {
            roomState = RoomState.Activate;

            //플레이어 배틀 시작
            playerSet.EnemyArray = Enemies;
            StartCoroutine(playerSet.CalculateDistanceWithPlayer());

            //몬스터 배틀 시작
            foreach (GameObject obj in monsters)
            {
                //보스를 위해 Enemy를 Monster로 바꿔야한다.
                obj.GetComponent<Monster>().StartOn();
            }
        }
    }

    //방이 클리어되면 작동하는 함수
    void IsClear()
    {
        roomState = RoomState.Clear;
        OpenAllDoor(); //모든 문 열기

        roomManager.miniMap.gameObject.SetActive(true); //미니맵 켜기
        miniMapPos.GetComponent<UISprite>().alpha = 1.0f; //

        #region 수정 필요 -- 이미지 로드, 포탈 표시 방식 -- 미니맵
        //포탈이 있는 방이라면 미니맵 상에서 포탈 표시
        if (!roomType.Equals(RoomType.Normal))
        {
            miniMapPos.transform.Find("Portal").GetComponent<UISprite>().enabled = true;
        }

        //방이 normal 이 아니라면 미니맵에서 특별한 색으로 표시
        switch (roomType)
        {
            case RoomType.Begin:
                miniMapPos.GetComponent<UISprite>().color = Color.cyan;
                break;
            case RoomType.NPC:
                miniMapPos.GetComponent<UISprite>().color = Color.yellow;
                break;
            case RoomType.Stair:
                miniMapPos.GetComponent<UISprite>().color = Color.green;
                gameObject.transform.Find("Stair").GetComponent<Stair>().IsOpen = true;
                break;
            case RoomType.Boss:
                miniMapPos.GetComponent<UISprite>().color = Color.red;
                //다음으로 넘어가는 문이 열려야 함
                gameObject.transform.Find("Stair").GetComponent<Stair>().IsOpen = true;
                break;
            default:
                break;
        }
        #endregion

        //포탈 켜기
        roomManager.PortalOn();

        //아이템 획득 - 미구현 상태, 드랍 구현 필요
        //CollectAll_Items();

    }

    private IEnumerator Gathering(float _gatherTime)
    {
        while (true)
        {
            if (roomState.Equals(RoomState.Clear) && !items.Count.Equals(0))
            {
                StartCoroutine(roomManager.GatheringItems(items, _gatherTime));
                yield return new WaitForSeconds(0.3f);
            }
            yield return null;
        }
    }

    /// <summary>
    /// [구현중] 방에 존재하는 모든 아이템을 인벤토리에 넣습니다.
    /// 아이템 드랍이 어떻게 될 것인지 먼저 정해야한다.
    /// </summary>
    void CollectAll_Items()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("item");
        #region 아이템 전부 먹기
        //구현 필요
        #endregion
    }

    /// <summary>
    /// 방이 클리어되면 문이 열립니다.
    /// </summary>
    void OpenAllDoor()
    {
        GameObject wall = transform.Find("wall").gameObject;
        GameObject North = wall.transform.Find("North").gameObject;
        GameObject South = wall.transform.Find("South").gameObject;
        GameObject West = wall.transform.Find("West").gameObject;
        GameObject East = wall.transform.Find("East").gameObject;

        foreach (GameObject obj in door_All)
        {
            if (obj == null) continue;

            switch (obj.GetComponent<Door>().Name)
            {
                case DoorName.North:
                    Destroy(North);
                    break;
                case DoorName.South:
                    Destroy(South);
                    break;
                case DoorName.West:
                    Destroy(West);
                    break;
                case DoorName.East:
                    Destroy(East);
                    break;
            }

            obj.GetComponent<Door>().OpenDoor();
        }
    }


    public void SetData(Vector2 _gridPos, RoomType _roomType, RoomManager _roomManager, int _depth, bool[] _door)
    {
        gridPos = _gridPos;
        roomType = _roomType;
        roomManager = _roomManager;
        depth = _depth;
        doorBot = _door[0];
        doorLeft = _door[1];
        doorRight = _door[2];
        doorTop = _door[3];
        if (!_roomType.Equals(RoomType.Begin))
        {
            gameObject.SetActive(false);
        }
        if (!roomType.Equals(RoomType.Normal) && !roomType.Equals(RoomType.Boss))
        {
            portal = transform.GetComponentInChildren<Portal>().gameObject;
        }
        else portal = null;
    }

    public void SetData(Room _room)
    {
        gridPos = _room.gridPos;
        roomType = _room.roomType;
        roomManager = _room.roomManager;
        depth = _room.depth;
        doorBot = _room.doorBot;
        doorLeft = _room.doorLeft;
        doorRight = _room.doorRight;
        doorTop = _room.doorTop;
        if (!roomType.Equals(RoomType.Begin))
        {
            gameObject.SetActive(false);
        }
        if (!roomType.Equals(RoomType.Normal) && !roomType.Equals(RoomType.Boss))
        {
            portal = transform.GetComponentInChildren<Portal>().gameObject;
        }
        else portal = null;
    }
}


// ==============================================================
// Map Creater
// Create all map data and object
//
// 2019-12-27: BossMap create method
// 2019-12-31: modify DoorSetting method
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-31
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class room
{
    public Vector2 gridPos;

    public RoomType type;

    public bool doorTop, doorBot, doorLeft, doorRight;

    public int depth;

    public room(Vector2 _gridPos, RoomType _type, int _depth)
    {
        gridPos = _gridPos;
        type = _type;
        depth = _depth;
    }
}

public class MapCreator : MonoBehaviour
{
    //load
    public string mapType = string.Empty;
    public bool isBossMap;
    //map grid data
    public GameObject[,] map_Data;
    //prefabs data
    public GameObject map_Base, map_Stair, map_Market;
    public GameObject[] map_Hiddens;
    public int map_Hiddens_Count = 1;
    public GameObject[] map_Prefabs;
    public int map_Prefabs_Count = 1;

    //room count
    public int numberOfRooms = 15;
    private int gridSizeX, gridSizeY, gridSizeX_Cen, gridSizeY_Cen;
    //stair pos
    public Vector2 stair_LocalPosition = new Vector2(0.0f, 0.0f);

    public Vector2 worldSize = new Vector2(6.0f, 6.0f);

    public room[,] rooms;
    private List<Vector2> takenPositions = new List<Vector2>();

    private void Awake()
    {
        SettingCreateRegion();
        ResourceLoadMap(); // 임시로 숲이라고 함
        Init();
    }

    //수정 필요함
    private void SettingCreateRegion()
    {
        //GameManager.Inst.CurrentStage = 3;
        int curStage = GameManager.Inst.CurrentStage;

#if UNITY_EDITOR
        Debug.Log(curStage % 4);
        Debug.Log(curStage / 4);
#endif
        if(curStage % 4 == 0)
        {
            //보스 맵
            isBossMap = true;
        }
        else
        {
            //일반 맵
            isBossMap = false;
        }

        if(0 < curStage && curStage <= 4)
        {
            //숲
            mapType = "Forest";
        }
        else if(4 < curStage && curStage <= 8)
        {
            //스테이지 추가시 추가
        }
    }

    /// <summary>
    /// Initialized data
    /// </summary>
    private void Init()
    {
        gridSizeX_Cen = (int)(worldSize.x / 2);
        gridSizeY_Cen = (int)(worldSize.y / 2);
        gridSizeX = (int)worldSize.x;
        gridSizeY = (int)worldSize.y;

        if (numberOfRooms >= gridSizeX * gridSizeY)
        {
            numberOfRooms = Mathf.RoundToInt(gridSizeX * gridSizeY);
        }
        map_Data = new GameObject[gridSizeX, gridSizeY];
    }

    /// <summary>
    /// Load All Map Prefabs
    /// </summary>
    /// <param name="_mapType"> Type is region type :: 지역명</param>
    void ResourceLoadMap()
    {
        if (!mapType.Equals(string.Empty))
        {
            if (isBossMap) ResourceLoadBossMap();
            else ResourceLoadNormalMap();
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("Load Failed MapData");
#endif
        }
    }

    private void ResourceLoadNormalMap()
    {
        map_Base = Resources.Load("Map/" + mapType + "/Base") as GameObject;
        map_Stair = Resources.Load("Map/" + mapType + "/Stair") as GameObject;
        map_Market = Resources.Load("Map/" + mapType + "/Market") as GameObject;
        map_Hiddens = new GameObject[map_Hiddens_Count];
        map_Prefabs = new GameObject[map_Prefabs_Count];

        for (int i = 0; i < map_Hiddens_Count; i++)
        {
            string name = "Map/" + mapType + "/Hidden";
            name += (i + 1).ToString();
            map_Hiddens[i] = Resources.Load(name) as GameObject;
        }
        for (int i = 0; i < map_Prefabs_Count; i++)
        {
            string name = "Map/" + mapType + "/Normal";
            name += (i + 1).ToString();
            map_Prefabs[i] = Resources.Load(name) as GameObject;
        }
    }

    private void ResourceLoadBossMap()
    {
        map_Base = Resources.Load("Map/" + mapType + "/Boss/Base") as GameObject;
        map_Stair = Resources.Load("Map/" + mapType + "/Boss/Boss") as GameObject;
    }

    void Start()
    {
        if (isBossMap)
        {
            CreateBossMap();
            SetRoomDoors();
            DrawBossMap();
        }
        else
        {
            CreateNormalMap(); //lays out the actual map
            SetRoomDoors(); //assigns the doors where rooms would connect
            DrawMap(); //instantiates objects to make up a map
        }
    }

    /// <summary>
    /// A와 B를 그리드 사이즈가 홀수냐 짝수냐에 따라 비교식을 바꿔 계산합니다.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    bool CompareAandB(int a, int b)
    {
        bool temp = false;
        if (gridSizeX % 2 == 1)
        {
            temp = a > b;
        }
        else
        {
            temp = a >= b;
        }

        return temp;
    }

    private void CreateBossMap()
    {
        int depth = 0;
        rooms = new room[gridSizeX, gridSizeY];
        rooms[gridSizeX_Cen, gridSizeY_Cen] = new room(Vector2.zero, RoomType.Begin, depth);
        rooms[gridSizeX_Cen, gridSizeY_Cen + 1] = new room(Vector2.up, RoomType.Boss, depth);
    }
    
    private void CreateNormalMap()
    {
        int depth = 0;
        //setup
        rooms = new room[gridSizeX, gridSizeY];

        //중앙 방 생성
        rooms[gridSizeX_Cen, gridSizeY_Cen] = new room(Vector2.zero, RoomType.Begin, depth);

        //중앙 방 위치 값 넣기
        takenPositions.Insert(0, Vector2.zero);

        Vector2 checkPos = Vector2.zero;

        //magic numbers
        float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;

        //add rooms
        for (int i = 0; i < numberOfRooms - 2; i++)
        {
            float randomPerc = ((float)i) / (((float)numberOfRooms - 1));

            randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc); // 왜 한거지?

            //grab new position
            checkPos = NewPosition();

            //test new position - 옆 방이 여러 개 있으면 위치를 다시 뽑음
            if (NumberOfNeighbors(checkPos, takenPositions) > 1 && Random.value > randomCompare) // 이해 안감
            {
                int iterations = 0;
                do
                {
                    checkPos = SelectiveNewPosition();
                    iterations++;
                } while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100); // 이해 안감
                if (iterations >= 50)
                    Debug.Log("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
            }
            //방이 얼마나 멀리 있는지 검사 합니다.
            depth = DepthCheck((int)checkPos.x + gridSizeX_Cen, (int)checkPos.y + gridSizeY_Cen);
            //finalize position
            rooms[(int)checkPos.x + gridSizeX_Cen, (int)checkPos.y + gridSizeY_Cen] = new room(checkPos, RoomType.Normal, depth);
            takenPositions.Insert(0, checkPos);
        }
    }

    int DepthCheck(int x, int y)
    {
        List<int> temp = new List<int>();

        if (x + 1 < gridSizeX)
        {
            if (rooms[x + 1, y] != null)
            {
                temp.Add(rooms[x + 1, y].depth);
            }
        }
        if (x - 1 >= 0)
        {
            if (rooms[x - 1, y] != null) 
            {
                temp.Add(rooms[x - 1, y].depth);
            }
        }
        if (y + 1 < gridSizeY)
        {
            if (rooms[x, y + 1] != null)
            {
                temp.Add(rooms[x, y + 1].depth);
            }
        }
        if (y - 1 >= 0)
        {
            if (rooms[x, y - 1] != null)
            {
                temp.Add(rooms[x, y - 1].depth);
            }
        }

        temp.Sort();
        return temp[0] + 1;
    }

    Vector2 NewPosition()
    {
        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;

        do
        {
            //현재 생성된 방중에 하나를 랜덤으로 뽑는 식
            int index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1)); // pick a random room
            x = (int)takenPositions[index].x;//capture its x, y position
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
            bool positive = (Random.value < 0.5f);//pick whether to be positive or negative on that axis
            if (UpDown)
            { //find the position bnased on the above bools
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || CompareAandB(x, gridSizeX_Cen) || x < -gridSizeX_Cen || CompareAandB(y, gridSizeY_Cen) || y < -gridSizeY_Cen); //make sure the position is valid

        return checkingPos;
    }

    Vector2 SelectiveNewPosition()
    { // method differs from the above in the two commented ways
        int index = 0, inc = 0;

        int x = 0, y = 0;
        Vector2 checkingPos = Vector2.zero;

        do
        {
            inc = 0;
            do
            {
                //instead of getting a room to find an adject empty space, we start with one that only 
                //as one neighbor. This will make it more likely that it returns a room that branches out
                index = Mathf.RoundToInt(Random.value * (takenPositions.Count - 1));
                inc++;
            } while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

            x = (int)takenPositions[index].x;
            y = (int)takenPositions[index].y;
            bool UpDown = (Random.value < 0.5f);
            bool positive = (Random.value < 0.5f);
            if (UpDown)
            {
                if (positive)
                {
                    y += 1;
                }
                else
                {
                    y -= 1;
                }
            }
            else
            {
                if (positive)
                {
                    x += 1;
                }
                else
                {
                    x -= 1;
                }
            }
            checkingPos = new Vector2(x, y);
        } while (takenPositions.Contains(checkingPos) || CompareAandB(x, gridSizeX_Cen) || x < -gridSizeX_Cen || CompareAandB(y, gridSizeY_Cen) || y < -gridSizeY_Cen);

        if (inc >= 100)
        { // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
            Debug.Log("Error: could not find position with only one neighbor");
        }
        return checkingPos;
    }

    int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
        int ret = 0; // start at zero, add 1 for each side there is already a room
        if (usedPositions.Contains(checkingPos + Vector2.right))
        { //using Vector.[direction] as short hands, for simplicity
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.left))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.up))
        {
            ret++;
        }
        if (usedPositions.Contains(checkingPos + Vector2.down))
        {
            ret++;
        }
        return ret;
    }

    private void DrawBossMap()
    {
        GameObject Map_Root = new GameObject("Map_Root", typeof(RoomManager));
        Map_Root.tag = "RoomManager";
        RoomManager Manager = Map_Root.GetComponent<RoomManager>();
        Map_Root.transform.SetPositionAndRotation(new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        Manager.SetGridData(gridSizeX_Cen, gridSizeY_Cen, gridSizeX, gridSizeY);

        foreach(room room in rooms)
        {
            if(room == null)
            {
                continue;
            }

            Vector2 drawPos = room.gridPos;
            drawPos.x *= 25f;//aspect ratio of map sprite
            drawPos.y *= 15f;
            int x = (int)room.gridPos.x;
            int y = (int)room.gridPos.y;

            if (room.type.Equals(RoomType.Begin)) map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen] = Instantiate(map_Base, drawPos, Quaternion.identity, Map_Root.transform);
            else map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen] = Instantiate(map_Stair, drawPos, Quaternion.identity, Map_Root.transform);

            Room roomManager = map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen].AddComponent<Room>();

            //방마다 데이터 저장
            bool[] door_dir = { room.doorBot, room.doorLeft, room.doorRight, room.doorTop };
            roomManager.SetData(room.gridPos, room.type, Manager, room.depth, door_dir); //Room Data Set

            //방의 문 설정
            DoorSetting(roomManager);
        }

        Manager.Map_Data = map_Data;
        Manager.SetPlayerPos(0, 0);
    }
    

    void DrawMap()
    {
        //전체 맵 최상위 생성
        GameObject Map_Root = new GameObject("Map_Root", typeof(RoomManager));
        Map_Root.tag = "RoomManager";
        RoomManager Manager = Map_Root.GetComponent<RoomManager>();
        Map_Root.transform.SetPositionAndRotation(new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        Manager.SetGridData(gridSizeX_Cen, gridSizeY_Cen, gridSizeX, gridSizeY);

        //모든 방 설정
        foreach (room room in rooms)
        {
            if (room == null)
            {
                continue; //skip where there is no room
            }
            Vector2 drawPos = room.gridPos;
            drawPos.x *= 25f;//aspect ratio of map sprite
            drawPos.y *= 15f;
            int x = (int)room.gridPos.x;
            int y = (int)room.gridPos.y;

            if (room.type.Equals(RoomType.Begin)) map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen] = Instantiate(map_Base, drawPos, Quaternion.identity, Map_Root.transform);
            else
            {
                int rand = Random.Range(0, map_Prefabs_Count);
                map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen] = Instantiate(map_Prefabs[rand], drawPos, Quaternion.identity, Map_Root.transform);
            }
            Room roomManager = map_Data[x + gridSizeX_Cen, y + gridSizeY_Cen].AddComponent<Room>();

            //방마다 데이터 저장
            bool[] door_dir = { room.doorBot, room.doorLeft, room.doorRight, room.doorTop };
            roomManager.SetData(room.gridPos, room.type, Manager, room.depth, door_dir); //Room Data Set

            //방의 문 설정
            DoorSetting(roomManager);
        }
        SetStairRoom(Map_Root.transform); //setting the stair room
        SetNpcRoom(Map_Root.transform);
        SetHiddenRoom(Map_Root.transform);
        Manager.Map_Data = map_Data;
        Manager.SetPlayerPos(0, 0);
    }
    void SetRoomDoors()
    {
        for (int x = 0; x < gridSizeX; x++)
        {
            for (int y = 0; y < gridSizeY; y++)
            {
                if (rooms[x, y] == null)
                {
                    continue;
                }

                Vector2 gridPosition = new Vector2(x, y);

                if (y - 1 < 0)
                { //check above
                    rooms[x, y].doorBot = false;
                }
                else
                {
                    rooms[x, y].doorBot = (rooms[x, y - 1] != null);
                }

                if (y + 1 >= gridSizeY)
                { //check bellow
                    rooms[x, y].doorTop = false;
                }
                else
                {
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }

                if (x - 1 < 0)
                { //check left
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }

                if (x + 1 >= gridSizeX)
                { //check right
                    rooms[x, y].doorRight = false;
                }
                else
                {
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                }
            }
        }

    }

    //계단방 세팅
    void SetStairRoom(Transform _parent)
    {
        List<GameObject> temp = new List<GameObject>();

        //gathering all room
        foreach (GameObject obj in map_Data)
        {
            if (obj == null) continue;

            temp.Add(obj);
        }

        if (temp.Count >= 2)
        {
            int count = temp.Count - 1;
            do
            {
                for (int j = 0; j < temp.Count - 1; j++)
                {
                    if (temp[j].GetComponent<Room>().depth > temp[j + 1].GetComponent<Room>().depth)
                    {
                        GameObject a = temp[j];
                        temp[j] = temp[j + 1];
                        temp[j + 1] = a;
                    }
                }
                count--;
            } while (count > 0);
        }

        GameObject stairTemp = Instantiate(map_Stair, temp[temp.Count - 1].transform.position, Quaternion.identity, _parent);
        Room map = stairTemp.AddComponent<Room>();
        temp[temp.Count - 1].GetComponent<Room>().roomType = RoomType.Stair;
        map.SetData(temp[temp.Count - 1].GetComponent<Room>());
        //map.roomType = RoomType.Stair;

        //방의 문 설정
        DoorSetting(map);

        int x = (int)map.gridPos.x + gridSizeX_Cen;
        int y = (int)map.gridPos.y + gridSizeY_Cen;
        Destroy(map_Data[x, y].gameObject);
        map_Data[x, y] = stairTemp;
    }

    //NPC방 생성
    void SetNpcRoom(Transform _parent)
    {
        SetNpc_Market(_parent);

        //다른 npc방 추가
    }

    void SetNpc_Market(Transform _parent)
    {
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject obj in map_Data)
        {
            if (obj == null) continue;
            if (obj.GetComponent<Room>().depth < 1) continue;
            if (!obj.GetComponent<Room>().roomType.Equals(RoomType.Normal)) continue;

            temp.Add(obj);
        }

        int rand = Random.Range(0, temp.Count);

        GameObject map_MarketTemp = Instantiate(map_Market, temp[rand].transform.position, Quaternion.identity, _parent);
        Room map = map_MarketTemp.AddComponent<Room>();

        temp[rand].GetComponent<Room>().roomType = RoomType.NPC;
        map.SetData(temp[rand].GetComponent<Room>());
        //map.roomType = RoomType.NPC;

        //방의 문 설정
        DoorSetting(map);

        //map_Data change
        int x = (int)map.gridPos.x + gridSizeX_Cen;
        int y = (int)map.gridPos.y + gridSizeY_Cen;
        Destroy(map_Data[x, y].gameObject);
        map_Data[x, y] = map_MarketTemp;
    }

    void SetHiddenRoom(Transform _parent)
    {
        //히든방이 붙어 있을 수 있는 방 선택
        List<GameObject> temp = new List<GameObject>();
        foreach (GameObject obj in map_Data)
        {
            if (obj == null) continue;
            if (obj.GetComponent<Room>().depth < 2) continue;
            if (obj.GetComponent<Room>().roomType.Equals(RoomType.NPC)) continue;

            int i = 0;
            foreach (GameObject obj2 in obj.GetComponent<Room>().door_All)
            {
                if (obj2 == null) continue;

                i++;
            }
            if (!i.Equals(4)) temp.Add(obj);
        }

        //히든방 베이스가 없으면 안만듬
        if (temp.Count.Equals(0))
        {
#if UNITY_EDITOR
            Debug.Log("Hidden room zero");
#endif
            return;
        }

        //어느 방을 기준으로 할지 정함
        int roomNum = Random.Range(0, temp.Count);
        Room baseRoom = temp[roomNum].GetComponent<Room>();

        //생성할 프리펩 준비
        int hiddenNum = Random.Range(0, map_Hiddens_Count);
        GameObject hiddenRoom = map_Hiddens[hiddenNum];

        //베이스 룸의 그리드를 mapdata 그리드로 변환
        int x = (int)baseRoom.gridPos.x + gridSizeX_Cen;
        int y = (int)baseRoom.gridPos.y + gridSizeY_Cen;
        Vector3 pos = Vector3.zero;
        bool[] door = new bool[4];
        //방 생성할 위치 체크
        do
        {
            if (!baseRoom.doorBot && y - 1 >= 0)
            {
                baseRoom.doorBot = true;
                DoorLoad(baseRoom, "South");
                door = new bool[4] { false, false, false, true };
                y--;
                break;
            }
            else if (!baseRoom.doorTop && y + 1 <= gridSizeY - 1)
            {
                baseRoom.doorTop = true;
                DoorLoad(baseRoom, "North");
                door = new bool[4] { true, false, false, false };
                y++;
                break;
            }
            else if (!baseRoom.doorLeft && x - 1 >= 0)
            {
                baseRoom.doorLeft = true;
                DoorLoad(baseRoom, "West");
                door = new bool[4] { false, false, true, false };
                x--;
                break;
            }
            else if (!baseRoom.doorRight && x + 1 <= gridSizeX - 1)
            {
                baseRoom.doorRight = true;
                DoorLoad(baseRoom, "East");
                door = new bool[4] { false, true, false, false };
                x++;
                break;
            }
        } while (false);
        pos = new Vector3((x - gridSizeX_Cen) * 25.0f, (y - gridSizeY_Cen) * 15.0f, 0.0f);
        GameObject hidden = Instantiate(hiddenRoom, pos, Quaternion.identity, _parent);
        Room map = hidden.AddComponent<Room>();
        map.SetData(new Vector2(x - gridSizeX_Cen, y - gridSizeY_Cen), RoomType.Hidden, _parent.GetComponent<RoomManager>(), -1, door);
        DoorSetting(map);
        map_Data[x, y] = hidden;
    }

    //방에 문을 달아줍니다.
    private void DoorLoad(Room _parent, string _direction)
    {
        Transform baseDW = _parent.transform.Find("DoorWall");
        GameObject obj = Instantiate(Resources.Load("Object/" + _direction) as GameObject, baseDW);
        Door temp = obj.transform.Find("Door").GetComponent<Door>();
        temp.GetComponent<Animator>().enabled = false;
        obj.transform.Find("Crack").GetComponent<Crack>().SetRoom(_parent, obj.transform.Find("Door").GetComponent<Door>());
        
        for(int i = 0; i < 4; i++)
        {
            if (_parent.door_All[i] != null) continue;

            _parent.door_All[i] = temp.gameObject;
            _parent.door_All[i].name = _direction;

            break;
        }
    }
    private void DoorSetting(Room _room)
    {
        //door setting
        GameObject doorWall = _room.transform.Find("DoorWall").gameObject;
        GameObject North = doorWall.transform.Find("North").gameObject;
        GameObject South = doorWall.transform.Find("South").gameObject;
        GameObject West = doorWall.transform.Find("West").gameObject;
        GameObject East = doorWall.transform.Find("East").gameObject;

        int i = 0;
        if (!_room.doorTop) Destroy(North);
        else
        {
            _room.door_All[i] = North.transform.Find("Door").gameObject;
            _room.door_All[i].AddComponent<Door>().Name = DoorName.North;
            i++;
        }
        if (!_room.doorBot) Destroy(South);
        else
        {
            _room.door_All[i] = South.transform.Find("Door").gameObject;
            _room.door_All[i].AddComponent<Door>().Name = DoorName.South;
            i++;
        }
        if (!_room.doorLeft) Destroy(West);
        else
        {
            _room.door_All[i] = West.transform.Find("Door").gameObject;
            _room.door_All[i].AddComponent<Door>().Name = DoorName.West;
            i++;
        }
        if (!_room.doorRight) Destroy(East);
        else
        {
            _room.door_All[i] = East.transform.Find("Door").gameObject;
            _room.door_All[i].AddComponent<Door>().Name = DoorName.East;
            i++;
        }
    }
}


// ==============================================================
// RoomManager
// Have Map and Minimap data, Almost BattleManager 
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-16
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
    //모든 방 모음
    public GameObject[,] Map_Data
    {
        get { return map_Data; }
        set
        {
            //map_Data = new GameObject[gridSizeX, gridSizeY];
            map_Data = value;
            //map data가 세팅될때 미니맵의 모양도 지정한다.
            miniMap.InitMiniMap();
        }
    }

    private GameObject[,] map_Data;

    public int player_PosX, player_PosY; //플레이어의 위치
    public int gridSizeX_Cen, gridSizeY_Cen; //그리드 중앙
    public int gridSizeX, gridSizeY; //전체 그리드 크기
    public int playerGridPosX, playerGridPosY; //플레이어의 그리드 위치

    public MiniMap miniMap;

    public ScreenTransitions screenTransitions;

    public GameObject player;

    public GameObject Mana_Large;
    public GameObject Hp;
    public List<Database.Inventory> items = new List<Database.Inventory>();
    public int mana = 0;
    public int hp = 0;

    private GameObject uiRoot;
    public GameObject resultPop;
    public BattleStatus battleStatus;

    private void Awake()
    {
        player_PosX = 0;
        player_PosY = 0;
        playerGridPosX = 0;
        playerGridPosY = 0;
        miniMap = GameObject.FindGameObjectWithTag("MiniMap").GetComponentInChildren<MiniMap>();
        miniMap.RoomManager = GetComponent<RoomManager>();
        screenTransitions = GameObject.FindGameObjectWithTag("ScreenTransitions").GetComponent<ScreenTransitions>();
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<BoxCollider2D>().enabled = false;
        uiRoot = GameObject.Find("UI Root").gameObject;
        resultPop = uiRoot.transform.Find("ResultPopup").gameObject;
        battleStatus = uiRoot.transform.Find("Status").GetComponent<BattleStatus>();
        resultPop.SetActive(false);
        ResourceLoad();
    }


    private void ResourceLoad()
    {
        Mana_Large = Resources.Load("Object/Mana_Large") as GameObject;
        Hp = Resources.Load("Object/Hp") as GameObject;
    }

    private void Start()
    {
        SoundManager.Inst.Ds_BGMPlayerDB(4);
        StartCoroutine(PlayStart());
    }

    private IEnumerator PlayStart()
    {
        Camera.main.GetComponent<CameraFollow>().enabled = false;
        player.transform.position = new Vector3(0.0f, 10.0f, 0.0f);
        StartCoroutine(Fade());
        float time = 0.0f;
        float y = 0.0f;
        int count = 0;
        while (time <= 2.0f)
        {
            if (time >= 0.5f && count == 0)
            {
                count++;
                StartCoroutine(GameManager.Inst.ParticlePlay("Effect/DustExplosion", new Vector2(0.0f, -0.5f)));
            }
            time += Time.deltaTime;
            y = Mathf.Lerp(player.transform.position.y, 0.0f, time * 0.5f);
            player.transform.position = new Vector3(0.0f, y, 0.0f);
            yield return null;
        }
        player.GetComponent<BoxCollider2D>().enabled = true;
        Camera.main.GetComponent<CameraFollow>().enabled = true;
    }

    //플레이어의 위치를 세팅
    public void SetPlayerPos(int _player_PosX, int _player_PosY)
    {
        player_PosX = _player_PosX;
        player_PosY = _player_PosY;
        playerGridPosX = gridSizeX_Cen + player_PosX;
        playerGridPosY = gridSizeY_Cen + player_PosY;
        miniMap.UpdateMiniMap();

    }
    public IEnumerator Fade()
    {
        StartCoroutine(screenTransitions.Fade(0.5f, false));
        yield return null;
    }
    //그리드 데이터를 세팅
    public void SetGridData(int _gridSizeX_Cen, int _gridSizeY_Cen, int _gridSizeX, int _gridSizeY)
    {
        gridSizeX_Cen = _gridSizeX_Cen;
        gridSizeY_Cen = _gridSizeY_Cen;
        gridSizeX = _gridSizeX;
        gridSizeY = _gridSizeY;
        playerGridPosX = _gridSizeX_Cen;
        playerGridPosY = _gridSizeY_Cen;
    }

    //플레이어 위치에서 주변 모든 방을 검색
    public Room[] PlayerLocationAroundRoomInMap()
    {
        Room[] temp = new Room[4];
        Room room = Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>();

        temp[0] = room.doorBot ? Map_Data[playerGridPosX, playerGridPosY - 1].GetComponent<Room>() : null;
        temp[1] = room.doorRight ? Map_Data[playerGridPosX + 1, playerGridPosY].GetComponent<Room>() : null;
        temp[2] = room.doorTop ? Map_Data[playerGridPosX, playerGridPosY + 1].GetComponent<Room>() : null;
        temp[3] = room.doorLeft ? Map_Data[playerGridPosX - 1, playerGridPosY].GetComponent<Room>() : null;

        return temp;
    }

    //현재 플레이어가 있는 방을 검색
    public Room PlayerLocationInMap()
    {
        return Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>();
    }

    //플레이어가 있는 방의 몬스터 데이터를 검색
    public List<GameObject> PlayerLocationRoomMonsterData()
    {
        return Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>().monsters;
    }

    //플레이어가 npc방에 있는가?
    public bool PlayerIsNPCRoom()
    {
        return Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>().roomType == RoomType.NPC ? true : false;
    }

    //플레이어가 있는 방이 클리어 상태인가?
    public bool PlayerIsClearRoom()
    {
        return Map_Data[playerGridPosX, playerGridPosY].GetComponent<Room>().roomState == RoomState.Clear ? true : false;
    }

    //포탈이 있는 방이 몇개나 클리어 됬는지 계산
    public int PortalRoomClearCount()
    {
        int count = 0;

        foreach (GameObject obj in map_Data)
        {
            if (obj == null) continue;

            Room temp = obj.GetComponent<Room>();
            if (!temp.roomState.Equals(RoomState.Clear) || temp.roomType.Equals(RoomType.Normal)) continue;

            count++;
        }

        return count;
    }

    //클리어 된 방의 포탈을 켠다.
    public void PortalOn()
    {
#if UNITY_EDITOR
        Debug.Log(PortalRoomClearCount());
        Debug.Log("portal on");
#endif
        if (PortalRoomClearCount() >= 2)
        {
            foreach (GameObject obj in map_Data)
            {
                if (obj == null) continue;

                Room temp = obj.GetComponent<Room>();
                if (temp.roomType.Equals(RoomType.Normal)) continue;
                if (temp.roomState.Equals(RoomState.Clear)) temp.portal.GetComponent<Portal>().IsPortalOn = true;
            }
        }
    }

    //플레이어의 위치를 x,y로 텔레포트
    public void PlayerTeleportation(int _x, int _y)
    {
        if (_x != player_PosX || _y != player_PosY)
        {
            PlayerLocationInMap().gameObject.SetActive(false);
            MiniMapMinimalize();
            SetPlayerPos(_x, _y);
            //player_PosX = (int)_x;
            //player_PosY = (int)_y;
            //playerGridPosX = gridSizeX_Cen + player_PosX;
            //playerGridPosY = gridSizeY_Cen + player_PosY;

            PlayerLocationInMap().gameObject.SetActive(true);

            //미니맵 관리
            //miniMap.UpdateMiniMap();
            GameObject.FindGameObjectWithTag("Player").transform.position = PlayerLocationInMap().transform.position;
        }
    }

    //미니맵의 크기를 키웁니다.
    public void MiniMapMaximalize()
    {
        miniMap.Maximalize();
        //miniMap.button.GetComponent<BoxCollider>().enabled = false;
    }

    //미니맵을 작게 합니다.
    public void MiniMapMinimalize()
    {
        miniMap.Minimalize();
        //miniMap.button.GetComponent<BoxCollider>().enabled = true;
    }

    //TODO : 림모탈에 연결하기
    public void DropItem_Rimmotal(Vector3 _pos)
    {
        List<Database.Weapon> weapons = new List<Database.Weapon>();
        for (int i = 0; i < Database.Inst.weapons.Count; i++)
        {
            if (Database.Inst.weapons[i].Class != CLASS.갑옷 && Database.Inst.weapons[i].rarity != RARITY.레전드)
            {
                bool isExist = false;
                //인벤토리 검사
                for (int j = 0; j < Database.Inst.playData.inventory.Count; j++)
                {
                    if (Database.Inst.playData.inventory[j].name == Database.Inst.weapons[i].name)
                    {
                        isExist = true;
                    }
                }
                //현재 얻은 아이템 검사
                for (int j = 0; j < items.Count; j++)
                {
                    if (items[j].name == Database.Inst.weapons[i].name)
                    {
                        isExist = true;
                    }
                }
                //존재하지 않을 경우 생성
                if (!isExist)
                {
                    weapons.Add(new Database.Weapon(Database.Inst.weapons[i]));
                }
            }
        }
        if (weapons.Count <= 0) return;

        int itemCount = weapons.Count;
        int rand = Random.Range(0, itemCount);
        string imagePath = string.Empty;

        imagePath = weapons[rand].imageName;
        items.Add(new Database.Inventory(weapons[rand]));

        //sprite로 imagePath적용 
        GameObject gameObject = new GameObject("Item", typeof(SpriteRenderer));
        gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Object/DropItem");
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
        gameObject.transform.SetParent(PlayerLocationInMap().transform);
        Vector3 temp = Random.insideUnitCircle * 0.4f;
        gameObject.transform.position = _pos + temp;
        gameObject.transform.rotation = Quaternion.identity;
        PlayerLocationInMap().items.Add(gameObject);

        weapons.Clear();
    }

    public void DropItem(bool isMana, Vector3 _pos)
    {
        if (!isMana)
        {
            if (Random.Range(0, 100) <= 10)
            {
                int itemCount = Database.Inst.weapons.Count + Database.Inst.armors.Count;
                int rand = Random.Range(0, itemCount);
                string imagePath = string.Empty;
                if (rand < Database.Inst.weapons.Count)
                {
                    imagePath = Database.Inst.weapons[rand].imageName;
                    items.Add(new Database.Inventory(Database.Inst.weapons[rand]));
                }
                else
                {
                    rand -= Database.Inst.weapons.Count;
                    imagePath = Database.Inst.armors[rand].imageName;
                    items.Add(new Database.Inventory(Database.Inst.armors[rand]));
                }
                //sprite로 imagePath적용 
                GameObject gameObject = new GameObject("Item", typeof(SpriteRenderer));
                gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Object/DropItem");
                gameObject.GetComponent<SpriteRenderer>().sortingOrder = 3;
                gameObject.transform.SetParent(PlayerLocationInMap().transform);
                Vector3 temp = Random.insideUnitCircle * 0.4f;
                gameObject.transform.position = _pos + temp;
                gameObject.transform.rotation = Quaternion.identity;
                PlayerLocationInMap().items.Add(gameObject);
            }
        }
        else
        {
            if (Random.Range(0, 10) <= 3)
            {
                Vector3 temp = Random.insideUnitCircle * 0.4f;
                if (Random.Range(0, 3) <= 1)
                {
                    mana += 20;
                    PlayerLocationInMap().items.Add(Instantiate(Mana_Large, _pos + temp, Quaternion.identity, PlayerLocationInMap().transform));
                }
                else
                {
                    hp += 5;
                    PlayerLocationInMap().items.Add(Instantiate(Hp, _pos + temp, Quaternion.identity, PlayerLocationInMap().transform));
                }
            }
        }
    }

    public IEnumerator GatheringItems(List<GameObject> gameObjects, float _playTime)
    {
        Vector3 pos = new Vector3();
        float time = 0.0f;
        //GameManager.Inst.EndGame_Get_Item();
        if (hp != 0)
        {
            player.GetComponent<Player>().HP+=hp;
            hp = 0;
        }
        
        battleStatus.ChangeAddMpLabel(mana);

        while (time <= _playTime)
        {
            if (gameObjects.Count.Equals(0)) break;

            time += Time.deltaTime;
            for (int i = 0; i < gameObjects.Count; i++)
            {
                pos = Vector3.Lerp(gameObjects[i].transform.position, player.transform.position, time * (1.0f / _playTime));
                gameObjects[i].transform.position = pos;
                if ((player.transform.position - gameObjects[i].transform.position).magnitude <= 0.2f)
                {
                    Destroy(gameObjects[i]);
                    gameObjects.RemoveAt(i);
                }
            }

            yield return null;
        }
    }

    public void OpenResultPop(bool playerIsDead)
    {
        if (!playerIsDead)
        {
            GameManager.Inst.Mp += mana;
            GameManager.Inst.Insert_Inventory_Item(items);
        }
        else
        {
            GameManager.Inst.PlayerDeadToInitialData();
        }

        resultPop.GetComponent<ResultPop>().OnResult(mana, !playerIsDead);
        resultPop.SetActive(true);
    }

}

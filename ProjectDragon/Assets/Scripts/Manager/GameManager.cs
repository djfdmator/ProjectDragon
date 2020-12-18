
// ==============================================================
// GameManager
// Connect between Database and other Classes.
//
// 2019-12-26: Change Name DataTransaction to GameManager and Load & Save Method
// 2020-01-09: Add Sound Table Load Method
// 2019-02-20: block emblem
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2020-02-20
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Mono.Data.Sqlite;
using System.Reflection;
using System.Linq;
using System.IO;
using System.Data;
using UnityEngine.Networking;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject QuitObject;
    bool quit;
    float quittime;
    public Stack<string> Scenestack = new Stack<string>();

    //데이터베이스 관련
    public Database database;
    public IDbCommand DEB_dbcmd;
    public bool loadComplete = false;

    private string DBName = "/DS_Database_vr_108.sqlite";

    private void Awake()
    {
        //ScreensizeReadjust();
        database = Database.Inst;

        StartCoroutine(DataPhasing());
        //DataPhasing1();
        //DataBaseConnecting();
        //LoadAllTableData();
    }

    private void OnApplicationPause(bool pause)
    {
        //ScreensizeReadjust();
    }

    public void ScreensizeReadjust()
    {
        Screen.SetResolution(Screen.width * 16 / 9, Screen.width, false);

        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.fullScreen = true;
        Screen.autorotateToPortrait = false;

        Screen.autorotateToPortraitUpsideDown = false;

        Screen.autorotateToLandscapeLeft = true;

        Screen.autorotateToLandscapeRight = true;

        Camera.main.backgroundColor = Color.black;
        Camera camera = Camera.main;

        Rect rect = camera.rect;
        //float scaleheight = ((float)Screen.height / (float)Screen.width) / ((float)16 / (float)9);
        //float scalewidth = 1f / scaleheight;
        //if (scaleheight < 1)
        //{
        //    rect.height = scaleheight;
        //    rect.y = (1f - scaleheight) / 2f;
        //}
        //else
        //{
        //    rect.width = scalewidth;
        //    rect.x = (1f - scalewidth) / 2f;
        //}
        //camera.rect = rect;
    }

    void Update()
    {
        if (Application.platform.Equals(RuntimePlatform.Android))
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (0.2f + quittime > Time.time)
                {
                    QuitObject.SetActive(true);
                }
                else
                {
                    ButtonManager.TouchBackButton();
                    quittime = Time.time;
                }
            }
        }
    }

    #region Database Connecting

    IEnumerator DataPhasing()
    {
        string conn;
        if (Application.platform.Equals(RuntimePlatform.Android))
        {
            conn = Application.persistentDataPath + DBName;
            if (!File.Exists(conn))
            {
                using (UnityWebRequest unityWebRequest = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets" + DBName))
                {
                    unityWebRequest.downloadedBytes.ToString();
                    yield return unityWebRequest.SendWebRequest().isDone;
                    File.WriteAllBytes(conn, unityWebRequest.downloadHandler.data);
                }
                //UnityWebRequest unityWebRequest = UnityWebRequest.Get("jar:file://" + Application.dataPath + "!/assets/" + "DS_Database.sqlite"); 
                //unityWebRequest.downloadedBytes.ToString();
                //yield return unityWebRequest.SendWebRequest().isDone;
                //File.WriteAllBytes(conn, unityWebRequest.downloadHandler.data);

                //조금 있으면 사라질 코드 형식입니다.
                //WWW loadDB = new WWW("jar: file://" + Application.dataPath + "!/assets/" + "DS_Database.sqlite");
                //loadDB.bytesDownloaded.ToString();
                //while (!loadDB.isDone) { }
                //File.WriteAllBytes(conn, loadDB.bytes);
            }
        }
        DataBaseConnecting();
        LoadAllTableData();
        yield return null;
        loadComplete = true;
    }

    //DB에 연결합니다.
    void DataBaseConnecting()
    {
        string conn;
        if (Application.platform == RuntimePlatform.Android)
        {
            conn = "URI=file:" + Application.persistentDataPath + DBName;
        }
        else
        {
            conn = "URI=file:" + Application.dataPath + "/StreamingAssets" + DBName;
        }
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open();
        DEB_dbcmd = dbconn.CreateCommand();
    }

    private void OnApplicationQuit()
    {
        DEB_dbcmd.Dispose();
    }

    #endregion

    #region Load&Save

    //모든 테이블의 정보를 로드 합니다.
    void LoadAllTableData()
    {
        //TODO: 업적 테이블 로드
        Load_Encyclopedia_Monster_Table();
        Load_Encyclopedia_Weapon_Table();

        Load_Weapon_Table();
        Load_Armor_Table();
        Load_ActiveSkill_Table();
        Load_Normal_Monster_Table();
        Load_Rare_Monster_Table();
        LoadPlayerData();
    }

    //플레이어 데이터를 로드합니다.
    public void LoadPlayerData()
    {
        //플레이어 테이블 데이터 로드
        Load_Inventory_Table();
        //플레이어의 패시브 데이터를 로드
        //Load_Emblem_PlayData();
        //플레이어 기본 데이터 로드
        Load_PlayerPrefs_Data();
    }

    public void SavePlayerData()
    {
        //플레이어 테이블 데이터 저장
        Save_Inventory_Table();
        //플레이어 기본 데이터 저장
        Save_PlayerPrefs_Data();
        //플레이어의 패시브를 저장
        //Save_Emblem_PlayData();
#if UNITY_EDITOR
        Debug.Log("Save Player Data Complete");
#endif
    }

    #endregion

    public void ResetPlayerData()
    {
        ResetInventory();
        Save_Inventory_Table();
        //ResetEmblem();
        //Save_Emblem_PlayData();
        PlayerPrefs.DeleteAll();
    }

    //구글 데이터베이스 연결 함수
    #region Google_Connecting_Method

    public void PlayDataSavedOnGoogle()
    {

    }

    public void PlayDataLoadedOnGoogle()
    {

    }

    public void PlayDataResetAndSaveOnGoogle()
    {

    }

    #endregion

    #region 편의성 함수 모음

    //배틀 - 스테이지 끝나고 맵상의 모든 아이템을 인벤토리에 세팅하기 위한 함수 입니다.
    public void EndGame_Get_Item(List<Database.Weapon> _weapon_List, List<Database.Armor> _armor_List, int _mp = 0)
    {
        List<Database.Inventory> inventories = new List<Database.Inventory>();

        inventories = Convert_InventoryList_fromItem(_weapon_List, _armor_List);

        //인벤토리에 아이템 삽입
        Insert_Inventory_Item(inventories);

        Mp += _mp;
    }

    /// <summary>
    /// return weapons and armor with a single inventory.
    /// </summary>
    /// <param name="weapons">weapons table</param>
    /// <param name="armors">armors table</param>
    /// <returns></returns>
    public List<Database.Inventory> Convert_InventoryList_fromItem(List<Database.Weapon> weapons, List<Database.Armor> armors)
    {
        List<Database.Inventory> inventories = new List<Database.Inventory>();

        if (!weapons.Count.Equals(0))
        {
            foreach (Database.Weapon obj in weapons)
            {
                inventories.Add(new Database.Inventory(obj));
            }
        }
        if (!armors.Count.Equals(0))
        {
            foreach (Database.Armor obj in armors)
            {
                inventories.Add(new Database.Inventory(obj));
            }
        }
        return inventories;
    }

    /// <summary>
    /// return inventory items skill
    /// </summary>
    /// <param name="_EquipItem">인벤토리에 있는 장비 아이템</param>
    /// <returns></returns>
    public Database.Skill Convert_ItemtoSkill(Database.Inventory _EquipItem)
    {
        if (_EquipItem.Class.Equals(CLASS.갑옷))
        {
#if UNITY_EDITOR
            Debug.LogError("DataTransaction::Convert_ItemtoSkill(), Please give me an item with skill");
#endif
            return null;
        }

        return Database.Inst.skill[_EquipItem.skill_Index];
    }

    /// <summary>
    /// add items to inventory
    /// </summary>
    /// <param name="_inventories">items</param>
    public void Insert_Inventory_Item(List<Database.Inventory> _inventories)
    {
        // 아이템 중복되는 것 있으면 amount 컨트롤 해야함
        Database.Inst.playData.inventory.AddRange(_inventories);
    }


    /// <summary>
    /// 인벤토리에서 아이템을 삭제합니다.
    /// </summary>
    /// <param name="_item_Inventory_Num"></param>
    public void Delete_Inventory_Item(int _item_Inventory_Num)
    {
        if (Database.Inst.GetInventoryCount() <= _item_Inventory_Num) return;

        Database.Inst.playData.inventory.RemoveAt(_item_Inventory_Num);

        for (int i = _item_Inventory_Num; i < Database.Inst.GetInventoryCount(); i++)
        {
            Database.Inst.playData.inventory[i].num--;
        }
    }

    /// <summary>
    /// Check the Play data and return the status.
    /// </summary>
    /// <returns></returns>
    //public int CheckingPlayData()
    //{
    //    Database.PlayData playData = Database.Inst.playData;
    //    int playState = -1;
    //    if (playData.nickName.Equals(string.Empty))
    //    {
    //        //first play
    //        playState = 0;
    //    }
    //    else if (playData.sex.Equals(SEX.None))
    //    {
    //        //no play data
    //        playState = 1;
    //    }
    //    else
    //    {
    //        //has play data
    //        playState = 2;
    //    }

    //    return playState;
    //}

    /// <summary>
    /// Load Loading prefab
    /// </summary>
    /// <param name="isBattle"></param>
    public void Loading(bool isBattle)
    {
        if (GameObject.Find("LoadingScene") != null) return;

        GameObject loadingScene = Instantiate(Resources.Load("Loading/LoadingScene"), GameObject.Find("UI Root").transform) as GameObject;
        Loading loading = loadingScene.GetComponent<Loading>();
        loading.LoadSceneAsync(isBattle);
    }

    /// <summary>
    /// Load and play cartoon
    /// </summary>
    /// <param name="_cartoonPrefabName"></param>
    /// <returns></returns>
    public CartoonController CartoonPlay(string _cartoonPrefabName)
    {
        GameObject obj = Instantiate(Resources.Load("Cartoon/Cartoon"), GameObject.Find("UI Root").transform) as GameObject;
        CartoonController cartoonController = obj.GetComponent<CartoonController>();
        cartoonController.gameObject.SetActive(false);
        cartoonController.cartoonName = _cartoonPrefabName;
        cartoonController.gameObject.SetActive(true);

        return cartoonController;
    }

    /// <summary>
    /// Create the particles you want in the specified location.
    /// </summary>
    /// <param name="_path"></param>
    /// <param name="_pos"></param>
    /// <returns></returns>
    public IEnumerator ParticlePlay(string _path, Vector2 _pos)
    {
        GameObject particle = Resources.Load(_path) as GameObject;
        Instantiate(particle, _pos, Quaternion.identity);

        ParticleSystem particleSystem = particle.GetComponent<ParticleSystem>();
        if (!particleSystem.main.loop)
        {
            float time = 0.0f;
            while (time <= particleSystem.main.startLifetime.constant)
            {
                time += Time.deltaTime;
                yield return null;
            }
#if UNITY_EDITOR
            //Debug.Log("destroy");
#endif
            //Destroy(particle);
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log("particle is looping");
#endif
        }
    }

    /// <summary>
    /// Get Sound Data Path In Sound Table
    /// </summary>
    /// <param name="_num">Table Index</param>
    /// <param name="isBG">is BackGround Music?</param>
    /// <returns></returns>
    public string LoadSoundQue(int _num, bool isBG)
    {
        if (_num < 1)
        {
#if UNITY_EDITOR
            Debug.LogError("LoadSoundQue Method :: Out of Range SoundTable Index!");
#endif
            return string.Empty;
        }

        //경로 및 쿼리 지정
        string path = string.Empty;
        string sqlQuery = string.Empty;
        if (isBG)
        {
            path += "Sound/BGM/";
            sqlQuery = "SELECT * FROM BGSoundTable WHERE Num = ";
        }
        else
        {
            path += "Sound/SFX/";
            sqlQuery = "SELECT * FROM SFXSoundTable WHERE Num = ";
        }
        //쿼리 완성
        sqlQuery += _num;

        //데이터 read
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        reader.Read();

        //경로 완성
        path += reader.GetString(1);

        //reader close
        reader.Close();
        reader = null;

        return path;
    }

    /// <summary>
    /// Get Skin Data(Path or Name) In Skin Table
    /// </summary>
    /// <param name="_num">Table Index</param>
    /// <param name="isBG">is skin imagePath?</param>
    /// <returns></returns>
    public string LoadSkinData(int _num, bool _isPath)
    {
        if (_num < 1)
        {
#if UNITY_EDITOR
            Debug.LogError("LoadSoundQue Method :: Out of Range SkinTable Index!");
#endif
            return string.Empty;
        }

        //쿼리 지정
        string sqlQuery = "SELECT * FROM BGSoundTable WHERE Num = " + _num;

        //데이터 read
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        reader.Read();

        //get data
        string data = _isPath ? reader.GetString(2) : reader.GetString(1);

        //reader close
        reader.Close();
        reader = null;

        return data;
    }


    #endregion


    #region player data method & property
    //플레이어의 데이터를 연결해주는 property 들

    public Database.PlayData PlayData
    {
        get { return Database.Inst.playData; }
    }

    public int MaxHp
    {
        get { return Database.Inst.playData.maxHp; }
    }
    public int BaseHp
    {
        get { return Database.Inst.playData.baseHp; }
    }
    public int CurrentHp
    {
        get { return Database.Inst.playData.currentHp; }
        set
        {
            Database.Inst.playData.currentHp = value;
            if (CurrentHp <= 0)
            {
                //die
                //InitializePlayData();
                //죽는 씬전환
                StartCoroutine(GameEnd());
            }
            else if (MaxHp < CurrentHp) CurrentHp = MaxHp;

        }
    }
    public int Atk_Min
    {
        get { return Database.Inst.playData.atk_Min; }
    }
    public int Atk_Max
    {
        get { return Database.Inst.playData.atk_Max; }
    }
    public float MoveSpeed
    {
        get { return Database.Inst.playData.moveSpeed; }
    }
    public float AttackSpeed
    {
        get { return Database.Inst.playData.atk_Speed; }
    }
    public float AttackRange
    {
        get { return Database.Inst.playData.atk_Range; }
    }
    public float NuckBack_Power
    {
        get { return Database.Inst.playData.nuckBack_Power; }
    }
    public float NuckBack_Percentage
    {
        get { return Database.Inst.playData.nuckBack_Percentage; }
    }
    public int CurrentStage
    {
        get { return Database.Inst.playData.currentStage; }
        set
        {
            Database.Inst.playData.currentStage = value;

            if (Database.Inst.playData.currentStage > Database.Inst.playData.finalStage)
            {
                Database.Inst.playData.currentStage = Database.Inst.playData.finalStage;
            }
        }
    }
    public int Mp
    {
        get { return Database.Inst.playData.mp; }
        set
        {
            Database.Inst.playData.mp = value;

            if (Mp < 0) Mp = 0;
            else if (9999999 < Mp) Mp = 9999999;
        }
    }
    public SEX Sex
    {
        get { return Database.Inst.playData.sex; }
        set
        {
            Database.Inst.playData.sex = value;
        }
    }
    //플레이어 현재 스킬
    public Database.Skill CurrentSkill
    {
        get
        {
            if (PlayerEquipWeapon == null)
            {
#if UNITY_EDITOR
                Debug.Log("장착중인 무기가 없습니다.");
#endif
                return null;
            }
            Database.Skill skill = new Database.Skill(Database.Inst.skill[PlayerEquipWeapon.skill_Index]);
            skill.atk += skill.enhanceValue * PlayerEquipWeapon.enhanceLevel;

            return skill;
        }
    }
    //현재 장착 무기
    public Database.Weapon CurrentEquipWeapon
    {
        get
        {
            if (PlayerEquipWeapon == null)
            {
#if UNITY_EDITOR
                Debug.Log("장착중인 무기가 없습니다.");
#endif
                return null;
            }
            return Database.Inst.weapons[PlayerEquipWeapon.DB_Num];
        }
    }
    //현재 장착 방어구
    public Database.Armor CurrentEquipArmor
    {
        get
        {
            if (PlayerEquipArmor == null)
            {
#if UNITY_EDITOR
                Debug.Log("장착중인 방어구가 없습니다.");
#endif
                return null;
            }
            return Database.Inst.armors[PlayerEquipArmor.DB_Num];
        }
    }
    //무기 장착 해제
    public Database.Inventory PlayerEquipWeapon
    {
        get
        {
            if (Database.Inst.playData.equiWeapon_InventoryNum.Equals(-1))
            {
#if UNITY_EDITOR
                Debug.Log("장착중인 무기가 없습니다.");
#endif
                return null;
            }
            return Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        }
        set
        {
            if (value != null && !value.Class.Equals(CLASS.갑옷))
            {
                Database.Weapon weapon = Database.Inst.weapons[value.DB_Num];
                Database.Inst.playData.equiWeapon_InventoryNum = value.num;
                Database.Inst.playData.atk_Min = weapon.atk_Min + weapon.enhanceValue * value.enhanceLevel;
                Database.Inst.playData.atk_Max = weapon.atk_Max + weapon.enhanceValue * value.enhanceLevel;
                Database.Inst.playData.atk_Speed = weapon.atk_Speed;
                Database.Inst.playData.atk_Range = weapon.atk_Range;
                Database.Inst.playData.nuckBack_Power = weapon.nuckback_Power;
                Database.Inst.playData.nuckBack_Percentage = weapon.nuckback_Percentage;

                if (!value.option_Index.Equals(-1))
                {
                    //옵션이 붙어 있으면 옵션 적용
                    Database.OptionTable option = LoadOptionData(weapon.optionTableName, value.option_Index);
                    ApplyOption(option);
                }
            }
        }
    }
    //방어구 장착 해제
    public Database.Inventory PlayerEquipArmor
    {
        get
        {
            if (Database.Inst.playData.equiArmor_InventoryNum.Equals(-1))
            {
#if UNITY_EDITOR
                Debug.Log("장착중인 방어구가 없습니다.");
#endif
                return null;
            }
            return Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum];
        }
        set
        {
            if (value != null && value.Class.Equals(CLASS.갑옷))
            {
                int preArmorHP = CurrentEquipArmor.hp;
                Database.Armor armor = Database.Inst.armors[value.DB_Num];
                Database.Inst.playData.equiArmor_InventoryNum = value.num;
                Database.Inst.playData.maxHp = armor.hp + BaseHp;
                Database.Inst.playData.currentHp = MaxHp;
                
                if (!value.option_Index.Equals(-1))
                {   //옵션이 붙어 있으면 옵션 적용
                    Database.OptionTable option = LoadOptionData(armor.optionTableName, value.option_Index);
                    ApplyOption(option);
                }
            }
        }
    }
    #endregion


    #region Option

    /// <summary>
    /// Apply Enhanced Items option
    /// </summary>
    /// <param name="_optionTableName"></param>
    /// <param name="_num"></param>
    /// <returns></returns>
    //옵션 데이터 로드
    private Database.OptionTable LoadOptionData(string _optionTableName, int _num)
    {
        string sqlQuery = "SELECT * FROM " + _optionTableName + " WHERE Num = " + _num;
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        reader.Read();

        int Num = reader.GetInt32(0);
        int Parameter = reader.GetInt32(1);
        string Description = reader.GetString(2);
        float Percentage = reader.GetFloat(3);
        string MethodName = reader.GetString(4);

        reader.Close();
        reader = null;
        return new Database.OptionTable(Num, Parameter, Description, Percentage, MethodName);
    }
    //옵션 적용
    private void ApplyOption(Database.OptionTable option)
    {
        Type type = this.GetType();
        MethodInfo method = type.GetMethod(option.methodName);
        if (method != null)
        {
            method.Invoke(this, new object[] { option.parameter });
        }
    }

    //옵션 함수 모음
    #region Normal_weapon Method
    private void IncreaseDamage(int param)
    {
        Database.Inst.playData.atk_Min += param;
        Database.Inst.playData.atk_Max += param;
    }
    private void IncreaseAttackSpeed(int param)
    {
        Database.Inst.playData.atk_Speed += param;
    }

    #endregion
    #region Normal_Armor Method
    private void IncreaseHp(int param)
    {
        //최대 체력과 현재 체력 다 올라감
        Database.Inst.playData.maxHp += param;
        CurrentHp += param;
    }
    private void IncreaseMoveSpeed(int param)
    {
        Database.Inst.playData.moveSpeed += Database.Inst.playData.moveSpeed * ((float)param / 100.0f);
    }
    private void IncreaseDamageDecrement(int param)
    {
        Database.Inst.playData.damage_Reduction += param;
    }

    #endregion

    #endregion

    /// <summary>
    /// 플레이어가 포기했을때 부르면 데이터가 초기화 됩니다.
    /// </summary>
    public void InitializePlayData()
    {
        InitialPlayData();
        SavePlayerData();
    }
    IEnumerator GameEnd()
    {
        //GameObject.FindGameObjectWithTag("Player").SetActive(false);
        yield return new WaitForSeconds(3.0f);
        //결과창 띄우기
        GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().OpenResultPop(true);
        //Debug.Log("GameEnd");
        yield return null;
    }

    public void PlayerDeadToInitialData()
    {
        Database.PlayData playData = Database.Inst.playData;

        playData.equiWeapon_InventoryNum = 0;
        playData.equiArmor_InventoryNum = 1;

        playData.maxHp = BaseHp;
        playData.currentHp = BaseHp;

        playData.moveSpeed = 1.0f;
        playData.currentStage = 0;
        playData.mp = 500;

        InitializePlayerStat();

        if (playData.inventory.Count > 2)
        {
            playData.inventory.RemoveRange(2, playData.inventory.Count - 2);
        }
    }
    #region 공사중 - 초기화 구조 바꿔야 함

    /// <summary>
    /// initialize player data
    /// </summary>
    private void InitialPlayData()
    {
        //ResetInventory();
        //ResetEmblem();
        Database.PlayData playData = Database.Inst.playData;

        playData.isMachineVibration = true;
        playData.isScreenVibration = true;
        playData.BGM_Volume = 0.8f;
        playData.SFX_Volume = 0.3f;

        //playData.nickName = string.Empty;
        playData.equiWeapon_InventoryNum = 0;
        playData.equiArmor_InventoryNum = 1;

        playData.maxHp = BaseHp;
        playData.currentHp = BaseHp;
#if UNITY_EDITOR
        Debug.Log("BaseHp "+ BaseHp);
        Debug.Log("currentHp " + playData.currentHp);
#endif
        playData.moveSpeed = 1.0f;
        playData.currentStage = 0;
        playData.mp = 500;
        playData.sex = SEX.None;

        playData.skin = 0;

        playData.atk_Max = 0;
        playData.atk_Min = 0;
        playData.atk_Range = 0.0f;
        playData.atk_Speed = 0.0f;
        playData.nuckBack_Percentage = 0.0f;
        playData.nuckBack_Power = 0.0f;
        //InitializePlayerStat();

        playData.resist_Fire = false;
        playData.resist_Water = false;
        playData.resist_Poison = false;
        playData.resist_Electric = false;
        playData.attackType_Fire = false;
        playData.attackType_Water = false;
        playData.attackType_Poison = false;
        playData.attackType_Electric = false;
        playData.damage_Reduction = 0.0f;
    }

    //아이템에 의한 능력치 조정
    private void InitializePlayerStat()
    {
        PlayerEquipWeapon = PlayerEquipWeapon;
        PlayerEquipArmor = PlayerEquipArmor;
    }

    //죽었을때 인벤토리 초기화
    private void ResetInventory()
    {
        Database.Inst.playData.inventory.RemoveRange(0, Database.Inst.playData.inventory.Count);
        //database.playData.inventory.Add(new Database.Inventory(database.weapons[0]));
        //database.playData.inventory.Add(new Database.Inventory(database.armors[0]));
    }
    /// <summary>
    /// reset emblem table and player data, if unlocked emblemes are retained
    /// </summary>
    //private void ResetEmblem()
    //{
    //    List<Database.Emblem> emblem = database.playData.emblem;
    //    for (int i = 0; i < emblem.Count; i++)
    //    {
    //        int Status = (int)emblem[i].status;

    //        if (Status > 1)
    //        {
    //            database.playData.emblem[i].status = EMBLEM_STATUS.Unlock;
    //            Status = 1;
    //        }
    //    }
    //}

    /// <summary>
    /// give Basic weapon and armor - 임시
    /// </summary>
    public void GivePlayerBasicItem(CLASS _class)
    {
        int weapon = 0;
        if (!_class.Equals(CLASS.검)) weapon = 1;
        Database.Inst.playData.inventory.Clear();
        Database.Inst.playData.inventory.Add(new Database.Inventory(Database.Inst.weapons[weapon]));
        Database.Inst.playData.inventory.Add(new Database.Inventory(Database.Inst.armors[0]));
        InitializePlayerStat();
    }
#endregion

    //테스트 완료
#region Database_Load_Player_Data
    //플레이어 데이터 로드 함수

    //플레이어 프리팹 로드
    void Load_PlayerPrefs_Data()
    {
        if (PlayerPrefs.HasKey("save"))
        {
            InitialPlayData();

            Database.PlayData playData = Database.Inst.playData;
            playData.isMachineVibration = PlayerPrefs.GetInt("isMachineVibration") == 1 ? true : false;
            playData.isScreenVibration = PlayerPrefs.GetInt("isScreenVibration") == 1 ? true : false;
            playData.BGM_Volume = PlayerPrefs.GetFloat("BGM_Volume");
            playData.SFX_Volume = PlayerPrefs.GetFloat("SFX_Volume");

            //playData.nickName = PlayerPrefs.GetString("nickName");
            playData.currentHp = PlayerPrefs.GetInt("currentHp");
            playData.mp = PlayerPrefs.GetInt("mp");
            playData.sex = (SEX)PlayerPrefs.GetInt("sex");
            playData.moveSpeed = PlayerPrefs.GetFloat("moveSpeed");
            playData.currentStage = PlayerPrefs.GetInt("currentStage");
            playData.equiWeapon_InventoryNum = PlayerPrefs.GetInt("equiWeapon_InventoryNum");
            playData.equiArmor_InventoryNum = PlayerPrefs.GetInt("equiArmor_InventoryNum");
            playData.skin = PlayerPrefs.GetInt("skin");

            InitializePlayerStat();
        }
        else
        {
            InitialPlayData();
            InitializePlayerStat();
        }
    }

    //인벤토리 테이블 로드
    void Load_Inventory_Table()
    {
        string sqlQuery = "SELECT * FROM Inventory";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            int DB_Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            RARITY Rarity = (RARITY)(reader.GetInt32(count++));
            CLASS Class = (CLASS)(reader.GetInt32(count++));
            int ItemValue = reader.GetInt32(count++);
            string ImageName = reader.GetString(count++);
            int skill_Index = reader.GetInt32(count++);
            int option_Index = reader.GetInt32(count++);
            bool isNew = reader.GetInt32(count++) == 1 ? true : false;
            int enhanceLevel = reader.GetInt32(count++);

            Database.Inst.playData.inventory.Add(new Database.Inventory(Num, DB_Num, Name, Rarity, Class, ItemValue, ImageName, skill_Index, option_Index, isNew, enhanceLevel));
        }
        reader.Close();
        reader = null;
    }

    //엠블럼 테이블 로드
    //void Load_Emblem_PlayData()
    //{
    //    string sqlQuery = "SELECT * FROM Emblem";
    //    DEB_dbcmd.CommandText = sqlQuery;
    //    IDataReader reader = DEB_dbcmd.ExecuteReader();
    //    while (reader.Read())
    //    {
    //        int count = 0;
    //        int Num = reader.GetInt32(count++);
    //        string Name = reader.GetString(count++);
    //        EMBLEM_STATUS Status = (EMBLEM_STATUS)reader.GetInt32(count++);
    //        string Description = reader.GetString(count++);
    //        int Parameter1 = reader.GetInt32(count++);
    //        int Parameter2 = reader.GetInt32(count++);
    //        string ImageName = reader.GetString(count++);
    //        string MethodName = reader.GetString(count++);

    //        database.playData.emblem.Add(new Database.Emblem(Num, Name, Status, Description, Parameter1, Parameter2, ImageName, MethodName));
    //    }
    //    reader.Close();
    //    reader = null;
    //}
#endregion

    //테스트 완료
#region Database_Save_Player_Data

    public void Save_PlayerPrefs_Data()
    {
        Database.PlayData playData = Database.Inst.playData;

        PlayerPrefs.SetInt("save", 1);
        PlayerPrefs.SetInt("isMachineVibration", playData.isMachineVibration ? 1 : 0);
        PlayerPrefs.SetInt("isScreenVibration", playData.isScreenVibration ? 1 : 0);
        PlayerPrefs.SetFloat("BGM_Volume", playData.BGM_Volume);
        PlayerPrefs.SetFloat("SFX_Volume", playData.SFX_Volume);

        //PlayerPrefs.SetString("nickName", playData.nickName);
        PlayerPrefs.SetInt("currentHp", playData.currentHp);
        PlayerPrefs.SetInt("mp", playData.mp);
        PlayerPrefs.SetInt("sex", (int)playData.sex);
        PlayerPrefs.SetFloat("moveSpeed", playData.moveSpeed);
        PlayerPrefs.SetInt("currentStage", playData.currentStage);
        PlayerPrefs.SetInt("equiWeapon_InventoryNum", playData.equiWeapon_InventoryNum);
        PlayerPrefs.SetInt("equiArmor_InventoryNum", playData.equiArmor_InventoryNum);
        PlayerPrefs.SetInt("skin", playData.skin);

        PlayerPrefs.Save();
    }

    private void Save_Inventory_Table()
    {
        //Reset Table
        string sqlQuery = "DELETE FROM Inventory";
        DEB_dbcmd.CommandText = sqlQuery;
        DEB_dbcmd.ExecuteNonQuery();

        //Insert Data into Table
        for (int i = 0; i < Database.Inst.playData.inventory.Count; i++)
        {
            int Num = Database.Inst.playData.inventory[i].num;
            int DB_Num = Database.Inst.playData.inventory[i].DB_Num;
            string Name = Database.Inst.playData.inventory[i].name;
            int Rarity = (int)Database.Inst.playData.inventory[i].rarity;
            int Class = (int)Database.Inst.playData.inventory[i].Class;
            int ItemValue = Database.Inst.playData.inventory[i].itemValue;
            string ImageName = Database.Inst.playData.inventory[i].imageName;
            int Skill_Index = Database.Inst.playData.inventory[i].skill_Index;
            int OptionIndex = Database.Inst.playData.inventory[i].option_Index;
            int isNew = Database.Inst.playData.inventory[i].isNew ? 1 : 0;
            int enhanceLevel = Database.Inst.playData.inventory[i].enhanceLevel;

            sqlQuery = "INSERT INTO Inventory(Num, DB_Num, Name, Rarity, Class, ItemValue, ImageName, Skill_Index, OptionIndex, isNew, EnhanceLevel) " +
                        "values(" + Num + "," + DB_Num + ",'" + Name + "'," + Rarity + "," + Class + "," + ItemValue + ",'" + ImageName + "'," + Skill_Index + "," + OptionIndex + "," + isNew + "," + enhanceLevel + ")";
            DEB_dbcmd.CommandText = sqlQuery;
            DEB_dbcmd.ExecuteNonQuery();
        }
    }

    // 쿼리 에러 뜸
    //void Save_Emblem_PlayData()
    //{
    //    List<Database.Emblem> emblem = database.playData.emblem;
    //    //Insert Data into Table
    //    for (int i = 0; i < emblem.Count; i++)
    //    {
    //        int Status = (int)emblem[i].status;

    //        string sqlQuery = "UPDATE Emblem" +
    //                          " SET Status = " + Status +
    //                          " WHERE Num = " + i;
    //        DEB_dbcmd.CommandText = sqlQuery;
    //        DEB_dbcmd.ExecuteNonQuery();
    //    }
    //}

    private void Save_Achievement_Table()
    {
        //Reset Table
        string sqlQuery = "DELETE FROM Achievement";
        DEB_dbcmd.CommandText = sqlQuery;
        DEB_dbcmd.ExecuteNonQuery();

        //Insert Data into Table
        for (int i = 0; i < Database.Inst.achievementList.Count; i++)
        {
            int Num = Database.Inst.playData.inventory[i].num;
            int DB_Num = Database.Inst.playData.inventory[i].DB_Num;
            string Name = Database.Inst.playData.inventory[i].name;
            int Rarity = (int)Database.Inst.playData.inventory[i].rarity;
            int Class = (int)Database.Inst.playData.inventory[i].Class;
            int ItemValue = Database.Inst.playData.inventory[i].itemValue;
            string ImageName = Database.Inst.playData.inventory[i].imageName;
            int Skill_Index = Database.Inst.playData.inventory[i].skill_Index;
            int OptionIndex = Database.Inst.playData.inventory[i].option_Index;
            int isNew = Database.Inst.playData.inventory[i].isNew ? 1 : 0;
            int enhanceLevel = Database.Inst.playData.inventory[i].enhanceLevel;

            sqlQuery = "INSERT INTO Inventory(Num, DB_Num, Name, Rarity, Class, ItemValue, ImageName, Skill_Index, OptionIndex, isNew, EnhanceLevel) " +
                        "values(" + Num + "," + DB_Num + ",'" + Name + "'," + Rarity + "," + Class + "," + ItemValue + ",'" + ImageName + "'," + Skill_Index + "," + OptionIndex + "," + isNew + "," + enhanceLevel + ")";
            DEB_dbcmd.CommandText = sqlQuery;
            DEB_dbcmd.ExecuteNonQuery();
        }
    }

    private void Save_Encyclopedia_Monster_Table()
    {
        //Reset Table
        string sqlQuery = "DELETE FROM Achievement";
        DEB_dbcmd.CommandText = sqlQuery;
        DEB_dbcmd.ExecuteNonQuery();

        //Insert Data into Table
        for (int i = 0; i < Database.Inst.encyclopedia_MonsterList.Count; i++)
        {
            sqlQuery = string.Format("");
            DEB_dbcmd.CommandText = sqlQuery;
            DEB_dbcmd.ExecuteNonQuery();
        }
    }

    private void Save_Encyclopedia_Weapon_Table()
    {
        //Reset Table
        string sqlQuery = "DELETE FROM Achievement";
        DEB_dbcmd.CommandText = sqlQuery;
        DEB_dbcmd.ExecuteNonQuery();

        //Insert Data into Table
        for (int i = 0; i < Database.Inst.encyclopedia_WeaponList.Count; i++)
        {

            sqlQuery = string.Format("");
            DEB_dbcmd.CommandText = sqlQuery;
            DEB_dbcmd.ExecuteNonQuery();
        }
    }

    #endregion


    //readonly data
    #region Database_Load_Method

    //쿼리문으로 직접 들고 오는 함수 - 예시
    public void LoadWeaponData()
    {
        //if (database.GetInventoryCount() <= _num) return null;

        string sqlQuery = "SELECT * FROM WeaponTable WHERE Num = 1";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();

        reader.Read();
        int count = 0;
        int Num = reader.GetInt32(count++);
        string Name = reader.GetString(count++);
        RARITY Rarity = (RARITY)(reader.GetInt32(count++));
        string Rarity_Text = reader.GetString(count++);
        CLASS Class = (CLASS)(reader.GetInt32(count++));
        int Atk_Min = reader.GetInt32(count++);
        int Atk_Max = reader.GetInt32(count++);
        float Attack_Range = reader.GetFloat(count++);
        float Attack_Speed = reader.GetFloat(count++);
        float Nuckback_Power = reader.GetFloat(count++);
        float Nuckback_Percentage = reader.GetFloat(count++);
        int Item_Value = reader.GetInt32(count++);
        string Description = reader.GetString(count++);
        string ImageName = reader.GetString(count++);
        int Skill_Index = reader.GetInt32(count++);
        string OptionTableName = reader.GetString(count++);
        int EnhanceValue = reader.GetInt32(count++);

#if UNITY_EDITOR
        Debug.Log(new Database.Weapon(Num, Name, Rarity, Rarity_Text, Class, Atk_Min, Atk_Max, Attack_Range, Attack_Speed, Nuckback_Power, Nuckback_Percentage, Item_Value, Description, ImageName, Skill_Index, OptionTableName, EnhanceValue));
#endif
        //return new Database.Weapon(Num, Name, Rarity, Class, Damage, Attack_Count, Attack_Range, Attack_Speed, Nuckback, Item_Value, Description, ImageName, Skill_Index, OptionTableName));
        reader.Close();
        reader = null;
    }

    //readonly
    //데이터베이스에서 테이블들을 가져오는 함수들
    void Load_Weapon_Table()
    {
        string sqlQuery = "SELECT * FROM WeaponTable";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            RARITY Rarity = (RARITY)(reader.GetInt32(count++));
            string Rarity_Text = reader.GetString(count++);
            CLASS Class = (CLASS)(reader.GetInt32(count++));
            int Atk_Min = reader.GetInt32(count++);
            int Atk_Max = reader.GetInt32(count++);
            float Attack_Range = reader.GetFloat(count++);
            float Attack_Speed = reader.GetFloat(count++);
            float Nuckback_Power = reader.GetFloat(count++);
            float Nuckback_Percentage = reader.GetFloat(count++);
            int Item_Value = reader.GetInt32(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);
            int Skill_Index = reader.GetInt32(count++);
            string OptionTableName = reader.GetString(count++);
            int EnhanceValue = reader.GetInt32(count++);

            Database.Inst.weapons.Add(new Database.Weapon(Num, Name, Rarity, Rarity_Text, Class, Atk_Min, Atk_Max, Attack_Range, Attack_Speed, Nuckback_Power, Nuckback_Percentage, Item_Value, Description, ImageName, Skill_Index, OptionTableName, EnhanceValue));
        }
        reader.Close();
        reader = null;
    }

    void Load_Armor_Table()
    {
        string sqlQuery = "SELECT * FROM ArmorTable";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            RARITY Rarity = (RARITY)(reader.GetInt32(count++));
            string Rarity_Text = reader.GetString(count++);
            CLASS Class = (CLASS)(reader.GetInt32(count++));
            int Hp = reader.GetInt32(count++);
            int Item_Value = reader.GetInt32(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);
            string OptionTableName = reader.GetString(count++);

            Database.Inst.armors.Add(new Database.Armor(Num, Name, Rarity, Rarity_Text, Class, Hp, Item_Value, Description, ImageName, OptionTableName));
        }
        reader.Close();
        reader = null;
    }

    void Load_ActiveSkill_Table()
    {
        string sqlQuery = "SELECT * FROM ActiveSkill";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            SKILLTYPE SkillType = (SKILLTYPE)(reader.GetInt32(count++));
            int Atk = reader.GetInt32(count++);
            int MpCost = reader.GetInt32(count++);
            float CoolTime = reader.GetFloat(count++);
            float Skill_Range = reader.GetFloat(count++);
            float Skill_Duration = reader.GetFloat(count++);
            int Parameter = reader.GetInt32(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);
            int EnhanceValue = reader.GetInt32(count++);

            Database.Inst.skill.Add(new Database.Skill(Num, Name, SkillType, Atk, MpCost, CoolTime, Skill_Range, Skill_Duration, Parameter, Description, ImageName, EnhanceValue));
        }
        reader.Close();
        reader = null;
    }

    void Load_Normal_Monster_Table()
    {
        string sqlQuery = "SELECT * FROM Normal_Monster";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            Monster_Rarity monster_Rarity = (Monster_Rarity)(reader.GetInt32(count++));
            int Hp = reader.GetInt32(count++);
            float Move_Speed = reader.GetFloat(count++);
            int Atk = reader.GetInt32(count++);
            float Atk_Speed = reader.GetFloat(count++);
            float Atk_Range = reader.GetFloat(count++);
            int Ready_Time = reader.GetInt32(count++);
            int Cooltime = reader.GetInt32(count++);
            int Knock_Resist = reader.GetInt32(count++);
            int Atk_Count = reader.GetInt32(count++);
            int Drop_Mana_Min = reader.GetInt32(count++);
            int Drop_Mana_Max = reader.GetInt32(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);

            Database.Inst.normal_Monsters.Add(new Database.Normal_Monster(Num, Name, monster_Rarity, Hp, Move_Speed, Atk, Atk_Speed, Atk_Range, Ready_Time,
                Cooltime, Knock_Resist, Atk_Count, Drop_Mana_Min, Drop_Mana_Max, Description, ImageName));
        }
        reader.Close();
        reader = null;
    }

    void Load_Rare_Monster_Table()
    {
        string sqlQuery = "SELECT * FROM Rare_Monster";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int Num = reader.GetInt32(count++);
            string Name = reader.GetString(count++);
            Monster_Rarity monster_Rarity = (Monster_Rarity)(reader.GetInt32(count++));
            int Hp = reader.GetInt32(count++);
            float Move_Speed = reader.GetFloat(count++);
            int Atk = reader.GetInt32(count++);
            float Atk_Speed = reader.GetFloat(count++);
            float Atk_Range = reader.GetFloat(count++);
            float Ready_Time = reader.GetFloat(count++);
            int Cooltime = reader.GetInt32(count++);
            int Knock_Resist = reader.GetInt32(count++);
            int Atk_Count1 = reader.GetInt32(count++);
            int Atk_Count2 = reader.GetInt32(count++);
            int Skill_Cooltime = reader.GetInt32(count++);
            int Skill_Damage = reader.GetInt32(count++);
            int Drop_Mana_Min = reader.GetInt32(count++);
            int Drop_Mana_Max = reader.GetInt32(count++);
            string Description = reader.GetString(count++);
            string ImageName = reader.GetString(count++);

            Database.Inst.rare_Monsters.Add(new Database.Rare_Monster(Num, Name, monster_Rarity, Hp, Move_Speed, Atk, Atk_Speed, Atk_Range, Ready_Time,
                Cooltime, Knock_Resist, Atk_Count1, Atk_Count2, Skill_Cooltime, Skill_Damage, Drop_Mana_Min, Drop_Mana_Max, Description, ImageName));
        }
        reader.Close();
        reader = null;
    }

    private void Load_Achievement_Table()
    {
        Database.Inst.achievementList.Clear();

        string sqlQuery = "SELECT * FROM Achievement";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int num = reader.GetInt32(count++);
            string title = reader.GetString(count++);
            string description = reader.GetString(count++);
            string imageName = reader.GetString(count++);
            int isSuccess = reader.GetInt32(count++);
            int targetValue = reader.GetInt32(count++);
            int currentValue = reader.GetInt32(count++);


            Database.Inst.achievementList.Add(new Database.Achievement(num, title, description, imageName, isSuccess, targetValue,currentValue));
        }
        reader.Close();
        reader = null;
    }

    private void Load_Encyclopedia_Monster_Table()
    {
        Database.Inst.encyclopedia_MonsterList.Clear();

        string sqlQuery = "SELECT * FROM Encyclopedia_Monster";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int num = reader.GetInt32(count++);
            string name = reader.GetString(count++);
            string description = reader.GetString(count++);
            string imageName = reader.GetString(count++);
            int isSuccess = reader.GetInt32(count++);
           

            Database.Inst.encyclopedia_MonsterList.Add(new Database.Encyclopedia(num, name, description, imageName, isSuccess));
        }
        reader.Close();
        reader = null;
    }
    private void Load_Encyclopedia_Weapon_Table()
    {
        Database.Inst.encyclopedia_WeaponList.Clear();

        string sqlQuery = "SELECT * FROM Encyclopedia_Weapon";
        DEB_dbcmd.CommandText = sqlQuery;
        IDataReader reader = DEB_dbcmd.ExecuteReader();
        while (reader.Read())
        {
            int count = 0;
            int num = reader.GetInt32(count++);
            string name = reader.GetString(count++);
            string description = reader.GetString(count++);
            string imageName = reader.GetString(count++);
            int isSuccess = reader.GetInt32(count++);


            Database.Inst.encyclopedia_WeaponList.Add(new Database.Encyclopedia(num, name, description, imageName, isSuccess));
        }
        reader.Close();
        reader = null;
    }

    #endregion
}
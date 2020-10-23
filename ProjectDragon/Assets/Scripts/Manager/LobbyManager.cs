//////////////////////////////////////////////////////////MADE BY Koo KyoSeok///2019-12-16/////////////////////////////////////////////
//////////////////////////////////////////////////////////MODIFY BY Kim DongHa///2020-10-16/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum LobbyState { Nomal, Enchant, Lock, Decomposition }
public enum ItemState { 기본, 무기, 갑옷 }
public enum ItemRarity { 기본, 노말, 레어, 유니크, 레전드 }
public enum StageName { 스테이지1번, 스테이지2번, 스테이지3번, 스테이지4번, 보스스테이지 }
public class LobbyManager : MonoBehaviour
{
    bool statepanelbutton, equipstatepanelbutton;
    public string BattleScenename;
    public static LobbyManager inst;
    bool isnight = true;
    GameObject particle;
    public GameObject playerimg, equipCharactor, playerStat, equipanel;
    public AudioClip fire;
    public GameObject statpanel;

    public Maintenance maintenance = null;
    public OptionWindow optionWindow = null;

    public StageName developerStageSetting;

    #region equipobject
    public GameObject inventoryback, currentweapon, currentArmor, currentActive, itemscrollview, skinScrollView, euiptIcons, changeEquip, ItemInfo, equipBGI, DecompositionPanel, DecompositionCountLabel;
    public List<UISprite> Weapon, Armor, weaponicon, armoricon, activeicon;
    public int changeequipdata, currentEquipdata, selectData = -1, enchantJam, m_sortSelect;
    public LobbyState lobbystate = LobbyState.Nomal;
    public List<int> Selecteditem;
    public ItemState itemclassselect;
    public ItemRarity ItemRarityselect;
    public float sizeoption = 2;
    public List<GUITestScrollView> gUITestScrollViews;
    public static string teststring;
    string classname = "null";
    double distance;
    //public UILabel testlabel, testlabel2;
    public UILabel fontchecklabel;
    #endregion

    private void Awake()
    {
        inst = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        SoundManager.Inst.Ds_BGMPlayerDB(2);
        Cursor.lockState = CursorLockMode.None;
        GameManager.Inst.SavePlayerData();
        GameManager.Inst.name.ToString();
        GameManager.Inst.gameObject.ToString();
        LobbyObjectSet();
        LobbyStateInit();
        selectData = -1;
        SetplayerStat();
    }

    /// <summary>
    /// 로비에서의 오브젝트 관리
    /// </summary>
    public void LobbyObjectSet()
    {
        //폰트 테스트용
        //FontChangeAll();
        //fireobject = GameObject.Find(string.Format("UI Root/LobbyPanel/LobbyBGI/Fire"));
        //playeranimation = GameObject.Find(string.Format("UI Root/LobbyPanel/LobbyBGI/PlayerAnimation"));
        playerimg = GameObject.Find(string.Format("UI Root/LobbyPanel/Statpanel/Panel/playersizebox/Charactor"));
        equipCharactor = equipanel.transform.Find(string.Format("Charactorpanel/playersizebox/Charactor")).gameObject;
        statpanel = GameObject.Find(string.Format("UI Root/LobbyPanel/Statpanel/Sprite"));
        inventoryback = equipanel.transform.Find(string.Format("Inventoryback")).gameObject;
        ItemInfo = equipanel.transform.Find(string.Format("BGIPanel/ItemBGI")).gameObject;
        equipBGI = equipanel.transform.Find(string.Format("BGIPanel/BlackBGI")).gameObject;
        currentActive = equipanel.transform.Find(string.Format("Charactorpanel/IconBGIBack/ActiveIconBGI")).gameObject;
        currentArmor = equipanel.transform.Find(string.Format("Charactorpanel/IconBGIBack/ArmorIconBGI")).gameObject;
        currentweapon = equipanel.transform.Find(string.Format("Charactorpanel/IconBGIBack/EquipWeaponIconBGI")).gameObject;
        itemscrollview = equipanel.transform.Find("ItemWindow/ItemScrollView").gameObject;
        euiptIcons = GameObject.Find(string.Format("UI Root/LobbyPanel/IconBackgroundU/EquipItem"));
        playerStat = GameObject.Find(string.Format("UI Root/LobbyPanel/IconBackgroundU/Player"));
        //equipStatPanel = equipanel.transform.Find(string.Format("Charactorpanel/Statpanel/Sprite/StatBGI")).gameObject;
        skinScrollView = equipanel.transform.Find("ItemWindow/SkinScrollview").gameObject;
        DecompositionPanel = equipanel.transform.Find("Decompositionpanel").gameObject;
        DecompositionCountLabel = DecompositionPanel.transform.Find("DecompositionInfo/Descompositiontitle/DecompositionCountBGI/DecompositionCountLabel").gameObject;

        #region Maintenance
        if (maintenance == null) maintenance = GameObject.Find(string.Format("UI Root")).transform.Find(string.Format("Maintenance")).GetComponent<Maintenance>();
        maintenance.gameObject.SetActive(false);
        #endregion

        #region OptionPanel
        if (optionWindow == null) optionWindow = GameObject.Find(string.Format("UI Root")).transform.Find(string.Format("PopupWindow/OptionWindow")).GetComponent<OptionWindow>();
        optionWindow.gameObject.SetActive(false);
        #endregion
    } 

    /// <summary>
    /// 로비 초기화
    /// </summary>
    public void LobbyStateInit()
    {
        GameObject font = Resources.Load<GameObject>(string.Format("Font/Icon"));
        //UILabel[] buttonIconhead = GameObject.Find("UI Root/LobbyPanel/IconBackgroundR").GetComponentsInChildren<UILabel>();
        //for (int i = 0; i < buttonIconhead.Length; i++)
        //{
        //    FontChange(buttonIconhead[i], font.GetComponent<UILabel>().trueTypeFont);
        //}
        //player 체력, 마나, 스텟 조정
        //playerStat.transform.Find("HPCountBG/HP").GetComponent<UISprite>().fillAmount = (float)GameManager.Inst.CurrentHp / (float)GameManager.Inst.MaxHp;
        //playerStat.transform.Find("HPCountBG/Label").GetComponent<UILabel>().text = string.Format("{0}/{1}", GameManager.Inst.CurrentHp, GameManager.Inst.MaxHp);
        //playerStat.transform.Find("Mana/ManaCountBG/ManaCountLabel").GetComponent<UILabel>().text = Database.Inst.playData.mp.ToString();

        //SoundManager.Inst.Ds_BgmPlayer(LobbyBGM);
        itemclassselect = ItemState.무기;
        ItemRarityselect = ItemRarity.기본;
        Selecteditem = new List<int>();
        if (isnight)
        {
            //SoundManager.Inst.Ds_BgmPlayer(fire);
            //fireobject.SetActive(true);
        }
        else
        {
        }
        Debug.Log(Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name + "," + Database.Inst.playData.equiWeapon_InventoryNum.ToString());
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        //Weapon.Add(euiptIcons.transform.Find("Equip1/WeaponImg").GetComponent<UISprite>());
        //euiptIcons.transform.Find("Equip1/WeaponImg").GetComponent<UISprite>().spriteName = item.imageName;
        if (item.Class.Equals(CLASS.검))
        {
            classname = "Worrior";
            if (Database.Inst.playData.sex.Equals(SEX.Male))
            {
                playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
            else
            {
                //playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
        }
        else if (item.Class.Equals(CLASS.활))
        {
            classname = "Archer";

        }
        else if (item.Class.Equals(CLASS.지팡이))
        {
            classname = "Wizard";
        }
        string playerclass = string.Format("PlayerCharactor/Skin/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);
        Debug.Log(classname);
        //playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        ChangeItemIcon(currentweapon, item);
        //Weapon.Add(playerimg.transform.Find("Weapon").GetComponent<UISprite>());
        Weapon.Add(equipCharactor.transform.Find("Weapon").GetComponent<UISprite>());
        CharactorSkinSet(playerclass);
        SetWeapon();
        statepanelbutton = true;
        equipstatepanelbutton = true;
    }

    void Update()
    {
        //터치시 파티클생성
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 touchpoint011 = UICamera.currentCamera.ScreenToWorldPoint(Input.mousePosition);
            Debug.DrawRay(touchpoint011, transform.forward * 10, Color.red, 0.3f);
            RaycastHit2D hit0 = Physics2D.Raycast(touchpoint011, transform.forward, 10);
            if (hit0)
            {
                Debug.Log(hit0.transform.gameObject.name);
            }
        }
        if (Input.GetMouseButtonUp(0))
        {

        }
        //터치 갯수구분
        if (Input.touchCount > 0)
        {
            if (Input.touchCount.Equals(1))
            {
                distance = -1;
            }
            //터치가 두개일때 두개 모두 캐릭터를 터치중이라면 캐릭터 확대축소(1.0f~1.5f까지)
            else if (Input.touchCount.Equals(2))
            {
                Vector3 touchpoint0 = UICamera.currentCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
                Vector3 touchpoint1 = UICamera.currentCamera.ScreenToWorldPoint(Input.GetTouch(1).position);

                RaycastHit2D hit0 = Physics2D.Raycast(touchpoint0, transform.forward, 10);
                RaycastHit2D hit1 = Physics2D.Raycast(touchpoint1, transform.forward, 10);
                if (hit0 && hit1)
                {
                    if (hit0.transform.gameObject.name.Equals("playersizebox") && hit1.transform.gameObject.name.Equals("playersizebox"))
                    {
                        if (distance.Equals(-1))
                        {
                            distance = Vector3.Distance(touchpoint0, touchpoint1);
                            Debug.Log("call");
                        }
                        else
                        {
                            if (distance < Vector3.Distance(touchpoint0, touchpoint1))
                            {
                                float currentdistance = Vector3.Distance(touchpoint0, touchpoint1);
                                Vector3 imagescale = new Vector3(playerimg.transform.localScale.x - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.y - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.z - ((float)distance - currentdistance) / sizeoption);
                                if (imagescale.x <= 1.5 && imagescale.y <= 1.5)
                                {
                                    playerimg.transform.localScale = imagescale;
                                    equipCharactor.transform.localScale = imagescale;
                                    Debug.Log(imagescale.x + " ," + imagescale.y + " ," + imagescale.z);
                                }
                                else
                                {
                                    playerimg.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                                    equipCharactor.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                                    Debug.Log("one" + imagescale.x + " ," + imagescale.y + " ," + imagescale.z);
                                }
                                distance = Vector3.Distance(touchpoint0, touchpoint1);
                            }
                            else if (distance > Vector3.Distance(touchpoint0, touchpoint1))
                            {
                                float currentdistance = Vector3.Distance(touchpoint0, touchpoint1);
                                Vector3 imagescale = new Vector3(playerimg.transform.localScale.x - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.y - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.z - ((float)distance - currentdistance) / sizeoption);
                                if (imagescale.x > 0.5 && imagescale.y > 0.5)
                                {
                                    playerimg.transform.localScale = imagescale;
                                    equipCharactor.transform.localScale = imagescale;
                                    Debug.Log(imagescale.x + " ," + imagescale.y + " ," + imagescale.z);
                                }
                                else
                                {
                                    playerimg.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                                    equipCharactor.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                                    Debug.Log(imagescale.x + " ," + imagescale.y + " ," + imagescale.z + "half");
                                }
                                distance = Vector3.Distance(touchpoint0, touchpoint1);
                            }
                        }
                    }
                }
                if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended))
                {
                    distance = -1;
                }
            }
        }
        //GameObject.Find("Panel/Label").GetComponent<UILabel>().text = test;
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

    public void ButtonSound1()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
    }

    public void FontChange(UILabel _label, Font _font)
    {

        Transform[] objects = GameObject.Find("UI Root").GetComponentsInChildren<Transform>(true);
        if (_label != null)
        {
            _label.GetComponent<UILabel>().trueTypeFont = _font;
        }
    }

    public void FontChangeAll()
    {
        UILabel[] labels;
        Transform[] objects = GameObject.Find("UI Root").GetComponentsInChildren<Transform>(true);
        labels = GetComponentsInChildren<UILabel>();

        GameObject font = Resources.Load<GameObject>(string.Format("Font/Main"));
        Debug.Log(font.GetComponent<UILabel>().fontStyle);
        for (int i = 0; i < objects.Length; i++)
        {
            if (objects[i].gameObject.GetComponent<UILabel>() != null)
            {
                objects[i].gameObject.GetComponent<UILabel>().trueTypeFont = font.GetComponent<UILabel>().trueTypeFont;
            }
        }
    }

    public void MaintenanceButton()
    {
        maintenance.OpenMaintenance();
    }

    public void OptionButton()
    {
        optionWindow.OpenOptionWindow();
    }
    /// <summary>
    /// 초기 스텟판넬의 움직임위치 조정(해상도마다 조정필요)
    /// </summary>
    public void StatpanelSetup()
    {
        if (statepanelbutton)
        {
            UIButton.current.GetComponent<TweenPosition>().from = UIButton.current.transform.localPosition;
            UIButton.current.GetComponent<TweenPosition>().to = UIButton.current.transform.localPosition;
            UIButton.current.GetComponent<TweenPosition>().to.x = UIButton.current.GetComponent<TweenPosition>().to.x + UIButton.current.transform.Find("StatBGI").GetComponent<UISprite>().localSize.x - UIButton.current.GetComponent<UISprite>().localSize.x;
            Debug.Log(UIButton.current.GetComponent<TweenPosition>().from.x + "::" + UIButton.current.GetComponent<TweenPosition>().to.x.ToString() + "::" + UIButton.current.transform.Find("StatBGI").GetComponent<UISprite>().localSize.x);
            Debug.Log(UIButton.current.GetComponent<TweenPosition>().method.ToString());
            statepanelbutton = false;

        }
    }
    /// <summary>
    /// 초기 장착스텟판넬의 움직임 위치조정(해상도마다 조정필요)
    /// </summary>
    public void EquipStatpanelSetup()
    {
        if (equipstatepanelbutton)
        {
            UIButton.current.GetComponent<TweenPosition>().from = UIButton.current.transform.localPosition;
            UIButton.current.GetComponent<TweenPosition>().to = UIButton.current.transform.localPosition;
            UIButton.current.GetComponent<TweenPosition>().to.x = UIButton.current.GetComponent<TweenPosition>().to.x + UIButton.current.transform.Find("StatBGI").GetComponent<UISprite>().localSize.x - UIButton.current.GetComponent<UISprite>().localSize.x;
            Debug.Log(UIButton.current.GetComponent<TweenPosition>().from.x + "::" + UIButton.current.GetComponent<TweenPosition>().to.x.ToString() + "::" + UIButton.current.transform.Find("StatBGI").GetComponent<UISprite>().localSize.x);
            Debug.Log(UIButton.current.GetComponent<TweenPosition>().method.ToString());
            equipstatepanelbutton = false;

        }
    }
    /// <summary>
    /// 캐릭터의 스킨 조정
    /// </summary>
    /// <param name="playerclass"></param>
    public void CharactorSkinSet(string playerclass)
    {
        Debug.Log("PlayerClass::" + playerclass);
        Texture2D Skin = Resources.Load<Texture2D>(playerclass);
        //playerimg.GetComponent<UITexture>().mainTexture = Skin;
        equipCharactor.GetComponent<UITexture>().mainTexture = Skin;
        //GameObject.Find("UI Root").transform.Find("Skin/playersizebox/Charactor").GetComponent<UITexture>().mainTexture = Skin;
    }
    /// <summary>
    /// 뒤로가기 버튼 사용필요시
    /// </summary>
    public void TouchBackButton()
    {
        Debug.Log(ButtonManager.TouchBackButton());
    }
    /// <summary>
    /// 오브젝트 컨트롤 사용필요시
    /// </summary>
    public void ObjectControl()
    {
        ButtonManager.ObjectlistControl();
    }

    /// <summary>
    /// 장비를 바꾸었을때 동작할것
    /// </summary>
    public void ChangeEquip()
    {
        TouchBackButton();
        if (Database.Inst.playData.inventory[changeequipdata].Class.Equals(CLASS.갑옷))
        {
            Database.Inst.playData.equiArmor_InventoryNum = changeequipdata;
        }
        string classname = "null";
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        if (item.Class.Equals(CLASS.검))
        {
            classname = "Worrior";
            if (Database.Inst.playData.sex.Equals(SEX.Male))
            {
                playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
            else
            {
                playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
        }
        else if (item.Class.Equals(CLASS.활))
        {
            classname = "Archer";
        }
        else if (item.Class.Equals(CLASS.지팡이))
        {
            classname = "Wizard";
        }
        string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);
        playerimg.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        equipCharactor.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        //playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        ChangeItemIcon(currentweapon, item);
        currentweapon.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = Database.Inst.weapons[item.DB_Num].atk_Min.ToString();
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        playerimg.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        currentActive.transform.Find("IconIcon").GetComponent<UISprite>().spriteName = Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index].imageName;
        UpdateAllScrollview(0);
        GameManager.Inst.SavePlayerData();
    }
    /// <summary>
    /// 장비를바꾸어 아이콘을 바꾸어야할때 쓸것
    /// </summary>
    /// <param name="Icon">바꾸고싶은 대상오브젝트</param>
    /// <param name="data">오브젝트에 들어가야할 데이터</param>
    public void ChangeItemIcon(GameObject Icon, Database.Inventory data)
    {
        Icon.transform.Find("EquipIcon").GetComponent<UISprite>().spriteName = data.imageName;

        //if (data.isLock)
        //{
        //    Icon.transform.Find("IsLock").GetComponent<UISprite>().spriteName = "Lock";
        //}
        //else
        //{
        //    Icon.transform.Find("IsLock").GetComponent<UISprite>().spriteName = "Unlock";
        //}
    }

    /// <summary>
    /// 여러개의 스크롤뷰가 모두갱신되어야 할때 사용
    /// </summary>
    public void UpdateAllScrollview(int num)
    {
        foreach (GUITestScrollView scrollView in gUITestScrollViews)
        {
            scrollView.EV_UpdateAll();
            scrollView.Setposition(num);
        }

    }


    //}
    /// <summary>
    /// 현재의 스킨장면임을 푸시 및 초기화
    /// </summary>
    public void SkinState()
    {
        GameManager.Inst.Scenestack.Push("Skin");
    }
    /// <summary>
    /// 현재의 인첸트장면임을 푸시 및 초기화
    /// </summary>
    public void EnchantState()
    {
        GameManager.Inst.Scenestack.Push("Enchant");
        lobbystate = LobbyState.Enchant;
        itemclassselect = ItemState.기본;
        ItemRarityselect = ItemRarity.기본;
    }
    /// <summary>
    /// 현재 종료확인장면임을 푸시 및 초기화
    /// </summary>

    /// <summary>
    /// 분해 화면임을 푸시 및 초기화
    /// </summary>
    public void DecompositionState()
    {
        GameManager.Inst.Scenestack.Push("Decomposition");
        lobbystate = LobbyState.Decomposition;
        //equipanel.transform.Find("ItemWindow/FilterButtonBGI/Label").GetComponent<UILabel>().text = "분해";
        equipanel.transform.Find("ItemWindow/FilterButtonBGI").GetComponent<UIButton>().isEnabled = false;
        //equipanel.transform.Find("ItemWindow/WeaponButtonBGI/Label").GetComponent<UILabel>().text = "장비";
        equipanel.transform.Find("Charactorpanel").gameObject.SetActive(false);
        //inventoryback.transform.Find("ArrangementButtons/SkinButtonBGI").gameObject.SetActive(false);
        DecompositionPanel.gameObject.SetActive(true);
        itemclassselect = ItemState.무기;
        foreach (UISprite button in inventoryback.transform.Find("ArrangementButtons").GetComponentsInChildren<UISprite>())
        {
            button.spriteName = "정비 UI 탭 버튼";
        }
        inventoryback.transform.Find("ArrangementButtons/WeaponButtonBGI").GetComponent<UISprite>().spriteName = "정비 UI 탭 버튼 클릭";
        itemscrollview.SetActive(true);
        skinScrollView.SetActive(false);

    }
    /// <summary>
    /// 장비관리 화면임을 푸시 및 초기화
    /// </summary>
    public void NomalState()
    {
        GameManager.Inst.Scenestack.Push("EquipPanel");
        Debug.Log(GameManager.Inst.Scenestack);
        lobbystate = LobbyState.Nomal;
        itemclassselect = ItemState.무기;
        ItemRarityselect = ItemRarity.기본;
        UpdateAllScrollview(0);
    }

    /// <summary>
    /// 인벤토리를 수집한 순서로 정렬 후 스크롤뷰 갱신
    /// </summary>
    public void inventoryAcquisitionorder()
    {
        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
            {

                if (a.num > b.num)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            });
        UpdateAllScrollview(0);
    }
    /// <summary>
    /// 인베노리를 희귀도순으로 정렬 후 스크롤뷰갱신
    /// </summary>
    public void inventoryClassorder()
    {
        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
            {
                if (a.rarity > b.rarity)
                {
                    return 1;
                }
                else if (a.rarity.Equals(b.rarity))
                {
                    if (a.Class > b.Class)
                    {
                        return 1;
                    }
                    else if (a.Class.Equals(b.Class))
                    {
                        Debug.Log("EE");
                        return a.num.CompareTo(b.num);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            });
        UpdateAllScrollview(0);
    }
    /// <summary>
    /// 인벤토리를 클래스별로 정렬후 갱신
    /// </summary>
    public void inventorytypeorder()
    {
        Database.Inst.playData.inventory.Sort(delegate (Database.Inventory a, Database.Inventory b)
            {
                if (a.Class < b.Class)
                {
                    return 1;
                }
                else if (a.Class.Equals(b.Class))
                {
                    if (a.rarity > b.rarity)
                    {
                        return 1;
                    }
                    else if (a.rarity.Equals(b.rarity))
                    {
                        return a.num.CompareTo(b.num);
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            });
        UpdateAllScrollview(0);
    }

    /// <summary>
    /// 마나 조작시에 해야할 것
    /// </summary>
    /// <param name="_mana"></param>
    public void Manacontrol(int _mana)
    {
        Database.Inst.playData.mp -= _mana;
        playerStat.transform.Find("Mana/ManaCountBG/ManaCountLabel").GetComponent<UILabel>().text = Database.Inst.playData.mp.ToString();
    }
    /// <summary>
    /// 무기 변경시에 로비에서 바뀌어야 할 것
    /// </summary>
    public void SetWeapon()
    {
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        string classname = "null";
        if (item.Class.Equals(CLASS.검))
        {
            classname = "Worrior";
            if (Database.Inst.playData.sex.Equals(SEX.Male))
            {
                playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
            else
            {
                //playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
        }
        else if (item.Class.Equals(CLASS.활))
        {
            classname = "Archer";

        }
        else if (item.Class.Equals(CLASS.지팡이))
        {
            classname = "Wizard";
        }
        //Assets / Resources / PlayerCharactor / Female_Worrior_Weapon.mat
        string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);

        NGUIAtlas weaponatlas = Resources.Load<NGUIAtlas>(playerclass + "_Weapon");
        for (int i = 0; Weapon.Count > i; i++)
        {
            Weapon[i].atlas = weaponatlas;
            Weapon[i].spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].imageName;
        }
        UpdateAllScrollview(0);
        SetplayerStat();
    }
    /// <summary>
    /// 아머가 바뀌었을 시에 해야할 것
    /// </summary>
    public void SetArmor()
    {
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum];
        string playerclass = string.Format("PlayerCharactor/{0}_{1}", Database.Inst.playData.sex.ToString(), classname);
        UpdateAllScrollview(0);
    }
    /// <summary>
    /// 장비창에서 상단 버튼을 터치시 동작
    /// </summary>
    public void Selectbutton()
    {
        foreach (UISprite button in UIButton.current.transform.parent.GetComponentsInChildren<UISprite>())
        {
            button.spriteName = "정비 UI 탭 버튼";
        }

        LobbyManager.inst.selectData = -1;
        LobbyManager.inst.UpdateAllScrollview(0);
        UIButton.current.GetComponent<UISprite>().spriteName = "정비 UI 탭 버튼 클릭";
        switch (lobbystate)
        {
            case LobbyState.Nomal:
                LobbyManager.inst.equipanel.transform.Find("ItemWindow/FilterButtonBGI").GetComponent<UIButton>().isEnabled = false;
                switch (UIButton.current.GetComponentInChildren<UILabel>().text)
                {
                    case "무기":
                        itemclassselect = ItemState.무기;
                        Debug.Log("무기");
                        break;
                    case "방어구":
                        itemclassselect = ItemState.갑옷;
                        Debug.Log("방어구");
                        break;
                    case "스킨":
                        //Resources.Load<>
                        if (skinScrollView.GetComponentInChildren<UIWrapContent>().transform.childCount < 1)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                GameObject SkinBGI = NGUITools.AddChild(skinScrollView.transform.Find("SkinGrid"), Resources.Load<GameObject>("UI/SkinBGI"));
                                Debug.Log(SkinBGI.name);
                                SkinBGI.transform.Find("Equip").gameObject.SetActive(false);
                                //SkinBGI.GetComponentInChildren<UITexture>().mainTexture.name("")
                                //skinScrollView.GetComponentInChildren<UIWra Content>().
                            }
                            skinScrollView.GetComponentInChildren<UIGrid>().Reposition();
                            //skinScrollView.GetComponent<UIScrollView>().ResetPosition();
                        }
                        else
                        {
                            skinScrollView.GetComponentInChildren<UICenterOnChild>().CenterOn(skinScrollView.transform.Find("SkinGrid").transform.GetChild(2));
                        }
                        //Debug.Log(skinScrollView.GetComponentInChildren<UIGrid>().);
                        skinScrollView.GetComponentInChildren<UIWrapContent>().SortBasedOnScrollMovement();
                        //skinScrollView.GetComponentInChildren<UICenterOnChild>().CenterOn(skinScrollView.transform.Find("SkinGrid").transform.GetChild(2));
                        Debug.Log(skinScrollView.GetComponentInChildren<UICenterOnChild>().centeredObject);
                        break;
                    default:

                        break;
                }
                break;
            case LobbyState.Enchant:
                break;
            case LobbyState.Decomposition:
                switch (UIButton.current.GetComponentInChildren<UILabel>().text)
                {
                    case "무기":
                        itemclassselect = ItemState.무기;
                        Debug.Log("무기");
                        break;
                    case "방어구":
                        itemclassselect = ItemState.갑옷;
                        Debug.Log("방어구");
                        break;
                    default:

                        break;
                }
                break;
            default:
                break;
        }

        UpdateAllScrollview(0);

    }

    /// <summary>
    /// Statpanel업데이트
    /// </summary>
    public void SetplayerStat()
    {
        //statpanel.transform.Find(string.Format("StatBGI/Nickname")).GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.nickName);
        //statpanel.transform.Find(string.Format("StatBGI/Damage")).GetComponent<UILabel>().text = string.Format("공격력 : {0}", Database.Inst.playData.atk_Min);
        //statpanel.transform.Find(string.Format("StatBGI/HP")).GetComponent<UILabel>().text = string.Format("체력 :{0}", Database.Inst.playData.currentHp);
        //statpanel.transform.Find(string.Format("StatBGI/Defence")).GetComponent<UILabel>().text = string.Format("피해감소량 : {0}", Database.Inst.playData.mp);
        //statpanel.transform.Find(string.Format("StatBGI/AttackSpeed")).GetComponent<UILabel>().text = string.Format("공격속도:{0}", Database.Inst.playData.atk_Speed);

        //GameObject EquipItem = GameObject.Find("UI Root/LobbyPanel/IconBackgroundU/EquipItem");
        //EquipItem.transform.Find(string.Format("Equip1/OptionBGI/Name")).GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name);
        //EquipItem.transform.Find(string.Format("Equip1/OptionBGI/Value")).GetComponent<UILabel>().text = string.Format("공격력:{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].itemValue);
        //EquipItem.transform.Find(string.Format("Equip1/OptionBGI/Option")).GetComponent<UILabel>().text = string.Format("옵션:{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].option_Index);

        //EquipItem.transform.Find(string.Format("Equip2/OptionBGI/Name")).GetComponent<UILabel>().text = string.Format("{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].name);
        //EquipItem.transform.Find(string.Format("Equip2/OptionBGI/Value")).GetComponent<UILabel>().text = string.Format("체력:{0}", Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum].itemValue);

        //equipStatPanel.transform.Find("Damage").GetComponent<UILabel>().text = string.Format("공격력 : {0}", Database.Inst.playData.atk_Min);
        //equipStatPanel.transform.Find("HP").GetComponent<UILabel>().text = string.Format("체력 :{0}", Database.Inst.playData.currentHp);
        //equipStatPanel.transform.Find("Defence").GetComponent<UILabel>().text = string.Format("피해감소량 : {0}", Database.Inst.playData.mp);
        //equipStatPanel.transform.Find("AttackSpeed").GetComponent<UILabel>().text = string.Format("공격속도:{0}", Database.Inst.playData.atk_Speed);

    }
    /// <summary>
    /// 배틀씬으로 가기
    /// </summary>
    public void GotoBattle()
    {
        GameManager.Inst.Loading(true);
        #region kks

        //GameManager.Inst.PlayData.currentStage = (int)developerStageSetting + 1;
        #endregion
        //GameObject.Find()
    }
    /// <summary>
    /// 스킨 움직임 버튼 재생 왼쪽클릭
    /// </summary>
    public void SkinLeft()
    {
        //애니메이션 재생
        //슬롯
        GameObject ItemWindow = GameObject.Find("UI Root/Skin/ItemWindow").gameObject;
        ItemWindow.GetComponent<Animator>().Play("LeftAnim");
    }
    /// <summary>
    /// 스킨 움직임 버튼 재생 오른쪽클릭
    /// </summary>
    public void SkinRight()
    {
        GameObject ItemWindow = GameObject.Find("UI Root/Skin/ItemWindow").gameObject;
        ItemWindow.GetComponent<Animator>().Play("RightAnim");
    }
    /// <summary>
    /// 스킨의 뎁스 원상 복귀
    /// </summary>
    public void SkinDepthorder()
    {
        GameObject ItemWindow = GameObject.Find("UI Root/Skin/ItemWindow").gameObject;

        for (int i = 0; i < 7; i++)
        {
            GameObject Skin = ItemWindow.transform.Find(string.Format("Skin{0}", i)).gameObject;
            Skin.GetComponent<UISprite>().depth = 6 - (Mathf.Abs(3 - i));
            Debug.Log(Skin.transform.GetChild(0).GetComponent<Texture>());
            Skin.transform.GetChild(0).GetComponent<UITexture>().depth = Skin.GetComponent<UISprite>().depth;
            Debug.Log(string.Format("SkinBGI:{0},Skin:{1},i:{2}", Skin.GetComponent<UISprite>().depth, Skin.GetComponentInChildren<UISprite>().depth, i));
        }

    }

    IEnumerator worstReset() //코루틴으로 15초 간격으로 최저 프레임 리셋해줌.
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            worstFps = 100f;
        }
    }

    float deltaTime = 0.0f;

    GUIStyle style;
    Rect rect;
    float msec;
    float fps;
    float worstFps = 100f;
    string text;

    void OnGUI()//소스로 GUI 표시.
    {

        msec = deltaTime * 1000.0f;
        fps = 1.0f / deltaTime;  //초당 프레임 - 1초에

        if (fps < worstFps)  //새로운 최저 fps가 나왔다면 worstFps 바꿔줌.
            worstFps = fps;
        text = msec.ToString("F1") + "ms (" + fps.ToString("F1") + ") //worst : " + worstFps.ToString("F1");
#if UNITY_EDITOR
        Debug.Log(text);
#endif
        //testlabel.text = text;
    }

    /// <summary>
    /// 현재 장착중인 무기를 터치시 보여줄것
    /// </summary>
    public void CurrentEquipItemInfoWeapon()
    {
        Database.Inventory inventory = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];

        equipBGI.SetActive(true);
        ItemInfo.SetActive(true);
        UIRect.AnchorPoint anchorPoint = ItemInfo.GetComponent<UISprite>().bottomAnchor;


        Color rarecolor = Color.white;
        switch (Database.Inst.weapons[inventory.DB_Num].rarity)
        {
            case RARITY.노말:
                rarecolor = Color.white;
                break;
            case RARITY.유니크:
                rarecolor = Color.blue;
                break;
            case RARITY.레전드:
                rarecolor = Color.yellow;
                break;
            default:
                break;
        }
        for (int i = 0; i < ItemInfo.transform.childCount; i++)
        {
            //ItemInfo.GetComponentsInChildren<UILabel>()[i].UpdateAnchors();
            Debug.Log(i + "(" + ItemInfo.transform.childCount + ")");
            ItemInfo.transform.GetChild(i).gameObject.SetActive(false);
            //ItemInfo.GetComponentsInChildren<UILabel>()[i].gameObject.SetActive(false);
        }
        ItemInfo.transform.Find("name").gameObject.SetActive(true);
        ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].name.ToString();
        ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().color = rarecolor;

        ItemInfo.transform.Find("discription").gameObject.SetActive(true);
        ItemInfo.transform.Find("discription").gameObject.GetComponent<UILabel>().text = "";

        ItemInfo.transform.Find("rare").gameObject.SetActive(true);
        ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].rarity.ToString();
        ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().color = rarecolor;

        ItemInfo.transform.Find("mindamage").gameObject.SetActive(true);
        ItemInfo.transform.Find("mindamage").gameObject.GetComponent<UILabel>().text = "최소 데미지";
        ItemInfo.transform.Find("mindamagenum").gameObject.SetActive(true);
        ItemInfo.transform.Find("mindamagenum").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].atk_Min.ToString();

        ItemInfo.transform.Find("maxdamage").gameObject.SetActive(true);
        ItemInfo.transform.Find("maxdamage").GetComponent<UILabel>().text = "최대 데미지";
        ItemInfo.transform.Find("maxdamagenum").gameObject.SetActive(true);
        ItemInfo.transform.Find("maxdamagenum").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].atk_Max.ToString();

        ItemInfo.transform.Find("attackspeed").gameObject.SetActive(true);
        ItemInfo.transform.Find("attackspeed").GetComponent<UILabel>().text = "공격 속도";
        ItemInfo.transform.Find("attackspeednum").gameObject.SetActive(true);
        ItemInfo.transform.Find("attackspeednum").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].atk_Speed.ToString();

        ItemInfo.transform.Find("knockback").gameObject.SetActive(true);
        ItemInfo.transform.Find("knockback").GetComponent<UILabel>().text = "넉백 확률";
        ItemInfo.transform.Find("knockbacknum").gameObject.SetActive(true);
        ItemInfo.transform.Find("knockbacknum").gameObject.GetComponent<UILabel>().text = Database.Inst.weapons[inventory.DB_Num].nuckback_Percentage.ToString();

        ItemInfo.transform.Find("optiondiscription").gameObject.SetActive(true);
        ItemInfo.transform.Find("optiondiscription").gameObject.GetComponent<UILabel>().text = "";


        anchorPoint.absolute = 250;
        ItemInfo.GetComponent<UISprite>().bottomAnchor = anchorPoint;
        ItemInfo.GetComponent<UISprite>().UpdateAnchors();
        for (int i = 0; i < ItemInfo.GetComponentsInChildren<UILabel>().Length; i++)
        {
            //ItemInfo.transform.GetChild(i).GetComponent<UILabel>().UpdateAnchors();
            //ItemInfo.GetComponentsInChildren<UILabel>()[i].gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 현재 장비중인 갑옷을 터치시 보여줄 것
    /// </summary>
    public void CurrentEquipItemInfoArmor()
    {
        Database.Inventory inventory = Database.Inst.playData.inventory[Database.Inst.playData.equiArmor_InventoryNum];
        equipBGI.SetActive(true);
        ItemInfo.SetActive(true);
        UIRect.AnchorPoint anchorPoint = ItemInfo.GetComponent<UISprite>().bottomAnchor;

        for (int i = 0; i < ItemInfo.transform.childCount; i++)
        {
            ItemInfo.transform.GetChild(i).gameObject.SetActive(false);
        }
        Color rarecolor = Color.white;
        switch (Database.Inst.armors[inventory.DB_Num].rarity)
        {
            case RARITY.노말:
                rarecolor = Color.white;
                break;
            case RARITY.유니크:
                rarecolor = Color.blue;
                break;
            case RARITY.레전드:
                rarecolor = Color.yellow;
                break;
            default:
                break;
        }
        ItemInfo.transform.Find("name").gameObject.SetActive(true);
        ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().text = Database.Inst.armors[inventory.DB_Num].name.ToString();
        ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().color = rarecolor;

        ItemInfo.transform.Find("discription").gameObject.SetActive(true);
        ItemInfo.transform.Find("discription").gameObject.GetComponent<UILabel>().text = "";

        ItemInfo.transform.Find("rare").gameObject.SetActive(true);
        ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().text = Database.Inst.armors[inventory.DB_Num].rarity.ToString();
        ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().color = rarecolor;

        ItemInfo.transform.Find("mindamage").gameObject.SetActive(true);
        ItemInfo.transform.Find("mindamage").gameObject.GetComponent<UILabel>().text = "추가 체력";
        ItemInfo.transform.Find("mindamagenum").gameObject.SetActive(true);
        ItemInfo.transform.Find("mindamagenum").gameObject.GetComponent<UILabel>().text = Database.Inst.armors[inventory.DB_Num].hp.ToString();

        ItemInfo.transform.Find("optiondiscription").gameObject.SetActive(true);
        ItemInfo.transform.Find("optiondiscription").gameObject.GetComponent<UILabel>().text = "";
        anchorPoint.absolute = 300;
        ItemInfo.GetComponent<UISprite>().bottomAnchor = anchorPoint;
        ItemInfo.GetComponent<UISprite>().UpdateAnchors();
        for (int i = 0; i < ItemInfo.transform.childCount; i++)
        {
            Debug.Log(i + "(" + ItemInfo.transform.childCount + ")");
        }
    }
    /// <summary>
    /// 현재 장비중인 스킬을 터치시 보여줄 것
    /// </summary>
    public void CurrentEquipItemInfoSkill()
    {
        Database.Skill Skill = Database.Inst.skill[Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].skill_Index];
        Color rarecolor = Color.white;
        equipBGI.SetActive(true);
        ItemInfo.SetActive(true);
        UIRect.AnchorPoint anchorPoint = ItemInfo.GetComponent<UISprite>().bottomAnchor;

        for (int i = 0; i < ItemInfo.transform.childCount; i++)
        {
            ItemInfo.transform.GetChild(i).gameObject.SetActive(false);
        }

        ItemInfo.transform.Find("name").gameObject.SetActive(true);
        ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().text = Skill.name;
        ItemInfo.transform.Find("name").gameObject.GetComponent<UILabel>().color = rarecolor;

        ItemInfo.transform.Find("discription").gameObject.SetActive(true);
        ItemInfo.transform.Find("discription").gameObject.GetComponent<UILabel>().text = Skill.skill_Duration.ToString();

        ItemInfo.transform.Find("rare").gameObject.SetActive(true);
        ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().text = Skill.skillType.ToString();
        ItemInfo.transform.Find("rare").gameObject.GetComponent<UILabel>().color = rarecolor;

        ItemInfo.transform.Find("mindamage").gameObject.SetActive(true);
        ItemInfo.transform.Find("mindamage").gameObject.GetComponent<UILabel>().text = "데미지";
        ItemInfo.transform.Find("mindamagenum").gameObject.SetActive(true);
        ItemInfo.transform.Find("mindamagenum").gameObject.GetComponent<UILabel>().text = Skill.atk.ToString();

        ItemInfo.transform.Find("maxdamage").gameObject.SetActive(true);
        ItemInfo.transform.Find("maxdamage").GetComponent<UILabel>().text = "쿨타임";
        ItemInfo.transform.Find("maxdamagenum").gameObject.SetActive(true);
        ItemInfo.transform.Find("maxdamagenum").gameObject.GetComponent<UILabel>().text = Skill.coolTime.ToString();

        ItemInfo.transform.Find("attackspeed").gameObject.SetActive(true);
        ItemInfo.transform.Find("attackspeed").GetComponent<UILabel>().text = "소모 마나";
        ItemInfo.transform.Find("attackspeednum").gameObject.SetActive(true);
        ItemInfo.transform.Find("attackspeednum").gameObject.GetComponent<UILabel>().text = Skill.mpCost.ToString();


        anchorPoint.absolute = 250;
        ItemInfo.GetComponent<UISprite>().bottomAnchor = anchorPoint;
        ItemInfo.GetComponent<UISprite>().UpdateAnchors();
    }
    public void Equipswitchbutton()
    {
        if (GameManager.Inst.Scenestack.Peek().Equals("Decomposition"))
        {
            TouchBackButton();
            UpdateAllScrollview(0);
        }
        else
        {
            DecompositionState();
            UpdateAllScrollview(0);
        }
    }
    public void EquipConfirm()
    {
        Debug.Log("click");
        if (lobbystate.Equals(LobbyState.Nomal))
        {
            if (Database.Inst.playData.inventory[selectData].Class.Equals(CLASS.갑옷))
            {
                Database.Inst.playData.equiArmor_InventoryNum = selectData;
                LobbyManager.inst.SetArmor();
            }
            else
            {
                Database.Inst.playData.equiWeapon_InventoryNum = selectData;
                LobbyManager.inst.SetWeapon();
            }
            LobbyManager.inst.selectData = -1;
        }
        else
        {
            if (lobbystate.Equals(LobbyState.Decomposition))
            {
                DecompositionPanel.GetComponent<Animator>().Play("DecompositionStart");
                for (int i = 0; i < Selecteditem.Count; i++)
                {
                    //GameManager.Inst.Delete_Inventory_Item(LobbyManager.inst.Selecteditem[i]);

                }
                Selecteditem.Clear();
                DecompositionCountLabel.GetComponent<UILabel>().text = "0";
                for (int i = 0; i < 7; i++)
                {
                    LobbyManager.inst.DecompositionPanel.transform.Find(string.Format("DecompositionInfo/Itemplace{0}/ItemIcon", LobbyManager.inst.Selecteditem.Count)).GetComponent<UISprite>().spriteName = "크리스탈";
                }
            }

        }
        LobbyManager.inst.SetplayerStat();
        UpdateAllScrollview(0);
        GameManager.Inst.SavePlayerData();

    }

}
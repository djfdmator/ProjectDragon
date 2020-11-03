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
    public GameObject playerStat, equipanel;
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

    public List<GUITestScrollView> gUITestScrollViews;
    public static string teststring;
    string classname = "null";

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
        //LobbyObjectSet();
        LobbyStateInit();
        selectData = -1;
        //SetplayerStat();
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
            SoundManager.Inst.Ds_BgmPlayer(fire);
            //fireobject.SetActive(true);
        }
        else
        {
        }
        Database.Inventory item = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        //Weapon.Add(euiptIcons.transform.Find("Equip1/WeaponImg").GetComponent<UISprite>());
        //euiptIcons.transform.Find("Equip1/WeaponImg").GetComponent<UISprite>().spriteName = item.imageName;
        if (item.Class.Equals(CLASS.검))
        {
            classname = "Worrior";
            if (Database.Inst.playData.sex.Equals(SEX.Male))
            {
                //playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                //equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
            else
            {
                //playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                //equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
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
        //playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        ChangeItemIcon(currentweapon, item);
        //Weapon.Add(playerimg.transform.Find("Weapon").GetComponent<UISprite>());
        //Weapon.Add(equipCharactor.transform.Find("Weapon").GetComponent<UISprite>());
        //CharactorSkinSet(playerclass);
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
            RaycastHit2D hit0 = Physics2D.Raycast(touchpoint011, transform.forward, 10);
            if (hit0)
            {
            }
        }


        //GameObject.Find("Panel/Label").GetComponent<UILabel>().text = test;
    }

    #region Usage
    public void ButtonSound1()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
    }


    public void MaintenanceButton()
    {
        maintenance.OpenMaintenance();
    }

    public void OptionButton()
    {
        optionWindow.OpenOptionWindow();
    }
    #endregion


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
            equipstatepanelbutton = false;

        }
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
                //layerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                //equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
            else
            {
                //playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                //equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
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
        //playerimg.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        //equipCharactor.GetComponent<UITexture>().mainTexture = Resources.Load(playerclass, typeof(Texture2D)) as Texture2D;
        //playeranimation.GetComponent<UISprite>().atlas = Resources.Load("Charactormarshmallow/" + Database.Inst.playData.sex.ToString() + "_marshmallow", typeof(NGUIAtlas)) as NGUIAtlas;
        ChangeItemIcon(currentweapon, item);
        currentweapon.transform.Find("ValueBGI/공격력수치").GetComponent<UILabel>().text = Database.Inst.weapons[item.DB_Num].atk_Min.ToString();
        //playerimg.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        //playerimg.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
        //equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().atlas = Resources.Load(playerclass + "_Weapon", typeof(NGUIAtlas)) as NGUIAtlas;
        //equipCharactor.transform.Find("Weapon").GetComponent<UISprite>().spriteName = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum].name;
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
    /// 장비관리 화면임을 푸시 및 초기화
    /// </summary>
    public void NomalState()
    {
        GameManager.Inst.Scenestack.Push("EquipPanel");
        lobbystate = LobbyState.Nomal;
        itemclassselect = ItemState.무기;
        ItemRarityselect = ItemRarity.기본;
        UpdateAllScrollview(0);
    }

    #region 정비창 정렬 기능
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
    #endregion


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
                        break;
                    case "방어구":
                        itemclassselect = ItemState.갑옷;
                        break;
                    case "스킨":
                        //Resources.Load<>
                        if (skinScrollView.GetComponentInChildren<UIWrapContent>().transform.childCount < 1)
                        {
                            for (int i = 0; i < 4; i++)
                            {
                                GameObject SkinBGI = NGUITools.AddChild(skinScrollView.transform.Find("SkinGrid"), Resources.Load<GameObject>("UI/SkinBGI"));
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
                        skinScrollView.GetComponentInChildren<UIWrapContent>().SortBasedOnScrollMovement();
                        //skinScrollView.GetComponentInChildren<UICenterOnChild>().CenterOn(skinScrollView.transform.Find("SkinGrid").transform.GetChild(2));
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
                        break;
                    case "방어구":
                        itemclassselect = ItemState.갑옷;
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
            Skin.transform.GetChild(0).GetComponent<UITexture>().depth = Skin.GetComponent<UISprite>().depth;
        }

    }





/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
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

    public void EquipConfirm()
    {
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
        //LobbyManager.inst.SetplayerStat();
        UpdateAllScrollview(0);
        GameManager.Inst.SavePlayerData();

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
               // playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
                //equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
            }
            else
            {
                //playerimg.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
               // equipCharactor.transform.Find("Weapon").transform.localPosition = new Vector3(1, -0.7f, 0);
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
        //SetplayerStat();
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
}
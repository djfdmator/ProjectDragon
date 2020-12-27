// ==============================================================
// 도감 및 업적 팝업창 UI
//
//  AUTHOR: Yang SeEun
// CREATED: 2020-12-25
// UPDATED: 2020-12-27
// ==============================================================



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encyclopedia_Popup : MonoBehaviour
{
    public enum Type { Weapon, Monster, Achievement }

    public GameObject itemCell;

    private Dictionary<Type, List<Encyclopedia_CellData>> cellList = new Dictionary<Type, List<Encyclopedia_CellData>>();
    private UIGrid weapon_grid;
    private UIGrid monsters_grid;
    private UIGrid achievement_grid;


    private EventDelegate slotEvent;


    //상세설명 팝업
    [SerializeField] private GameObject information_Popup;
    private UISprite Info_Icon;
    private UILabel Info_titleLabel;
    private UILabel Info_subTitleLabel;

    private void Awake()
    {
        itemCell = Resources.Load("UI/Encyclopedia_Cell") as GameObject;
        weapon_grid = transform.Find("Weapons").transform.Find("SlotList").GetComponent<UIGrid>();
        monsters_grid = transform.Find("Monsters").transform.Find("SlotList").GetComponent<UIGrid>();
        achievement_grid = transform.Find("Achievement").transform.Find("SlotList").GetComponent<UIGrid>();


        information_Popup = transform.Find("Information_Popup").gameObject;
        Info_Icon = information_Popup.transform.Find("Icon").GetComponent<UISprite>();
        Info_titleLabel = information_Popup.transform.Find("Title_Label").GetComponent<UILabel>();
        Info_subTitleLabel = information_Popup.transform.Find("Subtitle_Label").GetComponent<UILabel>();

        information_Popup.SetActive(false);
        slotEvent = new EventDelegate(this, "Event_InformationPopup");

    }
    private void Start()
    {
        CreateSlot();
        gameObject.SetActive(false);
    }

    public void Event_InformationPopup(Type type, int num)
    {
        OpenInfoPopup();
        SetInfo(type, num);
    }

    public void CreateSlot()
    {
        //무기 슬롯
        List<Database.Encyclopedia> weaponsData = GameManager.Inst.PlayData.encyclopedia_WeaponList;
        List<Encyclopedia_CellData> weaponCells = new List<Encyclopedia_CellData>();

        for (int i = 0; i < weaponsData.Count; i++)
        {
            Encyclopedia_CellData cell = Instantiate(itemCell, weapon_grid.transform).GetComponent<Encyclopedia_CellData>();
            cell.Init(weaponsData[i]);

            slotEvent = new EventDelegate(this, "Event_InformationPopup");
            slotEvent.parameters[0].value = Type.Weapon;
            slotEvent.parameters[1].value = cell.DB_Num;

            EventDelegate.Set(cell.button.onClick, slotEvent);
            weaponCells.Add(cell);
        }
        cellList.Add(Type.Weapon, weaponCells);
        weapon_grid.Reposition();

        //몬스터
        List<Database.Encyclopedia> monstersData = GameManager.Inst.PlayData.encyclopedia_MonsterList;
        List<Encyclopedia_CellData> monsterCells = new List<Encyclopedia_CellData>();
        for (int i = 0; i < monstersData.Count; i++)
        {
            Encyclopedia_CellData cell = Instantiate(itemCell, monsters_grid.transform).GetComponent<Encyclopedia_CellData>();
            cell.Init(monstersData[i]);
            Debug.Log(i + "  " + monstersData[i].active.ToString());

            slotEvent = new EventDelegate(this, "Event_InformationPopup");
            slotEvent.parameters[0].value = Type.Monster;
            slotEvent.parameters[1].value = cell.DB_Num;

            EventDelegate.Set(cell.button.onClick, slotEvent);
            monsterCells.Add(cell);
        }
        cellList.Add(Type.Monster, monsterCells);
        monsters_grid.Reposition();

        //업적
        List<Database.Achievement> achievementsData = GameManager.Inst.PlayData.achievementList;
        List<Encyclopedia_CellData> achievementCells = new List<Encyclopedia_CellData>();
        for (int i = 0; i < achievementsData.Count; i++)
        {
            Encyclopedia_CellData cell = Instantiate(itemCell, achievement_grid.transform).GetComponent<Encyclopedia_CellData>();
            cell.Init(achievementsData[i]);

            slotEvent = new EventDelegate(this, "Event_InformationPopup");
            slotEvent.parameters[0].value = Type.Achievement;
            slotEvent.parameters[1].value = cell.DB_Num;

            EventDelegate.Set(cell.button.onClick, slotEvent);
            achievementCells.Add(cell);
        }
        cellList.Add(Type.Achievement, achievementCells);
        achievement_grid.Reposition();
    }

    //모든 데이터 읽어와 각 셀에 적용
    private void RefreshSlots()
    {
        foreach (Encyclopedia_CellData cell in cellList[Type.Weapon])
        {
            cell.ToggleActive(GameManager.Inst.PlayData.encyclopedia_WeaponList[cell.DB_Num].active);
        }
        foreach (Encyclopedia_CellData cell in cellList[Type.Monster])
        {
            cell.ToggleActive(GameManager.Inst.PlayData.encyclopedia_MonsterList[cell.DB_Num].active);
        }
        foreach (Encyclopedia_CellData cell in cellList[Type.Achievement])
        {
            cell.ToggleActive(GameManager.Inst.PlayData.achievementList[cell.DB_Num].active);
        }
    }
    private void RefreshSlot(Type type, int DB_Num)
    {
        if (type.Equals(Type.Weapon))
        {
            GameManager.Inst.PlayData.encyclopedia_WeaponList[DB_Num].Refresh(true);
        }
        else if (type.Equals(Type.Monster))
        {
            GameManager.Inst.PlayData.encyclopedia_MonsterList[DB_Num].Refresh(true);
        }
        else
        {
        }
    }
    public void TestRefreshSlot()
    {
        GameManager.Inst.PlayData.encyclopedia_WeaponList[3].active = true;
        GameManager.Inst.PlayData.encyclopedia_WeaponList[1].active = true;
    }



    private void SetInfo(Type type, int DB_index)
    {
        string imageName = "Encyclopedia_ParchmentWindow_";
        string strTitle = string.Empty;
        string strSubtitle = string.Empty;

        if (type.Equals(Type.Weapon))
        {
            List<Database.Encyclopedia> data = Database.Inst.playData.encyclopedia_WeaponList;
            imageName += data[DB_index].imageName;
            strTitle = data[DB_index].name;
            strSubtitle = data[DB_index].description;
        }
        else if (type.Equals(Type.Monster))
        {
            List<Database.Encyclopedia> data = Database.Inst.playData.encyclopedia_MonsterList;
            imageName += data[DB_index].imageName;
            strTitle = data[DB_index].name;
            strSubtitle = data[DB_index].description;
        }
        else if (type.Equals(Type.Achievement))
        {
            List<Database.Achievement> data = Database.Inst.playData.achievementList;
            imageName += data[DB_index].imageName;
            strTitle = data[DB_index].title;
            strSubtitle = data[DB_index].description;
        }

        Info_Icon.spriteName = imageName;
        Info_titleLabel.text = strTitle;
        Info_subTitleLabel.text = strSubtitle;
    }


    #region OpenClosePopup
    public void OpenEncyclopediaPopup()
    {
        RefreshSlots();
        gameObject.SetActive(true);
    }
    public void CloseEncyclopediaPopup()
    {
        gameObject.SetActive(false);
    }

    private void OpenInfoPopup()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        information_Popup.SetActive(true);
    }

    public void CloseInfoPopup()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        information_Popup.SetActive(false);
    }
    #endregion

}


// ==============================================================
// Structure of Database
// 
// 2019-12-26: Change Database Struct
// 2020-02-20: block emblem data
// 2020-02-25: add skin table and game option value 
//
//  AUTHOR: Kim Dong Ha
// UPDATED: 2019-02-25
// ==============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RARITY
{
    노말,
    유니크,
    레전드
}

public enum Monster_Rarity
{
    Normal,
    Rare
}

public enum CLASS
{
    검,
    활,
    지팡이,
    갑옷
}

public enum SKILLTYPE
{
    즉발형,
    범위지정형,
    충전형,
    신체강화형
}

public enum SEX
{
    None,
    Male,
    Female
}

//public enum EMBLEM_STATUS
//{
//    Lock,
//    Unlock,
//    acheive,
//    Activate,
//    Equip
//}

//TODO: 먼저 얻은 순서용 인트 넣기, 이미지 이름, 아이템 설명

public class Database : MonoSingleton<Database>
{

    //클래스 모음
    #region Data_Class

    [System.Serializable]
    public class Weapon
    {
        public readonly int num;
        public readonly string name;
        public readonly RARITY rarity;
        public readonly string rarity_Text;
        public readonly CLASS Class;
        public readonly int atk_Min;
        public readonly int atk_Max;
        public readonly float atk_Range; // 사정 거리
        public readonly float atk_Speed; // 공속
        public readonly float nuckback_Power; // 공속
        public readonly float nuckback_Percentage; // 공속
        public readonly int item_Value;
        public readonly string description;
        public readonly string imageName; //이미지 이름
        public readonly int skill_Index; // 
        public readonly string optionTableName;
        public readonly int enhanceValue;


        public Weapon(int num, string name, RARITY rarity, string rarity_Text, CLASS _class, int atk_Min, int atk_Max, float atk_Range, float atk_Speed,
                        float nuckback_Power, float nuckback_Percentage, int item_Value, string description, string imageName, int skill_Index, string optionTableName, int enhanceValue)
        {
            this.num = num;
            this.name = name;
            this.rarity = rarity;
            this.rarity_Text = rarity_Text;
            this.Class = _class;
            this.atk_Min = atk_Min;
            this.atk_Max = atk_Max;
            this.atk_Range = atk_Range;
            this.atk_Speed = atk_Speed;
            this.nuckback_Power = nuckback_Power;
            this.nuckback_Percentage = nuckback_Percentage;
            this.item_Value = item_Value;
            this.description = description;
            this.imageName = imageName;
            this.skill_Index = skill_Index;
            this.optionTableName = optionTableName;
            this.enhanceValue = enhanceValue;
        }

        public Weapon(Weapon w)
        {
            this.num = w.num;
            this.name = w.name;
            this.rarity = w.rarity;
            this.rarity_Text = w.rarity_Text;
            this.Class = w.Class;
            this.atk_Min = w.atk_Min;
            this.atk_Max = w.atk_Max;
            this.atk_Range = w.atk_Range;
            this.atk_Speed = w.atk_Speed;
            this.nuckback_Power = w.nuckback_Power;
            this.nuckback_Percentage = w.nuckback_Percentage;
            this.item_Value = w.item_Value;
            this.description = w.description;
            this.imageName = w.imageName;
            this.skill_Index = w.skill_Index;
            this.optionTableName = w.optionTableName;
            this.enhanceValue = w.enhanceValue;
        }
    }

    [System.Serializable]
    public class Armor
    {
        public readonly int num;
        public readonly string name;
        public readonly RARITY rarity;
        public readonly string rarity_Text;
        public readonly CLASS Class;
        public readonly int hp;
        public readonly int item_Value;
        public readonly string description;
        public readonly string imageName;
        public readonly string optionTableName;

        public Armor(int num, string name, RARITY rarity, string rarity_Text, CLASS _class, int hp, int item_Value, string description, string imageName, string optionTableName)
        {
            this.num = num;
            this.name = name;
            this.rarity = rarity;
            this.rarity_Text = rarity_Text;
            this.Class = _class;
            this.hp = hp;
            this.item_Value = item_Value;
            this.description = description;
            this.imageName = imageName;
            this.optionTableName = optionTableName;
        }
    }

    [System.Serializable]
    public class Skill
    {
        public readonly int num;
        public readonly string name;
        public readonly SKILLTYPE skillType;
        public int atk; //데미지
        public readonly int mpCost;
        public readonly float coolTime; // 쿨타임
        public readonly float skill_Range; //사정거리
        public readonly float skill_Duration; // 실행 속도
        public readonly int parameter; //공격횟수
        public readonly string description;
        public readonly string imageName;
        public readonly int enhanceValue;

        public Skill(int num, string name, SKILLTYPE skillType, int atk, int mpCost, float coolTime, float skill_Range, float skill_Duration,
                    int parameter, string description, string imageName, int enhanceValue)
        {
            this.num = num;
            this.name = name;
            this.skillType = skillType;
            this.atk = atk;
            this.mpCost = mpCost;
            this.coolTime = coolTime;
            this.skill_Range = skill_Range;
            this.skill_Duration = skill_Duration;
            this.parameter = parameter;
            this.description = description;
            this.imageName = imageName;
            this.enhanceValue = enhanceValue;
        }

        public Skill(Skill skill)
        {
            this.num = skill.num;
            this.name = skill.name;
            this.skillType = skill.skillType;
            this.atk = skill.atk;
            this.mpCost = skill.mpCost;
            this.coolTime = skill.coolTime;
            this.skill_Range = skill.skill_Range;
            this.skill_Duration = skill.skill_Duration;
            this.parameter = skill.parameter;
            this.description = skill.description;
            this.imageName = skill.imageName;
            this.enhanceValue = skill.enhanceValue;
        }
    }

    //[System.Serializable]
    //public class Emblem
    //{
    //    public readonly int num;
    //    public readonly string name;
    //    public EMBLEM_STATUS status;
    //    public readonly string description;
    //    public readonly int parameter1;
    //    public readonly int parameter2;
    //    public readonly string imageName;
    //    public readonly string methodName;

    //    public Emblem(int num, string name, EMBLEM_STATUS status, string description, int parameter1, int parameter2, string imageName, string methodName)
    //    {
    //        this.num = num;
    //        this.name = name;
    //        this.status = status;
    //        this.description = description;
    //        this.parameter1 = parameter1;
    //        this.parameter2 = parameter2;
    //        this.imageName = imageName;
    //        this.methodName = methodName;
    //    }

    //    public Emblem(Database.Emblem src)
    //    {
    //        num = src.num;
    //        name = src.name;
    //        status = src.status;
    //        description = src.description;
    //        imageName = src.imageName;
    //        methodName = src.methodName;
    //    }

    //}

    [System.Serializable]
    public class Normal_Monster
    {
        public readonly int num;
        public readonly string name;
        public readonly Monster_Rarity monster_Rarity;
        public readonly int hp;
        public readonly float move_Speed; // 이동 속도
        public readonly int atk;
        public readonly float atk_Speed;
        public readonly float atk_Range;
        public readonly int ready_Time;
        public readonly int coolTime;
        public readonly int knock_Resist;
        public readonly int atk_Count;
        public readonly int drop_Mana_Min;
        public readonly int drop_Mana_Max;
        public readonly string description;
        public readonly string imageName;

        public Normal_Monster(int num, string name, Monster_Rarity monster_Rarity, int hp, float move_Speed, int atk, float atk_Speed,
                                float atk_Range, int ready_Time, int coolTime, int knock_Resist, int atk_Count, int drop_Mana_Min,
                                int drop_Mana_Max, string description, string imageName)
        {
            this.num = num;
            this.name = name;
            this.monster_Rarity = monster_Rarity;
            this.hp = hp;
            this.move_Speed = move_Speed;
            this.atk = atk;
            this.atk_Speed = atk_Speed;
            this.atk_Range = atk_Range;
            this.ready_Time = ready_Time;
            this.coolTime = coolTime;
            this.knock_Resist = knock_Resist;
            this.atk_Count = atk_Count;
            this.drop_Mana_Min = drop_Mana_Min;
            this.drop_Mana_Max = drop_Mana_Max;
            this.description = description;
            this.imageName = imageName;
        }
    }

    [System.Serializable]
    public class Rare_Monster
    {
        public readonly int num;
        public readonly string name;
        public readonly Monster_Rarity monster_Rarity;
        public readonly int hp;
        public readonly float move_Speed; // 이동 속도
        public readonly int atk;
        public readonly float atk_Speed;
        public readonly float atk_Range;
        public readonly float ready_Time;
        public readonly int coolTime;
        public readonly int knock_Resist;
        public readonly int atk_Count1;
        public readonly int atk_Count2;
        public readonly int skill_Cooltime;
        public readonly int skill_Damage;
        public readonly int drop_Mana_Min;
        public readonly int drop_Mana_Max;
        public readonly string description;
        public readonly string imageName;

        public Rare_Monster(int num, string name, Monster_Rarity monster_Rarity, int hp, float move_Speed, int atk, float atk_Speed, float atk_Range,
                            float ready_Time, int coolTime, int knock_Resist, int atk_Count1, int atk_Count2, int skill_Cooltime, int skill_Damage,
                            int drop_Mana_Min, int drop_Mana_Max, string description, string imageName)
        {
            this.num = num;
            this.name = name;
            this.monster_Rarity = monster_Rarity;
            this.hp = hp;
            this.move_Speed = move_Speed;
            this.atk = atk;
            this.atk_Speed = atk_Speed;
            this.atk_Range = atk_Range;
            this.ready_Time = ready_Time;
            this.coolTime = coolTime;
            this.knock_Resist = knock_Resist;
            this.atk_Count1 = atk_Count1;
            this.atk_Count2 = atk_Count2;
            this.skill_Cooltime = skill_Cooltime;
            this.skill_Damage = skill_Damage;
            this.drop_Mana_Min = drop_Mana_Min;
            this.drop_Mana_Max = drop_Mana_Max;
            this.description = description;
            this.imageName = imageName;
        }
    }

    [System.Serializable]
    public class OptionTable
    {
        public readonly int num;
        public readonly int parameter;
        public readonly string description;
        public readonly float percentage;
        public readonly string methodName;

        public OptionTable(int num, int parameter, string description, float percentage, string methodName)
        {
            this.num = num;
            this.parameter = parameter;
            this.description = description;
            this.percentage = percentage;
            this.methodName = methodName;
        }
    }

    [System.Serializable]
    public class Inventory
    { //0,1,2
        public int num; //인벤토리에서의 Index
        public int DB_Num; // 해당 아이템이 있는 DB에서의 Index
        public string name; // 아이템 이름
        public RARITY rarity; // 희귀도
        public CLASS Class; // 아이템 타입 ;; 소드냐 젬이냐 방어구냐 이런거
        public int itemValue; // 아이템 가치 - 강화젬의 강화 수치 같은 것들
        public string imageName; //이미지 이름
        public int skill_Index; // 아이템이 가진 액티브 스킬의 DB에서의 Index
        public int option_Index;
        public bool isNew;
        public int enhanceLevel;

        public Inventory(int _num, int _DB_Num, string _name, RARITY _rarity, CLASS _Class, int _itemValue,
                           string _imageName, int _skill_Index, int _option_Index, bool _isNew, int _enhanceLevel)
        {
            num = _num;
            DB_Num = _DB_Num;
            name = _name;
            rarity = _rarity;
            Class = _Class;
            itemValue = _itemValue;
            imageName = _imageName;
            skill_Index = _skill_Index;
            option_Index = _option_Index;
            isNew = _isNew;
            enhanceLevel = _enhanceLevel;
        }

        public Inventory(Database.Weapon weapon)
        {
            num = Database.Inst.GetInventoryCount();
            DB_Num = weapon.num;
            name = weapon.name;
            rarity = weapon.rarity;
            Class = weapon.Class;
            itemValue = weapon.item_Value;
            imageName = weapon.imageName;
            skill_Index = weapon.skill_Index;
            option_Index = -1;
            isNew = true;
            enhanceLevel = 0;
        }

        public Inventory(Database.Armor armor)
        {
            num = Database.Inst.GetInventoryCount();
            DB_Num = armor.num;
            name = armor.name;
            rarity = armor.rarity;
            Class = armor.Class;
            itemValue = armor.item_Value;
            imageName = armor.imageName;
            skill_Index = -1;
            option_Index = -1;
            isNew = true;
            enhanceLevel = 0;
        }
    }

    [System.Serializable]
    public class Achievement
    {
        public int num;
        public string title;
        public string description;
        public string imageName;
        public bool isSuccess = false;


        public int targetValue;
        public int currentValue;

        public Achievement(int _num, string _title, string _desc, string _imageName, int _isSuccess, int _targetValue, int _currentValue)
        {
            this.num = _num;
            this.title = _title;
            this.description = _desc;
            this.imageName = _imageName;
            this.isSuccess = System.Convert.ToBoolean(_isSuccess);
            this.targetValue = _targetValue;
            this.currentValue = _currentValue;
        }
    }

    [System.Serializable]
    public class Encyclopedia
    {
        public int num;
        public string name;
        public string description;
        public string imageName;
        public bool isSuccess = false;

        public Encyclopedia(int _num, string _name, string _desc, string _imageName, int _isSuccess)
        {
            this.num = _num;
            this.name = _name;
            this.description = _desc;
            this.imageName = _imageName;
            this.isSuccess = System.Convert.ToBoolean(_isSuccess);
        }
    }



    #endregion


    //플레이 데이터 집합소
    #region Data_Variable

    [System.Serializable]
    public class PlayData
    {
        //game option
        public bool isMachineVibration;
        public bool isScreenVibration;
        public float BGM_Volume;
        public float SFX_Volume;

        //=========================================================
        public List<Inventory> inventory = new List<Inventory>();
        //public List<Emblem> emblem = new List<Emblem>();
        //public string nickName;
        public readonly int baseHp = 250;
        public int mp; //money power, 돈의 힘
        public SEX sex;
        public float moveSpeed;

        //스킨
        public int skin;

        //방어구 관련
        public int maxHp;
        public int currentHp;

        //무기 관련
        public int atk_Min;
        public int atk_Max;
        public float atk_Range;
        public float atk_Speed;
        public float nuckBack_Power;
        public float nuckBack_Percentage;

        //시작방 설정
        //1~3 일반 방
        //4 - 보스방
        //스테이지 관련 변수
        public int currentStage;
        public int finalStage = 4;

        //장비 강화 패시브에 의해 변경되는 값들
        public bool resist_Fire;
        public bool resist_Water;
        public bool resist_Poison;
        public bool resist_Electric;
        public bool attackType_Fire;
        public bool attackType_Water;
        public bool attackType_Poison;
        public bool attackType_Electric;
        public float damage_Reduction;

        //장착중인 장비 데이터 따로 추가
        public int equiWeapon_InventoryNum;
        public int equiArmor_InventoryNum;
    }

    //Tables - Just Read

    public List<Weapon> weapons = new List<Weapon>();
    public List<Armor> armors = new List<Armor>();
    public List<Normal_Monster> normal_Monsters = new List<Normal_Monster>();
    public List<Rare_Monster> rare_Monsters = new List<Rare_Monster>();
    public List<Skill> skill = new List<Skill>();

    //업적, 도감
    public List<Achievement> achievementList = new List<Achievement>();
    public List<Encyclopedia> encyclopedia_MonsterList = new List<Encyclopedia>();
    public List<Encyclopedia> encyclopedia_WeaponList = new List<Encyclopedia>();


    //Player Game Data Instace
    public PlayData playData = new PlayData();

    #endregion

    #region Function
    //인벤토리에 현재 몇개의 아이템을 가지고 있는지 반환합니다.
    public int GetInventoryCount()
    {
        return playData.inventory.Count;
    }
    #endregion
}

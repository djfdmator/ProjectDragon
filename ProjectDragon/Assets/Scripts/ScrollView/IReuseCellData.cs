
public interface IReuseCellData
{
    int Index
    {
        get;
        set;
    }
    int inventoryNum
    {
        get;
        set;
    }
    int DB_Num
    {
        get;
        set;
    }
    string name
    {
        get;
        set;
    }
    int itemValue
    {
        get;
        set;
    } // 아이템 가치 - 강화젬의 강화 수치 같은 것들
    RARITY rarity
    {
        get;
        set;
    } // 희귀도
    CLASS Class
    {
        get;
        set;
    } // 아이템 타입
    string imageName
    {
        get;
        set;
    } //이미지 이름
    int skill_index
    {
        get;
        set;
    }
    float stat
    {
        get;
        set;
    }
    string discription
    {
        get;
        set;
    }

}

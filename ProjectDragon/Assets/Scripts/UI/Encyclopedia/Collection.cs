// ==============================================================
// 수집 관련된 도감 및 업적 관리
//
// AUTHOR: Yang SeEun
// CREATED: 2020-12-28
// UPDATED: 2020-12-28
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collection
{
    public List<Database.Achievement> achievementList = new List<Database.Achievement>();
    public List<Database.Encyclopedia> encyclopedia_MonsterList = new List<Database.Encyclopedia>();
    public List<Database.Encyclopedia> encyclopedia_WeaponList = new List<Database.Encyclopedia>();

    #region 수집시 슬롯 활성화

    /// <summary>
    /// 무기 수집
    /// </summary>
    public void WeaponCollection(List<Database.Inventory> items)
    {
        if(items.Count ==0)
        {
            return;
        }

        //[최초 아이템 획득] 업적 달성
        AchievementCollection(0);

        //bool isChange = false;

        foreach (Database.Inventory item in items)
        {
            if (!encyclopedia_WeaponList[item.DB_Num].active)
            {
                //슬롯 활성화
                encyclopedia_WeaponList[item.DB_Num].active = true;
                //isChange = true;
            }
        }

        ////도감 데이터 저장
        //if (isChange)
        //{
        //    GameManager.Inst.Save_Encyclopedia_Weapon_Table();
        //}
    }

    /// <summary>
    /// 몬스터 수집
    /// </summary>
    public void MonsterCollection(Monster monster)
    {
        //[슬라임 몬스터 처치] 업적 달성
        if (monster.db_Num.Equals(0))    
        {
            AchievementCollection(4);   
        }

        if (!encyclopedia_MonsterList[monster.db_Num].active)
        {
            //슬롯 활성화
            encyclopedia_MonsterList[monster.db_Num].active = true;
            ////도감 데이터 저장
            //GameManager.Inst.Save_Encyclopedia_Monster_Table();
        }
    }

    /// <summary>
    /// 업적 수집
    /// </summary>
    /// <param name="db_num"></param>
    public void AchievementCollection(int db_num)
    {
        if (achievementList[db_num].currentValue < achievementList[db_num].targetValue)
        {
            achievementList[db_num].ChangeCurrentValue();

            //마지막 업적 도전횟수 올리기
            if (achievementList[db_num].active) achievementList[12].ChangeCurrentValue();

            //GameManager.Inst.Save_Achievement_Table();
        }
    }


    #endregion

    #region 데이터 초기화
    /// <summary>
    /// 도감(무기,몬스터) 데이터 초기화
    /// </summary>
    public void ResetEncyclopedia()
    {
        for (int i = 0; i < encyclopedia_MonsterList.Count; i++)
        {
            encyclopedia_MonsterList[i].active = false;
        }
        for (int i = 0; i < encyclopedia_WeaponList.Count; i++)
        {
            encyclopedia_WeaponList[i].active = false;
        }
    }

    /// <summary>
    /// 모든 업적 데이터 초기화
    /// </summary>
    public void ResetAchievement()
    {
        for (int i = 0; i < achievementList.Count; i++)
        {
            achievementList[i].active = false;
            achievementList[i].currentValue = 0;
        }
    }




    ///// <summary>
    ///// 달성하지 못한 업적의 도전횟수만 초기화
    ///// </summary>
    //public void ResetAchievement_CurValue()
    //{
    //    for (int i = 0; i < achievementList.Count; i++)
    //    {
    //        if (!achievementList[i].active)
    //        {
    //            achievementList[i].currentValue = 0;
    //        }
    //    }
    //}
    #endregion

}

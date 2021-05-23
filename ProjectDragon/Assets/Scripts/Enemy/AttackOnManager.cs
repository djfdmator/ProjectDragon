
// ==============================================================
// Cracked EnemiesAttackOn_Manager
//
//  AUTHOR: Yang SeEun
// CREATED: 2020-01-08
// UPDATED: 2020-01-08
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnManager : MonoBehaviour
{


    //Add Animation Event Function 

    public void Slime_AttackOn(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponentInChildren<Slime_t>()!= null)
        {
            GetComponentInChildren<Slime_t>().Attack_On();
        }
    }

    public void Mr_Gobulhwa_AttackOn(AnimationEvent evt )
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponentInChildren<Mr_Gobulhwa_t>()!= null)
        {
            GetComponentInChildren<Mr_Gobulhwa_t>().Attack_On();
        }
    }

    public void Mr_Gobunin_AttackOn(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponentInChildren<Mr_Gobunin_t>()!= null)
        {
            GetComponentInChildren<Mr_Gobunin_t>().Attack_On();
        }
    }

    public void Rimmotal_AttackOn1(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponentInChildren<Rimmotal_t>()!= null)
        {
            GetComponentInChildren<Rimmotal_t>().Attack1_On();
        }
    }


    /// <summary>
    /// Player용 함수
    /// </summary>
    /// <param name="evt"></param>
    public void AttackOn(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponent<ShortRangeAttackArea>() != null)
        {
            GetComponent<ShortRangeAttackArea>().Attack_On();
        }
    }


}

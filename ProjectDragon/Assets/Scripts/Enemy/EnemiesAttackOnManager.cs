
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

public class EnemiesAttackOnManager : MonoBehaviour
{


    //Add Animation Event Function 

    public void Slime_AttackOn(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponentInChildren<Slime>()!= null)
        {
            GetComponentInChildren<Slime>().Attack_On();
        }
    }

    public void Mr_Gobulhwa_AttackOn(AnimationEvent evt )
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponentInChildren<Mr_Gobulhwa>()!= null)
        {
            GetComponentInChildren<Mr_Gobulhwa>().Attack_On();
        }
    }

    public void Mr_Gobunin_AttackOn(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponentInChildren<Mr_Gobunin>()!= null)
        {
            GetComponentInChildren<Mr_Gobunin>().Attack_On();
        }
    }

    public void Rimmotal_AttackOn1(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponentInChildren<Rimmotal>()!= null)
        {
            GetComponentInChildren<Rimmotal>().Attack1_On();
        }
    }
  
}

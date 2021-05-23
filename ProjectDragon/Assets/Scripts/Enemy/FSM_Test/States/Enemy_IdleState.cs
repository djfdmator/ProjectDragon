using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_IdleState : IState<BaseFSM_Enemy>
{
    protected float Current_readyTime;
    protected float Current_cooltime;

    public void OnEnter(BaseFSM_Enemy obj)
    {
        obj.isIdle = true;
    }
    public void OnExecute(BaseFSM_Enemy obj)
    {
    }
    public void OnExit(BaseFSM_Enemy obj)
    {
        obj.isIdle = false;
    }


    
}

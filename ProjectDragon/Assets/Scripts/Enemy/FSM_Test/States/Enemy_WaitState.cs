using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WaitState : IState<BaseFSM_Enemy>
{
    public void OnEnter(BaseFSM_Enemy obj)
    {
        obj.isIdle = true;
        obj.objectAnimator.SetBool("Wait", true);
        obj.rb2d.velocity = Vector2.zero;
    }
    public virtual void OnExecute(BaseFSM_Enemy obj)
    {
        if(obj.isHit)
        {
            return;
        }
        obj.rb2d.velocity = Vector2.zero;
    }
    public void OnExit(BaseFSM_Enemy obj)
    {
        obj.isIdle = false;
        obj.objectAnimator.SetBool("Wait", false);
    }


}

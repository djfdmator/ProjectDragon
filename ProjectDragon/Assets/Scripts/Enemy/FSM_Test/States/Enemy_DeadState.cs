using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_DeadState : IState<BaseFSM_Enemy>
{
    public virtual void OnEnter(BaseFSM_Enemy obj)
    {
        obj.isDead = true;
        obj.col.enabled = false;
        obj.triggerCol.enabled = false;
        obj.isAttacking = false;
        obj.invincible = false;
        obj.rb2d.velocity = Vector2.zero;
        obj.objectAnimator.SetTrigger("Dead");

        UnityEngine.Object.Destroy(obj, 5.0f);
    }
    public virtual void OnExecute(BaseFSM_Enemy obj)
    {
    }
    public virtual void OnExit(BaseFSM_Enemy obj)
    {
    }


}

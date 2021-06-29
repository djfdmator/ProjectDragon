using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeafAttack : IState<BaseFSM_Enemy>
{
    public void OnEnter(BaseFSM_Enemy obj)
    {
        obj.objectAnimator.SetBool("Attack1", true);
        obj.isAttacking = true;
    }
    public void OnExecute(BaseFSM_Enemy obj)
    {
    }
    public void OnExit(BaseFSM_Enemy obj)
    {
        obj.isAttacking = false;
        obj.objectAnimator.SetBool("Attack1", false);
    }

}
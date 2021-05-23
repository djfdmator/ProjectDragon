using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TackleAttack : IState<BaseFSM_Enemy>
{ 
    private float time;

    public void OnEnter(BaseFSM_Enemy obj)
    {
        obj.isAttacking = true;
        obj.objectAnimator.SetBool("Attack", obj.isAttacking);
        obj.objectAnimator.SetBool("isAttackActive", ((FSM_NormalEnemy_t)obj).isAttackActive);

        obj.isNuckback = false;

        time = 0f;
    }

    public void OnExecute(BaseFSM_Enemy obj)
    {
        if (!obj.objectAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            return;
        }
        AnimatorClipInfo[] clipInfo = obj.objectAnimator.GetCurrentAnimatorClipInfo(0);

        float cliptime = clipInfo[0].clip.length;
        float deleyTime = cliptime / obj.objectAnimator.GetCurrentAnimatorStateInfo(0).speed;

        if (time < deleyTime)
        {
            time += Time.deltaTime;
            return;
        }
        //yield return new WaitForSeconds(cliptime / obj.objectAnimator.GetCurrentAnimatorStateInfo(0).speed);

        time = 0f;
        obj.curAttackCount++;
    }

    public void OnExit(BaseFSM_Enemy obj)
    {
        obj.curAttackCount = 0;
        obj.isAttacking = false;
        obj.objectAnimator.SetBool("Attack", obj.isAttacking);
    }

}

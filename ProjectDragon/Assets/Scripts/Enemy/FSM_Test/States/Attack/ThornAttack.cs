using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThornAttack : IState<BaseFSM_Enemy>
{
    private float time = 0;
    Rimmotal_t rimmotal;

    public void OnEnter(BaseFSM_Enemy obj)
    {
        obj.isAttacking = true;
        obj.IsFix = true;
        obj.objectAnimator.SetBool("Attack2", true);
        time = 0f;

        rimmotal = (Rimmotal_t)obj;
    }

    public void OnExecute(BaseFSM_Enemy obj)
    {
        if (rimmotal != null)
        {
            if (rimmotal._thorn_attacking && !obj.isDead)
            {
                if (obj.inAtkDetectionRange)
                {
                    while (time < 2.5f)
                    {
                        time += Time.deltaTime;
                        return;
                    }
                    //가시 생성
                    rimmotal.thornPoint.Create(obj.projectileTargetList, Vector2.zero, new Vector2(0.7f, 0.7f), rimmotal.skillDamage, rimmotal.ThornAnimator, false, obj.other.position - new Vector3(0.0f, 0.5f, 0.0f));
                    SoundManager.Inst.EffectPlayerDB(28, obj.gameObject);
                    time = 0f;
                }
            }
        }
    }

    public void OnExit(BaseFSM_Enemy obj)
    {
        obj.IsFix = false;
        obj.isAttacking = false;
    }

}

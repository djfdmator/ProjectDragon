using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltAndDead : Enemy_DeadState
{
    public override void OnEnter(BaseFSM_Enemy obj)
    {
        //잔해 파티클
        obj.childDeadParticle.SetActive(true);
        obj.childDeadParticle.GetComponent<ParticleSystem>().Play();

        base.OnEnter(obj);
    }
    public override void OnExecute(BaseFSM_Enemy obj)
    {
        Debug.Log("Dead 상태 중간");
    }
    public override void OnExit(BaseFSM_Enemy obj)
    {
        Debug.Log("Dead 상태 끝");

    }
}

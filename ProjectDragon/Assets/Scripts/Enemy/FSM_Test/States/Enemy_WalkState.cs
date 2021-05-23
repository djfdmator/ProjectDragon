using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_WalkState : IState<BaseFSM_Enemy>
{
    public void OnEnter(BaseFSM_Enemy obj)
    {
        obj.objectAnimator.SetBool("Walk", true);
    }
    public void OnExecute(BaseFSM_Enemy obj)
    {
        if (obj.isHit)
        {
            return;
        }
        if (obj.rb2d.velocity != Vector2.zero)
        {
            obj.PushStop();
        }
        else
        {
            //move
            if (!obj.collisionPlayer)
            {
                obj.isWalk = true;
                //AStar
                obj.tracking.FindPathManager(obj.rb2d, obj.MoveSpeed);
                //rb2d.velocity = direction * MoveSpeed * 10.0f * Time.deltaTime;
                //transform.position = Vector3.MoveTowards(transform.position, other.transform.position, MoveSpeed * Time.deltaTime);
            }
            else { obj.isWalk = false; }
        }
    }
    public void OnExit(BaseFSM_Enemy obj)
    {
        obj.isWalk = false;
        obj.objectAnimator.SetBool("Walk", false);
    }


}

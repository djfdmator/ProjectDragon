using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollAndAttack : IState<BaseFSM_Enemy>
{
    private Vector3 attackDirection;
    private float time;
    private float readyTime;
    private bool attackReady = false;

    public void OnEnter(BaseFSM_Enemy obj)
    {
        obj.isAttacking = true;
        obj.objectAnimator.SetBool("Attack", obj.isAttacking);
        obj.objectAnimator.SetBool("isAttackActive", ((FSM_NormalEnemy_t)obj).isAttackActive);


        readyTime = 0f;
        attackReady = false;
        //yield return new WaitForSeconds(1.0f);         //대기

        //obj.invincible = true;                             //무적

        ////Attacking
        //obj.isAttacking = true;
        //obj.objectAnimator.Play("Attacking");
        //attackDirection = obj.direction;
    }

    public void OnExecute(BaseFSM_Enemy obj)
    {
        if (!attackReady)
        {
            if (readyTime < 1.0f)
            {
                readyTime += Time.deltaTime;
                return;
            }
            attackReady = true;
            //1초 대기
            obj.invincible = true;                             //무적

            //Attacking
            obj.isAttacking = true;
            obj.objectAnimator.Play("Attacking");
            attackDirection = obj.direction;
        }

        if(time <= Time.deltaTime)
        {
            time += Time.deltaTime;
            return;
        }
        time = 0f;
        obj.rb2d.AddForce(attackDirection * obj.ATTACKSPEED, ForceMode2D.Impulse);
        //yield return new WaitForSeconds(Time.deltaTime);
    }

    public void OnExit(BaseFSM_Enemy obj)
    { 
        obj.isAttacking = false;
        obj.invincible = false;
        obj.objectAnimator.SetBool("Attack", obj.isAttacking);

        //반동
        obj.rb2d.velocity = Vector2.zero;
        obj.rb2d.AddForce(-attackDirection * 1.5f, ForceMode2D.Impulse);
    }
}

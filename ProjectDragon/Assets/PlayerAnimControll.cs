using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControll : MonoBehaviour
{
    // 애니메이터 받아오기
    private Animator anim_Body;
    private Animator anim_Arm;
    private Animator anim_Weapon;

    public int Weapon_Num;

    // 애니메이터 Angle 제어
    private float Angle;


    // 해당 공격 타입 관련으로 세팅
    public AttackType myAttackType;
    public AttackType CurrentAttackType
    {
        get { return myAttackType; }
        set
        {
            myAttackType = value;
        }
    }

    // 해당 공격 상황 관련 세팅
    public State myState;
    [SerializeField]
    public State CurrentState
    {
        get { return myState; }
        set
        {
            //anim_Arm.speed = 0.1f;
            //anim_Weapon.speed = 0.1f;

            State m_State = State.Idle;
            for (int i = 1; i <= State.Hit.GetHashCode(); i++)
            {
                anim_Body.SetBool(ChangeState(m_State), false);
                anim_Arm.SetBool(ChangeState(m_State), false);
                anim_Weapon.SetBool(ChangeState(m_State), false);
                m_State++;
            }
            myState = value;
            anim_Body.SetBool(ChangeState(myState), true);
            anim_Arm.SetBool(ChangeState(myState), true);
            anim_Weapon.SetBool(ChangeState(myState), true);
        }
    }

    private void Awake()
    {
        Angle = 0;
        anim_Body = GetComponent<Animator>();
        anim_Arm = gameObject.transform.Find("Arm").GetComponent<Animator>();

        anim_Weapon = transform.Find("Weapon").GetComponent<Animator>();


        anim_Body.speed = 1f;
        anim_Arm.speed = 1f;
        anim_Weapon.speed = 1f;

        CurrentAttackType = GetComponent<Player>().attackType;
    }

    public string ChangeState(State state)
    {
        return "is" + state.ToString();
    }

    public void AnimationStop()
    {
        GetComponent<Player>().playerSkill.PlayerStop();
    }

    public void ChangeAngleAnim(float angle)
    {
        Angle = angle;
        anim_Body.SetFloat("Angle", Angle);
        anim_Arm.SetFloat("Angle", Angle);
        anim_Weapon.SetFloat("Angle", Angle);
    }

    // Update is called once per frame
    // 데모 체커
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CurrentState = State.Idle;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CurrentState = State.Walk;
            Debug.Log("Walk");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            CurrentState = State.Attack;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            CurrentState = State.Skill;
            Debug.Log("skill");

        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            CurrentState = State.Dead;
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            CurrentState = State.Get;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Angle = 180;
            anim_Body.SetFloat("Angle", Angle);
            anim_Arm.SetFloat("Angle", Angle);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Angle = 0;
            anim_Body.SetFloat("Angle", Angle);
            anim_Arm.SetFloat("Angle", Angle);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Angle = 90;
            anim_Body.SetFloat("Angle", Angle);
            anim_Arm.SetFloat("Angle", Angle);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Angle = 270;
            anim_Body.SetFloat("Angle", Angle);
            anim_Arm.SetFloat("Angle", Angle);
        }
    }
}

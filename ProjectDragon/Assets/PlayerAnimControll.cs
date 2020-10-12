using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControll : MonoBehaviour
{
    // 애니메이터 받아오기
    public Animator PlayerAnimation;
    public Animator PlayerAnimation_Arm;

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
            PlayerAnimation_Arm.speed = 0.1f;

            State m_State = State.Idle;
            for (int i = 1; i <= State.Hit.GetHashCode(); i++)
            {
                PlayerAnimation.SetBool(ChangeState(m_State), false);
                PlayerAnimation_Arm.SetBool(ChangeState(m_State), false);
                m_State++;
            }
            myState = value;
            PlayerAnimation.SetBool(ChangeState(myState), true);
            PlayerAnimation_Arm.SetBool(ChangeState(myState), true);
        }
    }

    private void Awake()
    {
        Angle = 0;
        PlayerAnimation = GetComponent<Animator>();
        PlayerAnimation_Arm = gameObject.transform.GetChild(0).GetComponent<Animator>();
        PlayerAnimation_Arm.speed = 0.1f;

        CurrentAttackType = GetComponent<Player>().attackType;
    }

    public string ChangeState(State state)
    {
        return "is" + state.ToString();
    }

    public void ChangeAngleAnim(float angle)
    {
        Angle = angle;
        PlayerAnimation.SetFloat("Angle", Angle);
        PlayerAnimation_Arm.SetFloat("Angle", Angle);
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
            PlayerAnimation.SetFloat("Angle", Angle);
            PlayerAnimation_Arm.SetFloat("Angle", Angle);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Angle = 0;
            PlayerAnimation.SetFloat("Angle", Angle);
            PlayerAnimation_Arm.SetFloat("Angle", Angle);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Angle = 90;
            PlayerAnimation.SetFloat("Angle", Angle);
            PlayerAnimation_Arm.SetFloat("Angle", Angle);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Angle = 270;
            PlayerAnimation.SetFloat("Angle", Angle);
            PlayerAnimation_Arm.SetFloat("Angle", Angle);
        }
    }
}

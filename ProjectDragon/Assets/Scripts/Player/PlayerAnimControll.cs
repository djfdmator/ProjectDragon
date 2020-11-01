//Animation Mecanim Controll Script

using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PlayerAnimControll : MonoBehaviour
{

    // 무기타입에 맞는 애니메이터
    [SerializeField] private Animator curAnimBody;
    [SerializeField] private Animator curAnim_Arm;
    [SerializeField] private Animator curAnim_Weapon;

    private Player player;

    // 애니메이터 Angle 제어
    private float Angle;


    // 해당 공격 타입 관련으로 세팅
    private AttackType myAttackType;
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
            //curAnim_Arm.speed = 0.1f;
            //curAnim_Weapon.speed = 0.1f;
            if (curAnimBody != null && curAnim_Arm != null && curAnim_Weapon != null)
            {
                State m_State = State.Idle;
                for (int i = 1; i <= State.Hit.GetHashCode(); i++)
                {
                    curAnimBody.SetBool(ChangeState(m_State), false);
                    curAnim_Arm.SetBool(ChangeState(m_State), false);
                    curAnim_Weapon.SetBool(ChangeState(m_State), false);
                    m_State++;
                }

                myState = value;
                if (myState.GetHashCode().Equals(State.Dead.GetHashCode()))
                {
                    curAnimBody.SetTrigger(ChangeState(myState));
                    curAnim_Arm.SetTrigger(ChangeState(myState));
                    curAnim_Weapon.SetTrigger(ChangeState(myState));
                }
                else
                {
                    curAnimBody.SetBool(ChangeState(myState), true);
                    curAnim_Arm.SetBool(ChangeState(myState), true);
                    curAnim_Weapon.SetBool(ChangeState(myState), true);
                }
            }
        }
    }

    private void Awake()
    {
        Angle = 0;
        curAnimBody = GetComponent<Animator>();
        curAnim_Arm = gameObject.transform.Find("Arm").GetComponent<Animator>();
        curAnim_Weapon = transform.Find("Weapon").GetComponent<Animator>();
        player = GetComponent<Player>();


       
    }

    private void Start()
    {
        LoadAnimator(player.weaponType);

        curAnimBody.speed = 1f;
        curAnim_Arm.speed = 1f;
        curAnim_Weapon.speed = 1f;
    }


    private void LoadAnimator(Player.WeaponType weaponType)
    {

        curAnimBody.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(string.Format("Animation/Player/{0}/Player_Body_{1}", weaponType, weaponType));
        curAnim_Weapon.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(string.Format("Animation/Player/{0}/Player_Weapon_{1}", weaponType, weaponType));
        string normalType = (player.attackType == AttackType.ShortRange) ? "NormalSword" : "NormalStaff";
        curAnim_Arm.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(string.Format("Animation/Player/{0}/Player_Arm_{1}", normalType, normalType));
        //Debug.Log(string.Format("Animation/Player/{0}/Player_Arm_{1}", normalType, normalType));
    }

    private readonly int idleStateHash = Animator.StringToHash("isIdle");
    private readonly int walkStateHash = Animator.StringToHash("isWalk");
    private readonly int attackStateHash = Animator.StringToHash("isAttack");
    private readonly int skillStateHash = Animator.StringToHash("isSkill");
    private readonly int deadStateHash = Animator.StringToHash("isDead");
    private readonly int getStateHash = Animator.StringToHash("isGet");
    private readonly int hitStateHash = Animator.StringToHash("isHit");

    public int ChangeState(State state)
    {
        switch(state)
        {
            case State.Idle:
                return idleStateHash;
            case State.Walk:
                return walkStateHash;
            case State.Attack:
                return attackStateHash;
            case State.Skill:
                return skillStateHash;
            case State.Dead:
                return deadStateHash;
            case State.Get:
                return getStateHash;
            case State.Hit:
                return hitStateHash;
        }
        return 0;
        //return "is" + state.ToString();
    }

    public void AnimationStop()
    {
        if (!player.isDead)
        {
            player.playerSkill.PlayerStop();
        }
    }

    public void ChangeAngleAnim(float angle)
    {
        Angle = angle;
        curAnimBody.SetFloat("Angle", Angle);
        curAnim_Arm.SetFloat("Angle", Angle);
        curAnim_Weapon.SetFloat("Angle", Angle);
    }
}

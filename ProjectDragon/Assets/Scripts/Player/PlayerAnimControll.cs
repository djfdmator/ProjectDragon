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
                Debug.Log("dd?");
                State m_State = State.Idle;
                for (int i = 1; i <= State.Hit.GetHashCode(); i++)
                {
                    curAnimBody.SetBool(ChangeState(m_State), false);
                    curAnim_Arm.SetBool(ChangeState(m_State), false);
                    curAnim_Weapon.SetBool(ChangeState(m_State), false);
                    m_State++;
                }

                myState = value;
                curAnimBody.SetBool(ChangeState(myState), true);
                curAnim_Arm.SetBool(ChangeState(myState), true);
                curAnim_Weapon.SetBool(ChangeState(myState), true);
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

        LoadAnimator(player.weaponType);

       
    }

    private void Start()
    {

        curAnimBody.speed = 1f;
        curAnim_Arm.speed = 1f;
        curAnim_Weapon.speed = 1f;
    }
    

    private void LoadAnimator(Player.WeaponType weaponType)
    {
        Debug.Log("Loading");
        curAnimBody.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(string.Format("Animation/Player/{0}/Player_Body_{1}", weaponType, weaponType));
        curAnim_Weapon.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(string.Format("Animation/Player/{0}/Player_Weapon_{1}", weaponType, weaponType));
        string normalType = (player.attackType == AttackType.ShortRange) ? "NormalSword" : "NormalStaff";
        curAnim_Arm.runtimeAnimatorController = Resources.Load<RuntimeAnimatorController>(string.Format("Animation/Player/{0}/Player_Arm_{1}", normalType, normalType));
        Debug.Log(string.Format("Animation/Player/{0}/Player_Arm_{1}", normalType, normalType));
       


        //object [] animator = Resources.LoadAll<RuntimeAnimatorController>("Animation/Player/");
        //foreach (RuntimeAnimatorController anim in animator)
        //{
        //    string t = anim.name.Split('_')[1];
        //    if (t.Equals("Weapon"))
        //    {
        //        Player.WeaponType type = (Player.WeaponType)Enum.Parse(typeof(Player.WeaponType),anim.name.Split('_')[3]);
        //        _weaponAnimators.Add(type,anim);
        //    }
        //    else if(t.Equals("Body"))
        //    {
        //        Player.WeaponType type = (Player.WeaponType)Enum.Parse(typeof(Player.WeaponType), anim.name.Split('_')[3]);
        //        _bodyAnimators.Add(type, anim);
        //    }
        //    else
        //    {
        //        AttackType type = (AttackType)Enum.Parse(typeof(AttackType), anim.name.Split('_')[2]);
        //        _armAnimators.Add(type, anim);
        //    }
        //}
        //foreach(KeyValuePair<Player.WeaponType, RuntimeAnimatorController> pair in _weaponAnimators)
        //{
        //    Debug.Log(pair.Key, pair.Value);
        //}
    }


    public string ChangeState(State state)
    {
        return "is" + state.ToString();
    }

    public void AnimationStop()
    {
        player.playerSkill.PlayerStop();
    }

    public void ChangeAngleAnim(float angle)
    {
        Angle = angle;
        curAnimBody.SetFloat("Angle", Angle);
        curAnim_Arm.SetFloat("Angle", Angle);
        curAnim_Weapon.SetFloat("Angle", Angle);
    }

#if UNITY_EDITOR
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
            curAnimBody.SetFloat("Angle", Angle);
            curAnim_Arm.SetFloat("Angle", Angle);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            Angle = 0;
            curAnimBody.SetFloat("Angle", Angle);
            curAnim_Arm.SetFloat("Angle", Angle);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Angle = 90;
            curAnimBody.SetFloat("Angle", Angle);
            curAnim_Arm.SetFloat("Angle", Angle);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            Angle = 270;
            curAnimBody.SetFloat("Angle", Angle);
            curAnim_Arm.SetFloat("Angle", Angle);
        }
    }
#endif
}

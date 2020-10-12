//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorControll : MonoBehaviour
{
    int counter;
    [Tooltip("재생할 오브젝트를 받아옵니다.")]
    public Player Anim_Master;
    public GameObject Arm;
    public GameObject Weapon;

    [Tooltip("변수 할당용")]
    public AnglePos anglepos;
    [Tooltip("변수 할당용")]
    public State my_state;

    public string AttackName;


    //플레이어 애니메이터
    [Header("플레이어 애니메이터(몸통)")]
    [Tooltip("now player animation")]
    public Animator playeranim;
    [Tooltip("runtime controller when the play begin")]
    public RuntimeAnimatorController controller;
    [Tooltip("override the controller after")]
    public AnimatorOverrideController overrideController;

    //플레이어 애니메이터(무기)
    [Header("플레이어 애니메이터(무기)")]
    [Tooltip("player weapon animation")]
    public Animator playeranim_Weapon;
    [Tooltip("runtime controller when the play begin")]
    public RuntimeAnimatorController controller_Weapon;
    [Tooltip("override the controller after")]
    public AnimatorOverrideController overrideController_Weapon;

    //플레이어 애니메이터(팔)
    [Header("플레이어 애니메이터(팔)")]
    [Tooltip("player Arm animation")]
    public Animator playeranim_Arm;
    [Tooltip("runtime controller when the play begin")]
    public RuntimeAnimatorController controller_Arm;
    [Tooltip("override the controller after")]
    public AnimatorOverrideController overrideController_Arm;

    //애니메이션 컨트롤

    [Tooltip("죽음에 관련된 애니메이션")]
    public AnimationClip[] animDead;
    [Tooltip("걷기에 관련된 애니메이션")]
    public AnimationClip[] animWalk;
    [Tooltip("Hit에 관련된 애니메이션")]
    public AnimationClip[] animHit;
    [Tooltip("스킬에 관련된 애니메이션")]
    public AnimationClip[] animSkill;
    [Tooltip("공격에 관련된 애니메이션")]
    public AnimationClip[] animAttack;
    [Tooltip("대기에 관련된 애니메이션")]
    public AnimationClip[] animIdel;

    public string ClearAnimator_Name
    {
        get
        {
            return clearAnimator_name;
        }
        set
        {
            if (!value.Equals(temp_name))
            {
                clearAnimator_name = value;
                AngleStringCast(clearAnimator_name);
            }
        }
    }
    //일시적으로 이름을 저장하는 장소
    public string temp_name;
    public string clearAnimator_name;
    //public Animation animation;
    //public RuntimeAnimatorController controller;
    //public AnimationClip animationClip;
    public void LateUpdate()
    {
        if (Anim_Master.AngleisAttack == true||Anim_Master.isSkillActive)
        {
            temp_name = clearAnimator_name;
            my_state = Anim_Master.CurrentState;
            anglepos = Anim_Master.Enemy_AngleCaseString(Anim_Master.enemy_angle);
            AttackName = "Female_DefaultCloth_"+Anim_Master.attackType.ToString()+"_" + my_state + "_" + anglepos.ToString();
            ClearAnimator_Name = AttackName;
           // AngleStringCast(AttackName);
        }
        else if (Anim_Master.AngleisAttack == false)
        {
            temp_name = clearAnimator_name;
            my_state = Anim_Master.CurrentState;
            anglepos = Anim_Master.Current_AngleCaseString(Anim_Master.current_angle);
            ClearAnimator_Name = "Female_DefaultCloth_"+Anim_Master.attackType.ToString()+ "_" + my_state + "_" + anglepos.ToString();
            //AngleStringCast(clearAnimator_name);
        }

    }
    private void Awake()
    {
        Anim_Master = GameObject.Find("테스터").GetComponent<Player>();
        Arm = Anim_Master.gameObject.transform.GetChild(1).gameObject;
        Weapon = Anim_Master.gameObject.transform.GetChild(0).gameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        anglepos = 0;
        //플레이어 팔 애니메이션
        playeranim_Arm = Arm.GetComponent<Animator>();
        controller_Arm = playeranim_Arm.runtimeAnimatorController;
        overrideController_Arm = new AnimatorOverrideController(playeranim_Arm.runtimeAnimatorController);
        //플레이어 무기 애니메이션
        playeranim_Weapon = Weapon.GetComponent<Animator>();
        controller_Weapon = playeranim_Weapon.runtimeAnimatorController;
        overrideController_Weapon = new AnimatorOverrideController(playeranim_Weapon.runtimeAnimatorController);
        //플레이어 몸통 애니메이션
        playeranim = GetComponent<Animator>();
        controller = playeranim.runtimeAnimatorController;
        overrideController = new AnimatorOverrideController(playeranim.runtimeAnimatorController);
        my_state = State.None;

        //Animation 
        for (int i = 1; i <= State.Hit.GetHashCode(); i++)
        {
            my_state++;
            clearAnimator_name = "Female_DefaultCloth_"+Anim_Master.attackType.ToString()+"_" + my_state.ToString();
            temp_name = clearAnimator_name;
            Player_AnimationController_CastingCurrentAnim(animationValueChanger(my_state), my_state);
            Player_AnimationController_CastingCurrentAnim_Arm(animationValueChanger(my_state), my_state);
            Player_AnimationController_CastingCurrentAnim_Weapon(animationValueChanger(my_state), my_state);
        }
        my_state = State.Idle;
    }
    void Player_AnimationController_CastingCurrentAnim(AnimationClip[] animationbundle, State state)
    {
        // clip = playeranim.GetCurrentAnimatorClipInfo(0)[0].clip;
        for (int i = 0; i < animationbundle.Length; i++)
        {
            anglepos++;
            animationbundle[i] = Resources.Load<AnimationClip>("Animation/Player/Female/DefaultCloth/"+ Anim_Master.attackType.ToString()+"/" + state.ToString() + "/" + temp_name + "_" + anglepos.ToString());
            overrideController[temp_name + "_" + anglepos.ToString()] = animationbundle[i];
            playeranim.runtimeAnimatorController = overrideController;
        }
        anglepos = AnglePos.None;
        //playeranim.runtimeAnimatorController = overrideController;
        //타임 스피드 변경
        playeranim.speed = 0.25f;
        //들고 있는 애니메이션 중에서 무엇을 재생할 것인지에 대한 정의
    }
    void Player_AnimationController_CastingCurrentAnim_Arm(AnimationClip[] animationbundle, State state)
    {
        // clip = playeranim.GetCurrentAnimatorClipInfo(0)[0].clip;
        for (int i = 0; i < animationbundle.Length; i++)
        {
            anglepos++;
            animationbundle[i] = Resources.Load<AnimationClip>("Animation/Player/Female/DefaultCloth/" + Anim_Master.attackType.ToString() + "/" + state.ToString() + "/" + temp_name + "_" + anglepos.ToString() + "_Arm");
            overrideController[temp_name + "_" + anglepos.ToString() + "_Arm"] = animationbundle[i];
            playeranim.runtimeAnimatorController = overrideController;
        }
        anglepos = AnglePos.None;
        //playeranim.runtimeAnimatorController = overrideController;
        //타임 스피드 변경
        playeranim_Arm.speed = 0.25f;
    }
    void Player_AnimationController_CastingCurrentAnim_Weapon(AnimationClip[] animationbundle, State state)
    {
        // clip = playeranim.GetCurrentAnimatorClipInfo(0)[0].clip;
        for (int i = 0; i < animationbundle.Length; i++)
        {
            //앵글 각도를 0으로 찍고 각 스트링에 따라 애니메이션 결합(Resoures.Load를 통함)
            anglepos++;
            animationbundle[i] = Resources.Load<AnimationClip>("Animation/Player/Female/DefaultCloth/" + Anim_Master.attackType.ToString() + "/" + state.ToString() + "/" + temp_name + "_" + anglepos.ToString() + "_Weapon");
            overrideController[temp_name + "_" + anglepos.ToString() + "_Weapon"] = animationbundle[i];
            playeranim.runtimeAnimatorController = overrideController;
        }
        anglepos = AnglePos.None;
        //playeranim.runtimeAnimatorController = overrideController;
        //타임 스피드 변경
        playeranim_Weapon.speed = 0.25f;
    }
    AnimationClip[] animationValueChanger(State state)
    {
        switch (state)
        {
            case State.Idle:
                return animIdel = new AnimationClip[4];
            case State.Walk:
                return animWalk = new AnimationClip[4];
            case State.Dead:
                return animDead = new AnimationClip[1];
            case State.Hit:
                return animHit = new AnimationClip[4];
            case State.Skill:
                return animSkill = new AnimationClip[4];

            case State.Attack:
                return animAttack = new AnimationClip[4];
        }
        return animIdel = new AnimationClip[4];
    }
    void AngleStringCast(string name)
    {
        if(!Anim_Master.isSkillActive)
        {
        playeranim.Play(name);
        playeranim_Arm.GetComponent<SpriteRenderer>().enabled = true;
        playeranim_Weapon.GetComponent<SpriteRenderer>().enabled = true;
        playeranim_Arm.Play(name + "_Arm");
        playeranim_Weapon.Play(name + "_Weapon");
        }
        if(Anim_Master.isSkillActive)
        {
            StartCoroutine(animation_Skill(name));
        }
    }
    public void AnimationStop()
    {
        Anim_Master.playerSkill.PlayerStop();
    }
    IEnumerator animation_Skill(string name)
    {
        playeranim.Play(name);
        yield return null;
        playeranim_Arm.GetComponent<SpriteRenderer>().enabled = false;
        playeranim_Weapon.GetComponent<SpriteRenderer>().enabled = false;
    }
}
//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAnimator : MonoBehaviour
{
    public Vector3 joystickPos;
    public GameObject joypadinput;
    [Header("PlayerObject")]
    public GameObject playerObject;

    [Header("PlayerScript")]
    public Player currentPlayer;

    [Header("SpriteRenderer")]
    public SpriteRenderer spriteRenderer;

    [Header("Player Sprite")]
    Sprite[] play_animtionSprite;
    string animationname { get { return m_animationName; } set { if (!value.Equals(m_animationName)) { m_animationName = value; AnimationNameChecker(); } } }
    string m_animationName;

    [Header("AttackTYPE")]
    public AttackType m_attacktype;
    public IsWear m_cloth;
    public State m_state;
    public SEX m_sex;
    //AnimationControl

    [Header("AnimationCounter")]
    public int AttackAniCount;
    public int WalkAniCount;
    public int HitAniCount;
    public int SkillAniCount;

    [Header("Dead")]
    public Sprite[] PlayerDead;
    [Header("Walk")]
    public Sprite[] PlayerWalkLeft;
    public Sprite[] PlayerWalkRight;
    public Sprite[] PlayerWalkFront;
    public Sprite[] PlayerWalkBack;
    [Header("Attack")]
    public Sprite[] PlayerAttackLeft;
    public Sprite[] PlayerAttackRight;
    public Sprite[] PlayerAttackFront;
    public Sprite[] PlayerAttackBack;
    [Header("Hit")]
    public Sprite[] PlayerHitLeft;
    public Sprite[] PlayerHitRight;
    public Sprite[] PlayerHitFront;
    public Sprite[] PlayerHitBack;
    [Header("Skill")]
    public Sprite[] PlayerSkillLeft;
    public Sprite[] PlayerSkillRight;
    public Sprite[] PlayerSkillFront;
    public Sprite[] PlayerSkillBack;


    // Start is called before the first frame Backdate
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        m_attacktype = AttackType.LongRange;
        m_cloth = IsWear.DefaultCloth;
        m_state = State.Walk;
        m_sex = SEX.Female;
        
        //   DeadAnim();
        AnimationRangeCheck();
        Walk_AnimationMatching();
        Attack_AnimationMatching();
        Hit_AnimationMatching();
        Skill_AnimationMatching();
        currentPlayer = gameObject.GetComponent<Player>();
        play_animtionSprite = PlayerWalkFront;
        StartCoroutine(AnimationControllPlayer(currentPlayer));
        animationname = "PlayerHit";
    }

    // Backdate is called once per frame
    void Update()
    {
        //animationname = AnimationName_Returner("Player", m_state, currentPlayer.p_AnglePos);
    }

    void AttackRangeAnimationSetting(AttackType m_attacktype)
    {
        switch (m_attacktype)
        {
            case AttackType.ShortRange:
                break;
            case AttackType.MiddleRange:
                break;
            case AttackType.LongRange:
                break;
        }
    }
    void DeadAnim()
    {
        PlayerDead = new Sprite[10];
        string name = ("Animation/Player/" + m_sex.ToString() + "/" + m_cloth.ToString() + "/" + m_attacktype.ToString() + "/" + State.Dead.ToString() + "/");
        for (int i = 1; i < PlayerDead.Length + 1; i++)
        {
            PlayerDead[i - 1] = Resources.Load<Sprite>(name + (i - 1));
        }
    }
    void AnimationRangeCheck()
    {
        switch (m_attacktype)
        {
            case AttackType.ShortRange:
                AttackAniCount = 5;
                WalkAniCount = 5;
                HitAniCount = 5;
                break;
            case AttackType.MiddleRange:
                AttackAniCount = 5;
                WalkAniCount = 5;
                HitAniCount = 5;
                break;
            case AttackType.LongRange:
                AttackAniCount = 5;
                WalkAniCount = 5;
                SkillAniCount = 5;
                HitAniCount = 5;
                break;
        }
    }
    void Attack_AnimationMatching()
    {
        PlayerAttackLeft = new Sprite[AttackAniCount];
        PlayerAttackRight = new Sprite[AttackAniCount];
        PlayerAttackFront = new Sprite[AttackAniCount];
        PlayerAttackBack = new Sprite[AttackAniCount];

        string name = ("Animation/Player/" + m_sex.ToString() + "/" + m_cloth.ToString() + "/" + m_attacktype.ToString() + "/" + State.Attack.ToString() + "/");
        for (int i = 0; i < AttackAniCount; i++)
        {
            PlayerAttackLeft[i] = Resources.Load<Sprite>(name + AnglePos.Left.ToString() + "/" + m_cloth.ToString() + State.Attack.ToString() + "Left" + i);
            PlayerAttackRight[i] = Resources.Load<Sprite>(name + AnglePos.Right.ToString() + "/" + m_cloth.ToString() + State.Attack.ToString() + "Right" + i);
            PlayerAttackFront[i] = Resources.Load<Sprite>(name + AnglePos.Front.ToString() + "/" + m_cloth.ToString() + State.Attack.ToString() + "Front" + i);
            PlayerAttackBack[i] = Resources.Load<Sprite>(name + AnglePos.Back.ToString() + "/" + m_cloth.ToString() + State.Attack.ToString() + "Back" + i);
        }
    }
    void Walk_AnimationMatching()
    {
        PlayerWalkLeft = new Sprite[WalkAniCount];
        PlayerWalkRight = new Sprite[WalkAniCount];
        PlayerWalkFront = new Sprite[WalkAniCount];
        PlayerWalkBack = new Sprite[WalkAniCount];
        string name = ("Animation/Player/" + m_sex.ToString() + "/" + m_cloth.ToString() + "/" + m_attacktype.ToString() + "/" + State.Walk.ToString() + "/");
        string sprite_name = m_sex.ToString() + "_" + m_cloth.ToString() + "_Walk_";
        for (int i = 0; i < WalkAniCount; i++)
        {
            PlayerWalkLeft[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Left.ToString() + "_" + i);
            PlayerWalkRight[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Right.ToString() + "_" + i);
            PlayerWalkFront[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Front.ToString() + "_" + i);
            PlayerWalkBack[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Back.ToString() + "_" + i);
        }
    }
    void Skill_AnimationMatching()
    {
        PlayerSkillLeft = new Sprite[SkillAniCount];
        PlayerSkillRight = new Sprite[SkillAniCount];
        PlayerSkillFront = new Sprite[SkillAniCount];
        PlayerSkillBack = new Sprite[SkillAniCount];
        string name = ("Animation/Player/" + m_sex.ToString() + "/" + m_cloth.ToString() + "/" + m_attacktype.ToString() + "/" + State.Skill.ToString() + "/");
        string sprite_name = m_sex.ToString() + "_" + m_cloth.ToString() + "_Skill_";
        for (int i = 0; i < SkillAniCount; i++)
        {
            PlayerSkillLeft[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Left.ToString() + "_" + i);
            PlayerSkillRight[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Right.ToString() + "_" + i);
            PlayerSkillFront[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Front.ToString() + "_" + i);
            PlayerSkillBack[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Back.ToString() + "_" + i);
        }
    }
    void Hit_AnimationMatching()
    {
        PlayerHitLeft = new Sprite[HitAniCount];
        PlayerHitRight = new Sprite[HitAniCount];
        PlayerHitFront = new Sprite[HitAniCount];
        PlayerHitBack = new Sprite[HitAniCount];
        string name = ("Animation/Player/" + m_sex.ToString() + "/" + m_cloth.ToString() + "/" + m_attacktype.ToString() + "/" + State.Hit.ToString() + "/");
        string sprite_name = m_sex.ToString() + "_" + m_cloth.ToString() + "_Hit_";
        for (int i = 0; i < HitAniCount; i++)
        {
            PlayerHitLeft[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Left.ToString() + "_" + i);
            PlayerHitRight[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Right.ToString() + "_" + i);
            PlayerHitFront[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Front.ToString() + "_" + i);
            PlayerHitBack[i] = Resources.Load<Sprite>(name + sprite_name + AnglePos.Back.ToString() + "_" + i);
        }
    }
    IEnumerator AnimationControllPlayer(Character c)
    {
        bool dead = false;
        float m_frameTime = 1;
        float collect_frameTime = 0.5f;

        while (!dead)
        {
            collect_frameTime = AnimationFramePlay(play_animtionSprite, m_frameTime);
            spriteRenderer.sprite = play_animtionSprite[c.current_Anim_Frame];
            c.current_Anim_Frame++;
            if (c.current_Anim_Frame >= play_animtionSprite.Length)
            {
                c.current_Anim_Frame = 0;
            }
            yield return new WaitForSeconds(collect_frameTime);
        }
    }
    float AnimationFramePlay(Sprite[] spritePack, float frameTime)
    {
        return frameTime / spritePack.Length;
    }
    void AnimationNameChecker()
    {
        switch (animationname)
        {
            case "PlayerWalkFront":
                play_animtionSprite = PlayerWalkFront;
                Debug.Log("PlayerWalkFront");
                break;
            case "PlayerWalkLeft":
                play_animtionSprite = PlayerWalkLeft;
                Debug.Log("PlayerWalkLeft");
                break;
            case "PlayerWalkRight":
                play_animtionSprite = PlayerWalkRight;
                Debug.Log("PlayerWalkRight");
                break;
            case "PlayerWalkBack":
                play_animtionSprite = PlayerWalkBack;
                Debug.Log("PlayerWalkBack");
                break;
        }
        currentPlayer.current_Anim_Frame = 0;
    }
    string AnimationName_Returner(string Player, State state, AnglePos angle)
    {
        return Player + state + angle;
    }
}

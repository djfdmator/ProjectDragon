//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { None = 0, Idel, Walk, Attack, Dead, Skill, Hit,Get}
public enum AnglePos { None = 0, Front, Right, Back, Left }
public enum AttackType { None = 0, LongRange, MiddleRange, ShortRange }


public class Character : MonoBehaviour
{
    [Header("스테이터스")]
    [SerializeField] protected int currentHp;
    [SerializeField] protected int hp;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int atk;
    [SerializeField] protected float nuckBack;

    [Header("속성 저항값")]
    protected int resist_Fire;
    protected int resist_Water;
    protected int resist_Poison;
    protected int resist_Electric;

    [Header("속성 데미지값")]
    protected int attackType_Fire;
    protected int attackType_Water;
    protected int attackType_Poison;
    protected int attackType_Electric;



//personal Specification
    [SerializeField] public int maxHp;
    [SerializeField] protected float atkSpeed;
    [SerializeField] protected float atkRange;
    public Vector3 myPos;
    public Vector3 myRotat;
    public float current_angle;

    public Transform other;

    // public bool flag_invincibility;
    // public float 

    public bool isAttack=true;

    protected bool isAttacking;
    [SerializeField]
    protected bool isWalk;
    public bool isDead;
    protected bool isHit;
    public bool isSkillActive;

/// <summary>
/// 데미지 팝업 변수
/// </summary>
    protected DamagePopup damagePopup;

    public List<string> projectileTargetList;

    protected virtual void Awake()
    {
        projectileTargetList = new List<string>();
        damagePopup = new DamagePopup();
    }
    protected virtual void Start()
    {

    }

    #region ATKSPEED
    public float ATTACKSPEED
    {
        get { return atkSpeed; }
        set
        {
            atkSpeed = value;
        }
    }

    public float ATKSpeedChanger(float _attackSpeed)
    {
        ATTACKSPEED = ATTACKSPEED + _attackSpeed;
        return ATTACKSPEED;
    }
    #endregion

    #region ATK
    public int ATTACKDAMAGE
    {
        get { return atk; }
        set
        {
            atk = value;
        }
    }

    public int ATKChanger(int attackDamage)
    {
        atk = atk + attackDamage;
        return atk;
    }
    #endregion
    
    public virtual void Dead()
    {
        isDead = true;
    }

    #region HPControll
    public virtual int HP
    {
        get { return hp; }
        set
        {
            if (value > 0)
            {
                hp = value;
                hp = Mathf.Clamp(value, 0, maxHp);
            }
            else if (!isDead)
            {
                hp = -1;
                Dead();
                Debug.Log("죽었습니다.");
            }
        }
    }
    /// <summary>
    /// 공격력 / 크리티컬 / 넉백 수치 / 회피율
    /// </summary>
    /// <param name="ATK"></param> : ATK = 공격력
    /// <param name="NukBack"></param> NukBack = 뒤로 얼마나 넉백할건지
    /// <param name="isCritical"></param> isCritical = 넉백을 시킬건지? (크리티컬이 터졌는지)
    /// <returns></returns>
    public virtual int HPChanged(int ATK, bool isCritical, int NukBack)
    {
        HP = HP - ATK;
        return HP;
    }
    /*
    //보이는 족족 수정 에정 이거 이제 안쓸거임
    public virtual int HPChanged(int ATK)
    {
        HP = HP - ATK;
        return HP;
    }
    */
    #endregion

    #region MoveSpeed
    public float MoveSpeed
    {
        get { return moveSpeed; }
        set
        {
            moveSpeed = value;
        }
    }

    public float MoveSpeedChanger(float _moveSpeed)
    {
        MoveSpeed = MoveSpeed - _moveSpeed;
        return MoveSpeed;
    }
    #endregion

    #region AtkRange
    public float AtkRange
    {
        get { return atkRange; }
        set
        {
            atkRange = value;
        }
    }

    public float AtkRangeChanger(float _atkRange)
    {
        AtkRange = _atkRange;
        return AtkRange;
    }
    #endregion



    //개체의 상태가 바뀔때마다 실행
    protected virtual void SetState<T>(T newState)
    {
    }


    //@ 삭제예정 (플레이어로?)
    //공격을 할때 각도에 따라서 모션을 보여주기 위해 만듬 (즉, 적이 있을때만 사용)

    [HideInInspector]
    public int current_Anim_Frame;
    [HideInInspector]
    public float enemy_angle;
    [HideInInspector]
    public AttackType myAttackType;
    [HideInInspector]
    public AnglePos myAnim_AnglePos;
}

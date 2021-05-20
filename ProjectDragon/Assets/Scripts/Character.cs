//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State { None = 0, Idle, Walk, Attack, Dead, Skill, Hit,Get}
public enum AnglePos { None = 0, Front, Right, Back, Left }
//현재사용안함.
public enum AttackType { ShortRange=0, LongRange=2 }


public class Character : MonoBehaviour
{
    [Header("스테이터스")]
    [SerializeField] public int maxHp;
    [SerializeField] protected int hp;
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected int atk;
    [SerializeField] protected float atkSpeed;
    [SerializeField] protected float atkRange;
    [SerializeField] public float nuckBackPower;


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
    public Vector3 myPos;
    public Vector3 myRotat;
    public float current_angle;

    public Transform other;



    public bool isAttacking;
    public bool isWalk;
    public bool isDead;
    public bool isHit;
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

    #region AtkSpeed
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

    #region Atk
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
    public virtual int HPChanged(int ATK, bool isCritical, float NukBack)
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
    #endregion



    //개체의 상태가 바뀔때마다 실행
    protected virtual void SetState<T>(T newState)
    {
    }

    //Animation Event Function 
    public virtual void Attack_On()
    {

    }


    //@ 삭제예정 (플레이어로?)
    //공격을 할때 각도에 따라서 모션을 보여주기 위해 만듬 (즉, 적이 있을때만 사용)

    [HideInInspector]
    public int current_Anim_Frame;
}

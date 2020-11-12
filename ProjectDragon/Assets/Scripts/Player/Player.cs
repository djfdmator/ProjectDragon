// ==============================================================
// Renewal Player 
//
//  AUTHOR: Yang SeEun
// CREATED: 2020-11-03
// UPDATED: 2020-11-03
// ==============================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : Character
{
    public enum WeaponType { NormalSword, NormalStaff, Excalibur, Nereides, Nyx,  };


    //flash white material damaged of player
    private FlashWhite damaged_flash;
    private IEnumerator damaged_flash_corrutine;

    private Projectile projectile = new Projectile();
    private TargetPoint targetProjectile = new TargetPoint();

    private CameraFollow camera;
    private IEnumerator P_Camera_Shake;

    public float temp_Movespeed;
    // 테스팅용
    public bool AngleisAttack;
    public bool inAttackTarget = false;
    public bool inSkillRange = false;


    public bool isActive;               //코루틴 제어 함수
    public bool isInvaid =false;
    public bool isCriticalHit = false;


    [SerializeField] protected State myState;
    public State CurrentState
    {
        get { return myState; }
        set
        {
            myState = value;

            //Anim
            GetComponent<PlayerAnimControll>().CurrentState = myState;

            if (isActive && (AngleisAttack || isSkillActive))       //angle
            {
                GetComponent<PlayerAnimControll>().ChangeAngleAnim(enemy_angle);
            }
            else
            {
                GetComponent<PlayerAnimControll>().ChangeAngleAnim(current_angle);
            }
        }
    }


    //애니메이터 리소스
    public RuntimeAnimatorController[] projectileAnimator;              
    private RuntimeAnimatorController proj_attackAnimator;              //현재 평타 투사체
    private RuntimeAnimatorController proj_skillAnimator;              //현재 스킬 투사체


    #region 플레이어 세팅

    [Header("<Setting>")]
    [SerializeField] private int mp;
    public CLASS attackType;
    public WeaponType weaponType;
    [SerializeField] private int skillDamage;
    [SerializeField] private float skillRange;
    [SerializeField] private float skillCoolTime;
    [SerializeField] private int skillMpCost;
    [SerializeField] private string skill_ImageName;

    //회피율
    public float invaid;
    //크리티컬
    public float critical;

    #endregion


    [Space(10)]
    //플레이어 정지
    public bool StopPlayer;
    private float StopTime;
    public float StopMaxTime;

    //플레이어 사운드
    public AudioClip walk_Sound;

    //대각 속도
    private float horizontalSpeed = 5.0f;
    private float verticalSpeed = 5.0f;


    [HideInInspector] public Rigidbody2D rigidbody2d;

    //************UI***********
    //JoyStick
    private GameObject uiRoot;
    private SkillButton skillButton;
    private BattleStatus uiStatus;

    private JoyPad joyPad;
    public Vector3 joystickPos;
    //Check JoyStick
    public float h;
    public float v;

    
    //적 찾기
    public List<GameObject> EnemyArray;
    public GameObject TempEnemy;
    private Transform m_EnemyPos;

    //MP 임시 사용
    public override int HP
    {
        get { return hp; }
        set
        {
            if (!isDead)
            {
                if (uiStatus != null)
                {
                    uiStatus.ChangeHpBar(hp, value);
                }
                if (value > 0)
                {
                    hp = value;
                    hp = Mathf.Clamp(value, 0, maxHp);
                }
                else
                {
                    hp = -1;
                    CurrentState = State.Dead;
                    Dead();
                }
                GameManager.Inst.CurrentHp = hp;
            }
        }
    }
    public int MP
    {
        get { return mp; }
        set
        {

            if (uiStatus != null)
            {
                uiStatus.ChangeMpBar(mp, value);
            }
            if (value > 0)
            {
                mp = value;
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("마나가 없습니다.");
#endif
                mp = -1;
            }
            GameManager.Inst.Mp = mp;
        }
    }


    public override int HPChanged(int ATK, bool isCritical, float NukBack)
    {
        int original_HP = HP;
        // GameManager.Inst.CurrentHp = HP;
        float currentATK=ATK;
        if(ATK>0)
        {
            float a = Random.Range(0.0f, 100.0f);
            if (invaid >= a)
            {
                currentATK = ATK - (ATK * 0.1f);
                // Debug.Log("회피성공");
                isInvaid = true;
            }
            else{
                isInvaid = false;
            }
        }
        else
        {
            isInvaid = false;
        }
        
        if(original_HP>=HP)
        {
            damaged_flash_corrutine = damaged_flash.Flash();
            IEnumerator A=transform.GetChild(0).GetComponent<FlashWhite>().Flash();
            IEnumerator B=transform.GetChild(1).GetComponent<FlashWhite>().Flash();
            StopCoroutine(A);
            StopCoroutine(B);
            StopCoroutine(damaged_flash_corrutine);
            StartCoroutine(A);
            StartCoroutine(B);
            StartCoroutine(damaged_flash_corrutine);
            damagePopup.Create(transform.position + new Vector3(0.0f, 0.5f, 0.0f), (int)currentATK, false, isInvaid, transform);
            
            original_HP = HP;
        }
        base.HPChanged((int)currentATK,isCritical,NukBack);
        //Debug.Log((float)HP / (float)maxHp);
        SoundManager.Inst.Ds_EffectPlayerDB(4);
        return HP;
    }

    public override void Dead()
    {
        base.Dead();
        GetComponent<BoxCollider2D>().enabled = false;
        rigidbody2d.velocity = Vector2.zero;
    }


    public void OnStop()
    {
        if (!isDead)
        {
            CurrentState = State.Idle;
            isSkillActive = false;
            StopPlayer = false;
        }
    }

    public IEnumerator CalculateDistanceWithPlayer()
    {
        // 적이 하나라도 있으면
        if (EnemyArray.Count >= 1)
        {
            isActive = true;
            //동작
            while (isActive && !isDead)
            {
                if (EnemyArray.Count > 0)
                {
                    for (int a = 0; a < EnemyArray.Count; a++)
                    {
                        TempEnemy = EnemyArray[0];

                        EnemyArray[a].GetComponent<Monster>().distanceOfPlayer = DistanceCheck(this.GetComponent<Transform>(), EnemyArray[a].GetComponent<Transform>());
                    }
                    for (int a = 0; a < EnemyArray.Count; a++)
                    {
                        if (TempEnemy.GetComponent<Monster>().distanceOfPlayer > EnemyArray[a].GetComponent<Monster>().distanceOfPlayer)
                        {
                            TempEnemy = EnemyArray[a];
                        }
                    }
                    this.enemy_angle = GetAngle(TempEnemy.transform.position, this.transform.position);

                    if (!isSkillActive)
                    {
                        if (DistanceCheck(this.GetComponent<Transform>(), TempEnemy.GetComponent<Transform>()) <= this.GetComponent<Player>().AtkRange && !isSkillActive)
                        {
                            inAttackTarget = true;

                            if (TempEnemy.GetComponent<Character>().HP > 0)
                            {
                                if (attackType == CLASS.지팡이 && joyPad.Pressed == false && !isSkillActive)
                                {
                                    moveSpeed = 0;
                                    AngleisAttack = true;
                                    this.CurrentState = State.Attack;
                                }
                                else if (attackType == CLASS.검 && joyPad.Pressed == true && !isSkillActive)
                                {
                                    moveSpeed = temp_Movespeed;
                                    AngleisAttack = false;
                                }
                                if (attackType == CLASS.검 && !isSkillActive)
                                {
                                    AngleisAttack = true;
                                    this.CurrentState = State.Attack;
                                }
                            }

                        }
                        else
                        {
                            inAttackTarget = false;
                            AngleisAttack = false;
                            if (AngleisAttack == false && !isSkillActive)
                            {
                                if (attackType == CLASS.지팡이)
                                {
                                    moveSpeed = temp_Movespeed;
                                }
                                if (enemy_angle != 0 && joyPad.Pressed == true)
                                {
                                    this.CurrentState = State.Walk;
                                }
                                if (joyPad.Pressed == false && !isSkillActive)
                                {
                                    this.CurrentState = State.Idle;
                                }
                            }
                        }


                        if (DistanceCheck(this.transform, TempEnemy.transform) <= this.skillRange)
                        {
                            inSkillRange = true;
                        }
                        else
                        {
                            inSkillRange = false;
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    isActive = false;
                    break;
                }
            }
        }
        inAttackTarget = false;
        inSkillRange = false;
        AngleisAttack = false;
    }
    public void TempNullSet()
    {
        TempEnemy = null;
        CurrentState = State.Idle;
        AngleisAttack = false;
    }
    

    protected override void Awake()
    {
        base.Awake();

        LoadPlayerPrefData();
        LoadWeaponData();

        //proj_attackAnimator = Resources.Load<RuntimeAnimatorController>(string.Format(""));
        //proj_skillAnimator = Resources.Load<RuntimeAnimatorController>(string.Format(""));

        Initalize_Player_Link();

        temp_Movespeed = moveSpeed;
        critical = 50f;
        invaid = 30f;

        projectileTargetList.Add("Enemy");
        //GameManager.Inst.SavePlayerData();
    }
    protected override void Start()
    {
        skillButton.Init(skillCoolTime, skillMpCost, skill_ImageName);
        uiStatus.Init(HP, maxHp, MP);

        CurrentState = State.Idle;

        StartCoroutine(CalculateDistanceWithPlayer());
    }

    public void OnSkillActive()
    {
        isSkillActive = true;
        StopPlayer = true;
        CurrentState = State.Skill;
        //각 SkillAnim에 이벤트함수로 넣음...
        //CreateProjectile();
  
    }


    /// Animation Event Function (애니메이션 이벤트)
    /// <summary>
    /// Skill Projectile
    /// </summary>
    /// <param name="type"></param>
    public void CreateProjectile(AnimationEvent evt)
    {
        if (evt.animatorClipInfo.weight > 0.5f && GetComponent<Player>() != null)
        {
            //투사체 앵글 변수
            float attackAngle = (EnemyArray.Count == 0) ? current_angle : enemy_angle;

            Vector2 offset = Vector2.zero;
            Vector2 capsuleColSize = Vector2.one;
            float radius = 0.2f;
            if (weaponType == Player.WeaponType.NormalSword)
            {
                projectile.Create(projectileTargetList, offset, radius, attackAngle, 1.5f, skillDamage, projectileAnimator[0], true, transform.position, nuckBackPower);
            }
            else if (weaponType == Player.WeaponType.NormalStaff)
            {
                capsuleColSize = new Vector2(0.6f, 1);
                if (TempEnemy != null)
                {
                    offset = new Vector2(0.02f, 0.5f);
                    capsuleColSize = new Vector2(0.8f, 1.3f);
                    targetProjectile.Create(projectileTargetList, offset, capsuleColSize, skillDamage, projectileAnimator[1], true, TempEnemy.transform.position);
                }
            }
            else if (weaponType == Player.WeaponType.Excalibur)
            {
                if (TempEnemy != null)
                {
                    offset = new Vector2(0.03f, 0.0f);
                    capsuleColSize = new Vector2(0.6f, 1);
                    targetProjectile.Create(projectileTargetList, offset, capsuleColSize, skillDamage, projectileAnimator[7], true, TempEnemy.transform.position);
                }
            }
            else if (weaponType == Player.WeaponType.Nereides)
            {
                projectile.Create(projectileTargetList, offset, radius, attackAngle, 1.5f, skillDamage, projectileAnimator[5], true, transform.position, nuckBackPower, true);

            }
            else if (weaponType == Player.WeaponType.Nyx)
            {
                if (TempEnemy != null)
                {
                    offset = new Vector2(0.03f, 0.5f);
                    capsuleColSize = new Vector2(0.8f, 1.3f);
                    targetProjectile.Create(projectileTargetList, offset, capsuleColSize, skillDamage, projectileAnimator[3], true, TempEnemy.transform.position);
                }
            }
        }
    }
    private void FixedUpdate()
    {
        if (!isDead)
        {
            if(!isSkillActive)
            {
                current_angle = joyPad.angle;
            }

            if (joyPad.Pressed)
            {
                joystickPos = joyPad.position;
                //joystick
                h = joystickPos.x;
                v = joystickPos.y;
            }

            //Make Right direction by Set Animatoion bool setting
            if (!StopPlayer.Equals(true))
            {
                rigidbody2d.velocity = new Vector2(10.0f * Time.deltaTime * h * horizontalSpeed * moveSpeed, 10.0f * Time.deltaTime * v * verticalSpeed * moveSpeed);
                ////transform.Translate(Vector2.right * Time.deltaTime * h * horizontalSpeed * moveSpeed, Space.World);
                //transform.Translate(Vector2.up * Time.deltaTime * v * verticalSpeed * moveSpeed, Space.World);
            }
            if (StopPlayer.Equals(true))
            {
                rigidbody2d.velocity = Vector2.zero;
                StopTime += Time.deltaTime;
                if (StopTime >= StopMaxTime)
                {
                    //   StopPlayer = false;
                    StopTime = 0;
                }
            }
        }
    }

    public static float GetAngle(Vector3 Start, Vector3 End)
    {
        Vector3 v = End - Start;
        return Quaternion.FromToRotation(Vector3.up, End - Start).eulerAngles.z;
    }
    public float DistanceCheck(Transform Player, Transform Enemy)
    {
        Vector3 offset = Player.position - Enemy.position;
        float sqrLen = offset.magnitude;
        return sqrLen;
    }

    #region Animation Changer
    public AnglePos Current_AngleCaseString(float angle)
    {
        if (angle == 0)
        {
            return AnglePos.Front;
        }
        if (angle < 45)
        {
            return AnglePos.Front;
        }
        else if (angle < 135)
        {
            return AnglePos.Right;
        }
        else if (angle < 225)
        {
            return AnglePos.Back;
        }
        else if (angle < 315)
        {
            return AnglePos.Left;
        }
        return AnglePos.Front;
    }
    public AnglePos Enemy_AngleCaseString(float angle)
    {
        if (angle == 0)
        {
            return AnglePos.Front;
        }
        if (angle >= 0 && angle < 45)
        {
            return AnglePos.Front;
        }
        else if (angle >= 45 && angle < 135)
        {
            return AnglePos.Right;
        }
        else if (angle >= 135 && angle < 225)
        {
            return AnglePos.Back;
        }
        else if (angle >= 225 && angle < 315)
        {
            return AnglePos.Left;
        }
        return AnglePos.Front;
    }
    //콜리젼에 따른 플레이어 밀림 방지
    protected void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {

                rigidbody2d.bodyType = RigidbodyType2D.Kinematic;
                rigidbody2d.velocity = Vector2.zero;
            }
        }
    }
    // 콜리젼이 해제 됐을 때의 플레이어 밀림 방지
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                rigidbody2d.bodyType = RigidbodyType2D.Dynamic;

            }
        }
    }

    
    public void CameraShake()
    {
        P_Camera_Shake = camera.Shake(1, 1.0f);
        StartCoroutine(P_Camera_Shake);
    }
    #endregion

    //무기 정보 세팅
    public void ChangeWeapon()
    {
        LoadWeaponData();

        GetComponent<PlayerAnimControll>().LoadAnimator(weaponType,attackType,ATTACKSPEED);
        skillButton.Init(skillCoolTime, skillMpCost, skill_ImageName);
    }

    //혹시몰라서 만들어둠.
    //장비 정보 세팅
    public void ChanageArmorData()
    {
        maxHp = GameManager.Inst.MaxHp;
        HP = GameManager.Inst.CurrentHp;

        uiStatus.Init(HP, maxHp, MP);
    }

    public void SavePlayerPrefData()
    {
        GameManager.Inst.CurrentHp = HP;
        GameManager.Inst.Mp = MP;
    }



    //던전시작할때 세팅
    private void LoadPlayerPrefData()
    {
        maxHp = GameManager.Inst.MaxHp;
        HP = GameManager.Inst.CurrentHp;
        MP = GameManager.Inst.Mp;
        MoveSpeed = GameManager.Inst.MoveSpeed + 2.0f;          //3
        ATTACKDAMAGE = GameManager.Inst.Atk_Min;                //3
        ATTACKSPEED = GameManager.Inst.AttackSpeed;            //1
        nuckBackPower = GameManager.Inst.NuckBack_Power;
        AtkRange = GameManager.Inst.AttackRange;

        //개발자 모드!
        maxHp = 10000;
        HP = 10000;

    }
    private void LoadWeaponData()
    {
        weaponType = (WeaponType)GameManager.Inst.CurrentEquipWeapon.num;
        attackType = GameManager.Inst.CurrentEquipWeapon.Class;

        skillDamage = GameManager.Inst.CurrentSkill.atk;
        skillRange = GameManager.Inst.CurrentSkill.skill_Range;
        skillCoolTime = GameManager.Inst.CurrentSkill.coolTime;
        skillMpCost = GameManager.Inst.CurrentSkill.mpCost;
        skill_ImageName = GameManager.Inst.CurrentSkill.imageName;
    }

    private void Initalize_Player_Link()
    {
        base.Start();
        damaged_flash = GetComponent<FlashWhite>();
        rigidbody2d = GetComponent<Rigidbody2D>();

        uiRoot = GameObject.Find("UI Root").gameObject;
        joyPad = uiRoot.transform.Find("JoyPad").GetComponent<JoyPad>();
        skillButton = uiRoot.transform.Find("SkillButton").GetComponent<SkillButton>();
        uiStatus = uiRoot.transform.Find("Status").GetComponent<BattleStatus>();

        camera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
    }
    

    
}

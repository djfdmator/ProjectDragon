//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum IsWear { None, DefaultCloth, AnimalCloth, Suit, DefultName, DefaltName2 }
public class Player : Character
{
    //평타 및 스킬
    
    public RuntimeAnimatorController[] projectileAnimator;

    [SerializeField]
    protected State myState;

    public PlayerSkill playerSkill;

    //flash white material damaged of player
    public FlashWhite damaged_flash;
    public IEnumerator damaged_flash_corrutine;

    // HP GAUGE
    public HPGauge HPBar;
    public MPGauge MPBar;

    public State CurrentState
    {
        get { return myState; }
        set
        {
            myState = value;
            SetState(myState);
        }
    }

    public CameraFollow Player_camera;
    public IEnumerator P_Camera_Shake;

    //템프 앵글
    public float temp_angle;
    public float temp_Movespeed;

    //회피율
    public float invaid;

    public float critical;

    //코루틴 제어 함수
    public bool isActive;
    // 플레이어 캐릭터 스테이터스
    // public bool isSkillActive;

    // 테스팅용
    public bool AngleisAttack;


    //  public UILabel CheckAngleLabel;

    //플레이어 세팅
    public SEX sex;
    public IsWear isWear;
    public AttackType attackType;


    public GameObject weaponSelection;
    private Animator weaponAnimator;

    //플레이어 정지

    public bool StopPlayer;
    public float StopTime;
    public float StopMaxTime;

    //플레이어 사운드

    public AudioClip walk_Sound;

    //대각 속도
    public float horizontalSpeed = 5.0f;
    public float verticalSpeed = 5.0f;
    public GameObject DeadPanel;
    public SEX playerSex;

    //플레이어 애니메이션 컨트롤
    public Animator playerAnimationStateChanger;
    // player controll vector

    public Rigidbody2D rigidbody2d;

    //JoyStick
    protected JoyPad joyPad;
    public GameObject joypadinput;
    public Vector3 joystickPos;
    private Transform m_EnemyPos;

    //Check JoyStick
    public float h;
    public float v;

    public bool isInvaid =false;
    public bool isCriticalHit = false;
    public Transform EnemyPos { get { return m_EnemyPos; } set { m_EnemyPos = value; } }

    //죽었을때 패널
    public GameObject EndPanel;

    //적 찾기
    public RoomManager EnemyRoom;
    public GameObject[] Enemy;
    public List<GameObject> EnemyArray;
    public GameObject TempEnemy;

    //MP 임시사용용 변수
    public int mp= 100;
    public int maxMp = 100;



    public override int HPChanged(int ATK, bool isCritical, int NukBack)
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
        Debug.Log((int)currentATK+"내 체력은 :"+HP);
        //hpBar.fillAmount = (float)HP-currentATK / (float)maxHp;
        if(original_HP>=HP)
        {
            // Debug.Log("Damaged!!!!! and Flash"+"HP : "+ HP+ "original_HP" + original_HP);
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
            Debug.Log("Damage:   " + currentATK);
            original_HP = HP;
        }
        base.HPChanged((int)currentATK,isCritical,NukBack);
        HPBar.Player_HP_Changed(HP,maxHp);
        Debug.Log((float)HP / (float)maxHp);
        SoundManager.Inst.Ds_EffectPlayerDB(4);
        return HP;
    }
    //MP 임시 사용
    public override int HP
    {
        get { return (int)GameManager.Inst.CurrentHp; }
        set
        {
            if (value > 0)
            {
                GameManager.Inst.CurrentHp = value;
                hp = (int)GameManager.Inst.CurrentHp;
                GameManager.Inst.CurrentHp = Mathf.Clamp(value, 0, maxHp);
            }
            else if (!isDead)
            {
                hp = -1;
                GameManager.Inst.CurrentHp = maxHp;
                isDead = true;
                CurrentState = State.Dead;
                Debug.Log("죽었습니다.");
                Time.timeScale = 0f;

            }
        }
    }
    public int MP
    {
        get { return mp; }
        set
        {
            if (value > 0)
            {
                mp = value;
                //mp = Mathf.Clamp(value, 0, maxMp);
            }
            else
            {
                Debug.Log("마나가 없습니다.");
                mp = -1;
            }
        }
    }
    public override void Dead()
    {
        base.Dead();

    }

    public virtual int MPChanged(int Cost)
    {
        MP = MP - Cost;
        return MP;
    }
    public IEnumerator CalculateDistanceWithPlayer()
    {
        // 적이 하나라도 있으면
        if (EnemyArray.Count >= 1)
        {
            isActive = true;
            //동작
            while (isActive)
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
                         
                    if (DistanceCheck(this.GetComponent<Transform>(), TempEnemy.GetComponent<Transform>()) <= this.GetComponent<Player>().AtkRange&&!isSkillActive)
                    {
                        if (TempEnemy.GetComponent<Character>().HP > 0)
                        {
                            if (attackType == AttackType.LongRange && joyPad.Pressed == false&&!isSkillActive)
                            {
                                moveSpeed = 0;
                                AngleisAttack = true;
                                this.CurrentState = State.Attack;
                            }
                            else if(attackType == AttackType.LongRange && joyPad.Pressed == true&&!isSkillActive)
                            {

                                 moveSpeed = temp_Movespeed;
                                 AngleisAttack = false;
                            }
                            if (attackType == AttackType.ShortRange&&!isSkillActive)
                            {
                                AngleisAttack = true;
                                this.CurrentState = State.Attack;
                            }
                        }
                    }
                    if (DistanceCheck(this.GetComponent<Transform>(), TempEnemy.GetComponent<Transform>()) > this.GetComponent<Player>().AtkRange&&!isSkillActive)
                    {
                        AngleisAttack = false;
                        if (AngleisAttack == false&&!isSkillActive)
                        {
                            if (attackType == AttackType.LongRange)
                            {
                                moveSpeed = temp_Movespeed;
                            }
                            if (enemy_angle != 0 && joyPad.Pressed == true)
                            {
                                this.CurrentState = State.Walk;
                            }
                            if (joyPad.Pressed == false&&!isSkillActive)
                            {
                                this.CurrentState = State.Idel;
                            }
                        }
                    }
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    break;
                }
            }
        }
    }
    public void TempNullSet()
    {
        TempEnemy = null;
        CurrentState = State.Idel;
        AngleisAttack = false;
    }
    void PlayerPrefData(ref int Damage1)
    {
        ATTACKDAMAGE = Damage1;
        maxHp = (int)GameManager.Inst.MaxHp;
        //damage = (int)Damage1;
        //hp = ref (int)GameManager.Inst.CurrentHp;
    }
    void PlayerPrefDataTrascation()
    {
        //hp = ref (int)GameManager.Inst.CurrentHp;
    }
    // Start is called before the first frame update
    protected override void Awake()
    {
        base.Awake();
        //근거리일때
        attackType = AttackType.ShortRange;
        //원거리일때
        //attackType = AttackType.LongRange;
        //TODO: 뒤에 로비 완성되면 무기 합칠것, 스테이터스를 DB에서 받아오기
        EndPanel.SetActive(false);
        isWear = IsWear.DefaultCloth;
        playerSex = SEX.Female;
        initializePlayerConverter();
        MoveSpeed = 3.0f;
        ATKChanger(3);
        ATKSpeedChanger(1.0f);
        CurrentState = State.Idel;
        AtkRangeChanger(3.5f);
        mp= 300;

        projectileTargetList.Add("Enemy");
     //  Database.Inst.playData.hp = 100.0f;
     //   GameManager.Inst.SavePlayerData();
    }
    protected override void Start()
    {
        StartCoroutine(CalculateDistanceWithPlayer());
        //내가 끼고 있는 칼에 대한 정의
        //Database.Inventory myWeapon = Database.Inst.playData.inventory[Database.Inst.playData.equiWeapon_InventoryNum];
        joypadinput = GameObject.Find("UI Root/GameObject");
        //CheckAngleLabel = GameObject.Find("UI Root/CurrentAngle").GetComponent<UILabel>();
        initalize_Player_Link();
        temp_Movespeed = moveSpeed;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
          if (isSkillActive)
        {
            float A = current_angle;
            current_angle = A;
            CurrentState = State.Skill;
        }
        else{
             current_angle = joyPad.angle;
        }
        //CheckAngleLabel.text = current_angle.ToString();
        myPos = gameObject.transform.position;
        joystickPos = joypadinput.GetComponent<JoyPad>().position;
        //joystick
        h = joystickPos.x;
        v = joystickPos.y;

        //Make Right direction by Set Animatoion bool setting
        if (!StopPlayer.Equals(true))
        {
            rigidbody2d.velocity = new Vector2(10.0f * Time.deltaTime * h * horizontalSpeed * moveSpeed, 10.0f * Time.deltaTime * v * verticalSpeed * moveSpeed);
            ////transform.Translate(Vector2.right * Time.deltaTime * h * horizontalSpeed * moveSpeed, Space.World);

            //transform.Translate(Vector2.up * Time.deltaTime * v * verticalSpeed * moveSpeed, Space.World);
        }
        if (StopPlayer.Equals(true))
        {
            rigidbody2d.velocity = new Vector2(0f,0f);
            StopTime += Time.deltaTime;
            if (StopTime >= StopMaxTime)
            {
             //   StopPlayer = false;
                StopTime = 0;
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
        float sqrLen = offset.sqrMagnitude;
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
        P_Camera_Shake = Player_camera.Shake(1, 1.0f);
        StartCoroutine(P_Camera_Shake);
    }
#endregion

    public void initializePlayerConverter()
    {
        PlayerPrefData(ref Database.Inst.playData.atk_Min);
        HP= (int)GameManager.Inst.CurrentHp;
        critical = 50f;
        invaid = 30f;
    }

    public void initalize_Player_Link()
    {
        damaged_flash = gameObject.GetComponent<FlashWhite>();
        playerSkill = GameObject.Find("UI Root/Skillbutton").GetComponent<PlayerSkill>();
        joyPad = FindObjectOfType<JoyPad>();
        rigidbody2d = GetComponent<Rigidbody2D>();
        playerAnimationStateChanger = GetComponent<Animator>();
        Player_camera = GameObject.Find("Main Camera").GetComponent<CameraFollow>();
        HPBar = GameObject.Find("UI Root/HPBar").GetComponent<HPGauge>();
        base.Start();
    }
}
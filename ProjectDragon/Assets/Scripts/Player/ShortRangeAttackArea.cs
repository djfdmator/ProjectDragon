//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeAttackArea : MonoBehaviour
{
    //public BattleManager battlemanager;
    public Player player;
    public float angle;
    [SerializeField] private bool m_bDebugMode = false;

    [Header("View Config")]
    [Range(0f, 360f)]
    [SerializeField] private float m_horizontalViewAngle = 120f; // 시야각
    [SerializeField] private float m_viewRadius = 1f; // 시야 범위
    [Range(-360f, 360f)]
    [SerializeField] private float m_viewRotateZ = 0f; // 시야각의 회전값

    [SerializeField] private LayerMask m_viewTargetMask;       // 인식 가능한 타켓의 마스크
    [SerializeField] private LayerMask m_viewObstacleMask;     // 인식 방해물의 마스크 

    private List<Collider2D> hitedTargetContainer = new List<Collider2D>(); // 인식한 물체들을 보관할 컨테이너

    private float m_horizontalViewHalfAngle = 0f; // 시야각의 절반 값

    private Projectile projectile;


    [Header("LongRangeAttack")]
    public GameObject attack_Pref;
    public Vector3 pref_Rot;

    private void Awake()
    {
        projectile = new Projectile();
        m_horizontalViewHalfAngle = m_horizontalViewAngle * 0.5f;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_viewObstacleMask = LayerMask.GetMask("Wall");
        //battlemanager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
    }
    private void Start()
    {
        //m_viewRadius = player.AtkRange - 2f;
        m_viewRadius = player.AtkRange;
        attack_Pref = Resources.Load<GameObject> ("test");
    }
    public void Update()
    {
        //if(player.attackType==AttackType.ShortRange)
        //{
        if (player.AngleisAttack == true)
        {
            angle = player.enemy_angle;
            m_viewRotateZ = player.enemy_angle;
        }
        else
        {
            player.MoveSpeed = player.temp_Movespeed;
            angle = player.current_angle;
            m_viewRotateZ = player.current_angle;
        }
        //}
    }
    //public void Update()
    //{

    //    if (player.AngleisAttack.Equals(false))
    //    {
    //        player.MoveSpeed = player.temp_Movespeed;
    //    }
    //    angle = player.current_angle;
    //    m_viewRotateZ = player.current_angle;

    //}

    #region 이전버전 AttackOn
    //public void AttackOn()
    //{
    //   FindViewTargets();
    //}
    //public void LongAttackOn()
    //{
    //    RongAttack_normal();
    //}
    #endregion
    /// <summary>
    /// Animation Event Function
    /// </summary>
    public void Attack_On()
    {
        if (player.attackType == CLASS.검)
        {
            FindViewTargets();
        }
        else if (player.attackType == CLASS.지팡이)
        {
            RongAttack_normal();
        }

    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (m_bDebugMode)
        {
            //m_viewRadius = player.AtkRange - 2f;
            m_viewRadius = player.AtkRange;

            m_horizontalViewHalfAngle = m_horizontalViewAngle * 0.5f;

            Vector3 originPos = transform.position;

            Gizmos.DrawWireSphere(originPos, m_viewRadius);

            Vector3 horizontalRightDir = AngleToDirZ(m_horizontalViewHalfAngle + m_viewRotateZ);
            Vector3 horizontalLeftDir = AngleToDirZ(-m_horizontalViewHalfAngle + m_viewRotateZ);
            Vector3 lookDir = AngleToDirZ(m_viewRotateZ);
            Debug.DrawRay(originPos, horizontalLeftDir * m_viewRadius, Color.cyan);
            Debug.DrawRay(originPos, lookDir * m_viewRadius, Color.green);
            Debug.DrawRay(originPos, horizontalRightDir * m_viewRadius, Color.cyan);

           // FindViewTargets();
        }
    }
#endif
    public int Take_Current_Damage()
    {
        int critical_Per = (int)player.critical;
        float a = Random.Range(0.0f, 100.0f);
        int Damage = player.ATTACKDAMAGE;
        if (critical_Per >= a)
        {
            player.isCriticalHit =true;
            Damage += (int)(Damage * 0.5f);
        }
        else
        {
            player.isCriticalHit =false;
        }
            return Damage;
    }

    public Collider2D[] FindViewTargets()
    {
        hitedTargetContainer.Clear();

        Vector2 originPos = transform.position;
        Collider2D[] hitedTargets = Physics2D.OverlapCircleAll(originPos, m_viewRadius, m_viewTargetMask);

        foreach (Collider2D hitedTarget in hitedTargets)
        {
            Vector2 targetPos = hitedTarget.transform.position;
            Vector2 dir = (targetPos - originPos).normalized;
            Vector2 lookDir = AngleToDirZ(m_viewRotateZ);

            // float angle = Vector3.Angle(lookDir, dir)
            // 아래 두 줄은 위의 코드와 동일하게 동작함. 내부 구현도 동일
            float dot = Vector2.Dot(lookDir, dir);
            float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;
            //   float angle = m_viewRotateZ * Mathf.Rad2Deg;

            if (angle <= m_horizontalViewHalfAngle)
            {
                RaycastHit2D rayHitedTarget = Physics2D.Raycast(originPos, dir, m_viewRadius, m_viewObstacleMask);
                if (rayHitedTarget)
                {
                    //Debug.Log(rayHitedTarget.transform.gameObject.name);
#if UNITY_EDITOR
                    if (m_bDebugMode)
                        Debug.DrawLine(originPos, rayHitedTarget.point, Color.yellow);
#endif
                }
                else
                {
                    hitedTargetContainer.Add(hitedTarget);
#if UNITY_EDITOR
                    if (m_bDebugMode)
                        Debug.DrawLine(originPos, targetPos, Color.red);
#endif
                    if (hitedTarget.CompareTag("Enemy") || hitedTarget.isActiveAndEnabled == true)
                    {
                        if (player.isAttacking)
                        //Player hit
                        {
                            SoundManager.Inst.Ds_EffectPlayerDB(8);
                            hitedTarget.GetComponent<Character>().HPChanged(Take_Current_Damage(),player.isCriticalHit,player.nuckBackPower);
                            player.isAttacking = false;
                        }
                        else
                        {
                            player.isAttacking = true;
                        }
                  //      hitedTarget.GetComponent<Character>().HPChanged(Take_Current_Damage());
                       // 임시 버젼
                     //   hitedTarget.GetComponent<SpriteRenderer>().color = Color.red;
                    }
                }
            }
        }
        if (hitedTargetContainer.Count > 0)
            return hitedTargetContainer.ToArray();
        else
            return null;
    }
    //0 ~360의 값을 Up Vector 기준 Local Direction으로 변환시켜줌.
    private Vector2 AngleToDirZ(float angleInDegree)
    {
        float radian = (transform.eulerAngles.z - angleInDegree + 180) * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), Mathf.Cos(radian));
    }

    /// <summary>
    /// 원거리 공격
    /// </summary>
    public void RongAttack_normal()
    {
        float _attackangle = (player.EnemyArray.Count != 0) ? player.enemy_angle : player.current_angle;

        Vector2 offset = new Vector2(0.0f, 0.0f);
        float radius = 0.15f;

        //수정 예정  projectileTargetList를 데이터베이스에서 이넘으로 받아서 실행할것
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///
        SoundManager.Inst.Ds_EffectPlayerDB(7);

        if (player.weaponType == Player.WeaponType.NormalStaff)
        {
            projectile.Create(player.projectileTargetList, offset, radius, _attackangle, 5.0f, Take_Current_Damage(), player.projectileAnimator[2], false, player.transform.position, player.nuckBackPower);
        }
        else if (player.weaponType == Player.WeaponType.Nyx)
        {
            projectile.Create(player.projectileTargetList, offset, radius, _attackangle, 5.0f, Take_Current_Damage(), player.projectileAnimator[4], false, player.transform.position, player.nuckBackPower);
        }
        else if (player.weaponType == Player.WeaponType.Nereides)
        {
            projectile.Create(player.projectileTargetList, offset, radius, _attackangle, 5.0f, Take_Current_Damage(), player.projectileAnimator[6], false, player.transform.position, player.nuckBackPower);
        }
    }
    public void AttackCoolDown()
    {
        if (!player.isDead)
        {
            player.CurrentState = State.Idle;
        }
    }
    IEnumerator AttackDamaged()
    {
        yield return new WaitForFixedUpdate();
    }
}

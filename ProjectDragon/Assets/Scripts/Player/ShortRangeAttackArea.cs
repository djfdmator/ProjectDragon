//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShortRangeAttackArea : MonoBehaviour
{
    //public BattleManager battlemanager;
    public Player My_Angle;
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
        My_Angle = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        m_viewObstacleMask = LayerMask.GetMask("Wall");
        //battlemanager = GameObject.Find("BattleManager").GetComponent<BattleManager>();
    }
    private void Start()
    {
        m_viewRadius = My_Angle.AtkRange - 2f;
        attack_Pref = Resources.Load<GameObject> ("test");
    }
    public void Update()
    {
        //if(My_Angle.attackType==AttackType.ShortRange)
        {
            if (My_Angle.AngleisAttack == true)
            {
                angle = My_Angle.enemy_angle;
                m_viewRotateZ = My_Angle.enemy_angle;
            }
            else
            {
                My_Angle.MoveSpeed = My_Angle.temp_Movespeed;   
                angle = My_Angle.current_angle;
                m_viewRotateZ = My_Angle.current_angle;
            }
        }
    }
    public void AttackOn()
    {
            FindViewTargets();
    }
    public void LongAttackOn()
    {
        RongAttack_normal();
    }
    private void OnDrawGizmos()
    {
        if (m_bDebugMode)
        {
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
    public int Take_Current_Damage()
    {
        int critical_Per = (int)My_Angle.critical;
        float a = Random.Range(0.0f, 100.0f);
        int Damage;
        Damage = My_Angle.ATTACKDAMAGE;
        if (critical_Per >= a)
        {
            My_Angle.isCriticalHit =true;
            Damage += (int)(Damage * 0.5f);
            return Damage;
        }
        else
        {
            My_Angle.isCriticalHit =false;
            My_Angle.ATTACKDAMAGE = My_Angle.ATTACKDAMAGE;
            return My_Angle.ATTACKDAMAGE;
        }
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
                    Debug.Log(rayHitedTarget.transform.gameObject.name);
                    if (m_bDebugMode)
                        Debug.DrawLine(originPos, rayHitedTarget.point, Color.yellow);
                }
                else
                {
                    hitedTargetContainer.Add(hitedTarget);
                    if (m_bDebugMode)
                        Debug.DrawLine(originPos, targetPos, Color.red);
                    if (hitedTarget.CompareTag("Enemy") || hitedTarget.isActiveAndEnabled == true)
                    {
                        if (My_Angle.isAttack)
                        //Player hit
                        {
                            hitedTarget.GetComponent<Character>().HPChanged(Take_Current_Damage(),My_Angle.isCriticalHit,0);
                            My_Angle.isAttack = false;
                        }
                        else
                        {
                            My_Angle.isAttack = true;
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

    public void RongAttack_normal()
    {
        float _swordAttackangle = My_Angle.enemy_angle;
            if(My_Angle.EnemyArray.Count ==0)
            {
               _swordAttackangle =My_Angle.current_angle;
            }
        Vector2 offset = new Vector2(0.0f, 0.0f);
        float radius = 0.5f;

        //수정 예정  projectileTargetList를 데이터베이스에서 이넘으로 받아서 실행할것
/////////////////////////////////////////////////////////////////////////////////////////////////////////////
        projectile.Create(My_Angle.projectileTargetList, offset, radius, _swordAttackangle, 3.0f, 10,  My_Angle.projectileAnimator[2], "ProjectileObj", false, My_Angle.transform.position);
    }
    public void AttackCoolDown()
    {
        My_Angle.CurrentState = State.Idel;
    }
    IEnumerator AttackDamaged()
    {
        yield return new WaitForFixedUpdate();
    }
}

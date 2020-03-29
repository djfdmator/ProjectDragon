//////////////////////////////////////////////////////////MADE BY Koo KyoSeok///2019-12-16/////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public string poolItemName = "ProjectileObj";

    public bool inited, isplayskill;
    public int damage;
    public float m_angle
    {
        get
        {
            return angle;
        }
        set
        {
            angle = value;
        }
    }
    public float angle, speed, lifetime, generationtime, targetpointrangex, targetpointrangey;

    public List<string> tagsString;
    Rigidbody2D rb2d;
    Animator anim;
    Projectile projectile;

    void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
    }

    public IEnumerator Reset;

    private void FixedUpdate()
    {
        if (inited)
        {
            if (generationtime < 3)
            {
                generationtime += Time.deltaTime;
            }
            else
            {
                StartCoroutine(Reset);
            }
        }
    }
    /// <summary>
    /// 프로젝타일 생성
    /// </summary>
    /// <param name="_angle">생성시 각도 아래를 0도로 두며 반시계방향으로 계산</param>
    /// <param name="_speed">해당 발사체의 속도</param>
    /// <param name="_damage">해당 발사체의 공격력</param>
    /// <param name="_projectileAnimator">발사체의 상속된 애니매이터</param>
    /// <param name="poolItemName"></param>
    /// <param name="_isplayskill">발사체의 스킬에의한 파괴여부</param>
    /// <param name="position">발사체 생성위치</param>
    /// <param name="parent">발사체의 부모</param>
    /// <returns></returns>
    /// /*
    /*
    public Projectile Create(float _angle, float _speed, int _damage, RuntimeAnimatorController _projectileAnimator, string poolItemName, bool _isplayskill, Vector3 position, Transform parent = null)
    {

        GameObject projectileObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        projectile = projectileObject.transform.GetComponent<Projectile>();
        projectile.gameObject.SetActive(true);
        projectile.ProjectileInit(_angle, _speed, _damage, _projectileAnimator, _isplayskill, position);
        return projectile;

        //ObjectPool.Instance.PushToPool("ProjectileObj", projectileObject);

    }
*/

    public Projectile Create(List<string> _tagsString ,Vector2 _offset,float _colRadius,float _angle, float _speed, int _damage, RuntimeAnimatorController _projectileAnimator, string poolItemName, bool _isplayskill, Vector3 position, Transform parent = null)
    {
        GameObject projectileObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        projectile = projectileObject.transform.GetComponent<Projectile>();
        projectile.gameObject.SetActive(true);
        projectile.GetComponent<CircleCollider2D>().offset = _offset;
        projectile.GetComponent<CircleCollider2D>().radius = _colRadius;

        projectile.tagsString = _tagsString;
        projectile.ProjectileInit(_angle, _speed, _damage, _projectileAnimator, _isplayskill, position);
        return projectile;

    }

    /// <summary>
    /// 투사체 초기화
    /// </summary>
    /// <param name="_angle"> 투사체 발사각도 위를 기준 0도</param>
    /// <param name="_speed">투사체의 이동 속도</param>
    /// <param name="_damage">투사체의 데미지</param>
    /// <param name="_projectilename">투사체의 이름</param>
    /// <param name="_isplayskill">쏘는이 판별여부(if플레이어=true)</param>
    /// <param name="position">쏘아지는 위치</param>
    public void ProjectileInit(float _angle, float _speed, int _damage, RuntimeAnimatorController _projectileAnimator, bool _isplayskill, Vector3 position)
    {
        Reset = ResetProjectile();
        inited = true;
        gameObject.transform.position = position;
        isplayskill = _isplayskill;
        m_angle = _angle;
        speed = _speed;
        damage = _damage;
        if (anim == null)
        {
            anim = gameObject.GetComponent<Animator>();
        }
        if (rb2d == null)
        {
            rb2d = gameObject.GetComponent<Rigidbody2D>();
        }
        anim.runtimeAnimatorController = _projectileAnimator;
        anim.Play("ProjecTileTest");
        anim.SetBool("Destory", false);
        gameObject.GetComponent<CircleCollider2D>().enabled = true;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, m_angle));

        rb2d = gameObject.GetComponent<Rigidbody2D>();
        rb2d.velocity = new Vector2(Mathf.Cos((m_angle - 90) / 360 * 2 * Mathf.PI) * speed, Mathf.Sin((m_angle - 90) / 360 * 2 * Mathf.PI) * speed);

    }
    /*
    /// <summary>
    /// 발사체가 벽에 부딛혔을때 동작할것
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag.Equals("Wall"))
        {
            StartCoroutine(Reset);
        }
    }
    */
    /// <summary>
    /// 발사체가 플레이어 에게 부딛혔을때 동작할 것
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {

        foreach (string s in tagsString)
        {
            if (collision.gameObject.CompareTag(s))
            {
                collision.GetComponent<Character>().HPChanged(damage, false, 0);
                if (Reset != null)
                {
                    StartCoroutine(Reset);
                    Reset = null;

                }
            }
        }
       
        if (collision.tag.Equals("Wall") && Reset!=null)
        {
            StartCoroutine(Reset);
            Reset = null;
        }
        

    }
    //private void OnBecameInvisible()
    //{
    //    if (gameObject.GetComponent<CircleCollider2D>().enabled)
    //    {
    //        if (Reset != null)
    //        {
    //            StartCoroutine(Reset);
    //            Reset = null;
    //        }

    //    }
    //}
    /// <summary>
    /// 발사체의 리셋
    /// </summary>
    /// <returns></returns>
    IEnumerator ResetProjectile()
    {
        generationtime = 0;
        float cliptime = 0;
        anim.SetBool("Destory", true);
        inited = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        while (!anim.GetCurrentAnimatorStateInfo(0).IsName("ProjectileDestroy"))
        {
            yield return null;
        }
        cliptime = anim.GetCurrentAnimatorStateInfo(0).length;
        rb2d.velocity = Vector3.zero;
        //Debug.Log(cliptime);
        yield return new WaitForSecondsRealtime(cliptime);
        //Push ObjectPoolList
        ObjectPool.Instance.PushToPool(poolItemName, gameObject, transform);
        yield return null;
    }
}
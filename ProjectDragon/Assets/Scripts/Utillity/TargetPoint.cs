// ==============================================================
// Cracked ThornPoint
//
//  AUTHOR: Yang SeEun
// CREATED: 2020-01-08
// UPDATED: 2020-01-08
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetPoint : MonoBehaviour
{
    public string poolItemName = "TargetPoint";
    public float projecTileReady =1, projecTileStart=1, projecTileEnd=1;
    private TargetPoint targetPoint;
    private HitEffect hitEffect = new HitEffect();
    private int attackDamage = 0;
    [SerializeField]  private Transform parentPool;

    [SerializeField] private GameObject targetObject;
    //[SerializeField] private List<GameObject> targetObject = new List<GameObject>();
    public List<string> tagsString = new List<string>();
    private bool isplayskill;
    private string attackType;

    private void Awake()
    {
        parentPool = GameObject.FindGameObjectWithTag("ObjectPool").transform;
    }


    private void Start()
    {
        GetComponent<Animator>().SetFloat("ReadyTime", projecTileReady);
        GetComponent<Animator>().SetFloat("StartTime", projecTileStart);
        GetComponent<Animator>().SetFloat("EndTime", projecTileEnd);
    }
    /// <summary>
    /// 애니메이션 이벤트 함수 (데미지 주는 프레임에)
    /// </summary>
    public void AttackOn()
    {
        //foreach (GameObject target in targetObject)
        //{
            if ((targetObject != null))
            {
                if (isplayskill)
                {
                    hitEffect.Create(targetObject.transform.position, attackType);
                }
             targetObject.GetComponent<Character>().HPChanged(attackDamage, false, 0);

                //if (targetObject.GetComponent<Character>().isDead) targetObject.Remove(targetObject);
            }
        //}
    }
    /// <summary>
    /// 애니메이션 이벤트 함수 넣기 (EndAnimation 마지막 프레임에)
    /// </summary>
    public void ResetProjectile()
    {
        ObjectPool.Instance.PushToPool(poolItemName, gameObject);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(string s in tagsString)
        {
            if (collision.gameObject.CompareTag(s))
            {
                //targetObject.Add(collision.gameObject);
                //Debug.Log("object " + gameObject.name);
                //Debug.Log("Collision is " + collision.gameObject.name);
                targetObject = collision.gameObject;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
         foreach(string s in tagsString)
        {
            if (collision.gameObject.CompareTag(s))
            {
                //targetObject.Clear();
                targetObject = null;
            }
        }
    }



    public TargetPoint Create(List<string> tagsStringList, Vector2 colOffset, float colRadius, int _damage, RuntimeAnimatorController _Animator, bool _isplayskill, Vector3 position, Transform parent = null)
    {
        Transform _parent = parent != null ? parent : parentPool;
        GameObject projectileObject = ObjectPool.Instance.PopFromPool(poolItemName, _parent);
        targetPoint = projectileObject.transform.GetComponent<TargetPoint>();
        targetPoint.attackDamage = _damage;
        targetPoint.tagsString = tagsStringList;
        targetPoint.GetComponent<CircleCollider2D>().offset = colOffset;
        targetPoint.GetComponent<CircleCollider2D>().radius = colRadius;
        targetPoint.GetComponent<Animator>().runtimeAnimatorController = _Animator;
        targetPoint.isplayskill = _isplayskill;
        targetPoint.transform.position = _isplayskill ?  position + Vector3.down*0.3f : position;               //몬스터와 플레이어 피벗이 달라서...임시로..
        targetPoint.attackType = _Animator.name.Split('_')[0];
        targetPoint.gameObject.SetActive(true);
        targetPoint.GetComponent<Animator>().Play("ProjecTileReady");
        return targetPoint;

    }

}

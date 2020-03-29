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
    public float projecTileReady, projecTileStart, projecTileEnd;
    TargetPoint thornPoint;
    int attackDamage = 0;

    [SerializeField]
    GameObject targetObject;
    public List<string> tagsString;
    // Start is called before the first frame update
    void Start()
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
        if ((targetObject != null))
        {
            targetObject.GetComponent<Character>().HPChanged(attackDamage,false,0);
        }
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
                targetObject = null;
            }
        }
    }

    

    public TargetPoint Create(List<string> tagsStringList , Vector2 colOffset, float colRadius, int _damage, RuntimeAnimatorController _Animator, string poolItemName, Vector3 position, Transform parent = null)
    {

        GameObject projectileObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        thornPoint = projectileObject.transform.GetComponent<TargetPoint>();
        thornPoint.attackDamage = _damage;
        thornPoint.transform.position = position;
        thornPoint.tagsString=tagsStringList;
        thornPoint.GetComponent<CircleCollider2D>().offset = colOffset;
        thornPoint.GetComponent<CircleCollider2D>().radius = colRadius;
        thornPoint.GetComponent<Animator>().runtimeAnimatorController = _Animator;
        thornPoint.gameObject.SetActive(true);
        thornPoint.GetComponent<Animator>().Play("ProjecTileReady");
        return thornPoint;

    }

}

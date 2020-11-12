using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//특정 지점에 타격을 가할 때 생성할 투사체 목적지(데미지의 주체)
public class BossTargetpoint : MonoBehaviour
{
   
    public string poolItemName = "BossTargetPointObj";
    public float projecTileReady, projecTileStart, projecTileEnd;
    BossTargetpoint targetpointobj;
    [SerializeField]
    Boss_MaDongSeok boss;
    [SerializeField]
    GameObject player;
    bool week;
    public int AttackPoint;
    public ParticleSystem[] explosion;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Animator>().SetFloat("ReadyTime", projecTileReady);
        GetComponent<Animator>().SetFloat("StartTime", projecTileStart);
        GetComponent<Animator>().SetFloat("EndTime", projecTileEnd);
        boss = GameObject.Find("BossCore").GetComponent<Boss_MaDongSeok>();
        explosion = GetComponentsInChildren<ParticleSystem>();
    }
    void Explosion()
    {
        for (int i = 0; i < explosion.Length; i++)
        {
            explosion[i].Play();
        }
    }
    public void ProjecTileEnd()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(16);
        Debug.Log(player);
        if (!(player == null))
        {
            player.GetComponent<Player>().HPChanged(AttackPoint,false,0);
        }
        GameObject Stone = Instantiate(Resources.Load<GameObject>("Object/Stone"));
        Stone.transform.position = gameObject.transform.position;
        if (!week)
        {
            Debug.Log(week + "sprite");
            Stone.GetComponent<MadongSeokStone>().weekstone = week;
            Stone.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Object/멀쩡한돌박힘");
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            player = collision.gameObject;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            player = null;
        }
    }
    public void ExplosionTarget()
    {
        if (boss.currentstate.Equals(BossState.Phase1))
        {
            boss.TargeExplosion(gameObject.transform.position);
        }
        else
        {
            if (player != null)
            {
                player.GetComponent<Character>().HPChanged(25,false,0);
            }
        }
    }

    public void ResetProjectile()
    {
        ObjectPool.Instance.PushToPool(poolItemName, gameObject, transform);
    }
    public BossTargetpoint Create(float _speed, int _damage, string poolItemName, Vector3 position, bool _week, Transform parent = null)
    {

        GameObject projectileObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        targetpointobj = projectileObject.transform.GetComponent<BossTargetpoint>();

        targetpointobj.transform.position = position;
        targetpointobj.gameObject.SetActive(true);
        targetpointobj.week = _week;
        targetpointobj.GetComponent<Animator>().Play("ProjecTileReady");
        if (week)
        {
            targetpointobj.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Object/멀쩡한돌");
        }
        return targetpointobj;

    }
}

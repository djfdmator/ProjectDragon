//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public string skillName = "TestSkill";
    public Player player;
    public GameObject playerChar;
    public bool StopPlayer = false;
    float my_Timer = 0.0f;
    public float speed;
    public Rigidbody2D rb2d;
    SpriteRenderer sprd;



    float attackDamage;

    public Vector3 myAngle = new Vector3(0, 0, -90);
    public Vector3 nowPos;
    public float PlayerAngle;

    private void Awake() 
    {
       
    }

    public void Start()
    {
        sprd = GetComponent<SpriteRenderer>();
        sprd.sortingOrder = 10;
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerChar = GameObject.FindGameObjectWithTag("Player");
        speed = 10.0f;
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        PlayerAngle = player.enemy_angle;
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, PlayerAngle));
        rb2d.velocity = new Vector2(Mathf.Cos((PlayerAngle - 90) / 360 * 2 * Mathf.PI) * speed, Mathf.Sin((PlayerAngle - 90) / 360 * 2 * Mathf.PI) * speed);
        //   gameObject.transform.rotation = Quaternion.Euler(myAngle);
        //   gameObject.GetComponent<BoxCollider2D>().size = new Vector2(5, 1);
        StartCoroutine(Object_LifeTime(3.0f));
        PlayerAngle = player.enemy_angle;
        this.gameObject.transform.rotation = Quaternion.Euler(0, 0, PlayerAngle - 90);
    }
    public void Update()
    {
        //this.gameObject.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Enemy"))
        {
            Handheld.Vibrate();
            collision.GetComponent<Character>().HPChanged(10,false,0);
            Destroy(this.gameObject);
        }
        if (collision.tag.Equals("Wall"))
        {
            StopCoroutine("Object_LifeTime");
        }
    }
    IEnumerator Object_LifeTime(float cool)
    {
        print("쿨타임 코루틴");
        float i = 0;
        while (cool > i)
        {
            i += Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }
        StopCoroutine("Object_LifeTime");
        Destroy(this.gameObject);
    }


    


}
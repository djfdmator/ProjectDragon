using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    public int roomnumber;
    public GameObject Boss;
    public Sprite EastDoor;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            if (roomnumber.Equals(0))
            {
                collision.transform.position = new Vector3(0, -2f, 0);
                Boss.GetComponent<Boss_MaDongSeok>().StartBoss();
            }
            else if (roomnumber.Equals(1))
            {
                gameObject.GetComponent<SpriteRenderer>().sprite = EastDoor;
                collision.transform.position = new Vector3(21.4f, -21.83f, 0);
            }
            else if (roomnumber.Equals(2))
            {
                collision.transform.position = new Vector3(8.51f, -21.64f, 0);
            }
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class armDemagecheck : MonoBehaviour
{
    Player player;
    public bool bdamageactivate = false;
    int ATK;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bdamageactivate)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player>().HPChanged(ATK, false, 0);
                bdamageactivate = false;
            }
            else if (collision.CompareTag("Stone"))
            {

                collision.transform.gameObject.SendMessage("LazserHit", 0);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player = null;
        }
        else if (collision.CompareTag("Stone"))
        {

            collision.transform.gameObject.SendMessage("LazserHit", 0);
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (bdamageactivate)
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Player>().HPChanged(ATK, false, 0);
                bdamageactivate = false;
            }
        }
        else if (collision.CompareTag("Stone"))
        {

            collision.transform.gameObject.SendMessage("LazserHit", 0);
        }
    }
    public bool DamageActivate(int _ATK)
    {
        ATK = _ATK;
        bdamageactivate = true;
        return false;
    }
}

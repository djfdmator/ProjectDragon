using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armhook : MonoBehaviour
{
    Player player;
    int ATK=1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player");
            player = collision.GetComponent<Player>();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("PlayerExit");
            player = null;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Player");
            player = collision.GetComponent<Player>();
        }
    }
    
    public void PlayerHit()
    {
        
        if (player != null)
        {
            player.HPChanged(ATK,false,0);
            Debug.Log(player);
        }
        gameObject.SetActive(false);
    }
}

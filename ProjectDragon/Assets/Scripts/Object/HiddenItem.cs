using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().items.Add(new Database.Inventory(Database.Inst.weapons[2]));

            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenItem : MonoBehaviour
{
    private bool isGet = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !isGet)
        {
            isGet = true;
            SoundManager.Inst.Ds_EffectPlayerDB(31);
            GameObject.FindGameObjectWithTag("RoomManager").GetComponent<RoomManager>().items.Add(new Database.Inventory(Database.Inst.weapons[2]));

            Destroy(transform.Find("Item").gameObject);
        }
    }
}

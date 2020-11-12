
// ==============================================================
// Cracked Wall Object
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2019-12-16
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : MonoBehaviour
{
    private Room room;
    private Door door;

    public Sprite sprite;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (room.roomState.Equals(RoomState.Clear) && collision.gameObject.CompareTag("Player"))
        {
            door.GetComponent<SpriteRenderer>().sprite = sprite;
            door.animator.enabled = true;
            Destroy(gameObject, 0.2f);
        }
    }
    public void SetRoom(Room _room, Door _door)
    {
        room = _room;
        door = _door;
        switch (door.Name)
        {
            case DoorName.East:
                sprite = Resources.Load<Sprite>("Object/Door_East");
                break;
            case DoorName.North:
                sprite = Resources.Load<Sprite>("Object/Door_North");
                break;
            case DoorName.South:
                sprite = Resources.Load<Sprite>("Object/Door_South");
                break;
            case DoorName.West:
                sprite = Resources.Load<Sprite>("Object/Door_West");
                break;
        }
    }
}

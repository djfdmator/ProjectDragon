using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAround : MonoBehaviour
{
    public Player player;
    public float orbit_Angle;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //transform.RotateAround(player.transform.position, Vector3.down, player.current_angle * Time.deltaTime);
    }
}

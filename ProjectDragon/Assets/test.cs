using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public GameObject boss,player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            boss.GetComponent<Character>().HPChanged(7,false,0);
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            player.GetComponent<Character>().HPChanged(7,false,0);
        }
    }
}

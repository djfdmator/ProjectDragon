using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MadongSeokStone : MonoBehaviour
{
    public bool weekstone = true;
    // Start is called before the first frame update
    void Start()
    {

    }
    public void LazserHit(int damage)
    {
        if (weekstone)
        {
            Destroy(gameObject);
        }
    }
}

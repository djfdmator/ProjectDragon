 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KkeudeokAnim : MonoBehaviour
{
    GameObject kkeudeok;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DecompositionStart()
    {
        GetComponent<Animator>().Play("DecompositionStart");
    }

    public void DecompositionAnimEnd()
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maintenance : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenMaintenance()
    {
        gameObject.SetActive(true);
    }
    public void CloseMaintenance()
    {
        gameObject.SetActive(false);
    }
}

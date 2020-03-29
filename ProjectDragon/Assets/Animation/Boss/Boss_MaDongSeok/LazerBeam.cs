//////////////////////////////////////////////////////////MADE BY Koo KyoSeok///2019-12-16/////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerBeam : MonoBehaviour
{
    private LineRenderer linerenderer;
    public List<string> stoptag;
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        linerenderer = GetComponent<LineRenderer>();
        linerenderer.enabled = true;
        linerenderer.useWorldSpace = true;
    }

    // Update is called once per frame
    void Update()
    {

        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.up, 15);
        bool tag = false;
        for (int i = 0; i < hit.Length; i++)
        {
            
            for (int j = 0; j < stoptag.Count; j++)
            {
                Debug.Log(hit[i].transform.name);
                if (hit[i].transform.CompareTag("Player"))
                {
                    tag = true;
                    Debug.DrawLine(transform.position, hit[i].point);
                    Debug.Log(hit[i].transform.gameObject.name);
                    linerenderer.SetPosition(0, transform.position);
                    linerenderer.SetPosition(1, hit[i].point);
                    hit[i].transform.GetComponent<Character>().HPChanged(damage,false,0);
                }
                else if (hit[i].transform.CompareTag(stoptag[j]))
                {
                    tag = true;
                    hit[i].transform.gameObject.SendMessage("LazserHit", damage);
                    Debug.DrawLine(transform.position, hit[i].point);
                    Debug.Log(hit[i].transform.gameObject.name);
                    linerenderer.SetPosition(0, transform.position);
                    linerenderer.SetPosition(1, hit[i].point);
                    break;
                }
            }
           
            // hit[i].transform.GetComponent

        }
        for (int i = 0; i < hit.Length; i++)
        {
            if (tag)
            {
                break;
            }
            if (hit[i].transform.CompareTag("Wall"))
            {
                Debug.DrawLine(transform.position, hit[i].point);
                Debug.Log(hit[i].transform.gameObject.name);
                linerenderer.SetPosition(0, transform.position);
                linerenderer.SetPosition(1, hit[i].point);
                break;
            }
        }
    }

}

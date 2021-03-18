using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CenterExtension : MonoBehaviour
{
    public UIScrollView mScrollView;
    Vector3[] corners ;
    Vector3 panelCenter;
    void Start()
    {
        mScrollView = transform.parent.parent.GetComponent<UIScrollView>();
        corners = mScrollView.panel.worldCorners;
        panelCenter = (corners[2] + corners[0]) * 0.5f;
    }
    // Update is called once per frame
    void Update()
    {
        if(0.7> Mathf.Abs(Vector3.Distance(panelCenter, gameObject.transform.position)))
        {
            gameObject.transform.localScale = Vector3.Lerp(new Vector3(1.2f,1.2f,1.2f), Vector3.one,Mathf.Abs(panelCenter.x- gameObject.transform.position.x)/0.7f);
        }
    }
}

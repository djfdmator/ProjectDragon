using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorRepresentation : MonoBehaviour
{

    public GameObject playerimg;
    public float sizeoption = 2;
    private double distance;

    void Start()
    {
        playerimg = transform.Find("Charactor").gameObject;
    }

    void Update()
    {
        if (Input.touchCount.Equals(1))
        {
            distance = -1;
        }
       
        else if (Input.touchCount.Equals(2))
        {
            Vector3 touchpoint0 = UICamera.currentCamera.ScreenToWorldPoint(Input.GetTouch(0).position);
            Vector3 touchpoint1 = UICamera.currentCamera.ScreenToWorldPoint(Input.GetTouch(1).position);

            RaycastHit2D hit0 = Physics2D.Raycast(touchpoint0, transform.forward, 10);
            RaycastHit2D hit1 = Physics2D.Raycast(touchpoint1, transform.forward, 10);
            if (hit0 && hit1)
            {
                if (hit0.transform.gameObject.name.Equals("playersizebox") && hit1.transform.gameObject.name.Equals("playersizebox"))
                {
                    if (distance.Equals(-1))
                    {
                        distance = Vector3.Distance(touchpoint0, touchpoint1);
                    }
                    else
                    {
                        if (distance < Vector3.Distance(touchpoint0, touchpoint1))
                        {
                            float currentdistance = Vector3.Distance(touchpoint0, touchpoint1);
                            Vector3 imagescale = new Vector3(playerimg.transform.localScale.x - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.y - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.z - ((float)distance - currentdistance) / sizeoption);
                            if (imagescale.x <= 1.5 && imagescale.y <= 1.5)
                            {
                                playerimg.transform.localScale = imagescale;
                            }
                            else
                            {
                                playerimg.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                            }
                            distance = Vector3.Distance(touchpoint0, touchpoint1);
                        }
                        else if (distance > Vector3.Distance(touchpoint0, touchpoint1))
                        {
                            float currentdistance = Vector3.Distance(touchpoint0, touchpoint1);
                            Vector3 imagescale = new Vector3(playerimg.transform.localScale.x - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.y - ((float)distance - currentdistance) / sizeoption, playerimg.transform.localScale.z - ((float)distance - currentdistance) / sizeoption);
                            if (imagescale.x > 0.5 && imagescale.y > 0.5)
                            {
                                playerimg.transform.localScale = imagescale;
                            }
                            else
                            {
                                playerimg.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
                            }
                            distance = Vector3.Distance(touchpoint0, touchpoint1);
                        }
                    }
                }
            }
            if (Input.GetTouch(0).phase.Equals(TouchPhase.Ended))
            {
                distance = -1;
            }
        }
    }


    /// <summary>
    /// 캐릭터의 스킨 조정
    /// </summary>
    /// <param name="playerclass"></param>
    public void CharactorSkinSet(string playerclass)
    {
        Texture2D Skin = Resources.Load<Texture2D>(playerclass);
        //equipCharactor.GetComponent<UITexture>().mainTexture = Skin;
    }

}

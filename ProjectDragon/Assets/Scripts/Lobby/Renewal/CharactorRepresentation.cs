using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactorRepresentation : MonoBehaviour
{

    public GameObject playerimg;
    public UISprite charactorIllustrate;
    public UISprite weaponIllustrate;
    public float sizeoption = 2;
    private double distance;

    private void Awake()
    {
        playerimg = transform.Find("CharactorView/Charactor").gameObject;
        if (charactorIllustrate == null) charactorIllustrate = playerimg.GetComponent<UISprite>();
        if (weaponIllustrate == null) weaponIllustrate = playerimg.transform.Find("Weapon").GetComponent<UISprite>();
    }

    void Start()
    {
        RefeshCharactorIllustrate();
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

    //CharactorView postion - (-160, -120, 0)
    //CharactorView collider rect - (0, 100, 800, 900)
    public void RefeshCharactorIllustrate()
    {
        int weaponName = GameManager.Inst.PlayerEquipWeapon.DB_Num;

        weaponIllustrate.spriteName = "LobbyIllustration_" + GameManager.Inst.CurrentEquipWeapon.imageName;
        weaponIllustrate.MakePixelPerfect();
        switch (weaponName)
        {
            case 0:
                //녹슨검
                charactorIllustrate.spriteName = "LobbyIllustration_Female_Worrior";
                weaponIllustrate.transform.localPosition = new Vector3(8.3f, 336.5f, 0.0f);
                break;
            case 1:
                //오래된 지팡이
                charactorIllustrate.spriteName = "LobbyIllustration_Female_Wizard";
                weaponIllustrate.transform.localPosition = new Vector3(162.0f, 538.0f, 0.0f);
                break;
            case 2:
                //승리의 검
                charactorIllustrate.spriteName = "LobbyIllustration_Female_Worrior";
                weaponIllustrate.transform.localPosition = new Vector3(7.3f, 382.5f, 0.0f);
                break;
            case 4:
                //네레이더스
                charactorIllustrate.spriteName = "LobbyIllustration_Female_Wizard";
                weaponIllustrate.transform.localPosition = new Vector3(141.6f, 714.5f, 0.0f);
                break;
            case 5:
                //닉스
                charactorIllustrate.spriteName = "LobbyIllustration_Female_Wizard";
                weaponIllustrate.transform.localPosition = new Vector3(200.0f, 680.0f, 0.0f);    
                break;
            default:
                break;
        }
    }

}

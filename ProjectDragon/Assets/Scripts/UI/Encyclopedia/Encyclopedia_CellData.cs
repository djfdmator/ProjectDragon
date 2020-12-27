using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encyclopedia_CellData : MonoBehaviour
{
    private GameObject deactivationObj;
    private GameObject activationObj;

    public UISprite deactivationSprite;
    public UISprite activationSprite;
    public UIButton button;

    public int DB_Num = -1;
    private string spriteName = "Encyclopedia_Slot_";

    private void Awake()
    {
        deactivationObj = transform.Find("Deactivation").gameObject;
        activationObj = transform.Find("Activation").gameObject;

        deactivationSprite = deactivationObj.GetComponent<UISprite>();
        activationSprite = activationObj.GetComponent<UISprite>();
        button = activationObj.GetComponent<UIButton>();

    }

    //초기화
    public void Init(Database.Encyclopedia encyclopedia)
    {
        DB_Num = encyclopedia.num;

        deactivationSprite.spriteName = spriteName + encyclopedia.imageName + "_Gray";
        activationSprite.spriteName = spriteName + encyclopedia.imageName;
        button.normalSprite = spriteName + encyclopedia.imageName;

        ToggleActive(encyclopedia.active);
    }
    public void Init(Database.Achievement achievement)
    {
        DB_Num = achievement.num;

        deactivationSprite.spriteName = spriteName + achievement.imageName + "_Gray";
        activationSprite.spriteName = spriteName + achievement.imageName;
        button.normalSprite = spriteName + achievement.imageName;

        ToggleActive(achievement.active);
    }


    public void ToggleActive(bool activation)
    {
        activationObj.SetActive(activation);
        deactivationObj.SetActive(!activation);
    }
   

}

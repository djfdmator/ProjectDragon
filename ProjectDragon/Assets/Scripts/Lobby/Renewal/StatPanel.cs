using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatPanel : MonoBehaviour
{ 
    public UILabel damage;
    public UILabel hp;
    public UILabel defence;
    public UILabel attackSpeed;

    public UISprite button;

    public bool isOpen = false;

    private void Awake()
    {
        if (damage == null) damage = transform.Find("Background/Damage/Label").GetComponent<UILabel>();
        if (hp == null) hp = transform.Find("Background/HP/Label").GetComponent<UILabel>();
        if (defence == null) defence = transform.Find("Background/Defence/Label").GetComponent<UILabel>();
        if (attackSpeed == null) attackSpeed = transform.Find("Background/AttackSpeed/Label").GetComponent<UILabel>();

        if (button == null) button = transform.Find("Background/Button").GetComponent<UISprite>();
    }

    private void Start()
    {
        //if (damage == null) damage = transform.Find("Background/Damage/Label").GetComponent<UILabel>();
        //if (hp == null) hp = transform.Find("Background/HP/Label").GetComponent<UILabel>();
        //if (defence == null) defence = transform.Find("Background/Defence/Label").GetComponent<UILabel>();
        //if (attackSpeed == null) attackSpeed = transform.Find("Background/AttackSpeed/Label").GetComponent<UILabel>();

        //if (button == null) button = transform.Find("Background/Button").GetComponent<UISprite>();
    }

    public void Toggle_ButtonImage()
    {
        isOpen = !isOpen;
        if(isOpen)
        {
            RefreshStatData();
            button.spriteName = "Lobby_Stats_L";
        }
        else
        {
            button.spriteName = "Lobby_Stats_R";
        }
    }

    public void RefreshStatData()
    {
        Debug.Log(damage);
        damage.text = GameManager.Inst.Atk_Max.ToString();
        hp.text = GameManager.Inst.MaxHp.ToString();
        defence.text = GameManager.Inst.CurrentEquipArmor.hp.ToString();
        attackSpeed.text = GameManager.Inst.AttackSpeed.ToString();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPGauge : MonoBehaviour
{
    //Player Find, and HP Changed
    public Player Current_player;
    public float HP_Slide;
    public int record_HPBar;

    public bool damaged_Hp_Flag = false;

    //Use this UISprite from How to visually looking by HPGauge. 
    public UISprite First_Block_HPGauge;
    public UISprite Second_Block_HPGauge;
    public UISprite Damaged_HPGauge;
    public UILabel record_HP;

  
    //if Vector3 equal Maximum_Transform_Position then changed sprite in this place.
    public string MaxPos_sprite_Name = "HP_04";
    //if Vector3 equal Minimal_Transform_Position then changed sprite goint to this place.
    public string MinPos_sprite_Name = "HP_03";


    // Start is called before the first frame update
    void Start()
    {

        Current_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
       // First_Block_HPGauge = GameObject.Find("UI Root/HPBar/HP_01").GetComponent<UISprite>();
        Second_Block_HPGauge = GameObject.Find("UI Root/HPBar/HP_02").GetComponent<UISprite>();
        Damaged_HPGauge = GameObject.Find("UI Root/HPBar/HP_Damaged").GetComponent<UISprite>();
      //  Third_Block_HPGauge.transform.localPosition = Minimal_Transform_Position;
        record_HPBar = Current_player.HP;
        record_HP.text = record_HPBar.ToString();
    }

    public void Player_HP_Changed(float playerHP, float playerMaxHP)
    {
        HP_Slide = playerHP / playerMaxHP;
        Second_Block_HPGauge.fillAmount = HP_Slide;
        Invoke("DMG",0.5f);
        record_HPBar = Current_player.HP;
        record_HP.text = record_HPBar.ToString();
    }
    public void DMG()
    {
        damaged_Hp_Flag = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(damaged_Hp_Flag)
        {
            Damaged_HPGauge.fillAmount = Mathf.Lerp(Damaged_HPGauge.fillAmount, Second_Block_HPGauge.fillAmount, Time.deltaTime*2f);
            if (Second_Block_HPGauge.fillAmount>=Damaged_HPGauge.fillAmount)
            {
                damaged_Hp_Flag = false;
                Damaged_HPGauge.fillAmount = Second_Block_HPGauge.fillAmount;
            }
        }
    }
}

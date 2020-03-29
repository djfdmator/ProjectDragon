using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPGauge : MonoBehaviour
{

    [SerializeField]
    private Player player;
    [SerializeField]
    public UILabel[] MP_Label= new UILabel[8];
    public int LengthSize;
    //숫자 카운트 새기용
    // 마나 수치
    public int mana_Point;
    // Start is called before the first frame update
    void Start()
    {
        initialize_Link();
    }
    // Update is called once per frame
    private void Update()
    {
        int i;
        mana_Point = player.mp;
        string A = mana_Point.ToString();
        LengthSize = A.Length;
        for (i = 0; i < A.Length; i++)
        {
            if (mana_Point == 0)
            {
                return;
            }
            else
            {
                MP_Label[i].text = A.Substring((A.Length-1)-i,1);
            }
        }
        // Counter don't use = Reset
        if(i<=8)
        {
            for(int j=0;j<8-i;j++)
            {
                MP_Label[7-j].text = " ";
            }
        }
        if(mana_Point.Equals(0))
        {
            MP_Label[0].text = "0";
        }
        if (mana_Point >= 99999999)
        {
            return;
        }
    }

    #region  MANA Count like slot machine
    /// <summary>
    /// 마나를 슬롯 머신의 형태로 해결하는 함수!
    /// 2020.01.02 이상준
    /// </summary>
    public void MP_slot_Counter()
    {
        //몇 자리까지 해야할 지 잘 몰겠는데 일단 100만 자리까지 가보자.
        //1000000
      
    }
    #endregion


    #region INITIALIZE LINK
    void initialize_Link()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
         for(int i=0;i<MP_Label.Length;i++)
        {
              MP_Label[i] = transform.GetChild(i).GetComponent<UILabel>();
        }
    
    }
    #endregion
}

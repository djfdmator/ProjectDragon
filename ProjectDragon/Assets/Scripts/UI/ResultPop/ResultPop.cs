/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2020-04-17////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ResultPop : MonoBehaviour
{
    
    public bool isSuccess;

    private GameObject BG;
    private GameObject successPopup;
    private GameObject failPopup;
    private GUIItemScrollView itemListView;

    private int acheiveMP;
    private UILabel mpLabel;

    public BattleEquipmentChangeWindow battleEquipmentChangeWindow;


    private void Awake()
    {
        BG = transform.Find("Background").gameObject;
        mpLabel = BG.transform.Find("MP").GetComponentInChildren<UILabel>();
        itemListView = BG.transform.Find("ItemListBG").GetComponentInChildren<GUIItemScrollView>();
        successPopup = BG.transform.Find("Success").gameObject;
        failPopup = BG.transform.Find("Fail").gameObject;

        failPopup.SetActive(false);
        successPopup.SetActive(false);
    }
  
    public void OnResult(int _mp, bool _isSuccess)
    {
        acheiveMP = _mp;
        isSuccess = _isSuccess;
        mpLabel.text = string.Format("{0:#,##0}", acheiveMP);

        if (isSuccess)
        {
            successPopup.SetActive(true);
        }
        else
        {
            failPopup.SetActive(true);
        }

        //itemList Setting
        itemListView.GetResultItem();
        Time.timeScale = 0.0f;

    }

    #region Button
    public void LobbyButton()
    {
        Time.timeScale = 1.0f;
        ButtonManager.GotoLobby();
    }
   
    public void NextStageButton()
    {
        Time.timeScale = 1.0f;
        GameManager.Inst.Loading(true);
    }

    public void NextButton()
    {
        battleEquipmentChangeWindow.Init();
        battleEquipmentChangeWindow.gameObject.SetActive(true);
    }
    #endregion

}

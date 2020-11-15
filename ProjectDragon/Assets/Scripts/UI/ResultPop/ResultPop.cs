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

            if (GameManager.Inst.CurrentStage == Database.Inst.playData.finalStage)
            {
                successPopup.transform.Find("NextStageButton").GetComponent<UISprite>().spriteName = "GoLobbyButton";
            }
            else
            {
                successPopup.transform.Find("NextStageButton").GetComponent<UISprite>().spriteName = "NextStageButton";
            }

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
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        Time.timeScale = 1.0f;
        GameManager.Inst.PlayerDeadToInitialData();
        GameManager.Inst.Loading(false);
    }

    public void NextStageButton()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        Time.timeScale = 1.0f;
        if (GameManager.Inst.CurrentStage == Database.Inst.playData.finalStage)
        {
            GameManager.Inst.PlayerDeadToInitialData();
            ButtonManager.GotoLobby();
        }
        else
        {
            GameManager.Inst.Loading(true);
        }
    }

    public void NextButton()
    {
        SoundManager.Inst.EffectPlayerDB(1, this.gameObject);
        Time.timeScale = 1.0f;

        if (GameManager.Inst.CurrentStage == Database.Inst.playData.finalStage)
        {
            GameManager.Inst.PlayerDeadToInitialData();
            GameManager.Inst.Loading(false);
        }
        else
        {
            gameObject.SetActive(false);
            battleEquipmentChangeWindow.Init();
        }
    }
    #endregion

}

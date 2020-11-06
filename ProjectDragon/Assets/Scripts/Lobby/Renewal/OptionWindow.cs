using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionWindow : MonoBehaviour
{
    public Transform gameSetting;
    public UISlider BGM, SFX;
    public UIToggle machineVibration, screenVibration;

    public GameObject resetGameDialogue; 
    public GameObject quitApplicationDialogue; 

    private void Awake()
    {
        if (gameSetting == null) gameSetting = transform.Find("Category").Find("GameSettingButton").Find("Enable").Find("SettingWindow");
        if (BGM == null) BGM = gameSetting.Find("BGMSetting").Find("Slider").GetComponent<UISlider>();
        if (SFX == null) SFX = gameSetting.Find("SFXSetting").Find("Slider").GetComponent<UISlider>();
        if (machineVibration == null) machineVibration = gameSetting.Find("DeviceVibrationSetting").Find("Toggle").Find("Disable").GetComponent<UIToggle>();
        if (screenVibration == null) screenVibration = gameSetting.Find("ScreenVibrationSetting").Find("Toggle").Find("Disable").GetComponent<UIToggle>();

        if (resetGameDialogue == null) resetGameDialogue = transform.Find("Category").Find("ResetGameButton").Find("Dialogue").gameObject;
        if (quitApplicationDialogue == null) quitApplicationDialogue = transform.Find("Category").Find("QuitButton").Find("Dialogue").gameObject;

        resetGameDialogue.SetActive(false);
        quitApplicationDialogue.SetActive(false);
    }

    public void OpenOptionWindow()
    {
        //TODO : 설정창 초기화
        machineVibration.Set(Database.Inst.playData.isMachineVibration);
        screenVibration.Set(Database.Inst.playData.isScreenVibration);
        BGM.value = Database.Inst.playData.BGM_Volume;
        SFX.value = Database.Inst.playData.SFX_Volume;

        gameObject.SetActive(true);
    }

    public void ToggleMachineVibration()
    {
        Database.Inst.playData.isMachineVibration = !(Database.Inst.playData.isMachineVibration);
    }

    public void ToggleScreenVibration()
    {
        Database.Inst.playData.isScreenVibration = !(Database.Inst.playData.isScreenVibration);
    }

    public void CloseOptionWindow()
    {
        GameManager.Inst.Save_PlayerPrefs_Data();
        gameObject.SetActive(false);
    }

    public void QuitApplication()
    {
        ButtonManager.GameQuit();
    }

    public void ResetGame()
    {
        //TODO : 데이터 초기화 수정해야함

        SceneManager.LoadScene("Title");

    }

    public void OpenDialogue_QuitApplication()
    {
        quitApplicationDialogue.SetActive(true);
    }

    public void CloseDialogue_QuitApplication()
    {
        quitApplicationDialogue.SetActive(false);
    }

    public void OpenDialogue_ResetGame()
    {
        resetGameDialogue.SetActive(true);
    }

    public void CloseDialogue_ResetGame()
    {
        resetGameDialogue.SetActive(false);
    }

    public void BGMValueChange(float value)
    {
        SoundManager.Inst.Ds_BGMSoundController(value);
    }

    public void SFXValueChange(float value)
    {
        SoundManager.Inst.Ds_SFXSoundController(value);
    }
}

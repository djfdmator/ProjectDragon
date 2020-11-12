using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseWindow : MonoBehaviour
{
    private GameObject dialogue;
    private void Awake()
    {
        dialogue = transform.Find("BackGround").transform.Find("Dialogue").gameObject;

        CloseDialogue();
        ClosePauseWindow();
    }

    private void TogglePause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }

    public void ClosePauseWindow()
    {
        gameObject.SetActive(false);
        TogglePause(false);
    }
    public void OpenPauseWindow()
    {
        gameObject.SetActive(true);
        TogglePause(true);
    }

    public void OpenDialogue()
    {
        dialogue.SetActive(true);
    }
    public void CloseDialogue()
    {
        dialogue.SetActive(false);
    }

    public void ApplicationQuitBuuton()
    {
        ButtonManager.GameQuit();
    }

    public void GotoLobbyBuuton()
    {
        ButtonManager.GotoLobby();
    }
}

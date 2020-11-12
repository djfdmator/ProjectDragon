﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseWindow : MonoBehaviour
{
    private GameObject dialogue;
    private void Awake()
    {
        dialogue = transform.Find("BackGround").transform.Find("Dialogue").gameObject;

        dialogue.SetActive(false);
        gameObject.SetActive(false);
        TogglePause(false);

        //CloseDialogue();
        //ClosePauseWindow();
    }

    private void TogglePause(bool pause)
    {
        Time.timeScale = pause ? 0 : 1;
    }

    public void ClosePauseWindow()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);

        gameObject.SetActive(false);
        TogglePause(false);
    }
    public void OpenPauseWindow()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(10);

        gameObject.SetActive(true);
        TogglePause(true);
    }

    public void OpenDialogue()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);

        dialogue.SetActive(true);
    }
    public void CloseDialogue()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);

        dialogue.SetActive(false);
    }

    public void ApplicationQuitBuuton()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);

        ButtonManager.GameQuit();
    }

    public void GotoLobbyBuuton()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);

        ButtonManager.GotoLobby();
    }
}

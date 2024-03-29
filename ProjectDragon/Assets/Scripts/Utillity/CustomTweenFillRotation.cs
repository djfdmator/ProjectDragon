﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTweenFillRotation : MonoBehaviour
{
    public UISprite sprite;
    public float playTime = 1.0f;
    public AnimationCurve ac;
    public UIButton button;

    void Start()
    {
        sprite.fillAmount = 0.0f;
    }

    public void FuncA()
    {
        button.isEnabled = false;
        StartCoroutine(AcTest());
    }

    private IEnumerator AcTest()
    {
        float time = 0.0f;
        sprite.fillAmount = 0.0f;
        while(time <= playTime)
        {
            sprite.fillAmount = ac.Evaluate(time / playTime);
            time += Time.deltaTime;
            yield return null;
        }
        yield return null;
        button.isEnabled = true;
    }
}

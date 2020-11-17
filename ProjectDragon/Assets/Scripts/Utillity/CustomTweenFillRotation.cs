using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomTweenFillRotation : MonoBehaviour
{
    public UISprite sprite;
    public float playTime = 1.0f;
    public AnimationCurve ac;

    public UIButton button;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void FuncA()
    {
        button.isEnabled = false;
        StartCoroutine(acTest());
    }

    private IEnumerator acTest()
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

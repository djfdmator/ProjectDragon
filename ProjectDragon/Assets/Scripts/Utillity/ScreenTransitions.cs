
// ==============================================================
// ScreenTransitions
// Scene Transition Effect Basic Class
//
//  AUTHOR: Yang Se Eun
// CREATED: 2020-01-02
// UPDATED: 
// ==============================================================

using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class ScreenTransitions : MonoBehaviour
{
    public Material TransitionMaterial;

    void OnRenderImage(RenderTexture src, RenderTexture dst)
    {
        if (TransitionMaterial != null)
            Graphics.Blit(src, dst, TransitionMaterial);
    }

    private void Awake()
    {
        //Assets / Resources / Effect / Materials / ScreenTransitions.mat
        TransitionMaterial = Resources.Load<Material>("Effect/Materials/ScreenTransitions");
        InitTransitionMaterial();
    }

    public void InitTransitionMaterial(bool isWhite = true)
    {
        if(isWhite)
        {
            TransitionMaterial.SetFloat("_Cutoff", 0.0f);
            TransitionMaterial.SetFloat("_Fade", 0.0f);
        }
        else
        {
            TransitionMaterial.SetFloat("_Cutoff", 1.0f);
            TransitionMaterial.SetFloat("_Fade", 1.0f);
        }
    }

    public void SetTransitionMaterial(string path)
    {
        TransitionMaterial.SetTexture("_TransitionTex", Resources.Load<Texture>(path));
    }

    public IEnumerator Fade(float fadeTime, bool isIn)
    {
        TransitionMaterial.SetFloat("_Cutoff", 1.0f);

        float fade = 0;
        float time = 0;

        while (time <= fadeTime)
        {
            time += Time.deltaTime;
            if (isIn) fade = Mathf.Lerp(0.0f, 1.0f, time / fadeTime);
            else fade = Mathf.Lerp(0.0f, 1.0f, 1 - time / fadeTime);
            TransitionMaterial.SetFloat("_Fade", fade);

            yield return null;
        }
    }

    public IEnumerator Play(string path, float transitionTime, bool isReverse)
    {
        TransitionMaterial.SetTexture("_TransitionTex", Resources.Load<Texture>(path));

        float cutoff = 0f;
        float time = 0;

        while (time <= transitionTime)
        {
            time += Time.deltaTime;
            if (!isReverse) cutoff = Mathf.Lerp(0.0f, 1.0f, time / transitionTime);
            else cutoff = Mathf.Lerp(0.0f, 1.0f, 1 - time / transitionTime);
            TransitionMaterial.SetFloat("_Cutoff", cutoff);

            yield return null;
        }
    }

    public IEnumerator Play(float transitionTime, bool isReverse)
    {
        float cutoff = 0f;
        float time = 0;

        while (time <= transitionTime)
        {
            time += Time.deltaTime;
            if (!isReverse) cutoff = Mathf.Lerp(0.0f, 1.0f, time / transitionTime);
            else cutoff = Mathf.Lerp(0.0f, 1.0f, 1 - time / transitionTime);
            TransitionMaterial.SetFloat("_Cutoff", cutoff);

            yield return null;
        }
    }
}

//////////////////////////////////////////////////////////MADE BY Koo KyoSeok///2019-12-16/////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioSource Ds_efxSource;//효과음
    public AudioSource Ds_musicSource;//배경음악
    public float Ds_BGMvolumeRange = 0.5f;//현재사운드 크기
    public float Ds_SFXvolumeRange = 0.5f;//현재사운드 크기
    int bgmnum=-1;
    public List<AudioClip> Clips=new List<AudioClip>();
    /// <summary>
    /// 초기의 배경음과 이펙스재생을 할 오디오 소스 생성
    /// </summary>
    private void Awake()
    {
        Ds_efxSource = gameObject.AddComponent<AudioSource>();
        Ds_efxSource.loop = false;
        Ds_musicSource = gameObject.AddComponent<AudioSource>();
        Ds_musicSource.loop = true;

    }
    /// <summary>
    /// 배경음 재생 SoundManager.Inst.Ds_BgmPlayer(Audio ***);
    /// </summary>
    /// <param name="clip"></param>
    public void Ds_BgmPlayer(AudioClip _clip)
    {
        Ds_musicSource.clip = _clip;
        Ds_musicSource.Play();
        Ds_musicSource.volume = Ds_BGMvolumeRange;
    }
    public void Ds_BGMPlayerDB(int _index)
    {
        
        AudioClip BGM=Resources.Load<AudioClip>(GameManager.Inst.LoadSoundQue(_index,true));
        if(!bgmnum.Equals(_index))
        {
            bgmnum=_index;
        #if UNITY_EDITOR
        //Debug.Log(GameManager.Inst.LoadSoundQue(_index,false));
        #endif
        Ds_BgmPlayer(BGM);
        }
    }
    
    public void Ds_EffectPlayerDB(int _index)
    {
        #if UNITY_EDITOR
        //Debug.Log(GameManager.Inst.LoadSoundQue(_index,false));
        #endif
        AudioClip Effect=Resources.Load<AudioClip>(GameManager.Inst.LoadSoundQue(_index,false));
        #if UNITY_EDITOR
        //Debug.Log(Effect.name);
        #endif
        Ds_PlaySingle(Effect);
    }
    /// <summary>
    /// 하나의 효과음재생 SoundManager.Inst.PlaySingle(Audio ***);
    /// </summary>
    /// <param name="clip"></param>
    public void Ds_PlaySingle(AudioClip _clip)
    {
        Ds_efxSource.clip = _clip;
        Ds_efxSource.Play();
        Ds_efxSource.volume = Ds_SFXvolumeRange;
    }
    /// <summary>
    /// 여러개의 효과음중 하나만 재생하고 싶을때 SoundManager.Inst.RandomizeSfx(Audio***,*** ...);
    /// </summary>
    /// <param name="clips">랜덤하게 재생할 오디오클립의 배열</param>
    public void Ds_RandomizeSfx(params AudioClip[] _clips)
    {
        int randomIndex = Random.Range(0, _clips.Length);
        Ds_efxSource.clip = _clips[randomIndex];
        Ds_efxSource.Play();
        Ds_efxSource.volume = Ds_SFXvolumeRange;

    }
    // Use this for initialization

    public void Ds_BGMSoundController(float value)//슬라이더가 움직일 때마다 호출되어 사운드의 조절을 해준다.
    {
        Debug.Log(value);
        Ds_BGMvolumeRange = value;
        Debug.Log(Ds_BGMvolumeRange);
        Ds_musicSource.volume = Ds_BGMvolumeRange;
    }
    public void Ds_SFXSoundController(float value)//슬라이더가 움직일 때마다 호출되어 사운드의 조절을 해준다.
    {
        Debug.Log(value);
        Ds_SFXvolumeRange = value;
        Debug.Log(Ds_SFXvolumeRange);
        Ds_efxSource.volume = Ds_SFXvolumeRange;
    }
    public void WalkSound(AudioClip _clip,State state)
    {
        if(state == State.Walk)
        {
            Ds_efxSource.clip = _clip;
            Ds_efxSource.Play();
            Ds_efxSource.volume = Ds_SFXvolumeRange;
        }
        Ds_efxSource.Stop();
    }
    //ERROR
    //public void SoundSet(params int[] _indexes)
    //{
    //    if(_indexes.Length>0)
    //    {
    //        for(int i=0;i< _indexes.Length;i++)
    //        {
    //            SoundTable(_indexes[i]);
    //        }
    //    }
    //}
    //ERROR
    //public void SoundTable(int _index)
    //{
    //    AudioClip soundclip;
    //    switch (_index)
    //    {
    //        case 1:
    //        //                soundclip = Resources.Load<AudioClip>("Sound/");
    //        default:
    //            break;
    //    }
    //}
    private void OnLevelWasLoaded(int level)
    {
        Clips.Clear();
    }
}

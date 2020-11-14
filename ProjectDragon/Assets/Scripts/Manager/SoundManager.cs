//////////////////////////////////////////////////////////MADE BY Koo KyoSeok///2019-12-16/////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoSingleton<SoundManager>
{
    public AudioSource Ds_efxSource;//효과음
    public AudioSource Ds_musicSource;//배경음악
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
        Ds_musicSource.volume = Database.Inst.playData.BGM_Volume;
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
        //Ds_PlaySingle(Effect);
        PlayEffectSound(this.gameObject, Effect);
    }

    /// <summary>
    /// 새로운 버전 효과음플레이!
    /// </summary>
    /// <param name="_index"></param>
    /// <param name="obj"></param>
    public void EffectPlayerDB(int _index,GameObject obj)
    {
        PlayEffectSound(this.gameObject, Resources.Load<AudioClip>(GameManager.Inst.LoadSoundQue(_index, false)));
    }

    private void PlayEffectSound(GameObject obj, AudioClip clip)
    {
        AudioSource audioSource;
        if (obj.GetComponent<AudioSource>() != null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            audioSource = obj.AddComponent<AudioSource>();
        }
        audioSource.volume = Database.Inst.playData.SFX_Volume;
        audioSource.PlayOneShot(clip);
    }
    /// <summary>
    /// 하나의 효과음재생 SoundManager.Inst.PlaySingle(Audio ***);
    /// </summary>
    /// <param name="clip"></param>
    public void Ds_PlaySingle(AudioClip _clip)
    {
        Ds_efxSource.clip = _clip;
        Ds_efxSource.Play();
        Ds_efxSource.volume = Database.Inst.playData.SFX_Volume;
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
        Ds_efxSource.volume = Database.Inst.playData.SFX_Volume;

    }
    // Use this for initialization

    public void Ds_BGMSoundController(float value)//슬라이더가 움직일 때마다 호출되어 사운드의 조절을 해준다.
    {
        Database.Inst.playData.BGM_Volume = value;
        Ds_musicSource.volume = value;
    }
    public void Ds_SFXSoundController(float value)//슬라이더가 움직일 때마다 호출되어 사운드의 조절을 해준다.
    {
        Database.Inst.playData.SFX_Volume = value;
        Ds_efxSource.volume = value;
    }
    public void WalkSound(AudioClip _clip,State state)
    {
        if(state == State.Walk)
        {
            Ds_efxSource.clip = _clip;
            Ds_efxSource.Play();
            Ds_efxSource.volume = Database.Inst.playData.SFX_Volume;
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

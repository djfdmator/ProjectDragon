
// ==============================================================
// TitleManager
// All presentation and Load db, login, data checking manager
// 
// 2020-01-31: TitleManager Complete
//
//  AUTHOR: Kim Dong Ha
// CREATED: 2020-01-22
// UPDATED: 2020-01-31
//===============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [Header("[Database]")]
    public string nickName;
    public SEX sex;
    public CLASS Item_Class;

    [Header("[ScreenTransition]")]
    public Camera _camera;

    [Header("[Scenes]")]
    public GameObject mainScene; //메인 화면
    public GameObject nickNameScene; //닉네임 화면
    public GameObject selectScene;

    [Header("[GameLogo]")]
    public GameObject gameLogo;
    public UITexture gameLogo_Effect;
    public GameObject gameLogo_Label;

    [Header("[SelectScene]")]
    string animSex, animWeapon;
    public GameObject[] selectButton = new GameObject[4]; //0-male, 1-female, 2-sword, 3-wand
    public GameObject animWindow;
    public GameObject nextbutton;
    public Animator playerBody;
    public Animator playerArm;
    public Animator playerWeapon;
    public Texture2D button_Activate;
    public Texture2D button_DeActivate;
    public Texture2D animWindow_Activate;
    public Texture2D animWindow_DeActivate;
    public Texture2D nextButton_Activate;
    public Texture2D nextButton_DeActivate;

    private void Awake()
    {
        Initialized();
    }

    /// <summary>
    /// Initialized Variable and Scene
    /// </summary>
    private void Initialized()
    {
        _camera = GameObject.FindGameObjectWithTag("ScreenTransitions").GetComponent<Camera>();

        #region GameLogo
        mainScene = transform.Find("MainScene").Find("BGImage").gameObject;
        gameLogo = mainScene.transform.Find("GameLogoPanel").gameObject;
        gameLogo_Effect = gameLogo.transform.Find("Effect").GetComponent<UITexture>();
        gameLogo_Label = mainScene.transform.Find("TTSPanel").gameObject;

        mainScene.GetComponent<BoxCollider>().enabled = false;
        gameLogo.SetActive(false);
        gameLogo_Label.SetActive(false);
        #endregion

        #region NickName
        nickNameScene = transform.Find("NickNameScene").Find("BGImage").gameObject;

        nickNameScene.SetActive(false);
        nickNameScene.transform.Find("NickNameSettingImage").Find("Failed").gameObject.SetActive(false);
        #endregion

        #region SelectScene
        selectScene = transform.Find("SelectScene").Find("BGImage").gameObject;
        animWindow = selectScene.transform.Find("CharacterAnim").gameObject;
        nextbutton = selectScene.transform.Find("NextButton").gameObject;
        playerBody = animWindow.transform.Find("PlayerAnim").GetComponent<Animator>();
        playerArm = playerBody.transform.Find("Arm").GetComponent<Animator>();
        playerWeapon = playerBody.transform.Find("Weapon").GetComponent<Animator>();
        selectButton[0] = selectScene.transform.Find("SelectionSex").Find("Male").gameObject;
        selectButton[1] = selectScene.transform.Find("SelectionSex").Find("Female").gameObject;
        selectButton[2] = selectScene.transform.Find("SelectionWeapon").Find("Sword").gameObject;
        selectButton[3] = selectScene.transform.Find("SelectionWeapon").Find("Wand").gameObject;

        playerBody.gameObject.SetActive(false);
        playerBody.speed = 0.5f;
        playerArm.speed = 0.5f;
        playerWeapon.speed = 0.5f;
        selectScene.SetActive(false);
        nextbutton.GetComponent<BoxCollider>().enabled = false;
        #endregion

    }

    private void Start()
    {
        SoundManager.Inst.Ds_BGMPlayerDB(1);
        StartCoroutine(Presentation_Logo());
    }

    #region Title Main
    private void Init_Logo()
    {
        mainScene.GetComponent<BoxCollider>().enabled = false;
        gameLogo.SetActive(false);
        gameLogo_Label.SetActive(false);
    }
    /// <summary>
    /// Presentation GameLogo
    /// </summary>
    /// <returns></returns>
    private IEnumerator Presentation_Logo()
    {
        //start delay
        yield return new WaitForSeconds(2.0f);
        //GameLogo On
        gameLogo.SetActive(true);
        SoundManager.Inst.Ds_EffectPlayerDB(7);
        yield return new WaitForSeconds(0.3f);

        float playTime = 1.0f;
        float time = 0.0f;
        float alpha = 1.0f;

        while (time <= playTime)
        {
            time += Time.deltaTime;
            alpha = Mathf.Lerp(1.0f, 0.0f, time / playTime);
            gameLogo_Effect.color = new Color(1.0f, 1.0f, 1.0f, alpha);
            yield return null;
        }

        gameLogo_Label.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        mainScene.GetComponent<BoxCollider>().enabled = true;
    }
    //로그인 버튼 - 게임 데이터를 확인하여 어디부터 시작할지 정함
    public void Button_LogIn()
    {
#if UNITY_EDITOR
        Debug.Log("LogIn");
        Debug.Log(GameManager.Inst.CheckingPlayData());
#endif
        gameLogo_Label.SetActive(false);
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        StartCoroutine(_camera.GetComponent<ScreenTransitions>().Fade(2.0f, true));

        switch (GameManager.Inst.CheckingPlayData())
        {
            case 0: //최초 실행시
                //구글 로그인
                StartCoroutine(FirstPlay());
                break;
            case 1: //플레이중인 데이터가 없을 시
                StartCoroutine(CharacterSelect());
                break;
            case 2: //플레이중인 데이터가 있을 시
                StartCoroutine(GotoLobby());
                break;
            default:
#if UNITY_EDITOR
                Debug.LogError("Play Data is Boom");
#endif
                break;
        }
    }
    //게임 처음할때
    private IEnumerator FirstPlay()
    {
        yield return new WaitForSeconds(2.0f);
        mainScene.SetActive(false);
        StartCoroutine(_camera.GetComponent<ScreenTransitions>().Fade(1.0f, false));
        nickNameScene.SetActive(true);
    }
    //처음은 아닌데 게임 플레이 데이터가 없을때
    private IEnumerator CharacterSelect()
    {
        yield return new WaitForSeconds(2.0f);
        mainScene.SetActive(false);
        StartCoroutine(_camera.GetComponent<ScreenTransitions>().Fade(1.0f, false));
        selectScene.SetActive(true);
    }
    //게임 플레이 데이터가 있을때
    private IEnumerator GotoLobby()
    {
        yield return new WaitForSeconds(2.0f);
        mainScene.SetActive(false);
        StartCoroutine(_camera.GetComponent<ScreenTransitions>().Fade(1.0f, false));
        GameManager.Inst.Loading(false);
    }
    #endregion

    #region NickName

    //닉네임 설정후 엔터 눌르면 데이터 확정
    public void NickNameInputSubmit()
    {
#if UNITY_EDITOR
        Debug.Log("NickName Submit");
#endif
        UIInput input = nickNameScene.transform.Find("NickNameSettingImage").Find("Input").GetComponent<UIInput>();
        nickName = input.label.text;
    }
    public void NickNameInputChange()
    {
        UIInput input = nickNameScene.transform.Find("NickNameSettingImage").Find("Input").GetComponent<UIInput>();
        Debug.Log(input.label.text.Length);
    }
    //닉네임이 올바른지 확인
    public void Button_NickNameConfirm()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        NickNameInputSubmit();
#if UNITY_EDITOR
        Debug.Log("NickName Confirm");
#endif
        string temp = string.Empty;

        if (nickName.Length < 2)
        {
#if UNITY_EDITOR
            Debug.Log("NickName Failed");
#endif
            nickNameScene.transform.Find("NickNameSettingImage").Find("Failed").gameObject.SetActive(true);
            //효과음 - 경고음 재생
            return;
        }

        foreach (char c in nickName)
        {
            if ('a' <= c && c <= 'z') temp += c;
            else if ('A' <= c && c <= 'Z') temp += c;
            else if ('0' <= c && c <= '9') temp += c;
            else if (0xAC00 <= c && c <= 0xD7A3) temp += c;
            else
            {
#if UNITY_EDITOR
                Debug.Log("NickName Failed");
#endif
                nickNameScene.transform.Find("NickNameSettingImage").Find("Failed").gameObject.SetActive(true);
                //효과음 - 경고음 재생
                return;
            }
        }

        StartCoroutine(_camera.GetComponent<ScreenTransitions>().Fade(2.0f, true));
        StartCoroutine(NickNameSucceed());
#if UNITY_EDITOR
        Debug.Log(temp);
        Debug.Log("NickName Succeed");
#endif
    }

    private IEnumerator NickNameSucceed()
    {
        yield return new WaitForSeconds(2.0f);
        nickNameScene.SetActive(false);
        StartCoroutine(_camera.GetComponent<ScreenTransitions>().Fade(1.0f, false));
        selectScene.SetActive(true);
    }
    public void Button_Close(GameObject gameObject)
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        gameObject.SetActive(false);
    }

    #endregion

    #region Character & Weapon Select

    public void Button_Select(GameObject gameObject)
    {
        //효과음 재생
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        string anim = string.Empty;
        //버튼에 따른 데이터 세팅
        switch (gameObject.name)
        {
            case "Male":
                sex = SEX.Male;
                Item_Class = CLASS.검;
                animSex = "Male";
                animWeapon = "ShortRange";
                break;
            case "Female":
                sex = SEX.Female;
                Item_Class = CLASS.검;
                animSex = "Female";
                animWeapon = "ShortRange";
                break;
            case "Sword":
                Item_Class = CLASS.검;
                animWeapon = "ShortRange";
                break;
            case "Wand":
                Item_Class = CLASS.지팡이;
                animWeapon = "LongRange";
                break;
        }
        anim += animSex + "_DefaultCloth_" + animWeapon + "_Idel_Front";

        if (!sex.Equals(SEX.None))
        {
            //애니메이션 업데이트
            animWindow.GetComponent<UITexture>().mainTexture = animWindow_Activate;
            if (sex.Equals(SEX.Female))
            {
                playerBody.Play(anim);
                playerArm.Play(anim + "_Arm");
                playerWeapon.Play(anim + "_Weapon");
                playerBody.gameObject.SetActive(true);
                
                //다음 버튼 활성화
                nextbutton.GetComponent<UITexture>().mainTexture = nextButton_Activate;
                nextbutton.GetComponent<BoxCollider>().enabled = true;
            }
            else if (sex.Equals(SEX.Male))
            {
                playerBody.gameObject.SetActive(false);

                //다음 버튼 비활성화
                nextbutton.GetComponent<UITexture>().mainTexture = nextButton_DeActivate;
                nextbutton.GetComponent<BoxCollider>().enabled = false;
            }
        }
        //버튼 텍스쳐 초기화
        foreach (GameObject obj in selectButton)
        {
            obj.GetComponent<UITexture>().mainTexture = button_DeActivate;
        }

        //성별 버튼 선택
        if (sex.Equals(SEX.Male))
        {
            selectButton[0].GetComponent<UITexture>().mainTexture = button_Activate;
        }
        else if (sex.Equals(SEX.Female))
        {
            selectButton[1].GetComponent<UITexture>().mainTexture = button_Activate;
        }
        //무기 버튼 선택
        if (Item_Class.Equals(CLASS.검))
        {
            selectButton[2].GetComponent<UITexture>().mainTexture = button_Activate;
        }
        else if (Item_Class.Equals(CLASS.지팡이))
        {
            selectButton[3].GetComponent<UITexture>().mainTexture = button_Activate;
        }
    }
    //selectscene 초기화
    private void Init_SelectScene()
    {
        sex = SEX.None;
        Item_Class = CLASS.갑옷;
        foreach (GameObject obj in selectButton)
        {
            obj.GetComponent<UITexture>().mainTexture = button_DeActivate;
        }
        animWindow.GetComponent<UITexture>().mainTexture = animWindow_DeActivate;
        nextbutton.GetComponent<UITexture>().mainTexture = nextButton_DeActivate;
        nextbutton.GetComponent<BoxCollider>().enabled = true;
        playerBody.gameObject.SetActive(false);
    }
    //select scene의 뒤로가기 버튼
    public void Button_Back()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        selectScene.SetActive(false);
        //로고 이벤트 초기화
        Init_Logo();
        mainScene.SetActive(true);
        //로고 이벤트 재생
        StartCoroutine(Presentation_Logo());
        //선택된 데이터 초기화
        Init_SelectScene();
    }

    //
    public void Button_Confirm()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        StartCoroutine(_camera.GetComponent<ScreenTransitions>().Fade(2.0f, true));
        StartCoroutine(SelectConfirm());
    }

    private IEnumerator SelectConfirm()
    {
        yield return new WaitForSeconds(2.0f);
        SavePlayerData();
        selectScene.SetActive(false);
#if UNITY_EDITOR
        Debug.Log(sex.ToString());
#endif
        CartoonController cartoonController = GameManager.Inst.CartoonPlay("Start");
        StartCoroutine(_camera.GetComponent<ScreenTransitions>().Fade(1.0f, false));

        while (!cartoonController.isCartoonEnd)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1.0f);
        GameManager.Inst.Loading(false);
    }

    private void SavePlayerData()
    {
        GameManager.Inst.Sex = sex;
        GameManager.Inst.PlayData.nickName = nickName;
        GameManager.Inst.GivePlayerBasicItem(Item_Class);
    }

    #endregion
}
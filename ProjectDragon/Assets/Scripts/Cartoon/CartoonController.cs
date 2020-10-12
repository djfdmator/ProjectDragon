
// ==============================================================
// Cartoon Controller
// Automatic Cartoon View
// 
// 2020-01-31: Add Button Interact Sound
//
//  AUTHOR: Kim Dong Ha
// CREATED: 2020-01-09
// UPDATED: 2020-01-31
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartoonController : MonoBehaviour
{
    public string cartoonName = string.Empty;
    [HideInInspector]
    public bool isCartoonEnd = false;
    public float cameraTranslateTime = 1.0f;
    private int cutCount;

    private CartoonData cartoonData = null;
    private Camera uiCamera;
    public GameObject nextButton;
    public GameObject prevButton;
    public GameObject skipButton;
    public GameObject skipDialog;
    private GameObject[] cuts;

    private bool isTranslateCamera = false;
    private int currentCut = 0;
    private float screenX, screenY;

    //만화 프리팹 로드
    public void CartoonLoad()
    {
        //불러올 만화의 이름이 설정되어 있지 않으면 안불러옴
        if(cartoonName.Equals(string.Empty))
        {
#if UNITY_EDITOR
            Debug.Log("cartoonName is null");
#endif      
            gameObject.SetActive(false);
            return;
        }

        if(Resources.Load("Cartoon/" + GameManager.Inst.Sex.ToString() + "/" + cartoonName) == null)
        {
#if UNITY_EDITOR
            Debug.Log("Cartoon prefeb is null");
#endif
            isCartoonEnd = true;
            gameObject.SetActive(false);

            return;
        }
        //이름에 맞는 만화 컷 프리팹 로드
        GameObject cuts = Instantiate(Resources.Load("Cartoon/" + GameManager.Inst.Sex.ToString() + "/" + cartoonName), gameObject.transform) as GameObject;
        cartoonData = cuts.GetComponent<CartoonData>();
    }

    private void OnEnable()
    {
#if UNITY_EDITOR
        Debug.Log(gameObject.GetComponentsInChildren<Transform>().Length);
#endif
        //해당 오브젝트 하위로 3개의 오브젝트만 있다면 만화가 없다는 것으로 판단
        if (cartoonData == null)
        {
            //만화 로드
            CartoonLoad();
            //로드시 예외에 의해 카툰컨트롤러가 꺼지면 작동 중지
            if (gameObject.activeSelf.Equals(false)) return;
        }

        skipDialog.SetActive(false);
        uiCamera = GameObject.FindGameObjectWithTag("ScreenTransitions").GetComponent<Camera>();
        cuts = cartoonData.cuts;
        cutCount = cartoonData.cutCount;

        screenX = GetComponent<UIWidget>().localSize.x;
        screenY = GetComponent<UIWidget>().localSize.y;
#if UNITY_EDITOR
        Debug.Log(screenX);
        Debug.Log(screenY);
#endif  
        //nextButton.GetComponent<UITexture>().SetAnchor(uiCamera.transform);
        //prevButton.GetComponent<UITexture>().SetAnchor(uiCamera.transform);
        nextButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        prevButton.transform.localPosition = cuts[currentCut].transform.localPosition - new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        skipButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 150.0f, screenY / 2 - 90.0f, 0.0f);
        prevButton.SetActive(false);
        uiCamera.transform.position = cuts[currentCut].transform.position;
    }

    private void OnDisable()
    {
        if (cartoonData != null) Destroy(cartoonData.gameObject);
    }

    public void ResizeButtonPostion()
    {
        nextButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        prevButton.transform.localPosition = cuts[currentCut].transform.localPosition - new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        skipButton.transform.localPosition = cuts[currentCut].transform.localPosition + new Vector3(screenX / 2 - 150.0f, screenY / 2 - 90.0f, 0.0f);
    }
    public void ResizeButtonPostion2()
    {
        nextButton.transform.localPosition = uiCamera.transform.localPosition + new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        prevButton.transform.localPosition = uiCamera.transform.localPosition - new Vector3(screenX / 2 - 100.0f, 0.0f, 0.0f);
        skipButton.transform.localPosition = uiCamera.transform.localPosition + new Vector3(screenX / 2 - 150.0f, screenY / 2 - 90.0f, 0.0f);
    }

    //다음 장면 버튼
    public void Button_NextCut()
    {
        //sfx
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        //카메라가 움직이고 있다면 작동 중지
        if (isTranslateCamera)
        {
            return;
        }

        currentCut++;
        //다음 컷이 컷의 갯수보다 크면 카툰 엔딩 작으면 다음 컷으로 카메라 무빙
        if(cutCount <= currentCut)
        {
            currentCut = cutCount - 1;
            //카툰엔딩
            StartCoroutine(CartoonEnding());
        }
        else
        {
            //카툰 마지막 장면에서 스킵 없애기
            //if(cutCount - 1 == currentCut)
            //{
            //    skipButton.SetActive(false);
            //}
            //카메라 이동
            StartCoroutine(TranslateCamera());
        }

        //버튼들의 위치 조정
       // ResizeButtonPostion();

        //이전 버튼 활성화
        if (0 != currentCut && prevButton.activeSelf.Equals(false))
        {
            prevButton.SetActive(true);
        }
    }

    //스킵 버튼
    public void Button_Skip()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        //바로 엔딩으로 넘어감
        skipDialog.SetActive(true);
    }

    public void Button_SkipDialogConfirm()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        StartCoroutine(CartoonEnding());
    }

    public void Button_SkipDialogClose()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        skipDialog.SetActive(false);
    }

    //카툰 엔딩 연출
    private IEnumerator CartoonEnding()
    {
        //화면 페이드인
        StartCoroutine(uiCamera.GetComponent<ScreenTransitions>().Fade(1.0f, true));
        yield return new WaitForSeconds(1.0f);
        isCartoonEnd = true;
        gameObject.SetActive(false);
    }

    //이전 버튼
    public void Button_PreCut()
    {
        SoundManager.Inst.Ds_EffectPlayerDB(1);
        //카메라 무빙중이면 작동 중지
        if (isTranslateCamera)
        {
            return;
        }

        currentCut--;
        //이전 컷이 처음 컷이면 이전 버튼 끄기
        if (0 >= currentCut)
        {
            currentCut = 0;
            prevButton.SetActive(false);
        }
        else if(cutCount > currentCut)
        {
            skipButton.SetActive(true);
        }

        StartCoroutine(TranslateCamera());
        //ResizeButtonPostion();
    }

    //카메라 무빙 연출
    private IEnumerator TranslateCamera()
    {
        isTranslateCamera = true;
        float time = 0.0f;
        Vector3 pos;
        while (time <= cameraTranslateTime)
        {
            time += Time.deltaTime;
            pos = Vector3.Lerp(uiCamera.transform.position, cuts[currentCut].transform.position, time * (1 / cameraTranslateTime));
            uiCamera.transform.position = pos;
            ResizeButtonPostion2();
            yield return null;
        }
        isTranslateCamera = false;
    }

    //카메라 스케일링 연출
    private IEnumerator CameraScaling(float _size)
    {
        float time = 0.0f;
        float size = 0.0f;
        while (time <= cameraTranslateTime)
        {
            time += Time.deltaTime;
            size = Mathf.Lerp(uiCamera.orthographicSize, _size, time * (1.0f / cameraTranslateTime));
            uiCamera.orthographicSize = size;
            yield return new WaitForFixedUpdate();
        }
    }
}

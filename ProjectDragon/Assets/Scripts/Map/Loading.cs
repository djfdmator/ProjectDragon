
// ==============================================================
// Async Loader
// Load the map asynchronously.
//
// 2019-12-26: Add Loading Progress Bar
// 2020-02-21: revise loading representation
//
//  AUTHOR: Kim Dong Ha
// CREATED:
// UPDATED: 2020-02-21
// ==============================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [SerializeField]
    private string sceneName = string.Empty;
    AsyncOperation asyncOperation;

    //프로그레스 바
    private UILabel loadingLabel;

    //로딩 프로그레스바 연출
    private GameObject loadingObj;
    public int imageCount = 0;

    //지역 이동 연출 관련
    private GameObject regionObj;
    private Transform player;
    public int[] regionPointIndex = new int[6];
    public Transform[] points;
    private Transform extraPoint;

    //페이드 아웃
    private ScreenTransitions screenTransitions;

    //플레이어 애니메이션
    public Sprite[] playerAnim;
    public bool isOne;
    int animCount = 0;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        loadingObj = transform.Find("Loading").gameObject;
        //setting loading background
        if (imageCount != 0)
        {
            int rand = Random.Range(0, imageCount);
            loadingObj.GetComponent<UISprite>().spriteName = "bg" + (rand + 1);
        }
        //find loading bar obj
        loadingLabel = loadingObj.transform.Find("ProgressBar").GetComponent<UILabel>();

        //find uiroot camera
        screenTransitions = GameObject.FindGameObjectWithTag("ScreenTransitions").GetComponent<ScreenTransitions>();

        //find region representation
        regionObj = transform.Find("Region").gameObject;

    }

    public void LoadSceneAsync(bool isBattle)
    {
        UIRoot uIRoot = GameObject.Find("UI Root").GetComponent<UIRoot>();
        uIRoot.manualHeight = 1080;
        uIRoot.manualWidth = 1920;
        //init
        if (isBattle)
        {
            sceneName = "Map_Generator";
            //increase stage
            Debug.Log("Loding : " + GameManager.Inst.CurrentStage);
            GameManager.Inst.CurrentStage++;
            BattleRepresentationInit();

            regionObj.GetComponent<UISprite>().enabled = false;
            loadingObj.SetActive(false);
        }
        else
        {
            sceneName = "Lobby";

            regionObj.SetActive(false);
        }  

        //fade out
        StartCoroutine(screenTransitions.Fade(0.5f, false));
        //asyncload
        asyncOperation = SceneManager.LoadSceneAsync(sceneName);
        asyncOperation.allowSceneActivation = false;

        //representation
        if (isBattle)
        {
            StartCoroutine(RegionRepresentation());
        }
        else
        {
            StartCoroutine(ProgressBarRepresentation());
        }
    }

    //battle 로딩시 나오는 연출 초기화 - 지역 넘어가기
    void BattleRepresentationInit()
    {
        screenTransitions.GetComponent<Camera>().orthographicSize = 1.0f;
        float ypos;
        switch(GameManager.Inst.CurrentStage)
        {
            case 1:
                ypos = 650.0f;
                break;
            case 2:
                ypos = 500.0f;
                break;
            case 3:
                ypos = 170.0f;
                break;
            case 4:
                ypos = -200.0f;
                break;
            case 5:
                ypos = -650.0f;
                break;
            default:
                ypos = -650.0f;
                break;
        }
        Transform backGround = regionObj.transform.Find("Background");
        backGround.localPosition = new Vector3(backGround.localPosition.x, ypos, backGround.localPosition.z);
        player = regionObj.transform.Find("Player");

        int a = regionPointIndex[GameManager.Inst.CurrentStage - 1];
        int b = regionPointIndex[GameManager.Inst.CurrentStage];
        int temp = b - a + 1;
        extraPoint = backGround.Find("Node").Find("extraPoint");
        points = new Transform[temp];
        //get all move points transform
        for (int i = 0; i < temp; i++)
        {
            points[i] = backGround.Find("Node").Find((a + i).ToString()).transform;
        }

        //changed clear stage node sprite
        for (int i = 1; i < GameManager.Inst.CurrentStage; i++)
        {
            backGround.Find("Node").Find(regionPointIndex[i].ToString()).GetComponent<UISprite>().spriteName = "Node_Clear";
        }

        if(GameManager.Inst.CurrentStage >= 4)
        {
            backGround.Find("Node").Find("9").localPosition = extraPoint.localPosition;
        }
    }

    //지역 넘어가는 연출
    IEnumerator RegionRepresentation()
    {
        //처음 시작
        isOne = true;
        player.GetComponent<SpriteRenderer>().sprite = playerAnim[0];
        player.transform.position = points[0].position;
        yield return new WaitForSeconds(1.0f);
        player.GetComponent<SpriteRenderer>().sprite = playerAnim[1];

        //representation
        if (GameManager.Inst.CurrentStage == 3)
        {
            //stage 3 representation
            for (int i = 1; i < 3; i++)
            {
                yield return StartCoroutine(Translate(player, points[i], 2.0f));
            }
            player.gameObject.SetActive(false);

            for (int i = 3; i < 5; i++)
            {
                yield return StartCoroutine(Translate(points[2], points[i], 2.0f));
            }
            yield return StartCoroutine(Translate(points[2], extraPoint, 2.0f));

            player.position = points[5].position;
            player.gameObject.SetActive(true);
        }
        else
        {
            //non stage 3 representation
            for (int i = 1; i < points.Length; i++)
            {
                yield return StartCoroutine(Translate(player, points[i], 2.0f));
            }
        }

        //arrive and stay
        yield return new WaitForSeconds(1.0f);
        player.GetComponent<SpriteRenderer>().sprite = playerAnim[3];

        //checked asyncload
        while (!asyncOperation.isDone)
        {
            yield return null;

            if (asyncOperation.progress >= 0.9f)
            {
                break;
            }
        }
        yield return new WaitForSeconds(1.0f);

        StartCoroutine(NextScene());
    }

    //Translate the "from" Object
    IEnumerator Translate(Transform from, Transform to, float playTime)
    {
        float time = 0;
        //set player move forward
        if (player.localScale.x > 0)
        {
            player.localScale = (to.position - from.position).x < 0.0f ? player.localScale : new Vector3(-player.localScale.x, player.localScale.y, player.localScale.z);
        }
        else
        {
            player.localScale = (to.position - from.position).x > 0.0f ? player.localScale : new Vector3(-player.localScale.x, player.localScale.y, player.localScale.z);
        }
        Vector3 trans = from.position;

        while (playTime >= time)
        {
            time += Time.deltaTime;
            //move
            from.position = Vector3.Lerp(trans, to.position, time / playTime);

            //player animation
            animCount++;
            if(animCount == 30)
            {
                if (isOne)
                {
                    player.GetComponent<SpriteRenderer>().sprite = playerAnim[2];
                    isOne = false; 
                }
                else
                {
                    player.GetComponent<SpriteRenderer>().sprite = playerAnim[1];
                    isOne = true;
                }
                animCount = 0;
            }
            yield return null;
        }
    }

    //AsyncLoad - progress bar, basic loading representation
    IEnumerator ProgressBarRepresentation()
    {
        yield return null;

        //init
        float percent = 0.0f;
        ProgressBar(percent);

        //check load complete
        while (!asyncOperation.isDone)
        {
            percent = asyncOperation.progress;
            ProgressBar(percent);
            yield return null;

            if (percent >= 0.9f)
            {
                ProgressBar(percent);
                yield return new WaitForSeconds(1.0f);
                //Load Complete
                ProgressBar(1.0f);
                break;
            }
        }

        //next scene
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(NextScene());
    }

    //apply progress data to progress representation
    void ProgressBar(float _percent)
    {
        _percent *= 100.0f;
        loadingLabel.text = "Loading..." + _percent + "%";
    }

    //goto next scene
    IEnumerator NextScene()
    {
        StartCoroutine(screenTransitions.Fade(1.0f, true));
        yield return new WaitForSeconds(1.1f);
        asyncOperation.allowSceneActivation = true;
    }
}

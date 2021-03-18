/////////////////////////////////////////////////
/////////////MADE BY Yang SeEun/////////////////
/////////////////2019-12-16////////////////////
//////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{

    public string poolItemName = "DamagePopupObj";

    [SerializeField] float moveYSpeed = 2.0f;
    [SerializeField] float disappearSpeed = 7.0f;
    [SerializeField] float disappearTimer = 0.3f;

    float Origin_fontSize;
    Color textColor;
    TextMeshPro textMesh;
    DamagePopup damagePopup;

    Transform _parent;

    void Initialize()
    {
        moveYSpeed = moveYSpeed == 0 ? 1.0f : moveYSpeed;
        disappearSpeed = disappearSpeed == 0 ? 10.0f : disappearSpeed;
        disappearTimer =disappearTimer == 0 ? 1.0f : disappearTimer;
        textColor.a = 1.0f;
        textMesh.color = Color.white ;
    }

    public DamagePopup Create(Vector3 position, int damageAmount, bool isCriticalHit,bool isMiss, Transform parent = null)
    {
        //damageObject.transform.position = position;
        //damagePopupTransform = Instantiate(GameAssets.i.pfDamagePopup, position, Quaternion.identity);
        //damagePopup = damagePopupTransform.GetComponent<DamagePopup>();

        // 만약 플레이어, 회피 bool = true/ 회피라는걸 띄워줌..../ 치명타일 경우 치명을 띄워줌.
        // if isCriticalHit = true DamagedPopUp
        // 이미지를 하나. 글자!
        
        _parent = parent;
        GameObject damageObject = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        damagePopup = damageObject.transform.GetComponent<DamagePopup>();
        damagePopup.Initialize();
        damagePopup.transform.position = position;
        damagePopup.Setup(damageAmount,isCriticalHit,isMiss);
        //damagePopup.Setup(damageAmount,isCriticalHit);
        damageObject.SetActive(true);

        return damagePopup;
    }

    private void Awake()
    {
        textMesh = transform.GetComponent<TextMeshPro>();
        Origin_fontSize = textMesh.fontSize;
    }
    public void Setup(int damageAmount, bool isCriticalHit, bool isMiss)
    {
        textMesh.fontSize =Origin_fontSize;
        
        if(isMiss)
        {
            textMesh.SetText("miss");

                //회피
                VertexGradient V= new VertexGradient();
                V= textMesh.colorGradient;
                V.topLeft = Color.white;
                V.topRight = Color.white;
                V.bottomRight =new Color32(130,130,130,255);
                V.bottomLeft = new Color32(130,130,130,255);
                textMesh.colorGradient = V;
                //invaid Hit
                textMesh.outlineColor = Color.black;
                textMesh.fontSize +=1;
                textColor = Color.white;
        }
        else
        {
            textMesh.SetText(damageAmount.ToString());

            if(isCriticalHit)
            {
                //크리티컬
                 //Ciritical Hit
               VertexGradient V= new VertexGradient();
                V= textMesh.colorGradient;
                V.topRight = new Color32(255,201,217,255);
                V.topLeft = new Color32(255,201,217,255);
                V.bottomLeft = new Color32(255,0,104,255);
                V.bottomRight = new Color32(255,0,104,255);
                textMesh.colorGradient = V;
                //invaid Hit
                textMesh.outlineColor = Color.black;
                textMesh.fontSize +=3;
                textColor = Color.white;
            }
            else
            {
                    //Normal Hit
                VertexGradient V = new VertexGradient();
                V = textMesh.colorGradient;
                V.topLeft =new Color32(255,191,126,255);
                V.topRight = new Color32(255,191,126,255);
                V.bottomRight =new Color32(255,72,0,255);
                V.bottomLeft = new Color32(255,72,0,255);
                textMesh.colorGradient = V;
                //invaid Hit
                textMesh.outlineColor =Color.black;
                textMesh.fontSize = Origin_fontSize;
                textColor =Color.white;
            }
        }
        textMesh.color = textColor;
    }
    public void Setup(int damageAmount,bool isCriticalHit)
    {
        textMesh.SetText(damageAmount.ToString());
        if (!isCriticalHit)
        {
            //Normal Hit
            textMesh.fontSize = textMesh.fontSize;
            textColor = textMesh.color;  //예시
        }
        else
        {
            //Ciritical Hit
            textMesh.fontSize += 3;
            textColor = textMesh.color;  //예시
        }
        textMesh.color = textColor;

    }

    //float increaseScaleAmount = 1f;
    //float decreaseScaleAmount = 1f;

    private void Update()
    {
        //Y axis move
        transform.position += new Vector3(0, moveYSpeed) * Time.deltaTime;

        disappearTimer -= Time.deltaTime;

        //if (disappearTimer > disappearTimer * 0.5f)
        //{
        //    transform.localScale += Vector3.Lerp();
        //}


        //Fade Out
        if (disappearTimer < 0)
        {
            textColor.a -= disappearSpeed * Time.deltaTime;
            textMesh.color = textColor;
            if (textColor.a < 0)
            {
                //Object
                ObjectPool.Instance.PushToPool(poolItemName, gameObject, _parent);
                //Destroy(gameObject);
            }
        }
    }


}

//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/ Made: void DamageShake////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    public float lerpSpeed = 1.0f;

    public Vector3 origin_Pos;

    public bool isShake = false;

    public bool following = true;
    private bool west = false;
    private bool east = false;
    private bool north = false;
    private bool south = false;

    private float posX;
    private float posY;

    void LateUpdate()
    {
        if (following)
        {
            bool isdoor = false;
            float prePosX = posX;
            float prePosY = posY;
            posX = player.localPosition.x;
            posY = player.localPosition.y;

            if (isShake == false)
            {
                if ((new Vector2(prePosX, prePosY) - new Vector2(posX, posY)).magnitude > 2.0f)
                {
                    isdoor = true;
                }

                if (west == true && (prePosX - posX) > 0.0f)
                {
                    posX = prePosX;
                }
                if (east == true && (prePosX - posX) < 0.0f)
                {
                    posX = prePosX;
                }
                if (north == true && (prePosY - posY) < 0.0f)
                {
                    posY = prePosY;
                }
                if (south == true && (prePosY - posY) > 0.0f)
                {
                    posY = prePosY;
                }

                if (isdoor)
                {
                    transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(posX, posY, transform.localPosition.z), lerpSpeed);

                }
                else transform.localPosition = new Vector3(posX, posY, transform.localPosition.z);

            }
            if (isShake == true)
            {
                Debug.Log("됐서");
            }
            origin_Pos = this.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "west")
        {
            west = true;
        }
        if (collision.tag == "east")
        {
            east = true;
        }
        if (collision.tag == "north")
        {
            north = true;
        }
        if (collision.tag == "south")
        {
            south = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "west")
        {
            west = false;
        }
        if (collision.tag == "east")
        {
            east = false;
        }
        if (collision.tag == "north")
        {
            north = false;
        }
        if (collision.tag == "south")
        {
            south = false;
        }
    }

    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {

            float xRand = Random.RandomRange(-_amount,_amount);
            float yRand = Random.RandomRange(-_amount, _amount);
            transform.localPosition = new Vector3(origin_Pos.x+xRand, origin_Pos.y+yRand, origin_Pos.z);
           //transform.localPosition = (Vector3)Random.insideUnitCircle * _amount + origin_Pos;
           //transform.localPosition = new Vector3(((Random.Range(0.1f,1.0f)*_amount) + origin_Pos.x), (origin_Pos.y), (origin_Pos.z));
            timer += Time.deltaTime;
            yield return null;
            transform.localPosition = origin_Pos;
        }

        transform.localPosition = origin_Pos;
        isShake = false;
    } public void BossFollow(GameObject _boss)
    {
        StartCoroutine(BossCheck(_boss));
    }
    public IEnumerator BossCheck(GameObject _boss)
    {
        following = false;
        float time = 0;
        Vector3 cameraposition=gameObject.transform.position;
        Vector3 bossposition = _boss.transform.position;
        bossposition.z -= 10;
        while (time < 1.0f)
        {
            if (time < 1)
            {
                time += Time.deltaTime;
                gameObject.transform.position=Vector3.Lerp(cameraposition, bossposition, time);
                yield return null;
            }
        }
        time = 0;
        while(time<3.0f)
        {
            time += Time.deltaTime;
            yield return null;
        }
        time = 0;
        Vector3 playerposition = player.position;
        playerposition.z -= 10;
        while (time<1.0f)
        {
            gameObject.transform.position = Vector3.Lerp(bossposition, playerposition, time);
            time += Time.deltaTime;
            yield return null;
        }
        following = true;
        yield return null;
        //방에 입장
        //보스에게 카메라 이동
        //보스 포효
        //포효 종료
        //플레이어 에게 카메라 이동
    }
    public void BoolChanger()
    {
        isShake = true;
        StartCoroutine(Shake(0.2f, 0.1f));
    }
}

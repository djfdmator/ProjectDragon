//////////////////////////////////////////////////////////MADE BY Lee Sang Jun///2019-12-13/////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

#pragma warning disable 0168

[AddComponentMenu("NGUI/Interaction/Drag Object")]
public class JoyPad : MonoBehaviour
{
    [HideInInspector]
    public float temp_angle;
    public float angle;
    public Player player;


    public Transform target;
    public Transform target2;
    public bool centerOnPress = true; //중앙이 클릭 되었을 때 이걸 이동시킬지 말지에 대해서
    public bool Pressed;
    Vector3 userInitTouchPos;


    //디버그용
    Touch touch01;
    Ray ray01;
    Vector3 fingerPoint01;
    RaycastHit2D whereIs_Hit;
    // Start is called before the first frame update

    //Joystick vars
    public int tapCount;
    public bool normalize = false;                          // Normalize output after the dead-zone?
    public Vector2 position;                                // [-1, 1] in x,y
    public float deadZone = 2f;                             // Control when position is output
    public float fadeOutAlpha = 0.2f;
    public float fadeOutDelay = 1f;
    public UIWidget widget;
    public Vector3 scale = Vector3.one;
    public float radius = 40f;

    void Awake()
    {
        userInitTouchPos = Vector3.zero;
        widget = gameObject.GetComponent<UIWidget>();
        target = gameObject.transform.GetChild(0);
        target2 = gameObject.transform.GetChild(0).GetChild(0);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    void Start()
    {
        target.GetComponent<UISprite>().enabled = false;
        target2.GetComponent<UISprite>().enabled = false;
    }
    IEnumerator fadeOutJoystick()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        for (int i = 0; i < 100; i++)
        {
            Color lastColor = widget.color;
            Color newColor = lastColor;
            newColor.a = fadeOutAlpha;
            //  TweenColor.Begin(, 0.5f, newColor).method = UITweener.Method.EaseOut;
        }
    }
    IEnumerator fadeInJoystick()
    {
        yield return new WaitForSeconds(fadeOutDelay);
        for (int i = 0; i < 100; i++)
        {
            Color lastColor = widget.color;
            Color newColor = lastColor;
            newColor.a += fadeOutAlpha;
            //  TweenColor.Begin(widget.gameObject, 0.5f, newColor).method = UITweener.Method.EaseOut;
        }
    }
    IEnumerator fadeJoyStick()
    {
        yield return new WaitForSeconds(0.3f);
        if(Pressed==false)
        {
            target2.GetComponent<UISprite>().spriteName = "ingameui_43";
            yield return new WaitForSeconds(0.2f);
            target.GetComponent<UISprite>().enabled = false;
            target2.GetComponent<UISprite>().enabled = false;
        }

    }
    public void OnPress(bool pressed)
    {
        if (pressed == false&&!player.isSkillActive)
        {
            player.CurrentState = State.Idel;
            StartCoroutine("fadeJoyStick");
        }
        if (pressed.Equals(true))
        {
            target.GetComponent<UISprite>().enabled = true;
            target2.GetComponent<UISprite>().enabled = true;
            target2.GetComponent<UISprite>().depth = target.GetComponent<UISprite>().depth + 1;
            target2.GetComponent<UISprite>().spriteName = "ingameui_40";
            if (Input.touchCount == 1)
            {
                touch01 = Input.GetTouch(0);
                fingerPoint01 = UICamera.currentCamera.ScreenToWorldPoint(touch01.position);
                whereIs_Hit = Physics2D.Raycast(fingerPoint01, transform.forward, 1);
                Ray ray = UICamera.currentCamera.ScreenPointToRay(touch01.position);
                for (int i = 0; i < Input.touchCount; i++)
                {
                    RaycastHit[] raycastHits = Physics.RaycastAll(ray, 10);
                    foreach (RaycastHit hit in raycastHits)
                    {
                        if (hit.collider.tag.Equals("button"))
                        {
                            ray = UICamera.currentCamera.ScreenPointToRay(touch01.position);
                            if (centerOnPress)
                            {

                                target.transform.position = fingerPoint01;
                                break;
                            }
                        }
                        if (!hit.collider.tag.Equals("button"))
                        {
                            return;
                        }
                    }
                }
                if (touch01.phase.Equals(TouchPhase.Ended))
                {
                }
            }
            if (Input.touchCount > 1)
            {
                touch01 = Input.GetTouch(0);
                fingerPoint01 = UICamera.currentCamera.ScreenToWorldPoint(touch01.position);
                whereIs_Hit = Physics2D.Raycast(fingerPoint01, transform.forward, 1);
                Ray ray = UICamera.currentCamera.ScreenPointToRay(touch01.position);
                for (int i = 0; i < Input.touchCount; i++)
                {
                    RaycastHit[] raycastHits = Physics.RaycastAll(ray, 10);
                    foreach (RaycastHit hit in raycastHits)
                    {
                        if (hit.collider.tag.Equals("button"))
                        {
                            ray = UICamera.currentCamera.ScreenPointToRay(touch01.position);
                            if (centerOnPress)
                            {
                                return;
                            }
                        }
                        if (!hit.collider.tag.Equals("button"))
                        {
                            touch01 = Input.GetTouch(1);
                            fingerPoint01 = UICamera.currentCamera.ScreenToWorldPoint(touch01.position);
                            target.transform.position = fingerPoint01;
                            Debug.Log("버튼이 아닙니다.");
                            return;
                        }
                    }
                }
            }
        }
        else
        {
            ResetJoystick();
        }
    }
    public void OnDrag(Vector2 delta)
    {
        //Debug.Log("delta " +  delta + " delta.magnitude = " + delta.magnitude);
        //Ray ray = UICamera.currentCamera.ScreenPointToRay(UICamera.lastTouchPosition);
        if (Input.touchCount > 0)
        {
            Touch touch01 = Input.GetTouch(0);
            Ray ray = UICamera.currentCamera.ScreenPointToRay(touch01.position);
            RaycastHit[] rayhited = Physics.RaycastAll(ray, 10);
            float dist = 0f;
            if (Input.touchCount > 1)
            {
                Debug.Log("일단 1보단 큼");
                Touch touch02 = Input.GetTouch(1);
                for (int i = 0; i < Input.touchCount; i++)
                {
                    foreach (RaycastHit hit in rayhited)
                    {
                        if (hit.collider.tag.Equals("button"))
                        {
                            ray = UICamera.currentCamera.ScreenPointToRay(touch01.position);
                            break;
                        }
                        else if (!hit.collider.tag.Equals("button"))
                        {
                            touch02 = Input.GetTouch(1);
                            fingerPoint01 = UICamera.currentCamera.ScreenToWorldPoint(touch02.position);
                            ray = UICamera.currentCamera.ScreenPointToRay(touch02.position);
                            break;
                        }
                    }

                }
            }
            Vector3 currentPos = ray.GetPoint(dist);
            Vector3 offset = currentPos - userInitTouchPos;

            if (offset.x != 0f || offset.y != 0f)
            {
                offset = target2.InverseTransformDirection(offset);
                offset.Scale(scale);
                offset = target2.TransformDirection(offset);
                offset.z = 0f;
            }

            target2.position = userInitTouchPos + offset;

            Vector3 zeroZpos = target2.position;
            zeroZpos.z = 0f;
            target2.position = zeroZpos;
            // Calculate the length. This involves a squareroot operation,
            // so it's slightly expensive. We re-use this length for multiple
            // things below to avoid doing the square-root more than one.

            float length = target2.localPosition.magnitude;

            if (length < deadZone)
            {
                // If the length of the vector is smaller than the deadZone radius,
                // set the position to the origin.
                position = Vector2.zero;
                target2.localPosition = position;
            }
            else if (length > radius)
            {
                target2.localPosition = Vector3.ClampMagnitude(target2.localPosition, radius);
                position = target2.localPosition;
            }
            if (player.CurrentState != State.Attack)
            {
                if (angle > 0&&!player.isSkillActive)
                {
                    player.CurrentState = State.Walk;
                }
                else if (angle == 0&&!player.isSkillActive)
                {
                    player.CurrentState = State.Idel;
                }
            }
            if (normalize)
            {
                // Normalize the vector and multiply it with the length adjusted
                // to compensate for the deadZone radius.
                // This prevents the position from snapping from zero to the deadZone radius.
                position = position / radius * Mathf.InverseLerp(radius, deadZone, 1);
            }
            if (player.current_angle > 0 && !player.AngleisAttack &&!player.isSkillActive)
            {
                player.CurrentState = State.Walk;
                angle = GetAngle(target2.transform.position, target.transform.position);
            }
        }
    }
    private void Update()
    {
        //각도 구하기
        angle = GetAngle(target2.transform.position, target.transform.position);
        if (angle == 0)
        {
            angle = temp_angle;
        }
        temp_angle = angle;
        if (Input.touchCount < 1)
        {
            Pressed = false;
        }
        else
        {
            Pressed = true;
        }

    }
    public static float GetAngle(Vector3 Start, Vector3 End)
    {
        Vector3 v = End - Start;
        return Quaternion.FromToRotation(Vector3.up, End - Start).eulerAngles.z;
    }
    void ResetJoystick()
    {
        // Release the finger control and set the joystick back to the default position
        position = Vector2.zero;
        target2.localPosition = userInitTouchPos;
        //for (int i = 0; i < 100; i++)
        //{
        //    Color lastColor = widget.color;
        //    lastColor.a = 0.1f;
        //    widget.color = lastColor;
        //}
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    enum PlayerState { None = 0 , IDEL , WALK, ATTACK, SKILLATTACK,DEAD};
    enum PlayerAnimationState { None = 0, }

    public Animation playerAnimationStateChanger;

    public float horizontalSpeed;
    public float verticalSpeed;

    //speed
    public float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float h = horizontalSpeed * Input.GetAxis("Horizontal");
        float v = verticalSpeed * Input.GetAxis("Vertical");
        transform.Rotate(v, h, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinSpriteDepth : MonoBehaviour
{

    public void DepthControl()
    {
        LobbyManager.inst.SkinDepthorder();
    }
    public void DepthAnimLeft()
    {
        //LobbyManager.inst.SkinDepthLeftAnim();
    }
    public void DepthAnimRight()
    {
        //LobbyManager.inst.SkinDepthRightAnim();
    }
}

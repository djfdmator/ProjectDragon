using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decompositionpanel : MonoBehaviour
{
    Vector3 firstposition;
    private void Start()
    {
        firstposition = gameObject.transform.position;
    }
    void DecompositionpanelAnim()
    {
        LobbyManager.inst.Selecteditem.Clear();
        LobbyManager.inst.DecompositionCountLabel.GetComponent<UILabel>().text = "0";
        for (int i = 0; i < 7; i++)
        {
            LobbyManager.inst.DecompositionPanel.transform.Find(string.Format("DecompositionInfo/Itemplace{0}/ItemIcon", LobbyManager.inst.Selecteditem.Count)).GetComponent<UISprite>().spriteName = "크리스탈";
            LobbyManager.inst.DecompositionPanel.transform.Find(string.Format("DecompositionInfo/Itemplace{0}/ItemIcon", LobbyManager.inst.Selecteditem.Count)).transform.localScale = Vector3.one;
            LobbyManager.inst.DecompositionPanel.transform.Find(string.Format("DecompositionInfo/Itemplace{0}/ItemIcon", LobbyManager.inst.Selecteditem.Count)).transform.position = firstposition;
        }
    }
}

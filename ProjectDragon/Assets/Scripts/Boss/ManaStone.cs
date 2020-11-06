using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaStone : Monster
{
    public Boss_MaDongSeok boss;
    // Start is called before the first frame update
    public void ManaStoneSumon()
    {
        gameObject.SetActive(true);
        boss.transform.parent.GetComponent<Room>().AddMonster(gameObject);
        gameObject.transform.parent.GetComponent<Animator>().Play("ManaStoneIdle");
        maxHp = 30;
        gameObject.GetComponent<SpriteRenderer>().material = GetComponentInChildren<FlashWhite>().originalMaterial;
        HP = 30;
        isDead = false;
    }
    public override void Dead()
    {
        base.Dead();
        bool active = false;
        gameObject.transform.parent.GetComponent<Animator>().Play("ManaStonePieces");
        gameObject.SetActive(false);
        if (HP < 0)
        {
            boss.HPChanged(34, false, 0);
            if (boss.currentstate.Equals(BossState.Phase2))
            {
                for (int i = 0; i < 4; i++)
                {
                    Debug.Log(gameObject.transform.parent.parent.transform.Find(string.Format("ManaStonePlace{0}", i + 1)).Find("ManaStone").gameObject.activeSelf);
                    if (gameObject.transform.parent.parent.transform.Find(string.Format("ManaStonePlace{0}", i + 1)).Find("ManaStone").gameObject.activeSelf)
                    {
                        active = true;
                    }
                }
                if (!active)
                {
                    boss.Phase2TimeCheckDestory();
                }
            }
        }

    }
    public override int HPChanged(int ATK, bool isCritical, int NukBack)
    {
        IEnumerator flash = GetComponentInChildren<FlashWhite>().Flash();
        if (ATK > 0 && HP > 0)
        {
            StartCoroutine(flash);
        }
        if (HP - ATK < 0)
        {
            StopCoroutine(flash);
            gameObject.GetComponent<SpriteRenderer>().material = GetComponentInChildren<FlashWhite>().originalMaterial;
        }
        return base.HPChanged(ATK, isCritical, NukBack);
    }
    public void ISDEADTRUE()
    {
        isDead=true;
    }
}

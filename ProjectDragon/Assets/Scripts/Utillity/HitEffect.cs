using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    public string poolItemName = "HitEffect";
    private HitEffect hitEffect;
    private Animator animator;

    private readonly int hashNormalStaff = Animator.StringToHash("NormalStaff");
    private readonly int hashNormalSword = Animator.StringToHash("NormalSword");
    private readonly int hashNereides = Animator.StringToHash("Nereides");
    private readonly int hashNyx = Animator.StringToHash("Nyx");
    private readonly int hashExcalibur = Animator.StringToHash("Excalibur");


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public HitEffect Create(Vector3 position, string animString, Transform parent = null)
    {
        GameObject effectObj = ObjectPool.Instance.PopFromPool(poolItemName, parent);
        hitEffect = effectObj.transform.GetComponent<HitEffect>();
        hitEffect.gameObject.SetActive(true);
        hitEffect.transform.position = position;
        hitEffect.ResetInit();
        hitEffect.animator.SetBool(animString, true);

        return hitEffect;
    }

    private void ResetInit()
    {
        animator.SetBool(hashNormalStaff, false);
        animator.SetBool(hashNormalSword, false);
        animator.SetBool(hashNereides, false);
        animator.SetBool(hashNyx, false);
        animator.SetBool(hashExcalibur, false);
    }
}

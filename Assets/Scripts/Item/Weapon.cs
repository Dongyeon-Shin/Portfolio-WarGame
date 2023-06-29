using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private Animator wearerAnimator;
    private Collider collider;

    private void Awake()
    {
        wearerAnimator = GetComponentInParent<Animator>();
        collider = GetComponentInParent<Collider>();
        SwitchWeaponCollider(false);
    }
    public void SwitchWeaponCollider(bool enable)
    {
        collider.enabled = enable;
    }
    private void OnTriggerStay(Collider other)
    {
        SwitchWeaponCollider(false);
        IHittable hittableObject = other.GetComponent<IHittable>();
        if (hittableObject == null)
        {
            StartCoroutine(BlockedRoutine());
        }
        else
        {
            hittableObject.Hit();
        }
    }
    IEnumerator BlockedRoutine()
    {
        WaitWhile waitAnimationFinish = new WaitWhile(() => wearerAnimator.GetCurrentAnimatorStateInfo(1).normalizedTime > 0f);
        SwitchWeaponCollider(false);
        wearerAnimator.SetFloat("AttackSpeed", -0.3f);
        yield return waitAnimationFinish;
        wearerAnimator.SetTrigger("Blocked");
        wearerAnimator.SetBool("Attack", false);
        wearerAnimator.SetFloat("AttackSpeed", 1f);
    }
}

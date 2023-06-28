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
            wearerAnimator.SetFloat("AttackSpeed", -0.3f);
        }
        else
        {
            hittableObject.Hit();
        }
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private float strength;
    private Weapon equipedWeapon;
    private Animator animator;
    private bool charging;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        equipedWeapon = GetComponentInChildren<Weapon>();
    }
    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            animator.SetFloat("AttackSpeed", 1f);
            charging = true;
            StartCoroutine(AttackRoutine());
        }
        else
        {
            charging = false;
        }
    }
    private void OnParry(InputValue value)
    {
        if (value.isPressed)
        {
            charging = false;
            animator.SetBool("Parry", true);
        }
        else
        {
            animator.SetBool("Parry", false);
        }
    }
    IEnumerator AttackRoutine()
    {
        WaitUntil waitTransition = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(1).IsName("2HIdle"));

        if (charging)
        {
            yield return StartCoroutine(ChargeRoutine());
            InitializeChargeMotion();
            animator.SetBool("Attack", true);
            equipedWeapon.SwitchWeaponCollider(true);
        }     
        yield return waitTransition;
        equipedWeapon.SwitchWeaponCollider(false);
        animator.SetBool("Attack", false);
    }
    IEnumerator ChargeRoutine()
    {
        Vector3 cameraDirection;
        while (charging)
        {
            InitializeChargeMotion();
            if (Camera.main.transform.position.y - transform.position.y < 1.5f)
            {
                animator.SetBool("ChargeUp", true);
            }
            else if (Camera.main.transform.position.y - transform.position.y > 3.5f)
            {
                animator.SetBool("ChargeDown", true);
            }
            else
            {
                cameraDirection = (Camera.main.transform.position - transform.position).normalized;
                if (Vector3.Dot(Vector3.up, (Vector3.Cross(transform.forward, cameraDirection))) > 0)
                {
                    animator.SetBool("ChargeLeft", true);
                }
                else
                {
                    animator.SetBool("ChargeRight", true);
                }
            }
            yield return null;
        }
    }
    private void InitializeChargeMotion()
    {
        animator.SetBool("ChargeUp", false);
        animator.SetBool("ChargeDown", false);
        animator.SetBool("ChargeLeft", false);
        animator.SetBool("ChargeRight", false);
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [SerializeField]
    private float damage;
    private Coroutine attackRoutine;
    private Animator animator;
    private bool charging;
    private bool parrying;
    private Weapon equipedWeapon;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.SetLayerWeight(1, 0);
        equipedWeapon = GetComponentInChildren<Weapon>();
    }
    private void OnAttack(InputValue value)
    { 
        if (value.isPressed)
        {
            animator.SetLayerWeight(1, 1);
            charging = true;
            parrying = false;
            animator.SetBool("Parry", false);
            if (attackRoutine != null)
            {
                StopCoroutine(attackRoutine);
            }
            attackRoutine = StartCoroutine(AttackRoutine());
        }
        else
        {
            if (parrying)
            {
                return;
            }
            animator.SetBool("Attack", true);
            equipedWeapon.SwitchWeaponCollider(true);
            charging = false;
            InitializeChargeMotion();
        }
    }
    private void OnParry(InputValue value)
    {
        if (value.isPressed)
        {
            parrying = true;
            animator.SetLayerWeight(1, 1);
            animator.SetBool("Parry", true);
        }
        else
        {
            parrying = false;
            animator.SetLayerWeight(1, 0);
            animator.SetBool("Parry", false);
        }
    }
    IEnumerator AttackRoutine()
    {
        while (animator.GetLayerWeight(1) > 0.1f)
        {
            if (charging)
            {
                yield return StartCoroutine(ChargeRoutine());
            }
            animator.SetLayerWeight(1, Mathf.Lerp(animator.GetLayerWeight(1), 0f, Time.deltaTime * 0.5f));
            yield return null;
        }
        animator.SetLayerWeight(1, 0);     
    }
    IEnumerator ChargeRoutine()
    {
        Vector3 cameraDirection = (Camera.main.transform.position - transform.position).normalized;
        while (charging)
        {
            InitializeChargeMotion();
            if (Camera.main.transform.position.y - transform.position.y < 2f)
            {
                animator.SetBool("ChargeUp", true);
            }
            else if (Camera.main.transform.position.y - transform.position.y > 3f)
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
    public void FinishBounceMotion()
    {
        if (animator.GetFloat("AttackSpeed") < 0f)
        {
            animator.SetBool("Attack", false);
        }
    }
    public void FinishAttackMotion()
    {
        animator.SetBool("Attack", false);
    }
    public void InitializeAttackSpeed()
    {
        animator.SetFloat("AttackSpeed", 1f);        
    }
    public void DisableWeaponCollider()
    {
        equipedWeapon.SwitchWeaponCollider(false);
    }
}

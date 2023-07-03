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
    [SerializeField]
    private BoxCollider guardAreaCollider;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        equipedWeapon = GetComponentInChildren<Weapon>();
        guardAreaCollider = GetComponentInChildren<BoxCollider>();
        guardAreaCollider.enabled = false;
    }
    private void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            if (animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
            {
                return;
            }
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
            equipedWeapon.SwitchWeaponCollider(false);
            animator.SetBool("Parry", true);
            guardAreaCollider.enabled = true;
        }
        else
        {
            animator.SetBool("Parry", false);
            guardAreaCollider.enabled= false;
        }
    }
    IEnumerator AttackRoutine()
    {
        WaitUntil waitAttackMotionFinish = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(1).IsTag("RetrieveWeapon"));
        WaitUntil waitEveryMotionsFinish = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(1).IsName("2HIdle"));
        if (charging)
        {
            yield return StartCoroutine(ChargeRoutine());
            InitializeChargeMotion();
            if (animator.GetCurrentAnimatorStateInfo(1).IsTag("Attack"))
            {
                yield break;
            }
            animator.SetBool("Attack", true);
            equipedWeapon.SwitchWeaponCollider(true);
        }
        yield return waitAttackMotionFinish;
        equipedWeapon.SwitchWeaponCollider(false);
        yield return waitEveryMotionsFinish;
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

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float jumpPower;
    private CharacterController controller;
    private Vector3 moveDirection;
    private float ySpeed = 0;
    private Animator animator;
    private bool walking = true;
    private float moveSpeed;
    private bool outofControl = false;
    private bool rideOnHorseback = false;
    public bool RideOnHorseback { get { return rideOnHorseback;} }
    private Horse ridingHorse;
    private Queue<IEnumerator> commandQueue = new Queue<IEnumerator>();

    // 테스트 코드
    [SerializeField]
    Transform temp;
    private bool cameraDirectionBinding = true;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void Move()
    {
        if (rideOnHorseback)
        {
            ridingHorse.Move(moveDirection, walking);
        }
        else
        {
            if (moveDirection.magnitude == 0)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.001f);
                animator.SetFloat("MoveSpeed", moveSpeed);
                return;
            }
            Vector3 forwardVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
            Vector3 rightVector = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
            if (walking)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.05f);
            }
            else
            {
                moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.05f);
            }
            controller.Move(forwardVector * moveDirection.z * moveSpeed * Time.deltaTime);
            controller.Move(rightVector * moveDirection.x * moveSpeed * Time.deltaTime);
            animator.SetFloat("MoveSpeed", moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forwardVector * moveDirection.z + rightVector * moveDirection.x), Time.deltaTime * 5f);
        }
    }
    private void OnMove(InputValue value)
    {
        if (outofControl)
        {
            return;
        }
        moveDirection.x = value.Get<Vector2>().x;
        moveDirection.z = value.Get<Vector2>().y;
    }
    // TODO: 인터페이스 구현하는것도 생각해볼것
    IEnumerator MovetoPositonRoutine(Vector3 positon)
    {
        outofControl = true;
        Vector3 direction = (positon - transform.position).normalized;
        float speed = moveSpeed > walkSpeed ? moveSpeed : walkSpeed;
        animator.SetFloat("MoveSpeed", speed);
        float dot = Vector3.Dot(transform.forward, direction);
        while ((positon - transform.position).sqrMagnitude > 0.1f)
        {
            yield return null;
            if (1f - dot > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2.5f);
                dot = Vector3.Dot(transform.forward, direction);
            }
            controller.Move(direction * Time.deltaTime * speed);
        }
        moveDirection.x = 0;
        moveDirection.z = 0;
        outofControl = false;
        yield return null;
    }
    public void Fall()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (rideOnHorseback)
        {
            ridingHorse.Fall(ySpeed);
        }
        else
        {
            if (controller.isGrounded && ySpeed < 0) // TODO: 커스텀 isGrounded 구현해서 사용하기
            {
                ySpeed = 0;
            }
            controller.Move(Vector3.up * ySpeed * Time.deltaTime);
        }
    }
    private void Jump()
    {
        ySpeed = jumpPower;
    }
    private void OnJump(InputValue value)
    {
        if (outofControl)
        {
            return;
        }
        Jump();
    }
    private void OnRun(InputValue value)
    {
        if (rideOnHorseback && value.isPressed)
        {
            ridingHorse.Accelerate();
        }
        else
        {
            walking = !value.isPressed;
        }
    }
    private void OnDecelerate(InputValue value)
    {
        ridingHorse.Decelerate(value.isPressed);
    }
    public void Mount(Horse ridingHorse)
    {
        this.ridingHorse = ridingHorse;
        StartCoroutine(MountRoutine());
        rideOnHorseback = true;
    }
    public void Dismount()
    {
        StartCoroutine(DismountRoutine());
        rideOnHorseback = false;
    }
    IEnumerator MountRoutine()
    {
        // 승마 동작: mountposition 이동, 회전, 애니메이션
        animator.SetTrigger("MountLeft");
        controller.enabled = false;
        yield return null;
    }
    IEnumerator DismountRoutine()
    {
        // 하마 동작: transform, 애니메이션
        animator.SetTrigger("DismountLeft");
        controller.enabled = true;
        yield return null;
    }
}

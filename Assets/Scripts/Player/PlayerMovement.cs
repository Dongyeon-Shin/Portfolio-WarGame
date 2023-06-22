using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, ICommandable
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
    private bool walk = true;
    private float moveSpeed;
    private bool outofControl = false;
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
    private void Update()
    {
        Fall();
    }
    private void OnEnable()
    {
        // 테스트 코드
        //transform.position = temp.position;
        //transform.rotation = temp.rotation;
        Cursor.lockState = CursorLockMode.Locked;
        StartCoroutine(MoveRoutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator MoveRoutine()
    {
        while (true)
        {
            yield return null;
            if (commandQueue.Count > 0)
            {
                yield return StartCoroutine(commandQueue.Dequeue());
            }
            if (moveDirection.magnitude == 0)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.05f);
                animator.SetFloat("MoveSpeed", moveSpeed);
                continue;
            }
            Vector3 forwardVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
            Vector3 rightVector = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
            if (walk)
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
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forwardVector * moveDirection.z + rightVector * moveDirection.x), 0.1f);
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
    public IEnumerator MovetoPositonRoutine(Vector3 positon)
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
    private void Fall()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;
        if (controller.isGrounded && ySpeed < 0) // TODO: 커스텀 isGrounded 구현해서 사용하기
        {
            ySpeed = 0;
        }
        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
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
        walk = !value.isPressed;
    }
}

using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    private bool walk = true;
    private float moveSpeed;
    private bool outofControl = false;
    private bool cameraDirectionBinding = true;
    // TODO: tempPoint 삭제
    [SerializeField]
    private Transform tempPoint;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        Move();
        Fall();
    }
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Confined;    
    }
    private void Move()
    {
        if (moveDirection.magnitude == 0)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.1f);
            animator.SetFloat("MoveSpeed", moveSpeed);
            return;
        }
        Vector3 forwardVector;
        Vector3 rightVector;
        if (cameraDirectionBinding)
        {
            forwardVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
            rightVector = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
        }
        else
        {
            forwardVector = Vector3.forward;
            rightVector = Vector3.right;
        }
        if (walk)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.1f);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, runSpeed, 0.1f);
        }
        controller.Move(forwardVector * moveDirection.z * moveSpeed * Time.deltaTime);
        controller.Move(rightVector * moveDirection.x * moveSpeed * Time.deltaTime);
        animator.SetFloat("MoveSpeed", moveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forwardVector * moveDirection.z + rightVector * moveDirection.x), 0.1f);
    }
    private void OnMove(InputValue value)
    {
        if (outofControl)
        {
            return;
        }
        moveDirection.x = value.Get<Vector2>().x;
        moveDirection.z = value.Get<Vector2>().y;
        Debug.Log(moveDirection.z);
    }
    // TODO: 인터페이스 구현하는것도 생각해볼것
    public IEnumerator HeadtoPositon(Vector3 positon)
    {
        outofControl = true;
        cameraDirectionBinding = false;
        Vector3 direction = (positon - transform.position).normalized;
        if (Vector3.Dot(transform.forward, direction) < 0)
        {
            outofControl = false;
            cameraDirectionBinding = true;
            // TODO: 해당 방향으로 부드럽게 회전하는 코드 구현
            yield break;
        }
        moveDirection.z = 1f;
        float distance = (positon - transform.position).sqrMagnitude;
        float learpRate = 0.005f;
        while (distance > 0.1f)
        {
            yield return null;
            distance = (positon - transform.position).sqrMagnitude;
            direction = positon - transform.position;
            // TODO: 진행방향을 바꾸는 모션을 조금 더 자연스럽게 할것
            if (direction.x > 0)
            {
                moveDirection.x = Mathf.Lerp(moveDirection.x, 0.71f, 1f * learpRate);
            }
            else if (direction.x < 0)
            {
                moveDirection.x = Mathf.Lerp(moveDirection.x, -0.71f, 1f * learpRate);
            }
            else
            {
                moveDirection.x = 0;
            }
            learpRate += learpRate * 0.01f;
        }
        moveDirection.x = 0f;
        moveDirection.z = 0f;
        outofControl = false;
        cameraDirectionBinding = true;
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

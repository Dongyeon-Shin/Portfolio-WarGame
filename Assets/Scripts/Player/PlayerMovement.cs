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
    private CharacterController controller;
    private CapsuleCollider hitbox;
    private Vector3 moveDirection;
    private float ySpeed = 0;
    private Animator animator;
    private bool walking = true;
    private float moveSpeed;
    private bool outofControl = false;
    private bool rideOnHorseback = false;
    public bool RideOnHorseback { get { return rideOnHorseback; } }
    private Horse ridingHorse;
    private PlayerController playerController;
    private float gradient;
    private float currentSlope;
    private float onSteepTime;
    private bool floating;
    private float floatingTime;
    private bool decelerate;
    private LayerMask groundLayerMask;
    private LayerMask slowAreaLayerMask;
    private LayerMask slipperyAreaLayerMask;

    // 테스트 코드
    [SerializeField]
    Transform temp;
    private bool cameraDirectionBinding = true;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        hitbox = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        playerController = GetComponent<PlayerController>();
        slowAreaLayerMask = LayerMask.GetMask("Mire") | LayerMask.GetMask("Water");
        slipperyAreaLayerMask = LayerMask.GetMask("Slippery");
        groundLayerMask = LayerMask.GetMask("Environment") | slowAreaLayerMask | slipperyAreaLayerMask;
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    public void Move()
    {
        if (rideOnHorseback)
        {
            ridingHorse.Move(moveDirection);
        }
        else
        {
            if (moveDirection.magnitude == 0)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, 0, 0.05f);
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
            SlopeCheck();
            DecelerateCheck();
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
    private void DecelerateCheck()
    {
        if (decelerate)
        {
            animator.SetBool("Decelerate", true);
            moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.1f);
            controller.height = 1.5f;
            controller.center = new Vector3 (controller.center.x, 1.25f, controller.center.z);
            hitbox.height = 1.5f;
            hitbox.center = new Vector3(hitbox.center.x, 1.25f, hitbox.center.z);
        }
        else
        {
            animator.SetBool("Decelerate", false);
            controller.height = 1.8f;
            controller.center = new Vector3(controller.center.x, Mathf.Lerp(controller.center.y, 1f, Time.deltaTime * 0.5f), controller.center.z);
            hitbox.height = 1.8f;
            hitbox.center = new Vector3(hitbox.center.x, Mathf.Lerp(hitbox.center.y, 1f, Time.deltaTime * 0.5f), hitbox.center.z);
        }
    }
    private void SlopeCheck()
    {
        if (gradient < 0.9f)
        {
            onSteepTime += Time.deltaTime;
        }
        else
        {
            onSteepTime = 0f;
        }
        if (onSteepTime > 0.1f)
        {
            if (currentSlope > 0f)
            {                
                animator.SetFloat("Gradient", Mathf.Lerp(animator.GetFloat("Gradient"), 1f, Time.deltaTime));
            }
            else
            {               
                animator.SetFloat("Gradient", Mathf.Lerp(animator.GetFloat("Gradient"), -1f, Time.deltaTime));
            }
            if (gradient < 0.8f)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, 0.25f * moveSpeed, 0.05f);
            }
            else if (gradient < 0.85f)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, 0.5f * moveSpeed, 0.05f);
            }
            else
            {
                moveSpeed = Mathf.Lerp(moveSpeed, 0.75f * moveSpeed, 0.05f);
            }
            return;
        }
        animator.SetFloat("Gradient", Mathf.Lerp(animator.GetFloat("Gradient"), 0f, Time.deltaTime * 3f));
    }
    IEnumerator MovetoPositonRoutine(Vector3 positon)
    {
        float elapsedTime = 0f;
        outofControl = true;
        Vector3 direction = (positon - transform.position).normalized;
        float speed = moveSpeed > walkSpeed ? moveSpeed : walkSpeed;
        animator.SetFloat("MoveSpeed", speed);
        float dot = Vector3.Dot(transform.forward, direction);
        while (true)
        {
            if ((positon - transform.position).sqrMagnitude < 0.05f)
            {
                break;
            }
            if (elapsedTime > 3f)
            {
                transform.Rotate(-transform.forward);
                StartCoroutine(MovetoPositonRoutine(positon));
            }
            if (1f - dot > 0.01f)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2.5f);
                dot = Vector3.Dot(transform.forward, direction);
            }
            controller.Move(direction * Time.deltaTime * speed);
            yield return null;
        }
        transform.Translate(positon);
        moveDirection.x = 0;
        moveDirection.z = 0;
        outofControl = false;
        yield return null;
    }
    IEnumerator RotateRoutine(Quaternion rotation)
    {
        while (true)
        {
            if (Mathf.Abs(Quaternion.Angle(transform.rotation, rotation)) > 0f)
            {
                break;
            }
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
            yield return null;
        }
        yield return null;
    }
    public void Fall()
    {
        if (rideOnHorseback)
        {
            ridingHorse.Fall();
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
            if (!floating && ySpeed < 0)
            {
                ySpeed = -1f;
            }
            controller.Move(Vector3.up * ySpeed * Time.deltaTime * 2f);
        }
    }
    public void GroundCheck()
    {
        floatingTime += Time.deltaTime;
        if (floatingTime > 0.1f)
        {
            floating = true;
        }
    }
    private void OnJump(InputValue value)
    {
        if (rideOnHorseback)
        {
            ridingHorse.Jump();
        }
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
        if (rideOnHorseback)
        {
            ridingHorse.Decelerate(value.isPressed);
        }
    }
    public void Mount(Horse ridingHorse)
    {
        this.ridingHorse = ridingHorse;
        playerController.GetCommand(MountRoutine());
        rideOnHorseback = true;
    }
    public void Dismount()
    {
        StartCoroutine(DismountRoutine());
    }
    IEnumerator MountRoutine()
    {
        animator.applyRootMotion = true;
        // 승마 동작: mountposition 이동, 회전, 애니메이션
        if ((transform.position - ridingHorse.LeftMountPoint.position).sqrMagnitude
            < (transform.position - ridingHorse.RightMountPoint.position).sqrMagnitude)
        {
            yield return StartCoroutine(MovetoPositonRoutine(ridingHorse.LeftMountPoint.position));
            yield return StartCoroutine(RotateRoutine(ridingHorse.LeftMountPoint.rotation));
            transform.position = ridingHorse.LeftMountPoint.position;
            transform.rotation = ridingHorse.LeftMountPoint.rotation;
            animator.SetTrigger("MountLeft");
        }
        else
        {
            yield return StartCoroutine(MovetoPositonRoutine(ridingHorse.RightMountPoint.position));
            yield return StartCoroutine(RotateRoutine(ridingHorse.RightMountPoint.rotation));
            transform.position = ridingHorse.RightMountPoint.position;
            transform.rotation = ridingHorse.RightMountPoint.rotation;
            animator.SetTrigger("MountRight");
        }
        controller.enabled = false;
        Camera.main.transform.GetChild(1).transform.Translate(Vector3.forward * 5f, Space.Self);
        yield return new WaitUntil (() => animator.GetCurrentAnimatorStateInfo(0).IsTag("RiderIdle"));
        animator.applyRootMotion = false;
    }
    IEnumerator DismountRoutine()
    {
        WaitUntil waitDismoutMotionFinish = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("Land"));
        // 하마 동작: transform, 애니메이션
        animator.applyRootMotion = true;
        if (moveDirection.x > 0)
        {
            animator.SetTrigger("DismountRight");
            yield return waitDismoutMotionFinish;
            animator.applyRootMotion = false;
            transform.position = ridingHorse.RightMountPoint.position;
            transform.rotation = ridingHorse.RightMountPoint.rotation;
            moveDirection.x = 0.7f;
            moveDirection.z = 1f;
        }
        else
        {
            animator.SetTrigger("DismountLeft");
            yield return waitDismoutMotionFinish;
            animator.applyRootMotion = false;
            transform.position = ridingHorse.LeftMountPoint.position;
            transform.rotation = ridingHorse.LeftMountPoint.rotation;
            moveDirection.x = -0.7f;
            moveDirection.z = 1f;
        }
        moveSpeed = walkSpeed;
        transform.parent = null;
        controller.enabled = true;
        rideOnHorseback = false;
        ridingHorse.UnLoad();
        Camera.main.transform.GetChild(1).transform.Translate(Vector3.back * 5f, Space.Self);
        yield return new WaitForSeconds(0.05f);
        moveDirection.x = 0f;
        moveDirection.z = 0f;
    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        int hitLayerMask = (1 << hit.gameObject.layer);
        if ((hitLayerMask & groundLayerMask) > 0)
        {
            gradient = Vector3.Dot(Vector3.up, hit.normal);
            floating = false;
            floatingTime = 0f;
            currentSlope = Vector3.Dot(-transform.forward, hit.normal);
            decelerate = (hitLayerMask & slowAreaLayerMask) > 0;
        }       
    }
}
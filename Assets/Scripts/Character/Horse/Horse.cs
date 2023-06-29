using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Horse : Character, IInteractable
{
    [SerializeField]
    private Transform leftMountPoint;
    [SerializeField]
    private Transform rightMountPoint;
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float ambleSpeed;
    [SerializeField]
    private float trotSpeed;
    [SerializeField]
    private float gallopSpeed;
    [SerializeField]
    private float sprintSpeed;
    private Animator animator;
    private float moveSpeed;
    private CharacterController controller;
    private Coroutine decelerateRoutine;
    private MoveSpeedState currentState;
    private float noInputTime;

    private enum MoveSpeedState
    {
        idle,
        walk,
        amble,
        trot,
        gallop,
        sprint
    }
    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
    public void Move(Vector3 moveDirection, bool walking)
    {
        //Debug.Log(moveDirection.x);
        //Debug.Log(currentState);
        //if (moveDirection.x == 0 && currentState != MoveSpeedState.idle)
        //{
        //    noInputTime += Time.deltaTime;
        //    if (noInputTime >= 1f)
        //    {
        //        currentState--;
        //        noInputTime = 0f;
        //    }
        //    animator.SetFloat("MoveSpeed", moveSpeed);
        //}
        //else
        //{
        //    noInputTime = 0f;
        //}
        //if (moveDirection.z > 0f && currentState == MoveSpeedState.idle)
        //{
        //    moveSpeed = Mathf.Lerp(moveSpeed, ambleSpeed, 0.01f);
        //    currentState = MoveSpeedState.amble;
        //}
        //Vector3 forwardVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        //Vector3 rightVector = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;
        //switch(currentState)
        //{
        //    case MoveSpeedState.idle:
        //        moveSpeed = Mathf.Lerp(moveSpeed, 0f, 0.01f);
        //        break;
        //    case MoveSpeedState.walk:
        //        moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.01f);
        //        break;
        //    case MoveSpeedState.amble:
        //        moveSpeed = Mathf.Lerp(moveSpeed, ambleSpeed, 0.01f);
        //        break;
        //    case MoveSpeedState.trot:
        //        moveSpeed = Mathf.Lerp(moveSpeed, trotSpeed, 0.01f);
        //        break;
        //    case MoveSpeedState.gallop:
        //        moveSpeed = Mathf.Lerp(moveSpeed, gallopSpeed, 0.01f);
        //        break;
        //    case MoveSpeedState.sprint:
        //        moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, 0.01f);
        //        break;
        //}
        //controller.Move(transform.forward * moveSpeed * Time.deltaTime);
        ////controller.Move(rightVector * moveDirection.x * moveSpeed * Time.deltaTime);
        //animator.SetFloat("MoveSpeed", moveSpeed);
        //switch (moveDirection.x)
        //{
        //    case -1f:
        //        transform.Rotate(Vector3.down * Time.deltaTime * 50f);
        //        break;
        //    case 1f:
        //        transform.Rotate(Vector3.up * Time.deltaTime * 50f);
        //        break;
        //    default:
        //        break;
        //}
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(forwardVector + rightVector * moveDirection.x), Time.deltaTime * 5f);
    }
    public void Fall(float ySpeed)
    {
        if (controller.isGrounded && ySpeed < 0) // TODO: 커스텀 isGrounded 구현해서 사용하기
        {
            ySpeed = 0;
        }
        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }
    public void Accelerate()
    {
        if (currentState == MoveSpeedState.sprint)
        {
            return;
        }
        currentState++;
    }
    public void Decelerate(bool isPressed)
    {
        if (isPressed)
        {
            decelerateRoutine = StartCoroutine(DecelerateRoutine());
        }
        else
        {
            StopCoroutine(decelerateRoutine);
        }
    }
    IEnumerator DecelerateRoutine()
    {
        WaitForSeconds waitPressingTime = new WaitForSeconds(1f);
        while (true)
        {
            if (currentState == MoveSpeedState.idle)
            {
                yield break;
            }
            currentState--;
            yield return waitPressingTime;
        }
    }
    public void React()
    {
        
    }
    public void Interact(GameObject player)
    {
        Load(player);
    }
    private void Load(GameObject character)
    {
        character.transform.SetParent(transform);
        // 무브멘트
    }
    protected override void HitReaction()
    {
        animator.SetTrigger("GetDamage");
        //RunAway();
    }
    private void RunAway(Vector3 direction)
    {

    }
    protected override void Collapse()
    {
        
    }
}

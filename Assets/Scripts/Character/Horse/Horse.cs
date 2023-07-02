using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Horse : Character, IInteractable
{
    [SerializeField]
    private Transform leftMountPoint;
    public Transform LeftMountPoint { get { return leftMountPoint; } }
    [SerializeField]
    private Transform rightMountPoint;
    public Transform RightMountPoint { get { return rightMountPoint; } }
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
    private float turn;
    private Animator riderAnimator;
    private int horseLayer;
    [SerializeField]
    private Transform horseback;

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
        horseLayer = gameObject.layer;
    }
    public void Move(Vector3 moveDirection)
    {
        if (moveDirection.magnitude == 0)
        {
            noInputTime += Time.deltaTime;
            if (noInputTime >= 1f && currentState != MoveSpeedState.idle)
            {
                currentState--;
                noInputTime = 0f;
                riderAnimator.SetTrigger("MountDecelerate");
            }
            animator.SetFloat("MoveSpeed", moveSpeed);
            animator.SetBool("Move", false);
        }
        else
        {
            animator.SetBool("Move", true);
            noInputTime = 0f;
            if (moveDirection.z > 0f && currentState == MoveSpeedState.idle)
            {
                moveSpeed = Mathf.Lerp(moveSpeed, ambleSpeed, 0.01f);
                currentState = MoveSpeedState.amble;
                riderAnimator.SetTrigger("MountAmble");
            }
        }
        if (moveDirection.x > 0f)
        {
            transform.Rotate(Vector3.up * Time.deltaTime * 70f);
        }
        if (moveDirection.x < 0f)
        {
            transform.Rotate(Vector3.down * Time.deltaTime * 70f);
        }
        turn = Mathf.Lerp(turn, moveDirection.x, Time.deltaTime * 2f);
        animator.SetFloat("Turn", turn);
        switch (currentState)
        {
            case MoveSpeedState.idle:
                moveSpeed = Mathf.Lerp(moveSpeed, 0f, 0.01f);
                break;
            case MoveSpeedState.walk:
                moveSpeed = Mathf.Lerp(moveSpeed, walkSpeed, 0.01f);
                break;
            case MoveSpeedState.amble:
                moveSpeed = Mathf.Lerp(moveSpeed, ambleSpeed, 0.01f);
                break;
            case MoveSpeedState.trot:
                moveSpeed = Mathf.Lerp(moveSpeed, trotSpeed, 0.01f);
                break;
            case MoveSpeedState.gallop:
                moveSpeed = Mathf.Lerp(moveSpeed, gallopSpeed, 0.01f);
                break;
            case MoveSpeedState.sprint:
                moveSpeed = Mathf.Lerp(moveSpeed, sprintSpeed, 0.01f);
                break;
        }
        animator.SetFloat("MoveSpeed", moveSpeed);
        controller.Move(transform.forward * moveSpeed * Time.deltaTime);
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
        riderAnimator.SetTrigger("MountAccelerate");
    }
    public void Decelerate(bool isPressed)
    {
        if (isPressed)
        {
            decelerateRoutine = StartCoroutine(DecelerateRoutine());
        }
        else
        {
            if (decelerateRoutine != null)
            {
                StopCoroutine(decelerateRoutine);
            }
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
            riderAnimator.SetTrigger("MountDecelerate");
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
    private void Load(GameObject rider)
    {
        rider.transform.SetParent(horseback);
        riderAnimator = rider.GetComponent<Animator>();
        gameObject.layer = rider.layer;
        moveSpeed = Mathf.Lerp(moveSpeed, 0f, 0.01f);
        currentState = MoveSpeedState.amble;
        // 무브멘트
    }
    public void UnLoad()
    {
        StartCoroutine(ChangeLayerRoutine());
        StartCoroutine(UnLoadRoutine());
    }
    IEnumerator ChangeLayerRoutine()
    {
        yield return new WaitForSeconds(1f);
        gameObject.layer = horseLayer;
    }
    IEnumerator UnLoadRoutine()
    {
        while (moveSpeed > 0.01f)
        {
            Move(Vector3.zero);
            yield return null;
        }
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

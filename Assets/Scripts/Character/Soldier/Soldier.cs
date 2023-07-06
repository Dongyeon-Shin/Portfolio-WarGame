using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soldier : Character
{
    private Soldier previousSoldier;
    private Soldier nextSoldier;
    private SoldierController soldierController;
    private CharacterController controller;
    private float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }
    private Vector3 moveDirection;

    private void Awake()
    {
        soldierController = GetComponent<SoldierController>();
        controller = GetComponent<CharacterController>();
        Debug.Log(Vector3.forward + Vector3.left);
    }
    private void Move(Vector3 position)
    {
        
    }
    private void KeepPace()
    {
        moveSpeed = previousSoldier.MoveSpeed;
    }
    private void MaintainFormation()
    {
        //Move(previousSoldier.);
        //nextSoldier. �������
    }
    //[SerializeField]
    //private float strength;
    //[SerializeField]
    //private float moveSpeed;
    //private CharacterController controller;
    //private CapsuleCollider hitbox;
    //private Animator animator;
    //private float currentSpeed;
    //private Vector3 moveDirection;
    //private float ySpeed;
    //private bool rideOnHorseback;
    //private Horse ridingHorse;
    //private float gradient;
    //private float currentSlope;
    //private float onSteepTime;
    //private bool floating;
    //private float floatingTime;
    //private bool decelerate;
    //private LayerMask groundLayerMask;
    //private LayerMask slowAreaLayerMask;
    //private LayerMask slipperyAreaLayerMask;

    //// ��ũ���ͺ� ������Ʈ�� ���� ���� ����

    //// ��ũ�� ����Ʈ�� ��� ����
    //private Soldier previousSoldier;  //prev node
    //private Soldier nextSoldier;   //next node
    //private SoldierController soldierController;   // item

    //// �� ��ũ��Ʈ���� �����Ϳ� �Լ����� ����
    //// �� �Լ��� �����Ű�� ���� SoldierController

    //// �׽�Ʈ �ڵ�
    //[SerializeField]
    //Transform tempPoint;

    //private void Awake()
    //{
    //    controller = GetComponent<CharacterController>();
    //    soldierController = GetComponent<SoldierController>();
    //    animator = GetComponent<Animator>();
    //    hitbox = GetComponent<CapsuleCollider>();
    //    slowAreaLayerMask = LayerMask.GetMask("Mire") | LayerMask.GetMask("Water");
    //    slipperyAreaLayerMask = LayerMask.GetMask("Slippery");
    //    groundLayerMask = LayerMask.GetMask("Environment") | slowAreaLayerMask | slipperyAreaLayerMask;
    //}

    // character controller �̵��̹Ƿ� y ��ǥ�� �����ؾ� �� ��
    // ĳ���Ͱ� ��ٶ����ų� ������ ����� ������ y���� ������ rotation ���� ���� ���� ����
    // ĳ���Ͱ� ���ϴ� rotation�� ���� rotation�� �ǵ��� �Ϸ��� ������ forward ���ͳ��� ������ ���� 1�̸� �ȴ�.

    // �̵���ü�� ĳ������ �������θ�
    // �����ؾ��ϴ� �� moveSpeed�� rotation ��
    //public void Move(Vector3 direction)
    //{
    //    if (direction == Vector3.zero)
    //    {
    //        return;
    //    }
    //    currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, 0.05f);
    //    SlopeCheck();
    //    DecelerateCheck();
    //    controller.Move(direction * currentSpeed * Time.deltaTime);
    //    animator.SetFloat("MoveSpeed", currentSpeed);
    //}
    //private void DecelerateCheck()
    //{
    //    if (decelerate)
    //    {
    //        animator.SetBool("Decelerate", true);
    //        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, 0.1f);
    //        controller.height = 1.5f;
    //        controller.center = new Vector3(controller.center.x, 1.25f, controller.center.z);
    //        hitbox.height = 1.5f;
    //        hitbox.center = new Vector3(hitbox.center.x, 1.25f, hitbox.center.z);
    //    }
    //    else
    //    {
    //        animator.SetBool("Decelerate", false);
    //        controller.height = 1.8f;
    //        controller.center = new Vector3(controller.center.x, Mathf.Lerp(controller.center.y, 1f, Time.deltaTime * 0.5f), controller.center.z);
    //        hitbox.height = 1.8f;
    //        hitbox.center = new Vector3(hitbox.center.x, Mathf.Lerp(hitbox.center.y, 1f, Time.deltaTime * 0.5f), hitbox.center.z);
    //    }
    //}
    //private void SlopeCheck()
    //{
    //    if (gradient < 0.9f)
    //    {
    //        onSteepTime += Time.deltaTime;
    //    }
    //    else
    //    {
    //        onSteepTime = 0f;
    //    }
    //    if (onSteepTime > 0.1f)
    //    {
    //        if (currentSlope > 0f)
    //        {
    //            animator.SetFloat("Gradient", Mathf.Lerp(animator.GetFloat("Gradient"), 1f, Time.deltaTime));
    //        }
    //        else
    //        {
    //            animator.SetFloat("Gradient", Mathf.Lerp(animator.GetFloat("Gradient"), -1f, Time.deltaTime));
    //        }
    //        if (gradient < 0.8f)
    //        {
    //            currentSpeed = Mathf.Lerp(currentSpeed, 0.25f * moveSpeed, 0.05f);
    //        }
    //        else if (gradient < 0.85f)
    //        {
    //            currentSpeed = Mathf.Lerp(currentSpeed, 0.5f * moveSpeed, 0.05f);
    //        }
    //        else
    //        {
    //            currentSpeed = Mathf.Lerp(currentSpeed, 0.75f * moveSpeed, 0.05f);
    //        }
    //        return;
    //    }
    //    animator.SetFloat("Gradient", Mathf.Lerp(animator.GetFloat("Gradient"), 0f, Time.deltaTime * 3f));
    //}

    //IEnumerator MovetoPositonRoutine(Vector3 positon)
    //{
    //    float elapsedTime = 0f;
    //    Vector3 direction = (positon - transform.position).normalized;       
    //    float dot = Vector3.Dot(transform.forward, direction);
    //    while (true)
    //    {
    //        currentSpeed = Mathf.Lerp(currentSpeed, moveSpeed, 0.05f);
    //        animator.SetFloat("MoveSpeed", currentSpeed);
    //        if ((positon - transform.position).sqrMagnitude < 0.05f)
    //        {
    //            break;
    //        }
    //        if (elapsedTime > 3f)
    //        {
    //            transform.Rotate(-transform.forward);
    //            yield return StartCoroutine(MovetoPositonRoutine(positon));
    //        }
    //        if (1f - dot > 0.01f)
    //        {
    //            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 2.5f);
    //            dot = Vector3.Dot(transform.forward, direction);
    //        }
    //        controller.Move(direction * Time.deltaTime * currentSpeed);
    //        yield return null;
    //    }
    //    transform.Translate(positon);
    //    moveDirection.x = 0;
    //    moveDirection.z = 0;
    //    yield return null;
    //}
    //IEnumerator RotateRoutine(Quaternion rotation)
    //{
    //    while (true)
    //    {
    //        if (Mathf.Abs(Quaternion.Angle(transform.rotation, rotation)) > 0f)
    //        {
    //            break;
    //        }
    //        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 10f);
    //        yield return null;
    //    }
    //    yield return null;
    //}
    //public void Fall()
    //{
    //    if (rideOnHorseback)
    //    {
    //        ridingHorse.Fall();
    //    }
    //    else
    //    {
    //        ySpeed += Physics.gravity.y * Time.deltaTime;
    //        if (!floating && ySpeed < 0)
    //        {
    //            ySpeed = -1f;
    //        }
    //        controller.Move(Vector3.up * ySpeed * Time.deltaTime * 2f);
    //    }
    //}
    //public void GroundCheck()
    //{
    //    floatingTime += Time.deltaTime;
    //    if (floatingTime > 0.1f)
    //    {
    //        floating = true;
    //    }
    //}
    //public void Mount(Horse ridingHorse)
    //{
    //    this.ridingHorse = ridingHorse;
    //    soldierController.GetCommand(MountRoutine());
    //    rideOnHorseback = true;
    //}
    //public void Dismount()
    //{
    //    StartCoroutine(DismountRoutine());
    //}
    //IEnumerator MountRoutine()
    //{
    //    animator.applyRootMotion = true;
    //    // �¸� ����: mountposition �̵�, ȸ��, �ִϸ��̼�
    //    if ((transform.position - ridingHorse.LeftMountPoint.position).sqrMagnitude
    //        < (transform.position - ridingHorse.RightMountPoint.position).sqrMagnitude)
    //    {
    //        //yield return StartCoroutine(MovetoPositonRoutine(ridingHorse.LeftMountPoint.position));
    //        yield return StartCoroutine(RotateRoutine(ridingHorse.LeftMountPoint.rotation));
    //        transform.position = ridingHorse.LeftMountPoint.position;
    //        transform.rotation = ridingHorse.LeftMountPoint.rotation;
    //        animator.SetTrigger("MountLeft");
    //    }
    //    else
    //    {
    //        //yield return StartCoroutine(MovetoPositonRoutine(ridingHorse.RightMountPoint.position));
    //        yield return StartCoroutine(RotateRoutine(ridingHorse.RightMountPoint.rotation));
    //        transform.position = ridingHorse.RightMountPoint.position;
    //        transform.rotation = ridingHorse.RightMountPoint.rotation;
    //        animator.SetTrigger("MountRight");
    //    }
    //    controller.enabled = false;
    //    Camera.main.transform.GetChild(1).transform.Translate(Vector3.forward * 5f, Space.Self);
    //    yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("RiderIdle"));
    //    animator.applyRootMotion = false;
    //}
    //IEnumerator DismountRoutine()
    //{
    //    WaitUntil waitDismoutMotionFinish = new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsTag("Land"));
    //    // �ϸ� ����: transform, �ִϸ��̼�
    //    animator.applyRootMotion = true;
    //    if (moveDirection.x > 0)
    //    {
    //        animator.SetTrigger("DismountRight");
    //        yield return waitDismoutMotionFinish;
    //        animator.applyRootMotion = false;
    //        transform.position = ridingHorse.RightMountPoint.position;
    //        transform.rotation = ridingHorse.RightMountPoint.rotation;
    //        moveDirection.x = 0.7f;
    //        moveDirection.z = 1f;
    //    }
    //    else
    //    {
    //        animator.SetTrigger("DismountLeft");
    //        yield return waitDismoutMotionFinish;
    //        animator.applyRootMotion = false;
    //        transform.position = ridingHorse.LeftMountPoint.position;
    //        transform.rotation = ridingHorse.LeftMountPoint.rotation;
    //        moveDirection.x = -0.7f;
    //        moveDirection.z = 1f;
    //    }
    //    currentSpeed = moveSpeed;
    //    transform.parent = null;
    //    controller.enabled = true;
    //    rideOnHorseback = false;
    //    ridingHorse.UnLoad();
    //    Camera.main.transform.GetChild(1).transform.Translate(Vector3.back * 5f, Space.Self);
    //    yield return new WaitForSeconds(0.05f);
    //    moveDirection.x = 0f;
    //    moveDirection.z = 0f;
    //}
    protected override void Collapse()
    {
        
    }
    protected override void HitReaction()
    {
        
    }
    //private void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    int hitLayerMask = (1 << hit.gameObject.layer);
    //    if ((hitLayerMask & groundLayerMask) > 0)
    //    {
    //        gradient = Vector3.Dot(Vector3.up, hit.normal);
    //        floating = false;
    //        floatingTime = 0f;
    //        currentSlope = Vector3.Dot(-transform.forward, hit.normal);
    //        decelerate = (hitLayerMask & slowAreaLayerMask) > 0;
    //    }
    //}
}
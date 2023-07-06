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
        //nextSoldier. 변경사항
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

    //// 스크립터블 오브젝트로 병과 병종 구현

    //// 링크드 리스트의 노드 역할
    //private Soldier previousSoldier;  //prev node
    //private Soldier nextSoldier;   //next node
    //private SoldierController soldierController;   // item

    //// 이 스크립트에는 데이터와 함수만을 포함
    //// 그 함수를 실행시키는 곳은 SoldierController

    //// 테스트 코드
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

    // character controller 이동이므로 y 좌표는 무시해야 할 것
    // 캐릭터가 고꾸라지거나 구르는 모션이 없으니 y축을 제외한 rotation 값이 변할 일은 없다
    // 캐릭터가 원하는 rotation과 같은 rotation이 되도록 하려면 서로의 forward 벡터끼리 내적한 값이 1이면 된다.

    // 이동자체는 캐릭터의 정면으로만
    // 조정해야하는 건 moveSpeed와 rotation 뿐
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
    //    // 승마 동작: mountposition 이동, 회전, 애니메이션
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
    //    // 하마 동작: transform, 애니메이션
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
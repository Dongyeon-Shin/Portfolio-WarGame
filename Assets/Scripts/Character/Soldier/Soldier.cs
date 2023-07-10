using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class Soldier : Character
{
    [SerializeField]
    protected MultiAimConstraint bodyAim;
    [SerializeField]
    protected MultiAimConstraint headAim;
    [SerializeField]
    private GameObject enemyShield;
    [SerializeField]
    private GameObject allyShield;
    [SerializeField]
    private SkinnedMeshRenderer allyRenderer;
    [SerializeField]
    private SkinnedMeshRenderer enemyRenderer;
    protected SoldierController soldierController;
    protected CharacterController controller;
    protected FormationData formationData;
    protected FormationData.FormationInfo currentFormation;
    protected SoldierData soldierData;
    protected SoldierData.SoldierInfo soldierInfo;
    // serializeField 지울것 임시 테스트용
    [SerializeField]
    protected Soldier previousSoldier;
    public Soldier PreviousSoldier { get { return previousSoldier; } set { previousSoldier = value; } }
    [SerializeField]
    protected Soldier nextSoldier;
    public Soldier NextSoldier { get { return nextSoldier; } set { nextSoldier = value; } }
    protected Vector3 objectivePoint;
    public Vector3 ObjectivePoint { get { return objectivePoint; } }
    // 스쿼드 리스트에서 인덱스를 받아오는 방식?
    //[SerializeField]
    //protected int formationNumberth;
    private IObjectPool<Soldier> soldierPool;
    protected bool enemy;
    public bool Enemy { get { return enemy; } set { enemy = value; } }
    protected float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }
    protected Animator animator;
    protected Weapon equipedWeapon;
    protected BoxCollider guardArea;
    protected Vector3 moveDirection;
    private RigBuilder rigBuilder;
    private LayerMask hostileLayerMask;
    private Transform confrontTarget;
    private bool alert;
    private bool rideOnHorseback;
    private Horse ridingHorse;
    private bool floating;
    private float ySpeed;

    // test
    [SerializeField]
    private Transform aimPoint;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private bool setEnemy;

    private void Awake()
    {
        // TODO: 임시코드
        player = GameObject.FindWithTag("Player").transform;
        Confront(player, true);
    }
    private void Start()
    {
        soldierController = GetComponent<SoldierController>();
        controller = GetComponent<CharacterController>();
        formationData = Resources.Load<FormationData>("ScriptableObject/FormationData");
        soldierData = Resources.Load<SoldierData>("ScriptableObject/SoldierData");
        Debug.Log(soldierData.SoldierClass[0].className);
        formationData.Setting();
        soldierData.Setting();
        soldierInfo = soldierData.SoldierClass[0];
        animator = GetComponent<Animator>();
        equipedWeapon = GetComponentInChildren<Weapon>();
        guardArea = GetComponentInChildren<BoxCollider>();
        // 이과정은 씬 로딩중에 끝낼것
        rigBuilder = GetComponent<RigBuilder>();
        aimPoint.parent = null;
        rigBuilder.Build();
    }
    // targetPosition = preiviousSoldier.position + formationData.() => direction * 1.5f
    // targetPosition - tranform.position = direction
    // Vector3.Dot(direction, transform.forward)
    // rotate
    // controller.move
    private void OnEnable()
    {
        // test
        enemy = setEnemy;
        
        //currentFormation = formationData.Formations[0];
        objectivePoint = player.position;
    }
    public void Move()
    {
        if (CompareDistanceWithoutHeight(objectivePoint, transform.position, 0.001f))
        {
            moveDirection = (objectivePoint - transform.position).normalized;
            //KeepPace();
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
            animator.SetFloat("MoveSpeed", moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 10f);
            Fall();
        }
        // 대형 내 위치해야하는 지점으로 계속해서 움직이게
        // 그리고 컨트롤러에서 state에 따라 움직이는 걸 멈추고 공격이나 방어 행동을 코루틴으로 제어한다
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
    private bool CompareDistanceWithoutHeight(Vector3 pos1, Vector3 pos2, float distance)
    {
        float f1 = pos1.x - pos2.x;
        float f2 = pos1.z - pos2.z;
        if (f1*f1 + f2*f2 > distance * distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // 캐릭터가 원하는 rotation과 같은 rotation이 되도록 하려면 서로의 forward 벡터끼리 내적한 값이 1이면 된다.
    // 해당 방향을 바라볼때까지
    IEnumerator RotateRoutine(Vector3 direction)
    {
        while (Vector3.Dot(transform.forward, direction) < 0.99f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
            yield return null;
        }
        yield return null;
    }
    // 해당 transform과 같은 rotation이 될때까지
    IEnumerator RotateRoutine(Transform target)
    {
        while (Vector3.Dot(transform.forward, target.forward) < 0.99f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, Time.deltaTime * 10f);
            yield return null;
        }
        yield return null;
    }
    private void KeepPace()
    {
        if ((previousSoldier.transform.position.z - transform.position.z) *
            (previousSoldier.transform.position.z - transform.position.z) + 
            (previousSoldier.transform.position.x - transform.position.x) *
            (previousSoldier.transform.position.x - transform.position.x) > 2.5f)
        {
            moveSpeed = Mathf.Lerp(moveSpeed, soldierInfo.runSpeed, Time.deltaTime);
        }
        else
        {
            moveSpeed = Mathf.Lerp(moveSpeed, soldierInfo.walkSpeed, Time.deltaTime);
        }
    }

    public void Array(Queue<Vector3> formation)
    {
        // 이전 병사의 대형내 위치를 기준으로
        objectivePoint = previousSoldier.ObjectivePoint + formation.Dequeue() * 1.5f;
        // 이전 병사의 실제 위치를 기준으로
        //objectivePoint = previousSoldier.transform.position + formation.Dequeue() * 1.5f;
        if (nextSoldier != null)
        {
            nextSoldier.Array(formation);
        }
    }
    private void MaintainFormation()
    {
        //Move(previousSoldier.);
        //nextSoldier. 변경사항
    }
    public void Confront(Transform target, bool active)
    {
        if (active)
        {
            bodyAim.weight = 1f;
            headAim.weight = 1f;
            aimPoint.parent = target;
            aimPoint.position = target.position;
        }
        else
        {
            bodyAim.weight = 0f;
            headAim.weight = 0f;
        }     
    }
    public void Attack()
    {
        animator.SetBool("Attack", true);
    }
    public void ShieldUp()
    {
        animator.SetBool("Guard", true);
    }
    public void ShieldDown()
    {
        animator.SetBool("Guard", false);
    }
    protected override void Collapse()
    {
        animator.SetBool("Collapse", true);
        // 풀링
        previousSoldier.NextSoldier = nextSoldier;
        nextSoldier.PreviousSoldier = previousSoldier;
    }
    IEnumerator DespawnBodyRoutine()
    {
        yield return new WaitForSeconds(5f);
        animator.SetBool("Collapse", false);
        soldierPool.Release(this);
    }
    protected void Regroup(Soldier soldier)
    {
        soldier.PreviousSoldier.NextSoldier = this;
        previousSoldier = soldier.PreviousSoldier;
        soldier.PreviousSoldier = this;
        nextSoldier = soldier;
    }
    protected override void HitReaction()
    {
        animator.SetTrigger("TakeDamage");
        soldierController.GetCommand(StaggerRoutine());
    }
    IEnumerator StaggerRoutine()
    {
        yield return new WaitForSeconds(0.5f);
    }
    private void DetectHostile(Collider collider)
    {
        if (((1 << controller.gameObject.layer) & hostileLayerMask) > 0)
        {
            confrontTarget = collider.transform;
            alert = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (previousSoldier == null)
        {
            //soldierController.Flee();
        }
        else
        {
            
        }
    }
    public void SetPool(IObjectPool<Soldier> pool)
    {
        soldierPool = pool;
    }
    public void Initialize(bool enemy)
    {
        gameObject.SetActive(true);
        this.enemy = enemy;
        if (enemy)
        {
            gameObject.layer = soldierData.EnemyLayer;
            hostileLayerMask = soldierData.PlayerLayerMask | soldierData.AllyLayerMask;
        }
        else
        {
            gameObject.layer = soldierData.AllyLayer;
            hostileLayerMask = soldierData.EnemyLayerMask;
        }
        enemyRenderer.gameObject.SetActive(enemy);
        enemyShield.SetActive(enemy);
        allyRenderer.gameObject.SetActive(!enemy);
        allyShield.SetActive(!enemy);
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
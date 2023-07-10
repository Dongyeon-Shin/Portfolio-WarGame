using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Pool;

public class TempSoldier : Character
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
    protected TempSoldier soldierData;
    protected SoldierData.SoldierInfo soldierInfo;
    // serializeField ����� �ӽ� �׽�Ʈ��
    [SerializeField]
    protected TempSoldier previousSoldier;
    public TempSoldier PreviousSoldier { get { return previousSoldier; } set { previousSoldier = value; } }
    [SerializeField]
    protected TempSoldier nextSoldier;
    public TempSoldier NextSoldier { get { return nextSoldier; } set { nextSoldier = value; } }
    protected Vector3 objectivePoint;
    public Vector3 ObjectivePoint { get { return objectivePoint; } }
    // ������ ����Ʈ���� �ε����� �޾ƿ��� ���?
    //[SerializeField]
    //protected int formationNumberth;
    protected bool enemy;
    public bool Enemy { get { return enemy; } set { enemy = value; } }
    protected float moveSpeed;
    public float MoveSpeed { get { return moveSpeed; } }
    protected Animator animator;
    protected Weapon equipedWeapon;
    protected BoxCollider guardArea;
    protected Vector3 moveDirection;
    private RigBuilder rigBuilder;
    public LayerMask hostileLayerMask;
    public Transform alertTarget;
    public bool alert;
    private bool rideOnHorseback;
    private Horse ridingHorse;
    private bool floating;
    private float ySpeed;
    private Queue<IEnumerator> commandQueue = new Queue<IEnumerator>();

    // test
    [SerializeField]
    private Transform aimPoint;
    [SerializeField]
    private bool setEnemy;

    private void Awake()
    {
        aimPoint.parent = null;
    }
    private void Start()
    {
        soldierController = GetComponent<SoldierController>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        equipedWeapon = GetComponentInChildren<Weapon>();
        guardArea = GetComponentInChildren<BoxCollider>();
        // �̰����� �� �ε��߿� ������
        rigBuilder = GetComponent<RigBuilder>();
        rigBuilder.Build();
        Initialize(setEnemy);
        objectivePoint = transform.position;
        StartCoroutine(SoldierBehaveRoutine());
    }
    // targetPosition = preiviousSoldier.position + formationData.() => direction * 1.5f
    // targetPosition - tranform.position = direction
    // Vector3.Dot(direction, transform.forward)
    // rotate
    // controller.move
    IEnumerator SoldierBehaveRoutine()
    {
        yield return null;
        while (true)
        {
            Fall();
            if (commandQueue.Count > 0)
            {
                yield return StartCoroutine(commandQueue.Dequeue());
            }
            Move();
            yield return null;
        }
    }
    public void Move()
    {
        if (CompareDistanceWithoutHeight(objectivePoint, transform.position, 0.001f))
        {
            moveDirection = (objectivePoint - transform.position).normalized;
            //KeepPace();
            moveSpeed = 3f;
            controller.Move(moveDirection * moveSpeed * Time.deltaTime);
            animator.SetFloat("MoveSpeed", moveSpeed);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(moveDirection), Time.deltaTime * 10f);
        }
        // ���� �� ��ġ�ؾ��ϴ� �������� ����ؼ� �����̰�
        // �׸��� ��Ʈ�ѷ����� state�� ���� �����̴� �� ���߰� �����̳� ��� �ൿ�� �ڷ�ƾ���� �����Ѵ�
    }
    Coroutine combatRoutine;
    IEnumerator AlertRoutine()
    {
        float distance = Vector3.SqrMagnitude(alertTarget.position - transform.position);
        while (distance < 1000f)
        {
            Debug.Log(distance);
            if (distance < 3.5f)
            {
                objectivePoint = transform.position;
                combatRoutine = StartCoroutine(CombatRoutine());
            }
            else
            {
                objectivePoint = alertTarget.position;
                Debug.Log(objectivePoint);
                if (combatRoutine != null)
                {
                    StopCoroutine(combatRoutine);
                }
            }
            yield return null;
            distance = Vector3.SqrMagnitude(alertTarget.position - transform.position);
        }
        Confront(null, false);
        alert = false;
        yield return null;
    }
    IEnumerator CombatRoutine()
    {
        while (true)
        {
            ShieldDown();
            yield return new WaitForSeconds(0.5f);
            Attack();
            yield return new WaitForSeconds(1f);
            ShieldUp();
            yield return new WaitForSeconds(2f);
        }
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
        if (f1 * f1 + f2 * f2 > distance * distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    // ĳ���Ͱ� ���ϴ� rotation�� ���� rotation�� �ǵ��� �Ϸ��� ������ forward ���ͳ��� ������ ���� 1�̸� �ȴ�.
    // �ش� ������ �ٶ󺼶�����
    IEnumerator RotateRoutine(Vector3 direction)
    {
        while (Vector3.Dot(transform.forward, direction) < 0.99f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * 10f);
            yield return null;
        }
        yield return null;
    }
    // �ش� transform�� ���� rotation�� �ɶ�����
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
        // ���� ������ ������ ��ġ�� ��������
        objectivePoint = previousSoldier.ObjectivePoint + formation.Dequeue() * 1.5f;
        // ���� ������ ���� ��ġ�� ��������
        //objectivePoint = previousSoldier.transform.position + formation.Dequeue() * 1.5f;
        if (nextSoldier != null)
        {
            nextSoldier.Array(formation);
        }
    }
    private void MaintainFormation()
    {
        //Move(previousSoldier.);
        //nextSoldier. �������
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
        animator.SetTrigger("Attack");
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
        // Ǯ��
        gameObject.SetActive(false);
        previousSoldier.NextSoldier = nextSoldier;
        nextSoldier.PreviousSoldier = previousSoldier;
        StartCoroutine (DespawnBodyRoutine());
    }
    IEnumerator DespawnBodyRoutine()
    {
        yield return new WaitForSeconds(5f);
        animator.SetBool("Collapse", false);
        gameObject.SetActive(false);
    }
    protected void Regroup(TempSoldier soldier)
    {
        soldier.PreviousSoldier.NextSoldier = this;
        previousSoldier = soldier.PreviousSoldier;
        soldier.PreviousSoldier = this;
        nextSoldier = soldier;
    }
    protected override void HitReaction()
    {
        animator.SetTrigger("TakeDamage");
        commandQueue.Enqueue(StaggerRoutine());
    }
    IEnumerator StaggerRoutine()
    {
        yield return new WaitForSeconds(0.5f);
    }
    public void DetectHostile(Collider collider)
    {
        if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "Ally")
        {
            alertTarget = collider.transform;
            Confront(collider.transform, true);
            alert = true;
            StartCoroutine(AlertRoutine());
        }
    }
    public void Initialize(bool enemy)
    {
        gameObject.SetActive(true);
        this.enemy = enemy;
        if (enemy)
        {
            gameObject.layer = 13;
            hostileLayerMask = 1 >> 3 | 1 >> 12;
        }
        else
        {
            gameObject.layer = 12;
            hostileLayerMask = 1 >> 13;
        }
        enemyRenderer.gameObject.SetActive(enemy);
        enemyShield.SetActive(enemy);
        allyRenderer.gameObject.SetActive(!enemy);
        allyShield.SetActive(!enemy);
    }
}
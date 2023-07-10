using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierController : MonoBehaviour
{
    public enum State
    {
        neautral,
        aggressive,
        deffensive,
        discourage,
        collapse
    }
    [SerializeField]
    private bool squadLeader;

    // neautral �� �����Ÿ��� �������� ����
    // aggressive�� ���� �������ų� ������ ������������ ���� ���� ����
    // deffensive�� upper layer���� ���и���� Ȱ��ȭ�ϰ� �̵���
    StateMachine<State, SoldierController> stateMachine;
    private Queue<IEnumerator> commandQueue = new Queue<IEnumerator>();
    private Soldier soldier;

    protected bool maintainFormation;

    // �⺻������ ������ ���� commandQueue�� �Ϲ� ����� �ʿ������ ���� ���� ���翡 �����ϰ�
    // ���� ������ ��ũ��Ʈ�� ���� �Ϲ� �������� ���� ������ ������� ��� ������ ��ɸ� ���� �� �ִ� �ӽ� ����
    private void Awake()
    {
        soldier = GetComponent<Soldier>();
        stateMachine = new StateMachine<State, SoldierController>(this);
        stateMachine.AddState(State.neautral, new NeautralState(this, stateMachine));
        stateMachine.AddState(State.aggressive, new AggressiveState(this, stateMachine));
        stateMachine.AddState(State.deffensive, new DeffensiveState(this, stateMachine));
        stateMachine.AddState(State.discourage, new DiscourageState(this, stateMachine));
        stateMachine.AddState(State.collapse, new CollapseState(this, stateMachine));
    }
    private void OnEnable()
    {
        stateMachine.SetUp(State.neautral);
        maintainFormation = true;
        StartCoroutine(SoldierBehaveRoutine(squadLeader));
        StartCoroutine(SoldierAffectedRoutine(squadLeader));
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator SoldierBehaveRoutine(bool squadLeader)
    {
        yield return null;
        while (true)
        {
            if (commandQueue.Count > 0)
            {
                yield return StartCoroutine(commandQueue.Dequeue());
            }
            stateMachine.Update();
            if (maintainFormation)
            {
                
                soldier.Move();
            }
            yield return null;
        }
    }
    IEnumerator SoldierAffectedRoutine(bool squadLeader)
    {
        yield return null;
        while (true)
        {
            soldier.Fall();
            yield return null;
        }
    }
    public void GetCommand(IEnumerator command)
    {
        commandQueue.Enqueue(command);
    }
    public void Flee()
    {
        stateMachine.ChangeState(State.discourage);
    }
    public void Regroup()
    {
        stateMachine.ChangeState(State.neautral);
    }
    private abstract class SoldierState : StateBase<State, SoldierController>
    {

        protected SoldierState(SoldierController owner, StateMachine<State, SoldierController> stateMachine) : base(owner, stateMachine)
        {

        }
    }
    private class NeautralState : SoldierState
    {
        public NeautralState(SoldierController owner, StateMachine<State, SoldierController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Transition()
        {
            
        }

        public override void Update()
        {
            
        }
    }
    private class AggressiveState : SoldierState
    {
        public AggressiveState(SoldierController owner, StateMachine<State, SoldierController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Transition()
        {
            
        }

        public override void Update()
        {
            
        }
    }
    private class DeffensiveState : SoldierState
    {
        public DeffensiveState(SoldierController owner, StateMachine<State, SoldierController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Transition()
        {
            
        }

        public override void Update()
        {
            
        }
    }
    private class DiscourageState : SoldierState
    {
        
        public DiscourageState(SoldierController owner, StateMachine<State, SoldierController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            owner.maintainFormation = false;
        }

        public override void Exit()
        {
            owner.maintainFormation = true;
        }

        public override void Setup()
        {
            
        }

        public override void Transition()
        {
            
        }

        public override void Update()
        {
            
        }
    }
    private class CollapseState : SoldierState
    {
        public CollapseState(SoldierController owner, StateMachine<State, SoldierController> stateMachine) : base(owner, stateMachine)
        {
        }

        public override void Enter()
        {
            
        }

        public override void Exit()
        {
            
        }

        public override void Setup()
        {
            
        }

        public override void Transition()
        {
            
        }

        public override void Update()
        {
            
        }
    }
}
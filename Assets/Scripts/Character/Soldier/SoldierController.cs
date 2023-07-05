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
    StateMachine<State, SoldierController> stateMachine;
    private Queue<IEnumerator> commandQueue = new Queue<IEnumerator>();

    // 기본적으론 대형을 유지 commandQueue도 일반 병사는 필요없을듯 그저 기준 병사에 의존하게
    // 솔져 리더는 스크립트를 따로 일반 솔져에는 솔져 리더가 사라졌을 경우 간략한 명령만 내릴 수 있는 임시 리더
    private void Awake()
    {
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
        StartCoroutine(SoldierBehaveRoutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    IEnumerator SoldierBehaveRoutine()
    {
        yield return null;
        while (true)
        {
            if (commandQueue.Count > 0)
            {
                yield return StartCoroutine(commandQueue.Dequeue());
            }
            stateMachine.Update();
            yield return null;
        }
    }
    public void GetCommand(IEnumerator command)
    {
        commandQueue.Enqueue(command);
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
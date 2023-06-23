using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerController : MonoBehaviour
{
    public UnityEvent Behave;
    public UnityEvent PhysicsAffect;
    public UnityEvent Look;
    private Queue<IEnumerator> commandQueue = new Queue<IEnumerator>();

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        StartCoroutine(PlayerBehaveRoutine());
        StartCoroutine(PlayerAffectedRoutine());
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    // 플레이어가 움직일 수 없는 상태면 수행하지 못해야 하는 행동들
    IEnumerator PlayerBehaveRoutine()
    {
        yield return null;
        while (true)
        {
            Look?.Invoke();
            if (commandQueue.Count > 0)
            {
                yield return StartCoroutine(commandQueue.Dequeue());
            }
            Behave?.Invoke();
            yield return null;
        }
    }
    // 플레이어가 움직일 수 없는 상태여도 영향을 받아야 하는 행동들
    IEnumerator PlayerAffectedRoutine()
    {
        yield return null;
        while(true)
        {
            PhysicsAffect?.Invoke();
            yield return null;
        }
    }
    public void GetCommand(IEnumerator command)
    {
        commandQueue.Enqueue(command);
    }
}

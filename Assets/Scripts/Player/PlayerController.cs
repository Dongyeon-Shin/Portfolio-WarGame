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
    // �÷��̾ ������ �� ���� ���¸� �������� ���ؾ� �ϴ� �ൿ��
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
    // �÷��̾ ������ �� ���� ���¿��� ������ �޾ƾ� �ϴ� �ൿ��
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

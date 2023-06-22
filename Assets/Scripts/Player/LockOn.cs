using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LockOn : MonoBehaviour
{
    [SerializeField]
    private MultiAimConstraint bodyAim;
    [SerializeField]
    private MultiAimConstraint headAim;
    private Vector3 cameraDirection;

    private void Update()
    {
        cameraDirection = (Camera.main.transform.position - transform.position).normalized;
        if (Vector3.Dot(transform.forward, cameraDirection) > 0.8f)
        {
            headAim.weight = Mathf.Lerp(headAim.weight, 0f, Time.deltaTime * 5f);
            bodyAim.weight = Mathf.Lerp(bodyAim.weight, 0f, Time.deltaTime * 5f);
        }
        else
        {
            headAim.weight = Mathf.Lerp(headAim.weight, 1f, Time.deltaTime * 3f);
            bodyAim.weight = Mathf.Lerp(bodyAim.weight, 1f, Time.deltaTime * 3f);
        }
    }
}

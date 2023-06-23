using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerSight : MonoBehaviour
{
    [SerializeField]
    private MultiAimConstraint bodyAim;
    [SerializeField]
    private MultiAimConstraint headAim;
    [SerializeField]
    private CinemachineFreeLook freeLookCamera;
    private Vector3 cameraDirection;
    private float mouseXAxisSensitity;
    private float mouseYAxisSensitity;

    private void OnEnable()
    {
        // TODO: 나중에 마우스 민감도 설정 구현할 실 구현 방법에 따라 바꿀것
        mouseXAxisSensitity = freeLookCamera.m_XAxis.m_MaxSpeed;
    }
    public void Gaze()
    {
        cameraDirection = (Camera.main.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, cameraDirection);
        if (dot > 0.5f)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = Mathf.Lerp(freeLookCamera.m_XAxis.m_MaxSpeed, mouseXAxisSensitity * 0.3f, Time.deltaTime * 100f);
            if (dot > 0.85f)
            {
                headAim.weight = Mathf.Lerp(headAim.weight, 0.1f, Time.deltaTime * 5f);
                bodyAim.weight = Mathf.Lerp(bodyAim.weight, 0.1f, Time.deltaTime * 5f);
            }
            else
            {
                headAim.weight = Mathf.Lerp(headAim.weight, 1f, Time.deltaTime * 3f);
                bodyAim.weight = Mathf.Lerp(bodyAim.weight, 1f, Time.deltaTime * 3f);
            }
        }
        else
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = Mathf.Lerp(freeLookCamera.m_XAxis.m_MaxSpeed, mouseXAxisSensitity, Time.deltaTime * 500f);
        }
    }
}

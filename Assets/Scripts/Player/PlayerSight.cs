using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;

public class PlayerSight : MonoBehaviour
{
    [SerializeField]
    private MultiAimConstraint bodyAim;
    [SerializeField]
    private MultiAimConstraint headAim;
    [SerializeField]
    private CinemachineFreeLook freeLookCamera;
    [SerializeField]
    [Range(5, 30)]
    private float zoomInOutSpeed;
    private Vector3 cameraDirection;
    private float mouseXAxisSensitity;
    private float mouseYAxisSensitity;
    private float mouseScroll;

    private void OnEnable()
    {
        // TODO: 나중에 마우스 민감도 설정 구현할 실 구현 방법에 따라 바꿀것
        mouseXAxisSensitity = freeLookCamera.m_XAxis.m_MaxSpeed;
        mouseScroll = 0f;
    }
    public void Gaze()
    {
        cameraDirection = (Camera.main.transform.position - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, cameraDirection);
        if (dot > 0.5f)
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = Mathf.Lerp(freeLookCamera.m_XAxis.m_MaxSpeed, mouseXAxisSensitity * 0.5f, Time.deltaTime * 100f);
            if (dot > 0.8f)
            {
                headAim.weight = Mathf.Lerp(headAim.weight, 0f, Time.deltaTime * 6f);
                bodyAim.weight = Mathf.Lerp(bodyAim.weight, 0.1f, Time.deltaTime * 6f);
            }
            else
            {
                headAim.weight = Mathf.Lerp(headAim.weight, 1f, Time.deltaTime * 3f);
                bodyAim.weight = Mathf.Lerp(bodyAim.weight, 1f, Time.deltaTime * 3f);
            }
        }
        else
        {
            freeLookCamera.m_XAxis.m_MaxSpeed = Mathf.Lerp(freeLookCamera.m_XAxis.m_MaxSpeed, mouseXAxisSensitity, Time.deltaTime * 100f);
            if (Camera.main.transform.position.y - transform.position.y > 2.5f)
            {
                bodyAim.weight = Mathf.Lerp(bodyAim.weight, 0.4f, Time.deltaTime * 10f);
            }
            else
            {
                bodyAim.weight = Mathf.Lerp(bodyAim.weight, 1f, Time.deltaTime * 3f);
            }
        }
    }
    public void ZoomInOut()
    {
        if ((mouseScroll > 0 ? mouseScroll : -mouseScroll) < 0.1f)
        {
            return;
        }
        else
        {
            mouseScroll = Mathf.Lerp(mouseScroll, 0f, Time.deltaTime * 5f);
        }
        if (freeLookCamera.m_Lens.FieldOfView < 10f)
        {
            freeLookCamera.m_Lens.FieldOfView = 10f;
            return;
        }
        if (freeLookCamera.m_Lens.FieldOfView > 60f)
        {
            freeLookCamera.m_Lens.FieldOfView = 60f;
            return;
        }
        freeLookCamera.m_Lens.FieldOfView += mouseScroll;
    }
    private void OnZoom(InputValue value)
    {
        if (value.Get<float>() > 0)
        {
            mouseScroll -= Time.deltaTime * zoomInOutSpeed;
        }
        else if (value.Get<float>() < 0)
        {
            mouseScroll += Time.deltaTime * zoomInOutSpeed;
        }
    }
}
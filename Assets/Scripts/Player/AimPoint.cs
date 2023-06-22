using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimPoint : MonoBehaviour
{
    [SerializeField]
    private Transform virualAimPoint;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, virualAimPoint.position, Time.deltaTime * 6f);
    }
}

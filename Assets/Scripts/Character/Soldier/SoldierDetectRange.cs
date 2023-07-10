using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class SoldierDetectRange : MonoBehaviour
{
    TempSoldier soldier;

    private void Awake()
    {
        soldier = GetComponentInParent<TempSoldier>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!soldier.alert)
        {
            soldier.DetectHostile(other);
        }

        if (soldier.PreviousSoldier == null)
        {
            //soldierController.Flee();
        }
        else
        {
            if (!soldier.alert)
            {
                soldier.DetectHostile(other);
            }
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "SoldierData", menuName = "Data/Soldier")]
public class SoldierData : ScriptableObject
{
    [SerializeField]
    private SoldierInfo[] soldierClass;
    public SoldierInfo[] SoldierClass { get { return soldierClass; } }
    private int allyLayer;
    public int AllyLayer { get { return allyLayer; } }
    private int enemyLayer;
    public int EnemyLayer { get { return enemyLayer; } }
    private LayerMask allyLayerMask;
    public LayerMask AllyLayerMask { get { return allyLayerMask; } }
    private LayerMask enemyLayerMask;
    public LayerMask EnemyLayerMask { get { return enemyLayerMask; } }
    private LayerMask soldierLayerMask;
    public LayerMask SoldierLayerMask { get { return soldierLayerMask; } }
    private LayerMask playerLayerMask;
    public LayerMask PlayerLayerMask { get { return playerLayerMask; } }

    private void OnEnable()
    {
        allyLayer = LayerMask.NameToLayer("AllySoldier");
        enemyLayer = LayerMask.NameToLayer("EnemySoldier");
        allyLayerMask = 1 << allyLayer;
        enemyLayerMask = 1 << enemyLayer;
        soldierLayerMask = allyLayerMask | enemyLayerMask;
        playerLayerMask = LayerMask.GetMask("Player");
    }

    [Serializable]
    public class SoldierInfo
    {
        public string className;
        public float maximumHealthPoint;
        public float strength;
        public float armor;
        public float walkSpeed;
        public float runSpeed;
    }
}
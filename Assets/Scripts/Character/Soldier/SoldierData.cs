using System;
using System.Collections;
using System.Collections.Generic;
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

    private void Awake()
    {
        allyLayer = LayerMask.NameToLayer("AllyLayer");
        enemyLayer = LayerMask.NameToLayer("EnemyLayer");
    }

    [Serializable]
    public class SoldierInfo
    {
        public string className;
        public float maximumHealthPoint;
        public float strength;
        public float armor;
        public float moveSpeed;
    }
}

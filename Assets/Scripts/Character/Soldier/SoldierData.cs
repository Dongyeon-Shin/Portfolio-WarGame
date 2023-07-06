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

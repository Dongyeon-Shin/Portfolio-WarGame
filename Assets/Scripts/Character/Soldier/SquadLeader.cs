using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLeader : Soldier
{
    private void Awake()
    {
        previousSoldier = this;
    }
}

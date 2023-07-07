using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquadLeader : Soldier
{
    private GameObject body;
    private Animator bodyAnimator;
    private Queue<Vector3> formation;
    private bool isFirstSquad;
    protected int squadSize;

    private void Awake()
    {
        previousSoldier = this;
        bodyAnimator = body.GetComponent<Animator>();
        // TODO: temp
        isFirstSquad = true;
    }
    public void InitializeFormation()
    {
        if (isFirstSquad)
        {
            formationData.GetFirstSquadFormation(currentFormation, squadSize, ref formation);
        }
        else
        {
            formationData.GetSecondSquadFormation(currentFormation, squadSize, ref formation);
        }
    }
    protected override void Collapse()
    {
        bodyAnimator.SetBool("Collapse", true);
        // 애니메이션 끝나면 body disable하고 회수
        transform.position = nextSoldier.transform.position;
        transform.rotation = nextSoldier.transform.rotation;
        nextSoldier.enabled = false; // pool manage할것
        nextSoldier = nextSoldier.NextSoldier;
    }
    
}

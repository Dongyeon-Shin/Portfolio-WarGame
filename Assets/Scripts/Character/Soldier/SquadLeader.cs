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
        // �ִϸ��̼� ������ body disable�ϰ� ȸ��
        transform.position = nextSoldier.transform.position;
        transform.rotation = nextSoldier.transform.rotation;
        nextSoldier.enabled = false; // pool manage�Ұ�
        nextSoldier = nextSoldier.NextSoldier;
    }
    
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : Character
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    protected override void Collapse()
    {
        
    }
    protected override void HitReaction()
    {
        animator.SetTrigger("TakeDamage");
    }
    public void InitializeAttackSpeed()
    {

    }
}

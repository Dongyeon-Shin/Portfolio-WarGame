using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : Character, IInteractable
{
    [SerializeField]
    private Transform leftMountPoint;
    [SerializeField]
    private Transform rightMountPoint;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    public void React()
    {
        
    }
    public void Interact(GameObject player)
    {
        
    }
    private void Load(GameObject character)
    {
        transform.SetParent(character.transform);
        // ¹«ºê¸àÆ®
    }
    protected override void HitReaction()
    {
        animator.SetTrigger("GetDamage");
        //RunAway();
    }
    private void RunAway(Vector3 direction)
    {

    }
    protected override void Collapse()
    {
        
    }
}

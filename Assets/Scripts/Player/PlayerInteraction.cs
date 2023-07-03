using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInteraction : MonoBehaviour
{
    private LayerMask mask;
    protected PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }
    protected void DetectInteractableObject(float detectingRange, int layer)
    {
        mask = 1;
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectingRange, mask << layer);
        foreach (Collider collider in colliders)
        {
            IInteractable interactableObject = collider.GetComponent<IInteractable>();
            if (interactableObject != null)
            {
                Interact(interactableObject, collider);
                break;
            }
        }
    }
    public abstract void Interact(IInteractable interactableObject, Collider interactableObjectCollider);
}


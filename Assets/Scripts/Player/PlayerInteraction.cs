using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInteraction : MonoBehaviour
{
    LayerMask mask;
    protected void DetectInteractableObject(float detectingRange, int layer)
    {
        mask = 1;
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectingRange, mask << layer);
        foreach (Collider collider in colliders)
        {
            IInteractable interactableObject = collider.GetComponent<IInteractable>();
            if (interactableObject != null)
            {
                Interact(interactableObject);
                break;
            }
        }
    }
    public abstract void Interact(IInteractable interactableObject);
}


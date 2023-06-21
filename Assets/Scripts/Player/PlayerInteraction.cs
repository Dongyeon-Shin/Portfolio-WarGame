using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerInteraction : MonoBehaviour
{
    protected void DetectInteractableObject(float detectingRange)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectingRange);
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


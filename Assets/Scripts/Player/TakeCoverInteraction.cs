using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TakeCoverInteraction : PlayerInteraction
{
    [SerializeField]
    private float detectingRange;

    public override void Interact(IInteractable interactableObject)
    {
        
    }
    private void OnTakeCover(InputValue value)
    {
        DetectInteractableObject(detectingRange);
    }
}

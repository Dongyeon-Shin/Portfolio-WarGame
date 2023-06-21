using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MountInteraction : PlayerInteraction
{
    [SerializeField]
    private float detectingRange;

    public override void Interact(IInteractable interactableObject)
    {
        
    }
    private void OnMount(InputValue value)
    {
        DetectInteractableObject(detectingRange);
    }
}

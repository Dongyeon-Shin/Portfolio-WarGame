using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MountInteraction : PlayerInteraction
{
    [SerializeField]
    private float detectingRange;
    [SerializeField]
    private int layer;

    public override void Interact(IInteractable interactableObject)
    {
        interactableObject.Interact(gameObject);
    }
    private void OnMount(InputValue value)
    {
        DetectInteractableObject(detectingRange, layer);
    }
}

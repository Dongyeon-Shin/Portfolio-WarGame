using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class MountInteraction : PlayerInteraction
{
    [SerializeField]
    private float detectingRange;
    [SerializeField]
    private int layer;

    public override void Interact(IInteractable interactableObject, Collider horse)
    {
        interactableObject.Interact(gameObject);
        playerMovement.Mount(horse.GetComponent<Horse>());
        
    }
    private void OnMount(InputValue value)
    {
        if (playerMovement.RideOnHorseback)
        {
            playerMovement.Dismount();
        }
        else
        {
            DetectInteractableObject(detectingRange, layer);
        }
    }
}

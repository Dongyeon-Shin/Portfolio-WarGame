using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public void React();
    public void Interact(GameObject player);
}

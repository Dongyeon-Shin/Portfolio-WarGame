using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Horse : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform leftMountPoint;
    [SerializeField]
    private Transform rightMountPoint;

    public void React()
    {
        
    }
    public void Interact(GameObject player)
    {
        
    }
    private void Load(GameObject character)
    {
        transform.SetParent(character.transform);
    }
}

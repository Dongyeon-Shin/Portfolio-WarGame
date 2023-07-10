using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IHittable
{
    [SerializeField]
    protected float healthPoint;

    public void Hit()
    {
        HitReaction();
        GetDamage();
    }
    protected abstract void HitReaction();
    private void GetDamage()
    {
        healthPoint--;
        if (healthPoint < 0)
        {
            Collapse();
        }
    }
    protected abstract void Collapse();
}

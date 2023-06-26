using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IHittable
{
    [SerializeField]
    private float healthPoint;

    public void Hit()
    {
        HitReaction();
        GetDamage();
    }
    protected abstract void HitReaction();
    private void GetDamage()
    {
        if (healthPoint < 0)
        {

        }
    }
    protected abstract void Collapse();
}

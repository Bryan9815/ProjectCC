using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireRateMod : PowerUp
{
    public float fireRateMod;

    public override void ActivateEffect()
    {
        GetComponentInParent<Entity>().ModifyFireRate(fireRateMod);
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifyFireRate(-fireRateMod);
    }
}

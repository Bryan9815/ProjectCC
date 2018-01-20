using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthMod : PowerUp
{
    public float healthMod;

    public override void ActivateEffect()
    {
        GetComponentInParent<Entity>().ModifyMaxHP(healthMod);
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifyMaxHP(-healthMod);
    }
}

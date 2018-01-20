using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedMod : PowerUp
{
    public float speedMod;

    public override void ActivateEffect()
    {
        GetComponentInParent<Entity>().ModifySpeed(speedMod);
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifySpeed(-speedMod);
    }
}

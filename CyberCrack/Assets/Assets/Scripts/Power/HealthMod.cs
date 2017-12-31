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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PickUp(collision.gameObject);
        }
    }
}

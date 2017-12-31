using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMod : PowerUp
{
    public float attackMod;

    public override void ActivateEffect()
    {
        GetComponentInParent<Entity>().ModifyDamage(attackMod);
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifyDamage(-attackMod);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            PickUp(collision.gameObject);
        }
    }
}
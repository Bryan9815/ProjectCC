using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : PowerUp
{
    public float attackMod;

    public override void ActivateEffect()
    {
        Debug.Log("Activating attack up, parent is: " + gameObject.name);
        GetComponentInParent<Entity>().ModifyDamage(attackMod);
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifyDamage(-attackMod);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!used && collision.tag == "Player")
        {
            used = true;
            PickUp(collision.gameObject);
        }
    }
}
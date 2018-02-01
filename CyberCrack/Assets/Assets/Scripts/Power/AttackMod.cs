using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AttackMod : PowerUp
{
    public float attackMod;

    private void Start()
    {
        powerName = "Attack Upgrade";
        price = 15;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = powerName;
    }

    public override void ActivateEffect()
    {
        if (transform.parent.parent.tag == "Player")
            GetComponentInParent<Entity>().ModifyPlayerDamage(attackMod);
        else
        {
            GetComponentInParent<Entity>().ModifyMobDamage(true);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.parent.parent.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    public override void DeactivateEffect()
    {
        if (transform.parent.parent.tag == "Player")
            GetComponentInParent<Entity>().ModifyPlayerDamage(-attackMod);
        else
        {
            GetComponentInParent<Entity>().ModifyMobDamage(false);
        }
    }
}
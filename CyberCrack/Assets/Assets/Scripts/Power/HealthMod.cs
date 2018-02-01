using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthMod : PowerUp
{
    public float healthMod;

    private void Start()
    {
        powerName = "Health Upgrade";
        price = 10;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = powerName;
    }

    public override void ActivateEffect()
    {
        if (transform.parent.parent.tag == "Enemy")
        {
            GetComponentInParent<Entity>().ModifyMaxHP(healthMod);
            transform.GetChild(0).gameObject.SetActive(false);
            transform.parent.parent.GetComponent<SpriteRenderer>().color = Color.green;
        }
        GetComponentInParent<Entity>().ModifyHP(healthMod);
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifyHP(-healthMod);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if(PlayerCharacter.instance.GetHP() < PlayerCharacter.instance.GetMaxHP())
                PickUp(collision.gameObject);
        }
    }
}

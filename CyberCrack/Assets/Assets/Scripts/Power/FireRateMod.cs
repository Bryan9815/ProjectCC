using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FireRateMod : PowerUp
{
    public float fireRateMod;

    private void Start()
    {
        powerName = "Fire Rate Upgrade";
        price = 10;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = powerName;
    }

    public override void ActivateEffect()
    {
        GetComponentInParent<Entity>().ModifyFireRate(fireRateMod);

        if (transform.parent.parent.tag == "Enemy")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.parent.parent.GetComponent<SpriteRenderer>().color = Color.gray;
        }
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifyFireRate(-fireRateMod);
    }
}

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
        price = 5;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = powerName;
    }

    public override void ActivateEffect()
    {
        GetComponentInParent<Entity>().ModifyHP(healthMod);
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifyHP(-healthMod);
    }
}

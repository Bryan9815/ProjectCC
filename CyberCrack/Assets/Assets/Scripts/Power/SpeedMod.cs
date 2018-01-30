using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedMod : PowerUp
{
    public float speedMod;

    private void Start()
    {
        powerName = "Speed Upgrade";
        price = 10;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = powerName;
    }

    public override void ActivateEffect()
    {
        GetComponentInParent<Entity>().ModifySpeed(speedMod);
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifySpeed(-speedMod);
    }
}

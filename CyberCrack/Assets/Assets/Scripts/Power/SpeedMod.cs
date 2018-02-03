using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SpeedMod : PowerUp
{
    public float speedMod;

    private void Start()
    {
        sound = GetComponent<AudioSource>();
        sound.volume = GameData.instance.GetVolume();
        powerName = "Speed Upgrade";
        price = 10;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = powerName;
    }

    public override void ActivateEffect()
    {
        GetComponentInParent<Entity>().ModifySpeed(speedMod);

        if(transform.parent.parent.tag == "Enemy")
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.parent.parent.GetComponent<SpriteRenderer>().color = new Color32(128, 0, 255, 255);
        }
    }

    public override void DeactivateEffect()
    {
        GetComponentInParent<Entity>().ModifySpeed(-speedMod);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Usable : PowerUp
{

	// Use this for initialization
	void Start ()
    {
        sound = GetComponent<AudioSource>();
        sound.volume = GameData.instance.GetVolume();
        powerName = "Multi Shot";
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = powerName;
    }

    public override void PickUp(GameObject newParent)
    {
        // Disable collider and renderer
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        GameController.instance.usableIcon.SetActive(true);
        ActivateEffect();
        sound.PlayOneShot(Resources.Load<AudioClip>("Audio/pickUp"));
    }

    public override void ActivateEffect()
    {
        PlayerCharacter.instance.usable = true;
    }
}

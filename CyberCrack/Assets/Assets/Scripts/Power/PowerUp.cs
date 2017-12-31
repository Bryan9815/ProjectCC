using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject originalItem;
    protected bool used;

    // Use this for initialization
    public virtual void PickUp(GameObject newParent)
    {
        // Disable collider and renderer
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        // Add to new parent's power up list
        newParent.GetComponent<Entity>().AddPowerUp(this);
    }

    public virtual void ActivateEffect(){}
    public virtual void DeactivateEffect() { }
}

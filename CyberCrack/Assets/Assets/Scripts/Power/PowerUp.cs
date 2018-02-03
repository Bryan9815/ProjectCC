using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public int price;
    protected string powerName, powerDescription;
    protected AudioSource sound;

    public virtual void PickUp(GameObject newParent)
    {
        // Disable collider and renderer
        gameObject.GetComponent<Collider2D>().enabled = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        // Add to new parent's power up list
        newParent.GetComponent<Entity>().AddPowerUp(this);
    }

    public virtual void Dropped(Transform room)
    {
        // Add to new parent's power up list
        gameObject.transform.parent = room;

        // Disable collider and renderer
        gameObject.GetComponent<Collider2D>().enabled = true;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        // Enable text
        transform.GetChild(0).gameObject.SetActive(true);
        transform.localRotation = Quaternion.identity;
    }

    public virtual void ActivateEffect(){}
    public virtual void DeactivateEffect() {}

    //public virtual void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        PickUp(collision.gameObject);
    //    }
    //}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PickUp(collision.gameObject);
            sound.PlayOneShot(Resources.Load<AudioClip>("Audio/pickUp"));
        }
    }
}

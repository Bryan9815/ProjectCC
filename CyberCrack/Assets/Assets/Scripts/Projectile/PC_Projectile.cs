using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_Projectile : Projectile
{
    private Animator anim;
    private GameObject player;
    private AudioSource sound;
    // Use this for initialization
    void Start ()
    {
		
	}

    public void Init(Vector3 pos, Quaternion rot, Vector3 dir, float dmg, float life)
    {
        transform.position = pos;
        transform.rotation = rot;
        GetComponent<Rigidbody2D>().velocity = dir;
        damage = dmg;
        lifetime = life;
    }
	
	// Update is called once per frame
	public override void Update ()
    {
        base.Update();

	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If special bullet, make particle effects on collision
        switch (collision.gameObject.tag)
        {
            case "Enemy":
                collision.gameObject.GetComponent<Entity>().ReduceHP(damage);
                Destroy(gameObject);
                break;
            case "Object":
                // Object hp reduced
                Destroy(gameObject);
                break;
            case "Terrain":
                Destroy(gameObject);
                break;
        }
    }
}

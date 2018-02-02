using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : Projectile
{
    private Animator anim;
    private GameObject player;
    private AudioSource sound;
    // Use this for initialization
    void Start()
    {

    }

    public void Init(Vector3 pos, Quaternion rot, Vector3 dir, bool dmgHigh, float life)
    {
        transform.position = pos;
        transform.rotation = rot;
        GetComponent<Rigidbody2D>().velocity = dir;
        mobDamage = dmgHigh;
        lifetime = life;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If special bullet, make particle effects on collision
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerCharacter>().IsHit(mobDamage);
                GameObject particles = Instantiate(Resources.Load<GameObject>("Prefabs/ProjectileParticle"), transform.position, Quaternion.identity, transform.parent);
                particles.GetComponent<ParticleSystem>().startColor = Color.red;
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

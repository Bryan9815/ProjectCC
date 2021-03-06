﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    protected bool mobDamage;
    protected float damage;
    protected float lifetime = 1.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (lifetime > 0)
            lifetime -= Time.deltaTime;
        else
            Destroy(gameObject);
    }

    public virtual void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Burst : Entity
{
    float timer;
	// Use this for initialization
	void Start ()
    {
        hp = 3;
        damage = 1;
        speed = 5;
        fireRate = 1.5f;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
            Destroy(gameObject);

        if(timer < fireRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            FireProjectile();
        }
    }

    void FireProjectile()
    {
        for(int i = 0; i < transform.childCount; i ++)
        {
            Debug.Log("Target: " + i);
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);

            Vector3 direction = (transform.GetChild(i).position - transform.position) * 7.5f;
            Debug.Log("Direction: " + direction);
            bullet.GetComponent<Enemy_Projectile>().Init(transform.GetChild(i).position, transform.GetChild(i).rotation, direction, damage, 1.0f);
        }
    }
}
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Entity
{
    // Use this for initialization
    void Start()
    {
        Init();

        hp = 3;
        damage = 1;
        speed = 0.01f;
        fireRate = 0.33f;

        target = GameController.instance.playerCharacter.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
            Destroy(gameObject);

        Vector3 dir = new Vector3();
        dir = (target.position - transform.position).normalized;

        transform.position += dir * speed;
        transform.Rotate(0, 0, transform.rotation.z + 5);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerCharacter>().IsHit(damage);
        }
    }
}

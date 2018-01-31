using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chaser : Entity
{
    private void Awake()
    {
        Start();
    }

    // Use this for initialization
    void Start()
    {
        Init();

        //maxHP = hp = 1;
        maxHP = hp = 5 + (5 * GameController.instance.GetCurrentLevel());
        speed = 0.25f;
        fireRate = 0.33f;

        chanceToDropMoney = 3;
        moneyMin = 0 + (1 * GameController.instance.GetCurrentLevel());
        moneyMax = 3 + (2 * GameController.instance.GetCurrentLevel());

        target = PlayerCharacter.instance.transform;
        //target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (hp <= 0)
                Death();

            Vector3 dir = new Vector3();
            dir = (target.position - transform.position).normalized;

            transform.position += dir * speed;
            //transform.Rotate(0, 0, transform.rotation.z + 5);
            transform.localEulerAngles += new Vector3(0, 0, 7.5f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerCharacter>().IsHit(mobDamageHigh);
        }
    }
}

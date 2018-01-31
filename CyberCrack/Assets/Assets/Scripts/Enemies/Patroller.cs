using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : Entity
{
    float timer;
    bool vertical;
    // Use this for initialization
    void Start()
    {
        Init();

        //maxHP = hp = 1;
        maxHP = hp = 10 + (5 * GameController.instance.GetCurrentLevel());
        speed = 5;
        fireRate = 3.5f;
        timer = 0;
        chanceToDropMoney = 3;
        moneyMin = 0 + (1 * GameController.instance.GetCurrentLevel());
        moneyMax = 5 + (3 * GameController.instance.GetCurrentLevel());

        if (transform.localEulerAngles.z == 90 || transform.localEulerAngles.z == 270)
        {
            vertical = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (hp <= 0)
                Death();

            if (timer < 5 / fireRate)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                FireProjectile();
            }

            // Movement
            if(vertical)
                transform.localPosition += new Vector3(0, 5 * speed * Time.deltaTime, 0);
            else
                transform.localPosition += new Vector3(5 * speed * Time.deltaTime, 0, 0);
        }
    }

    void FireProjectile()
    {
        for (int i = 0; i < target.childCount; i++)
        {
            //Debug.Log("Target: " + (i+1));
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);

            Vector3 direction = (target.GetChild(i).position - transform.position) * 7.5f;
            //Debug.Log("Direction: " + direction);
            bullet.GetComponent<Enemy_Projectile>().Init(target.GetChild(i).position, target.GetChild(i).rotation, direction, mobDamageHigh, 1.0f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                speed *= -1;
                collision.gameObject.GetComponent<PlayerCharacter>().IsHit(mobDamageHigh);
                break;
            case "Object":
                speed *= -1;
                break;
            case "Terrain":
                speed *= -1;
                break;
        }
    }
}

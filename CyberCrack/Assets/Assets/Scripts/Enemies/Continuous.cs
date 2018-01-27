using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Continuous : Entity
{
    float timer;
    // Use this for initialization
    void Start()
    {
        Init();

        maxHP = hp = 15;
        damage = 1;
        speed = 5;
        fireRate = 6.25f;
        timer = 0;
        chanceToDropMoney = 3;
        moneyMin = 1;
        moneyMax = 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (hp <= 0)
                Death();

            if (timer < 5/fireRate)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                StartCoroutine(FireProjectile());
            }
        }
    }

    IEnumerator FireProjectile()
    {
        for (int i = 0; i < target.childCount; i++)
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);

            Vector3 direction = (target.GetChild(i).position - transform.position) * 7.5f;
            bullet.GetComponent<Enemy_Projectile>().Init(target.GetChild(i).position, target.GetChild(i).rotation, direction, damage, 1.0f);

            yield return new WaitForSeconds(0.1f);
        }
    }
}

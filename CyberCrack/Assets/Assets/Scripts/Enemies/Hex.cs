﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hex : Entity
{
    float timer;
    int spawnCounter;
    Vector3 dirVec;

    // Use this for initialization
    void Start()
    {
        Init();

        //maxHP = hp = 1;
        maxHP = hp = 150 + (15 * GameController.instance.GetCurrentLevel());
        damage = 1;
        speed = 0.5f;
        fireRate = 3.333f;
        timer = 0;
        dirVec = new Vector3(1, 1, 0);
        chanceToDropMoney = 0;
        moneyMin = 5 + (2 * GameController.instance.GetCurrentLevel());
        moneyMax = 20 + (2 * GameController.instance.GetCurrentLevel());
    }

    public override void ToggleActive(bool active)
    {
        if (active)
        {
            isActive = true;
            GameController.instance.bossHPBar.gameObject.SetActive(true);
        }
        else
        {
            isActive = false;
            GameController.instance.bossHPBar.gameObject.SetActive(false);
        }
    }

    protected override void Death()
    {
        base.Death();
        GameController.instance.bossesDefeated++;
        GoToNextLevel.CreateDoorToNextLevel(transform.parent);
        GameController.instance.bossHPBar.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            // HP
            if (hp <= 0)
                Death();

            // Update HP Bar
            float hpBarScale = 800 * (hp / maxHP);
            hpBarScale = Mathf.Max(0.01f, hpBarScale);
            Mathf.Clamp(hpBarScale, 0.01f, 80.0f);
            if (hp >= maxHP)
                hpBarScale = 800;

            GameController.instance.bossHPBar.GetChild(2).GetComponent<RectTransform>().sizeDelta = new Vector2(hpBarScale, 100);

            #region Attacks
            if (timer < 5/fireRate)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                spawnCounter++;
                FireProjectile();
            }

            if (spawnCounter > 5)
            {
                spawnCounter = 0;
                if (HelperFunctions.RandomBool())
                    SpawnAdds();
            }
            #endregion

            //Movement
            transform.localPosition += dirVec * speed;
            transform.localEulerAngles += new Vector3(0, 0, 0.25f);
        }
    }

    void FireProjectile()
    {
        for (int i = 0; i < target.childCount; i++)
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);

            Vector3 direction = (target.GetChild(i).position - transform.position) * 7.5f;
            bullet.GetComponent<Enemy_Projectile>().Init(target.GetChild(i).position, target.GetChild(i).rotation, direction, mobDamageHigh, 1.0f);
        }
    }

    void SpawnAdds()
    {
        transform.parent.GetComponent<RoomInstance>().mobList.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Chaser"), target.GetChild(1).position, target.GetChild(0).rotation, transform.parent));
        transform.parent.GetComponent<RoomInstance>().mobList.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Chaser"), target.GetChild(3).position, target.GetChild(0).rotation, transform.parent));
        transform.parent.GetComponent<RoomInstance>().mobList.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Chaser"), target.GetChild(4).position, target.GetChild(0).rotation, transform.parent));
    }

    void ChangeMovement()
    {
        switch (Random.Range(0, 3))
        {
            case 0:
                dirVec *= -1;
                break;
            case 1:
                dirVec.x *= -1;
                break;
            case 2:
                dirVec.y *= -1;
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch(collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerCharacter>().IsHit(mobDamageHigh);
                break;
            case "PickUp":
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                break;
            case "Terrain":
                ChangeMovement();
                break;
            case "Object":
                ChangeMovement();
                break;
        }
    }
}

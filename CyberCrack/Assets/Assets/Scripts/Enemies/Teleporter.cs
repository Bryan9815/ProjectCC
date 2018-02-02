using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Teleporter : Entity
{
    float timer, teleportTimer, colorChanger;
    Vector3 dirVec;
    GameObject crosshair;
    Color start, end;
    bool teleport;

    // Use this for initialization
    void Start()
    {
        Init();

        maxHP = hp = 1;
        maxHP = hp = 150 + (15 * GameController.instance.GetCurrentLevel());
        speed = 60f;
        //fireRate = 6.25f;
        fireRate = 60f;
        timer = 0;
        teleportTimer = 0;
        chanceToDropMoney = 0;
        moneyMin = 5 + (2 * GameController.instance.GetCurrentLevel());
        moneyMax = 20 + (2 * GameController.instance.GetCurrentLevel());

        crosshair = transform.parent.GetChild(1).gameObject;
        start = Color.white;
        end = Color.red;
        colorChanger = 0;
        teleport = false;
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
        GoToNextLevel.CreateDoorToNextLevel(transform.parent.parent);
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

            #region Track Player
            dirVec = new Vector2(PlayerCharacter.instance.transform.position.x - crosshair.transform.position.x, PlayerCharacter.instance.transform.position.y - crosshair.transform.position.y);
            #endregion

            #region Attacks
            if (timer < 5 / fireRate)
            {
                timer += Time.deltaTime;
            }
            else
            {
                timer = 0;
                FireProjectile();
            }
            transform.localEulerAngles += new Vector3(0, 0, 1.25f);
            #endregion

            #region Teleport
            GetComponent<SpriteRenderer>().color = Color.Lerp(start, end, colorChanger);
            if (teleportTimer < 5.0f)
            {
                teleportTimer += Time.deltaTime;
                if (teleportTimer > 2.5f)
                {
                    colorChanger += Time.deltaTime / 2.5f;

                    if (!crosshair.GetComponent<SpriteRenderer>().enabled)
                        crosshair.GetComponent<SpriteRenderer>().enabled = true;

                    crosshair.transform.position += dirVec.normalized * speed * Time.deltaTime;
                }
            }
            else
            {
                StartCoroutine(Teleport());
            }

            GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            #endregion
        }
    }

    void FireProjectile()
    {
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);
        if (hp < maxHP / 4) // Quarter HP
        {
            Vector3 direction = (target.GetChild(0).position - transform.position) * 7.5f;
            bullet.GetComponent<Enemy_Projectile>().Init(target.GetChild(0).position, target.GetChild(0).rotation, direction, mobDamageHigh, 1.0f);

            GameObject bullet2 = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);
            Vector3 direction2 = (target.GetChild(2).position - transform.position) * 7.5f;
            bullet2.GetComponent<Enemy_Projectile>().Init(target.GetChild(2).position, target.GetChild(2).rotation, direction2, mobDamageHigh, 1.0f);

            GameObject bullet3 = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);
            Vector3 direction3 = (target.GetChild(4).position - transform.position) * 7.5f;
            bullet3.GetComponent<Enemy_Projectile>().Init(target.GetChild(4).position, target.GetChild(4).rotation, direction3, mobDamageHigh, 1.0f);

            GameObject bullet4 = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);
            Vector3 direction4 = (target.GetChild(6).position - transform.position) * 7.5f;
            bullet4.GetComponent<Enemy_Projectile>().Init(target.GetChild(6).position, target.GetChild(6).rotation, direction4, mobDamageHigh, 1.0f);
        }
        else if(hp < maxHP / 2) // Half HP
        {
            Vector3 direction = (target.GetChild(0).position - transform.position) * 7.5f;
            bullet.GetComponent<Enemy_Projectile>().Init(target.GetChild(0).position, target.GetChild(0).rotation, direction, mobDamageHigh, 1.0f);

            GameObject bullet2 = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);
            Vector3 direction2 = (target.GetChild(4).position - transform.position) * 7.5f;
            bullet2.GetComponent<Enemy_Projectile>().Init(target.GetChild(4).position, target.GetChild(4).rotation, direction2, mobDamageHigh, 1.0f);
        }
        else // Above Half HP
        {
            Vector3 direction = (target.GetChild(0).position - transform.position) * 7.5f;
            bullet.GetComponent<Enemy_Projectile>().Init(target.GetChild(0).position, target.GetChild(0).rotation, direction, mobDamageHigh, 1.0f);
        }        
    }

    IEnumerator Teleport()
    {
        teleport = true;
        transform.position = new Vector3(crosshair.transform.position.x, crosshair.transform.position.y, 0);
        crosshair.transform.position = transform.position;
        crosshair.GetComponent<SpriteRenderer>().enabled = false;
        colorChanger = 0;

        yield return new WaitForSeconds(0.1f);
        teleport = false;
        teleportTimer = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerCharacter>().IsHit(mobDamageHigh);
                if (teleport)
                {
                    Vector2 knockBackDir = new Vector3(PlayerCharacter.instance.transform.position.x - transform.position.x, PlayerCharacter.instance.transform.position.y - transform.position.y);
                    collision.gameObject.GetComponent<PlayerCharacter>().Knockedback(knockBackDir.normalized * 100);
                    GameObject particles = Instantiate(Resources.Load<GameObject>("Prefabs/ProjectileParticle"), collision.transform.position, Quaternion.identity, transform.parent);
                    particles.GetComponent<ParticleSystem>().startColor = Color.red;
                }
                break;
            case "PickUp":
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                break;
        }
    }
}
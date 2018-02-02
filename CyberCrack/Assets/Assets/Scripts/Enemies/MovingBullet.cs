using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBullet : Entity
{
    float timer;
    int chargeCounter;
    Vector2 dirVec;
    bool charging;

    // Use this for initialization
    void Start()
    {
        Init();

        //maxHP = hp = 1;
        maxHP = hp = 150 + (15 * GameController.instance.GetCurrentLevel());
        damage = 1;
        speed = 1.0f;
        fireRate = 3.333f;
        timer = 0;
        charging = false;
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

            #region Look at player
            dirVec = new Vector2(PlayerCharacter.instance.transform.position.x - transform.position.x, PlayerCharacter.instance.transform.position.y - transform.position.y);
            float angle = Mathf.Atan2(dirVec.y, dirVec.x) * Mathf.Rad2Deg;
            Vector3 newRot = Quaternion.AngleAxis(angle, new Vector3(0, 0, 1)).eulerAngles;
            transform.eulerAngles = new Vector3(0, 0, newRot.z - 90);
            #endregion

            #region Attacks
            if (!charging)
            {
                if (timer < 5 / fireRate)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 0;
                    chargeCounter++;
                    FireProjectile();
                }
                if (chargeCounter > 3)
                {
                    charging = true;
                    chargeCounter = 0;
                    StartCoroutine(Charge());
                }
            }
            #endregion
        }
    }

    void FireProjectile()
    {
        sound.PlayOneShot(Resources.Load<AudioClip>("Audio/enemyShot"));
        for (int i = 0; i < target.childCount; i++)
        {
            //Debug.Log("Target: " + (i+1));
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);

            Vector3 direction = (target.GetChild(i).position - transform.position) * 7.5f;
            //Debug.Log("Direction: " + direction);
            bullet.GetComponent<Enemy_Projectile>().Init(target.GetChild(i).position, target.GetChild(i).rotation, direction, mobDamageHigh, 1.0f);
        }
    }

    IEnumerator Charge()
    {
        GetComponent<Rigidbody2D>().velocity += -(dirVec.normalized * 25 * speed);
        yield return new WaitForSeconds(0.75f);
        GetComponent<Rigidbody2D>().velocity += (dirVec.normalized * 150 * speed);
        yield return new WaitForSeconds(1.5f);
        charging = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                collision.gameObject.GetComponent<PlayerCharacter>().IsHit(mobDamageHigh);
                GameObject particles = Instantiate(Resources.Load<GameObject>("Prefabs/ProjectileParticle"), collision.transform.position, Quaternion.identity, transform.parent);
                particles.GetComponent<ParticleSystem>().startColor = Color.red;
                if (charging)
                    collision.gameObject.GetComponent<PlayerCharacter>().Knockedback();
                break;
            case "PickUp":
                Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.gameObject.GetComponent<Collider2D>());
                break;
        }
    }
}

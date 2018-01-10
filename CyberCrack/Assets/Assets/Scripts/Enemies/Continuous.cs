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

        hp = 3;
        damage = 1;
        speed = 5;
        fireRate = 0.8f;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (hp <= 0)
                Destroy(gameObject);

            if (timer < fireRate)
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
            Debug.Log("Target: " + i);
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Enemy_Projectile"), transform.parent);

            Vector3 direction = (target.GetChild(i).position - transform.position) * 7.5f;
            Debug.Log("Direction: " + direction);
            bullet.GetComponent<Enemy_Projectile>().Init(target.GetChild(i).position, target.GetChild(i).rotation, direction, damage, 1.0f);

            yield return new WaitForSeconds(0.1f);
        }
    }
}

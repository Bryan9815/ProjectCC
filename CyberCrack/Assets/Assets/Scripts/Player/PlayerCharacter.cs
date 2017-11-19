﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity
{
    Transform projectileSpawn;

    bool projectileCooldown;
    bool usable = false;
    int shotStyle = 0;
    string fireDirection = "";
	// Use this for initialization
	void Start ()
    {
        hp = 3;
        damage = 1;
        speed = 5;
        fireRate = 0.33f;
        projectileCooldown = false;

        projectileSpawn = transform.GetChild(0);
	}
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
        Shooting();
	}

    void Movement()
    {
        //Move Up
        if (Input.GetKey(KeyCode.W))
        {
            transform.localPosition += new Vector3(0, speed, 0);
        }

        // Move Left
        if (Input.GetKey(KeyCode.A))
        {
            transform.localPosition += new Vector3(-speed, 0, 0);
        }

        // Move Down
        if (Input.GetKey(KeyCode.S))
        {
            transform.localPosition += new Vector3(0, -speed, 0);
        }

        // Move Right
        if (Input.GetKey(KeyCode.D))
        {
            transform.localPosition += new Vector3(speed, 0, 0);
        }
    }

    void Shooting()
    {
        // Replace player character model with a pointer
        // Rotate pointer in direction pressed and then instantiate bullet and make it move forward
        // 8-directional shooting
        // Create a bullet system so that the instantiated bullet has all power-up effects
        bool firing = false;
        Quaternion newRot = transform.localRotation;
        Quaternion temp = newRot;

        #region Rotation & firing bool
        // Up
        if (Input.GetKey(KeyCode.UpArrow))
        {
            firing = true;
            // Up
            newRot.eulerAngles = new Vector3(0, 0, 0);
            fireDirection = "Vertical";
            // Upper Left & Right
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                newRot.eulerAngles = new Vector3(0, 0, 45);
                fireDirection = "Diagonal_RtL";
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                newRot.eulerAngles = new Vector3(0, 0, -45);
                fireDirection = "Diagonal_LtR";
            }
        }
        else if (Input.GetKey(KeyCode.DownArrow)) // Down
        {
            firing = true;
            // Down
            newRot.eulerAngles = new Vector3(0, 0, -180);
            fireDirection = "Vertical";
            // Lower Left & Right
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                newRot.eulerAngles = new Vector3(0, 0, -225);
                fireDirection = "Diagonal_LtR";
            }
            else if (Input.GetKey(KeyCode.RightArrow))
            {
                newRot.eulerAngles = new Vector3(0, 0, 225);
                fireDirection = "Diagonal_RtL";
            }
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) // Left
        {
            firing = true;
            newRot.eulerAngles = new Vector3(0, 0, 90);
            fireDirection = "Horizontal";
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // Right
        {
            firing = true;
            newRot.eulerAngles = new Vector3(0, 0, -90);
            fireDirection = "Horizontal";
        }

        if (newRot != temp)
            transform.localRotation = newRot;
        #endregion

        if(firing & !projectileCooldown)
        {
            StartCoroutine(FireBullet(firing));
        }
    }

    IEnumerator FireBullet(bool trigger)
    {
        trigger = false;
        projectileCooldown = true;

        string tempFireStyle = fireDirection;
        switch(shotStyle)
        {
            case 0: // Single Shot
                {
                    GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/PC_Projectile"), transform.parent);

                    Vector3 direction = (projectileSpawn.transform.position - transform.position) * 7.5f;

                    bullet.GetComponent<PC_Projectile>().Init(projectileSpawn.transform.position, projectileSpawn.transform.rotation, direction, damage, 1.0f);
                }
                break;
            case 1: // Spread Shot
                {
                    for (int i = 0; i < transform.childCount; i++)
                    {
                        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/PC_Projectile"), transform.parent);

                        Vector3 direction = (transform.GetChild(i).position - transform.position) * 7.5f;

                        bullet.GetComponent<PC_Projectile>().Init(transform.GetChild(i).position, transform.GetChild(i).rotation, direction, damage, 1.0f);
                    }
                }
                break;
            case 2: // Triple Shot
                {
                    GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/PC_Projectile"), transform.parent);

                    Vector3 direction = (projectileSpawn.transform.position - transform.position) * 7.5f;

                    bullet.GetComponent<PC_Projectile>().Init(projectileSpawn.transform.position, projectileSpawn.transform.rotation, direction, damage, 1.0f);

                    yield return new WaitForSeconds(0.05f);

                    GameObject bulletL = Instantiate(bullet, bullet.transform);
                    GameObject bulletR = Instantiate(bullet, bullet.transform);

                    Vector3 posL = new Vector3();
                    Vector3 posR = new Vector3();
                    switch (tempFireStyle)
                    {
                        case "Horizontal":
                            posL = new Vector3(bullet.transform.position.x, bullet.transform.position.y - 0.5f, 0);
                            posR = new Vector3(bullet.transform.position.x, bullet.transform.position.y + 0.5f, 0);
                            break;
                        case "Vertical":
                            posL = new Vector3(bullet.transform.position.x - 0.5f, bullet.transform.position.y, 0);
                            posR = new Vector3(bullet.transform.position.x + 0.5f, bullet.transform.position.y, 0);
                            break;
                        case "Diagonal_LtR":
                            posL = new Vector3(bullet.transform.position.x - 0.33f, bullet.transform.position.y + 0.33f, 0);
                            posR = new Vector3(bullet.transform.position.x + 0.33f, bullet.transform.position.y - 0.33f, 0);
                            break;
                        case "Diagonal_RtL":
                            posL = new Vector3(bullet.transform.position.x + 0.33f, bullet.transform.position.y + 0.33f, 0);
                            posR = new Vector3(bullet.transform.position.x - 0.33f, bullet.transform.position.y - 0.33f, 0);
                            break;
                    }
                    Debug.Log("tempFireStyle: " + tempFireStyle);

                    bulletL.transform.position = posL;
                    bulletL.transform.localScale = new Vector3(1, 1, 1);
                    bulletL.transform.parent = bullet.transform.parent;
                    bulletL.GetComponent<Rigidbody2D>().velocity = bullet.GetComponent<Rigidbody2D>().velocity;

                    bulletR.transform.position = posR;
                    bulletR.transform.localScale = new Vector3(1, 1, 1);
                    bulletR.transform.parent = bullet.transform.parent;
                    bulletR.GetComponent<Rigidbody2D>().velocity = bullet.GetComponent<Rigidbody2D>().velocity;
                }
                break;
        }        

        yield return new WaitForSeconds(fireRate);
        projectileCooldown = false;        
    }

    public void ActivateTripleShot()
    {
        shotStyle += 1;
        if (shotStyle > 2)
        {
            shotStyle = 0;
        }
        else
        {
            if(transform.childCount > 1)
            {
                Destroy(transform.GetChild(1));
                Destroy(transform.GetChild(2));
            }
            GameObject projectileSpawnL = Instantiate<GameObject>(projectileSpawn.gameObject, projectileSpawn.parent);
            Vector3 spawnLpos = new Vector3(-1.25f, projectileSpawn.localPosition.y, 0);
            projectileSpawnL.transform.localPosition = spawnLpos;

            GameObject projectileSpawnR = Instantiate<GameObject>(projectileSpawn.gameObject, projectileSpawn.parent);
            Vector3 spawnRpos = new Vector3(1.25f, projectileSpawn.localPosition.y, 0);
            projectileSpawnR.transform.localPosition = spawnRpos;
        }
    }
}

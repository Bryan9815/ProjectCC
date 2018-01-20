using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity
{
    public static PlayerCharacter instance;
    RoomInstance currentRoom;

    bool isHit;
    bool projectileCooldown;
    bool usable = false;

    int respawns;

    int shotStyle = 0;
    string fireDirection = "";

    void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Use this for initialization
    void Start ()
    {
        Init();

        // Stats
        maxHP = hp = 3;
        damage = 1;
        speed = 1;
        fireRate = 0.33f;
        projectileCooldown = false;
        isHit = false;
        projectileSpeed = 10.0f;
        
        UpdateHealthDisplay();
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isActive)
        {
            Movement();
            Shooting();
        }
    }

    public void IsHit(float damage)
    {
        if (!isHit)
        {
            isHit = true;
            hp -= damage;
            StartCoroutine(ResetInvincibility());
            UpdateHealthDisplay();
            // Play damage flashing anim
        }
    }

    private IEnumerator ResetInvincibility()
    {
        yield return new WaitForSeconds(1.0f);

        isHit = false;
        Debug.Log("Invincibility reset");
    }

    void UpdateHealthDisplay()
    {
        GameObject heartContainer = GameController.instance.uiCanvas.GetChild(0).gameObject;
        if(hp != heartContainer.transform.childCount)
        {
            if (heartContainer.transform.childCount != 0)
            {
                for (int i = 0; i < heartContainer.transform.childCount; i++)
                {
                    Destroy(heartContainer.transform.GetChild(i).gameObject);
                }
            }            
            for (int i = 0; i < hp; i++)
            {
                Instantiate(Resources.Load<GameObject>("Prefabs/Heart"), heartContainer.transform);
            }
        }
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

                    Vector3 direction = (target.GetChild(0).position - transform.position) * projectileSpeed;

                    bullet.GetComponent<PC_Projectile>().Init(target.GetChild(0).position, target.GetChild(0).rotation, direction, damage, 1.0f);

                    // Parent the bullet to the room
                    bullet.transform.parent = GameController.instance.gameplayCanvas.GetChild(1);
                }
                break;
            case 1: // Spread Shot
                {
                    for (int i = 0; i < target.childCount; i++)
                    {
                        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/PC_Projectile"), transform.parent);

                        Vector3 direction = (target.GetChild(i).position - transform.position) * projectileSpeed;

                        bullet.GetComponent<PC_Projectile>().Init(target.GetChild(i).position, target.GetChild(i).rotation, direction, damage, 1.0f);

                        // Parent the bullet to the room
                        bullet.transform.parent = GameController.instance.gameplayCanvas.GetChild(1);
                    }
                }
                break;
            case 2: // Triple Shot
                {
                    GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/PC_Projectile"), transform.parent);

                    Vector3 direction = (target.GetChild(0).position - transform.position) * projectileSpeed;

                    bullet.GetComponent<PC_Projectile>().Init(target.GetChild(0).position, target.GetChild(0).rotation, direction, damage, 1.0f);

                    // Parent the bullet to the room
                    bullet.transform.parent = GameController.instance.gameplayCanvas.GetChild(1);

                    yield return new WaitForSeconds(0.05f);

                    GameObject bulletL = Instantiate(bullet, bullet.transform);
                    GameObject bulletR = Instantiate(bullet, bullet.transform);

                    Vector3 posL = new Vector3();
                    Vector3 posR = new Vector3();
                    switch (tempFireStyle)
                    {
                        case "Horizontal":
                            posL = new Vector3(bullet.transform.position.x, bullet.transform.position.y - 0.5f * 15, 0);
                            posR = new Vector3(bullet.transform.position.x, bullet.transform.position.y + 0.5f * 15, 0);
                            break;
                        case "Vertical":
                            posL = new Vector3(bullet.transform.position.x - 0.5f * 15, bullet.transform.position.y, 0);
                            posR = new Vector3(bullet.transform.position.x + 0.5f * 15, bullet.transform.position.y, 0);
                            break;
                        case "Diagonal_LtR":
                            posL = new Vector3(bullet.transform.position.x - 0.33f * 15, bullet.transform.position.y + 0.33f * 15, 0);
                            posR = new Vector3(bullet.transform.position.x + 0.33f * 15, bullet.transform.position.y - 0.33f * 15, 0);
                            break;
                        case "Diagonal_RtL":
                            posL = new Vector3(bullet.transform.position.x + 0.33f * 15, bullet.transform.position.y + 0.33f * 15, 0);
                            posR = new Vector3(bullet.transform.position.x - 0.33f * 15, bullet.transform.position.y - 0.33f * 15, 0);
                            break;
                    }
                    //Debug.Log("tempFireStyle: " + tempFireStyle);

                    bulletL.transform.position = posL;
                    bulletL.transform.localScale = new Vector3(1, 1, 1);
                    bulletL.transform.parent = bullet.transform.parent;
                    bulletL.GetComponent<Rigidbody2D>().velocity = bullet.GetComponent<Rigidbody2D>().velocity;

                    // Parent the bullet to the room
                    bulletL.transform.parent = GameController.instance.gameplayCanvas.GetChild(1);

                    bulletR.transform.position = posR;
                    bulletR.transform.localScale = new Vector3(1, 1, 1);
                    bulletR.transform.parent = bullet.transform.parent;
                    bulletR.GetComponent<Rigidbody2D>().velocity = bullet.GetComponent<Rigidbody2D>().velocity;

                    // Parent the bullet to the room
                    bulletR.transform.parent = GameController.instance.gameplayCanvas.GetChild(1);
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
            if(target.childCount > 1)
            {
                Destroy(target.GetChild(1).gameObject);
                Destroy(target.GetChild(2).gameObject);
            }
            GameObject projectileSpawnL = Instantiate<GameObject>(target.gameObject, target);
            Vector3 spawnLpos = new Vector3(-1.25f, target.GetChild(0).localPosition.y, 0);
            projectileSpawnL.transform.localPosition = spawnLpos;

            GameObject projectileSpawnR = Instantiate<GameObject>(target.gameObject, target);
            Vector3 spawnRpos = new Vector3(1.25f, target.GetChild(0).localPosition.y, 0);
            projectileSpawnR.transform.localPosition = spawnRpos;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch(collision.tag)
        {
            case "Room":
                currentRoom = collision.GetComponent<RoomInstance>();
                break;
            default:
                break;
        }
    }
}

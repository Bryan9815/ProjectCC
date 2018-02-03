using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacter : Entity
{
    public static PlayerCharacter instance;
    RoomInstance currentRoom;

    bool isHit, knockback, firing;
    bool projectileCooldown;
    public bool usable = false;

    int shotStyle = 0;
    string fireDirection = "";
    HealthContainer heartContainer;
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
        InitializePlayer();
    }

    public void InitializePlayer()
    {
        Init();
        transform.position = new Vector3(0, -GameController.instance.GetComponent<SheetAssigner>().verticalOffset / 5.4f, 0);

        // Stats
        maxHP = 10;
        hp = 3;
        damage = 5;
        speed = 1;
        fireRate = 15.0f;
        projectileCooldown = false;
        isHit = false;
        knockback = false;
        firing = false;
        projectileSpeed = 10.0f;
        heartContainer = GameController.instance.uiCanvas.GetChild(0).Find("HealthContainer").GetComponent<HealthContainer>();

        UpdateHealthDisplay();
        GameController.instance.singletons.Add(this.gameObject);
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isActive)
        {
            Movement();
            Shooting();
            Usable();
        }
    }

    public override void ModifyHP(float mod)
    {
        base.ModifyHP(mod);

        if (hp >= maxHP)
            hp = maxHP;

        UpdateHealthDisplay();
    }

    public override void AddPowerUp(PowerUp newPowerUp)
    {
        base.AddPowerUp(newPowerUp);
        GameController.instance.powerUpsObtained++;
        GameData.instance.UpdateData("totalPowerUps", GameData.instance.totalPowerUps+1);
    }

    public void IsHit(bool high)
    {
        if (!isHit)
        {
            isHit = true;
            if (!high)
                ModifyHP(-1);
            else
                ModifyHP(-2);

            if (hp > 0)
                anim.SetTrigger("Hit");
            else
                StartCoroutine(PlayerDeath());
        }
    }

    void ResetInvincibility()
    {
        isHit = false;
    }

    IEnumerator PlayerDeath()
    {
        // Play death animation
        yield return new WaitForSeconds(0.1f);
        GameController.instance.OpenRespawnMenu();
    }

    void UpdateHealthDisplay()
    {
        heartContainer.SpawnHearts((int)hp);
    }

    public void Knockedback()
    {
        knockback = true;
        StartCoroutine(Knockback());
    }

    public void Knockedback(Vector3 knockDirection)
    {
        knockback = true;
        GetComponent<Rigidbody2D>().velocity = knockDirection;
        StartCoroutine(Knockback());
    }

    IEnumerator Knockback()
    {
        yield return new WaitForSeconds(0.5f);
        knockback = false;
        GetComponent<Rigidbody2D>().velocity = Vector3.zero;
    }

    void Movement()
    {
        if (!knockback)
        {
            //Move Up
            if (Input.GetKey(GameData.instance.playerKeys.up))
            {
                transform.localPosition += new Vector3(0, speed, 0);
                if (!firing)
                    transform.localEulerAngles = Vector3.zero;
            }

            // Move Left
            if (Input.GetKey(GameData.instance.playerKeys.left))
            {
                transform.localPosition += new Vector3(-speed, 0, 0);
                if (!firing)
                    transform.localEulerAngles = new Vector3(0, 0, 90);
            }

            // Move Down
            if (Input.GetKey(GameData.instance.playerKeys.down))
            {
                transform.localPosition += new Vector3(0, -speed, 0);
                if (!firing)
                    transform.localEulerAngles = new Vector3(0, 0, 180);
            }

            // Move Right
            if (Input.GetKey(GameData.instance.playerKeys.right))
            {
                transform.localPosition += new Vector3(speed, 0, 0);
                if (!firing)
                    transform.localEulerAngles = new Vector3(0, 0, -90);
            }
        }
    }

    void Shooting()
    {
        // Rotate pointer in direction pressed and then instantiate bullet and make it move forward
        // 8-directional shooting
        Quaternion newRot = transform.localRotation;
        Quaternion temp = newRot;
        firing = false;
        #region Rotation & firing bool
        // Up
        if (Input.GetKey(GameData.instance.shootKeys.up))
        {
            firing = true;
            // Up
            newRot.eulerAngles = new Vector3(0, 0, 0);
            fireDirection = "Vertical";
            // Upper Left & Right
            if (Input.GetKey(GameData.instance.shootKeys.left))
            {
                newRot.eulerAngles = new Vector3(0, 0, 45);
                fireDirection = "Diagonal_RtL";
            }
            else if (Input.GetKey(GameData.instance.shootKeys.right))
            {
                newRot.eulerAngles = new Vector3(0, 0, -45);
                fireDirection = "Diagonal_LtR";
            }
        }
        else if (Input.GetKey(GameData.instance.shootKeys.down)) // Down
        {
            firing = true;
            // Down
            newRot.eulerAngles = new Vector3(0, 0, -180);
            fireDirection = "Vertical";
            // Lower Left & Right
            if (Input.GetKey(GameData.instance.shootKeys.left))
            {
                newRot.eulerAngles = new Vector3(0, 0, -225);
                fireDirection = "Diagonal_LtR";
            }
            else if (Input.GetKey(GameData.instance.shootKeys.right))
            {
                newRot.eulerAngles = new Vector3(0, 0, 225);
                fireDirection = "Diagonal_RtL";
            }
        }
        else if (Input.GetKey(GameData.instance.shootKeys.left)) // Left
        {
            firing = true;
            newRot.eulerAngles = new Vector3(0, 0, 90);
            fireDirection = "Horizontal";
        }
        else if (Input.GetKey(GameData.instance.shootKeys.right)) // Right
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

    void Usable()
    {
        if(usable)
        {
            if (Input.GetKeyDown(GameData.instance.interact))
                ActivateTripleShot();
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
                    sound.PlayOneShot(Resources.Load<AudioClip>("Audio/playerShot"));

                    Vector3 direction = (target.GetChild(0).position - transform.position) * projectileSpeed;

                    bullet.GetComponent<PC_Projectile>().Init(target.GetChild(0).position, target.GetChild(0).rotation, direction, damage, 1.0f);

                    // Parent the bullet to the room
                    bullet.transform.parent = GameController.instance.gameplayCanvas.GetChild(1);
                }
                break;
            case 1: // Spread Shot
                {
                    sound.PlayOneShot(Resources.Load<AudioClip>("Audio/playerShot"));
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
                    sound.PlayOneShot(Resources.Load<AudioClip>("Audio/playerShot"));

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
        
        yield return new WaitForSeconds(5/fireRate);
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

    public void PlaySound(string soundName)
    {
        string path = "Audio/" + soundName;
        sound.PlayOneShot(Resources.Load<AudioClip>(path));
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

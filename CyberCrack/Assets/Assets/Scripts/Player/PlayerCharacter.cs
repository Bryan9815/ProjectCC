using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity
{
    Transform projectileSpawn;

    bool projectileCooldown;
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

            // Upper Left & Right
            if (Input.GetKey(KeyCode.LeftArrow))
                newRot.eulerAngles = new Vector3(0, 0, 45);
            else if (Input.GetKey(KeyCode.RightArrow))
                newRot.eulerAngles = new Vector3(0, 0, -45);
        }
        else if (Input.GetKey(KeyCode.DownArrow)) // Down
        {
            firing = true;
            // Down
            newRot.eulerAngles = new Vector3(0, 0, -180);

            // Lower Left & Right
            if (Input.GetKey(KeyCode.LeftArrow))
                newRot.eulerAngles = new Vector3(0, 0, -225);
            else if (Input.GetKey(KeyCode.RightArrow))
                newRot.eulerAngles = new Vector3(0, 0, 225);
        }
        else if (Input.GetKey(KeyCode.LeftArrow)) // Left
        {
            firing = true;
            newRot.eulerAngles = new Vector3(0, 0, 90);
        }
        else if (Input.GetKey(KeyCode.RightArrow)) // Right
        {
            firing = true;
            newRot.eulerAngles = new Vector3(0, 0, -90);
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

        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/PC_Projectile"), transform.parent);

        Vector3 direction = (projectileSpawn.transform.position - transform.position) * 7.5f;

        bullet.GetComponent<PC_Projectile>().Init(projectileSpawn.transform.position, projectileSpawn.transform.rotation, direction, damage, 1.0f);

        yield return new WaitForSeconds(fireRate);
        projectileCooldown = false;        
    }
}

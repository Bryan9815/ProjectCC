using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : Entity
{

	// Use this for initialization
	void Start ()
    {
        hp = 3;
        damage = 1;
        speed = 5;
	}
	
	// Update is called once per frame
	void Update ()
    {
        Movement();
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
    }
}

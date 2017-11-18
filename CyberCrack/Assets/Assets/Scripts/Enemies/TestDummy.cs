using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDummy : Entity
{

	// Use this for initialization
	void Start ()
    {
        hp = 3;
        damage = 1;
        speed = 5;
        fireRate = 0.33f;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (hp <= 0)
            Destroy(gameObject);
	}
}

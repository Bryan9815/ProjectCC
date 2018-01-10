﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected float maxHP;
    protected float hp;
    protected float damage;
    protected float speed;
    protected float fireRate;
    protected bool isActive = false;
    protected bool isDead = false;
    protected AudioSource sound;
    protected Animator anim;

    protected List<PowerUp> powerUps = new List<PowerUp>();
    protected Transform powerUpList, target;
	// Use this for initialization
	protected void Init()
    {
        try
        {
            sound = GetComponent<AudioSource>();
            anim = GetComponent<Animator>();

            powerUpList = transform.Find("PowerUps");
            target = transform.Find("Targets");
        }
        catch(Exception e) { Debug.Log(gameObject.name + " could not init properly: " + e); }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public bool GetIsDead()
    {
        return isDead;
    }

    public void ToggleActive(bool active)
    {
        if (active)
            isActive = true;
        else
            isActive = false;
    }

    public void ModifyHP(float mod)
    {
        hp += mod;
    }

    public void ModifyMaxHP(float mod)
    {
        maxHP += mod;
    }

    public void ModifyDamage(float mod)
    {
        damage += mod;
    }

    public void ModifySpeed(float mod)
    {
        speed += mod;
    }

    public void ModifyFireRate(float mod)
    {
        fireRate += mod;
    }

    public void AddPowerUp(PowerUp newPowerUp)
    {
        powerUps.Add(newPowerUp);
        powerUps[powerUps.Count - 1].transform.parent = powerUpList;
        powerUps[powerUps.Count - 1].ActivateEffect();
    }
}

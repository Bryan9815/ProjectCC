using System;
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
    protected float projectileSpeed;
    protected bool mobDamageHigh = false;
    protected bool isActive = true;
    protected bool isDead = false;
    protected int chanceToDropMoney; // The higher the value, the less likely it is for money to drop.
    protected int moneyMin, moneyMax;
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

    public float GetMaxHP()
    {
        return maxHP;
    }

    public float GetHP()
    {
        return hp;
    }

    public float GetDamage()
    {
        return damage;
    }

    public float GetSpeed()
    {
        return speed;
    }

    public float GetFireRate()
    {
        return fireRate;
    }

    public float GetProjectileSpeed()
    {
        return projectileSpeed;
    }

    public bool GetIsDead()
    {
        return isDead;
    }

    protected virtual void Death()
    {
        isDead = true;
        GameController.instance.currentRoom.CheckMobs();

        // Drops

        // Money
        int moneyChance = UnityEngine.Random.Range(0, chanceToDropMoney); // Chance to drop money
        if (moneyChance == 0)
            Money.SpawnMoneyRand(transform.position, transform.parent, moneyMin, moneyMax);

        int hpChance = UnityEngine.Random.Range(0, 4);
        if (hpChance != 3)
            RecoverHP.SpawnHP(transform.position, transform.parent);

        // Power Ups
        if (powerUps.Count > 0)
        {
            List<PowerUp> powerUpsToDrop = new List<PowerUp>();
            foreach (PowerUp upgrade in powerUps)
                powerUpsToDrop.Add(upgrade);

            foreach (PowerUp upgrade in powerUpsToDrop)
                DropPowerUp(upgrade);
        }
    }

    public virtual void ToggleActive(bool active)
    {
        if (active)
            isActive = true;
        else
            isActive = false;
    }

    public virtual void ModifyHP(float mod)
    {
        hp += mod;
    }

    public void ModifyMaxHP(float mod)
    {
        maxHP += mod;
    }

    public void ModifyPlayerDamage(float mod)
    {
        damage += mod;
    }

    public void ModifyMobDamage(bool high)
    {
        mobDamageHigh = high;
    }

    public void ModifySpeed(float mod)
    {
        speed += mod;
    }

    public void ModifyFireRate(float mod)
    {
        fireRate += mod;
    }

    public List<PowerUp> GetPowerUps()
    {
        return powerUps;
    }

    public void AddPowerUp(PowerUp newPowerUp)
    {
        powerUps.Add(newPowerUp);
        powerUps[powerUps.Count - 1].transform.parent = powerUpList;
        powerUps[powerUps.Count - 1].transform.localPosition = Vector3.zero;
        powerUps[powerUps.Count - 1].ActivateEffect();
    }

    public void DropPowerUp(int index)
    {
        PowerUp droppedPowerUp = powerUps[index];
        powerUps.Remove(powerUps[index]);
        droppedPowerUp.Dropped(gameObject.transform.parent);
    }

    public void DropPowerUp(PowerUp upgrade)
    {
        PowerUp droppedPowerUp = upgrade;
        powerUps.Remove(upgrade);
        droppedPowerUp.Dropped(gameObject.transform.parent);
    }

    public void RefreshPowerUp()
    {
        foreach(PowerUp powerUp in powerUps)
        {
            powerUp.ActivateEffect();
        }
    }

    public PowerUp RemoveRandPowerUp()
    {
        int rand = UnityEngine.Random.Range(0, powerUps.Count);

        PowerUp removedPowerUp = powerUps[rand];
        removedPowerUp.DeactivateEffect();
        powerUps.Remove(powerUps[rand]);

        return removedPowerUp;
    }
}

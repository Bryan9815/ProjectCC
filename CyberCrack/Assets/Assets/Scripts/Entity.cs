using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    protected float hp;
    protected float damage;
    protected float speed;
    protected float fireRate;
    protected bool isDead = false;
    protected AudioSource sound;
    protected Animator anim;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ReduceHP(float reduction)
    {
        hp -= reduction;
    }
}

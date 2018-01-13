using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandPowerUp : MonoBehaviour
{
    public List<GameObject> powerUpList = new List<GameObject>();
	// Use this for initialization
	void Start ()
    {
        foreach (GameObject powerUp in Resources.LoadAll("Prefabs/PowerUps"))
            powerUpList.Add(powerUp);

        SpawnRandPowerUp();
	}

    void SpawnRandPowerUp()
    {
        GameObject newPowerUp = Instantiate(powerUpList[Random.Range(0, powerUpList.Count)], transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

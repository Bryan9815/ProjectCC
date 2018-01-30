using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandPowerUp : MonoBehaviour
{
    public List<GameObject> powerUpList = new List<GameObject>();
	// Use this for initialization
	void Start ()
    {
        if (transform.parent.tag == "Object")
        {
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;
            transform.localScale = new Vector3(1, 1, 1);
            Debug.Log("rand power up transform fixed");
        }
        foreach (GameObject powerUp in Resources.LoadAll("Prefabs/PowerUps"))
            powerUpList.Add(powerUp);

        SpawnRandPowerUp();
	}

    void SpawnRandPowerUp()
    {        
        GameObject newPowerUp = Instantiate(powerUpList[Random.Range(0, powerUpList.Count)], transform.position, transform.rotation, transform.parent);
        if (transform.parent.tag == "Object")
        {
            newPowerUp.transform.localPosition = Vector3.zero;
            newPowerUp.transform.localEulerAngles = Vector3.zero;
            newPowerUp.transform.localScale = new Vector3(1, 1, 1);
        }
        Destroy(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

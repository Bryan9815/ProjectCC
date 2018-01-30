using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthContainer : MonoBehaviour
{
    List<GameObject> heartList = new List<GameObject>();
	// Use this for initialization
	void Start ()
    {
		
	}

    public void SpawnHearts(int hp)
    {
        // Clear old hearts
        if (heartList.Count != 0)
        {
            foreach (GameObject heart in heartList)
            {
                Destroy(heart);
            }
            heartList.Clear();
        }

        if(hp < 6)
        {
            for (int i = 0; i < hp; i++)
                heartList.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Heart"), transform));
        }
        else
        {
            for (int i = 0; i < 5; i++)
                heartList.Add(Instantiate(Resources.Load<GameObject>("Prefabs/Heart"), transform));

            for (int j = 0; j < (hp - 5); j++)
                heartList[j].GetComponent<Image>().sprite = Resources.Load<Sprite>("Images/greenHeart");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    int price = 15;
    bool purchased = false;
    Transform power;
	// Use this for initialization
	void Start ()
    {
        StartCoroutine(GetPower());
    }
	
	IEnumerator GetPower()
    {
        yield return new WaitForSeconds(0.1f);

        power = transform.GetChild(0);
        power.GetComponent<Collider2D>().enabled = false;
        power.transform.localPosition = Vector3.zero;
        power.transform.localEulerAngles = Vector3.zero;
        power.transform.localScale = new Vector3(1, 1, 1);

        price = power.GetComponent<PowerUp>().price;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log("Player has insufficient money");
            if(GameController.instance.GetMoney() >= price)
            {
                Debug.Log("Player has enough money");
                if (!purchased)
                {
                    purchased = true;
                    GameController.instance.ModMoney(-price);
                }
                power.parent = transform.parent;
                power.GetComponent<Collider2D>().enabled = true;
                Destroy(gameObject);
            }
        }
    }
}

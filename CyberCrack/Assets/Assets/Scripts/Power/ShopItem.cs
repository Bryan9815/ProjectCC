using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopItem : MonoBehaviour
{
    int price = 15;
    bool purchased = false;
    Transform power;
	// Use this for initialization
	void Start ()
    {
        if (transform.GetChild(0).name == "RandPowerUp")
            StartCoroutine(GetRandPower());
        else
            StartCoroutine(GetPower());
    }

    IEnumerator GetPower()
    {
        yield return new WaitForSeconds(0.1f);

        power = transform.GetChild(0);
        power.GetComponent<Collider2D>().enabled = false;
        price = power.GetComponent<PowerUp>().price;

        transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = price.ToString();
    }
	
	IEnumerator GetRandPower()
    {
        yield return new WaitForSeconds(0.1f);

        power = transform.GetChild(1);
        power.GetComponent<Collider2D>().enabled = false;
        price = power.GetComponent<PowerUp>().price;

        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = price.ToString();
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

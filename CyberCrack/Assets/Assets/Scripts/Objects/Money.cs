using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    int moneyToAdd;

    public void Init(int value)
    {
        moneyToAdd = value;
    }

    public void InitRand(int min, int max)
    {
        moneyToAdd = Random.Range(min, max++);        
    }

    public static void SpawnMoney(Vector3 pos, Transform parent, int value)
    {
        GameObject spawnedCurrency = Instantiate(Resources.Load<GameObject>("Prefabs/Currency"), pos, parent.rotation, parent);
        spawnedCurrency.GetComponent<Money>().Init(value);
    }

    public static void SpawnMoneyRand(Vector3 pos, Transform parent, int min, int max)
    {
        GameObject spawnedCurrency = Instantiate(Resources.Load<GameObject>("Prefabs/Currency"), pos, parent.rotation, parent);
        spawnedCurrency.GetComponent<Money>().InitRand(min, max);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            GameController.instance.ModMoney(moneyToAdd);
            GameData.instance.UpdateData("totalMoney", (GameData.instance.totalMoney + moneyToAdd));
            Destroy(gameObject);
        }
    }
}

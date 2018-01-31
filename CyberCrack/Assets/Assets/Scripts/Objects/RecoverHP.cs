using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecoverHP : MonoBehaviour
{
    int healthToAdd = 1;

    public static void SpawnHP(Vector3 pos, Transform parent)
    {
        GameObject spawnedHP = Instantiate(Resources.Load<GameObject>("Prefabs/RecoverHP"), pos, parent.rotation, parent);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (PlayerCharacter.instance.GetHP() < PlayerCharacter.instance.GetMaxHP())
            {
                PlayerCharacter.instance.ModifyHP(healthToAdd);
                Destroy(gameObject);
            }
        }
    }
}

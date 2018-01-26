using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandEnemy : MonoBehaviour
{

    public List<GameObject> enemyList = new List<GameObject>();
    // Use this for initialization
    void Start()
    {
        foreach (GameObject enemy in Resources.LoadAll("Prefabs/Enemies"))
            enemyList.Add(enemy);

        SpawnRandEnemy();
    }

    void SpawnRandEnemy()
    {
        GameObject newEnemy = Instantiate(enemyList[Random.Range(0, enemyList.Count)], transform.position, transform.rotation, transform.parent);
        Destroy(gameObject);
        foreach (RoomInstance room in GameController.instance.GetComponent<SheetAssigner>().roomList)
            room.RefreshMobList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

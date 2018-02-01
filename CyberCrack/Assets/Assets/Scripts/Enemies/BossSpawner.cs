using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        SpawnScheduledBoss();
        Destroy(gameObject);
    }

    void SpawnScheduledBoss()
    {
        switch(GameController.instance.GetCurrentLevel())
        {
            case 1:
                GameObject hex = Instantiate(Resources.Load<GameObject>("Prefabs/Bosses/Hex"), transform.position, transform.rotation, transform.parent);
                break;
            case 2:
                GameObject movingBullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bosses/MovingBullet"), transform.position, transform.rotation, transform.parent);
                break;
            case 3:
                GameObject teleporter = Instantiate(Resources.Load<GameObject>("Prefabs/Bosses/TeleportParent"), transform.position, transform.rotation, transform.parent);
                break;
        }

        foreach (RoomInstance room in GameController.instance.GetComponent<SheetAssigner>().roomList)
            room.RefreshMobList();
    }

    // Update is called once per frame
    void Update()
    {

    }
}

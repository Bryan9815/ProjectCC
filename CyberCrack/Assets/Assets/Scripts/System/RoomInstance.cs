﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInstance : MonoBehaviour
{
	public Texture2D tex;
	[HideInInspector]
	public Vector2 gridPos;
    public Room.roomType type;
    [HideInInspector]
    public bool doorTop, doorBot, doorLeft, doorRight;
    [HideInInspector]
    public bool playerInside = false;
    [HideInInspector]
    public GameObject doorU, doorD, doorL, doorR;
    [SerializeField]
	GameObject door, doorWall;
	[SerializeField]
	ColorToGameObject[] mappings;
	float tileSize = 16;
	Vector2 roomSizeInTiles = new Vector2(9,17);

    [HideInInspector]
    public List<GameObject> mobList = new List<GameObject>();
    [HideInInspector]
    public List<Door> doorList = new List<Door>();

	public void Setup(Texture2D _tex, Vector2 _gridPos, Room.roomType _type, bool _doorTop, bool _doorBot, bool _doorLeft, bool _doorRight)
    {
		tex = _tex;
		gridPos = _gridPos;
		type = _type;
		doorTop = _doorTop;
		doorBot = _doorBot;
		doorLeft = _doorLeft;
		doorRight = _doorRight;
        MakeDoors();
		GenerateRoomTiles();
	}

    private void Update()
    {
        try
        {
            if (mobList.Count == 0)
            {
                foreach (Door door in doorList)
                    door.ToggleActive(true);
            }
            else
            {
                foreach (GameObject mob in mobList)
                {
                    if (mob.GetComponent<Entity>().GetIsDead())
                    {
                        mobList.Remove(mob);
                        Destroy(mob);
                    }
                }
            }
        }
        catch(Exception e) { Debug.Log(e); }
    }

    void MakeDoors()
    {
		//top door, get position then spawn
		Vector3 spawnPos = transform.position + Vector3.up*(roomSizeInTiles.y/4 * tileSize) - Vector3.up*(tileSize/4);
        if(doorTop)
        {
            GameObject spawnedDoor = Instantiate(Resources.Load<GameObject>("Prefabs/DoorTrigger"), spawnPos, Quaternion.identity, transform);
            spawnedDoor.name = "doorTop";
            doorU = spawnedDoor;
            doorList.Add(spawnedDoor.GetComponent<Door>());
        }
        else
            Instantiate(doorWall, spawnPos, Quaternion.identity).transform.parent = transform;
        
		//bottom door
		spawnPos = transform.position + Vector3.down*(roomSizeInTiles.y/4 * tileSize) - Vector3.down*(tileSize/4);
        if (doorBot)
        {
            GameObject spawnedDoor = Instantiate(Resources.Load<GameObject>("Prefabs/DoorTrigger"), spawnPos, Quaternion.identity, transform);
            spawnedDoor.name = "doorBot";
            doorD = spawnedDoor;
            doorList.Add(spawnedDoor.GetComponent<Door>());
        }
        else
            Instantiate(doorWall, spawnPos, Quaternion.identity).transform.parent = transform;

		//right door
		spawnPos = transform.position + Vector3.right*(roomSizeInTiles.x * tileSize) - Vector3.right*(tileSize);
        if (doorRight)
        {
            GameObject spawnedDoor = Instantiate(Resources.Load<GameObject>("Prefabs/DoorTrigger"), spawnPos, Quaternion.identity, transform);
            spawnedDoor.name = "doorRight";
            doorR = spawnedDoor;
            doorList.Add(spawnedDoor.GetComponent<Door>());
        }
        else
            Instantiate(doorWall, spawnPos, Quaternion.identity).transform.parent = transform;

        //left door
        spawnPos = transform.position + Vector3.left*(roomSizeInTiles.x * tileSize) - Vector3.left*(tileSize);
        if (doorLeft)
        {
            GameObject spawnedDoor = Instantiate(Resources.Load<GameObject>("Prefabs/DoorTrigger"), spawnPos, Quaternion.identity, transform);
            spawnedDoor.name = "doorLeft";
            doorL = spawnedDoor;
            doorList.Add(spawnedDoor.GetComponent<Door>());
        }
        else
            Instantiate(doorWall, spawnPos, Quaternion.identity).transform.parent = transform;
    }

	void GenerateRoomTiles()
    {
        //loop through every pixel of the texture
        for (int x = 0; x < tex.width; x++)
        {
            for (int y = 0; y < tex.height; y++)
            {
                GenerateTile(x, y);
            }
        }
	}

	void GenerateTile(int x, int y)
    {
		Color pixelColor = tex.GetPixel(x,y);
		//skip clear spaces in texture
		if (pixelColor.a == 0)
        {
			return;
		}
		//find the color to math the pixel
		foreach (ColorToGameObject mapping in mappings)
        {
			if (mapping.color.Equals(pixelColor))
            {
				Vector3 spawnPos = PositionFromTileGrid(x,y);
				GameObject entity = Instantiate<GameObject>(mapping.prefab, spawnPos, Quaternion.identity, this.transform);
                if (entity.tag == "Enemy")
                {
                    mobList.Add(entity);
                    entity.GetComponent<Entity>().ToggleActive(false);
                }
			}
		}
	}

	Vector3 PositionFromTileGrid(int x, int y)
    {
		Vector3 ret;
		//find difference between the corner of the texture and the center of this object
		Vector3 offset = new Vector3((-roomSizeInTiles.x + 1)*tileSize, (roomSizeInTiles.y/4)*tileSize - (tileSize/4), 0);
		//find scaled up position at the offset
		ret = new Vector3(tileSize * (float) x, -tileSize * (float) y, 0) + offset + transform.position;
		return ret;
	}

    void ToggleRoomActive()
    {
        foreach (GameObject mob in mobList)
            mob.GetComponent<Entity>().ToggleActive(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Debug.Log(mobList);
            ToggleRoomActive();
            GameController.instance.currentRoom = GetComponent<RoomInstance>();
        }
    }
}

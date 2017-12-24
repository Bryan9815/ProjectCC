﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetAssigner : MonoBehaviour
{
    public GameObject roomParent;

    [SerializeField]
	Texture2D[] sheetsNormal;
	[SerializeField]
	GameObject RoomObj;

    public Vector2 roomDimensions = new Vector2(16*17,16*9);
	public Vector2 gutterSize = new Vector2(16*9,16*4);

	public void Assign(Room[,] rooms)
    {
        int roomNumber = 0;
        foreach (Room room in rooms)
        {
            //skip point where there is no room
            if (room == null)
            {
                continue;
            }

            //pick a random index for the array
            int index = Mathf.RoundToInt(Random.value * (sheetsNormal.Length - 1));
            //find position to place room
            Vector3 pos = new Vector3(room.gridPos.x * (roomDimensions.x + gutterSize.x), room.gridPos.y * (roomDimensions.y + gutterSize.y), 0);
            RoomInstance myRoom = Instantiate(RoomObj, pos, Quaternion.identity).GetComponent<RoomInstance>();
            if (room.type == Room.roomType.enter)
            {
                GameObject player = Instantiate(Resources.Load<GameObject>("Prefabs/PlayerCharacter"));
                GameObject testEnemy = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Burst"), myRoom.transform);
            }
            myRoom.Setup(sheetsNormal[index], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
            myRoom.thisRoom = room;
            GetComponent<LevelGeneration>().roomList.Add(room);

            // Name the room and parent it to canvas
            roomNumber++;
            myRoom.name = "Room" + roomNumber;
            myRoom.transform.parent = roomParent.transform;
        }
    }
}

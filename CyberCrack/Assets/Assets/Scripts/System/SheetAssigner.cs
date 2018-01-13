using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheetAssigner : MonoBehaviour
{
    public GameObject roomParent;

    [SerializeField]
	List<Texture2D> normalTemplates = new List<Texture2D>();
    [SerializeField]
    List<Texture2D> bossTemplates = new List<Texture2D>();
    [SerializeField]
    List<Texture2D> upgradeTemplates = new List<Texture2D>();
    [SerializeField]
	GameObject RoomObj;

    public Vector2 roomDimensions = new Vector2(16*17,16*9);
	public Vector2 gutterSize = new Vector2(16*9,16*4);

    private void Awake()
    {
        foreach (Texture2D template in Resources.LoadAll<Texture2D>("roomTemplates/Normal"))
        {
            normalTemplates.Add(template);
        }
        //foreach (Texture2D template in Resources.LoadAll<Texture2D>("roomTemplates/Boss"))
        //{
        //    bossTemplates.Add(template);
        //}
        //foreach (Texture2D template in Resources.LoadAll<Texture2D>("roomTemplates/Upgrade"))
        //{
        //    upgradeTemplates.Add(template);
        //}
    }

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
            int randTemplate;
            //find position to place room
            Vector3 pos = new Vector3(room.gridPos.x * (roomDimensions.x + gutterSize.x), room.gridPos.y * (roomDimensions.y + gutterSize.y), 0);
            RoomInstance myRoom = Instantiate(RoomObj, pos, Quaternion.identity).GetComponent<RoomInstance>();
            switch (room.type)
            {
                case Room.roomType.enter:
                    myRoom.Setup(normalTemplates[0], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
                    //Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Hex"), myRoom.transform);
                    Instantiate(Resources.Load<GameObject>("Prefabs/RandPowerUp"), myRoom.transform);
                    break;
                case Room.roomType.normal:
                    randTemplate = Mathf.RoundToInt(Random.value * (normalTemplates.Count - 1));
                    myRoom.Setup(normalTemplates[randTemplate], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
                    break;
                case Room.roomType.boss:
                    randTemplate = Mathf.RoundToInt(Random.value * (bossTemplates.Count - 1));
                    myRoom.Setup(bossTemplates[randTemplate], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
                    break;
                case Room.roomType.upgrade:
                    randTemplate = Mathf.RoundToInt(Random.value * (upgradeTemplates.Count - 1));
                    myRoom.Setup(upgradeTemplates[randTemplate], room.gridPos, room.type, room.doorTop, room.doorBot, room.doorLeft, room.doorRight);
                    break;
            }

            // Name the room and parent it to canvas
            roomNumber++;
            myRoom.name = "Room" + roomNumber;
            myRoom.transform.parent = roomParent.transform;
        }
    }

    IEnumerator Debug(float seconds, RoomInstance myRoom)
    {
        yield return new WaitForSeconds(seconds);
        Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Hex"), myRoom.transform);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
	public Vector2 gridPos;
    public enum roomType { normal, enter, upgrade, item, boss }
    public roomType type;
	public bool doorTop, doorBot, doorLeft, doorRight;

	public Room(Vector2 _gridPos, roomType _type)
    {
		gridPos = _gridPos;
		type = _type;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGeneration : MonoBehaviour
{
	public Vector2 worldSize = new Vector2(4,4);
    public int numberOfRooms;
    Room[,] rooms;
	List<Vector2> takenPositions = new List<Vector2>();
    [HideInInspector]
    int gridSizeX, gridSizeY; 
	public GameObject roomWhiteObj;
	public Transform mapRoot;

    bool correctSpawn;

    void Start ()
    {
        correctSpawn = true;

        if (numberOfRooms >= (worldSize.x * 2) * (worldSize.y * 2))
        { 
            // make sure we dont try to make more rooms than can fit in our grid
			numberOfRooms = Mathf.RoundToInt((worldSize.x * 2) * (worldSize.y * 2));
		}
		gridSizeX = Mathf.RoundToInt(worldSize.x); //note: these are half-extents
		gridSizeY = Mathf.RoundToInt(worldSize.y);

		CreateRooms(); //lays out the actual map
		SetRoomDoors(); //assigns the doors where rooms would connect

        if (takenPositions.Count < numberOfRooms)
            correctSpawn = false;

        if (correctSpawn)
        {
            GetComponent<SheetAssigner>().Assign(rooms); //passes room info to another script which handles generatating the level geometry
            DrawMap(); //instantiates objects to make up a map
        }
        else
        {
            Debug.Log("Rooms with zero neighbors, restarting level generation");
            takenPositions.Clear();
            Start();
        }
    }

    void CreateRooms()
    {
		//setup
		rooms = new Room[gridSizeX * 4,gridSizeY * 4];
		rooms[gridSizeX,gridSizeY] = new Room(Vector2.zero, Room.roomType.enter);

		takenPositions.Insert(0,Vector2.zero);
		Vector2 checkPos = Vector2.zero;

		//magic numbers
		float randomCompare = 0.2f, randomCompareStart = 0.2f, randomCompareEnd = 0.01f;
		//add rooms
		for (int i = 1; i <= numberOfRooms; i++)
        {
            // Leave the last 2 rooms for special rooms (Upgrade, Shop, Boss)
            if (i < numberOfRooms - 2)
            {
                float randomPerc = ((float)i) / (((float)numberOfRooms - 1));
                randomCompare = Mathf.Lerp(randomCompareStart, randomCompareEnd, randomPerc);
                //grab new position
                checkPos = NewPosition();
                //test new position
                if (NumberOfNeighbors(checkPos, takenPositions) > 1 && UnityEngine.Random.value > randomCompare)
                {
                    int iterations = 0;
                    do
                    {
                        checkPos = SelectiveNewPosition();
                        iterations++;
                    }
                    while (NumberOfNeighbors(checkPos, takenPositions) > 1 && iterations < 100);
                    if (iterations >= 50)
                        Debug.Log("error: could not create with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
                }
                //finalize position
                rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, Room.roomType.normal);
                takenPositions.Add(checkPos);
            }
            else // Upgrade, Shop, Boss
            {
                //checkPos = NewPosition();
                //// Make sure it has only one neighbor
                //if (NumberOfNeighbors(checkPos, takenPositions) != 1)
                //{
                //    int iterations = 0;
                //    do
                //    {
                //        iterations++;
                //        checkPos = SelectiveNewPosition();
                //        if (NumberOfNeighbors(checkPos, takenPositions) == 1)
                //            break;
                //    }
                //    while (NumberOfNeighbors(checkPos, takenPositions) != 1 && iterations < 100);
                //    if (iterations >= 50)
                //        Debug.Log("error: could not create special room with fewer neighbors than : " + NumberOfNeighbors(checkPos, takenPositions));
                //}

                checkPos = SpawnSpecialRoom();

                //finalize position
                try
                {
                    if (i == numberOfRooms - 1)
                        rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, Room.roomType.upgrade);
                    else if (i == numberOfRooms)
                        rooms[(int)checkPos.x + gridSizeX, (int)checkPos.y + gridSizeY] = new Room(checkPos, Room.roomType.boss);
                }
                catch(Exception e) { Debug.Log(e); }
                takenPositions.Add(checkPos);
            }
        }	
	}

	Vector2 NewPosition()
    {
		int x = 0, y = 0;
		Vector2 checkingPos = Vector2.zero;
		do
        {
			int index = Mathf.RoundToInt(UnityEngine.Random.value * (takenPositions.Count - 1)); // pick a random room
			x = (int) takenPositions[index].x;//capture its x, y position
			y = (int) takenPositions[index].y;
			bool UpDown = (UnityEngine.Random.value < 0.5f);//randomly pick wether to look on hor or vert axis
			bool positive = (UnityEngine.Random.value < 0.5f);//pick whether to be positive or negative on that axis
			if (UpDown)
            { 
                //find the position bnased on the above bools
				if (positive)
                {
					y += 1;
				}
                else
                {
					y -= 1;
				}
			}
            else
            {
				if (positive)
                {
					x += 1;
				}
                else
                {
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}
        while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY); //make sure the position is valid
		return checkingPos;
	}

	Vector2 SelectiveNewPosition()
    { 
        // method differs from the above in the two commented ways
		int index = 0, inc = 0;
		int x =0, y =0;

		Vector2 checkingPos = Vector2.zero;
		do
        {
			inc = 0;
			do
            { 
				//instead of getting a room to find an adject empty space, we start with one that only has one neighbor. This will make it more likely that it returns a room that branches out
				index = Mathf.RoundToInt(UnityEngine.Random.value * (takenPositions.Count - 1));
				inc ++;
			}
            while (NumberOfNeighbors(takenPositions[index], takenPositions) > 1 && inc < 100);

			x = (int) takenPositions[index].x;
			y = (int) takenPositions[index].y;
			bool UpDown = (UnityEngine.Random.value < 0.5f);
			bool positive = (UnityEngine.Random.value < 0.5f);

			if (UpDown)
            {
				if (positive)
                {
					y += 1;
				}
                else
                {
					y -= 1;
				}
			}
            else
            {
				if (positive)
                {
					x += 1;
				}
                else
                {
					x -= 1;
				}
			}
			checkingPos = new Vector2(x,y);
		}
        while (takenPositions.Contains(checkingPos) || x >= gridSizeX || x < -gridSizeX || y >= gridSizeY || y < -gridSizeY);

        if (inc >= 100)
        { // break loop if it takes too long: this loop isnt garuanteed to find solution, which is fine for this
			print("Error: could not find position with only one neighbor");
		}
		return checkingPos;
	}

    Vector2 SpawnSpecialRoom()
    {
        bool matchFound = false;
        Vector2 newPos = Vector2.zero;

        do
        {
            foreach (Vector2 occupiedPos in takenPositions)
            {
                if (NumberOfNeighbors(occupiedPos, takenPositions) == 1)
                {
                    // set checkPos to one of the unoccupied sides of the taken position
                    newPos = FillUnoccupiedSide(occupiedPos);
                    matchFound = true;

                    // check new position to see if it has multiple neighbors
                    if (NumberOfNeighbors(newPos, takenPositions) != 1)
                    {
                        matchFound = false;
                        continue;
                    }

                    break;
                }
            }
        }
        while (!matchFound);

        return newPos;
    }

	int NumberOfNeighbors(Vector2 checkingPos, List<Vector2> usedPositions)
    {
		int ret = 0; // start at zero, add 1 for each side there is already a room
		if (usedPositions.Contains(checkingPos + Vector2.right))
        { //using Vector.[direction] as short hands, for simplicity
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.left))
        {
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.up))
        {
			ret++;
		}
		if (usedPositions.Contains(checkingPos + Vector2.down))
        {
			ret++;
		}
		return ret;
	}

    Vector2 FillUnoccupiedSide(Vector2 checkingPos)
    {
        List<Vector2> emptySides = new List<Vector2>();

        if (!takenPositions.Contains(checkingPos + Vector2.right))
        {
            emptySides.Add(checkingPos + Vector2.right);
        }
        if (!takenPositions.Contains(checkingPos + Vector2.left))
        {
            emptySides.Add(checkingPos + Vector2.left);
        }
        if (!takenPositions.Contains(checkingPos + Vector2.up))
        {
            emptySides.Add(checkingPos + Vector2.up);
        }
        if (!takenPositions.Contains(checkingPos + Vector2.down))
        {
            emptySides.Add(checkingPos + Vector2.down);
        }

        if (emptySides.Count > 1)
            checkingPos = emptySides[UnityEngine.Random.Range(0, emptySides.Count)];
        else
            checkingPos = emptySides[0];

        return checkingPos;
    }

	public void DrawMap()
    {
		foreach (Room room in rooms)
        {
			if (room == null)
            {
				continue; //skip where there is no room
			}

			Vector2 drawPos = room.gridPos;
			drawPos.x *= 16*16;//aspect ratio of map sprite
			drawPos.y *= 8*16;
			//create map obj and assign its variables
			MapSpriteSelector mapper = GameObject.Instantiate(roomWhiteObj, drawPos, Quaternion.identity).GetComponent<MapSpriteSelector>();
            mapper.transform.localScale *= 16;
            mapper.type = room.type;
			mapper.up = room.doorTop;
			mapper.down = room.doorBot;
			mapper.right = room.doorRight;
			mapper.left = room.doorLeft;
			mapper.gameObject.transform.parent = mapRoot;
		}
	}

    public void ClearMap()
    {
        for(int i = 0; i < mapRoot.childCount - 1; i++)
        {
            Destroy(mapRoot.GetChild(i).gameObject);
        }
    }

	void SetRoomDoors()
    {
        for (int x = 0; x < ((gridSizeX * 4)); x++)
        {
            for (int y = 0; y < ((gridSizeY * 4)); y++)
            {
                if (!correctSpawn)
                    break;

                if (rooms[x, y] == null)
                {
                    continue;
                }
                Vector2 gridPosition = new Vector2(x, y);
                if (y - 1 < 0)
                { //check above
                    rooms[x, y].doorBot = false;
                }
                else
                {
                    rooms[x, y].doorBot = (rooms[x, y - 1] != null);
                }
                if (y + 1 >= gridSizeY * 2)
                { //check bellow
                    rooms[x, y].doorTop = false;
                }
                else
                {
                    rooms[x, y].doorTop = (rooms[x, y + 1] != null);
                }
                if (x - 1 < 0)
                { //check left
                    rooms[x, y].doorLeft = false;
                }
                else
                {
                    rooms[x, y].doorLeft = (rooms[x - 1, y] != null);
                }
                if (x + 1 >= gridSizeX * 2)
                { //check right
                    rooms[x, y].doorRight = false;
                }
                else
                {
                    rooms[x, y].doorRight = (rooms[x + 1, y] != null);
                }

                if (!rooms[x, y].doorTop && !rooms[x, y].doorBot && !rooms[x, y].doorLeft && !rooms[x, y].doorRight)
                {
                    Debug.Log("Corrupted spawn");
                    correctSpawn = false;
                    break;
                }
                else
                    correctSpawn = true;
            }
        }
    }

    public void RestartLevelGeneration()
    {
        Debug.Log("Level generation restarted by Sheet Assigner");
        for (int i = 0; i < GameController.instance.gameplayCanvas.Find("Rooms").childCount; i++)
            Destroy(GameController.instance.gameplayCanvas.Find("Rooms").GetChild(i).gameObject);

        takenPositions.Clear();
        Start();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Transform rooms;
    float roomVerticalMove, roomHorizontalMove, playerVerticalMove, playerHorizontalMove;

    GameObject minimap, player;
    // Use this for initialization
    void Start ()
    {
        rooms = transform.parent.parent;
        roomVerticalMove = 208 * transform.parent.localScale.x;
        roomHorizontalMove = 416 * transform.parent.localScale.x;
        playerVerticalMove = 96;
        playerHorizontalMove = 224;

        minimap = GameController.instance.gameplayCanvas.GetChild(1).gameObject;
        player = GameController.instance.playerCharacter;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    IEnumerator UpdateMap()
    {
        // Set the previous active room to inactive
        for(int i = 0; i < GameController.instance.gameplayCanvas.GetChild(1).childCount - 1; i++)
        {
            minimap.transform.GetChild(i).GetComponent<RoomInstance>().playerInside = false;
        }

        yield return new WaitForSeconds(0.5f);

        // Set the new room to active
        GameController.instance.GetComponent<LevelGeneration>().ClearMap();
        GameController.instance.GetComponent<LevelGeneration>().DrawMap();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            switch(name)
            {
                case "doorTop":
                    collision.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y - playerVerticalMove, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x, rooms.localPosition.y - roomVerticalMove, rooms.localPosition.z);
                    UpdateMap();
                    break;
                case "doorBot":
                    collision.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + playerVerticalMove, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x, rooms.localPosition.y + roomVerticalMove, rooms.localPosition.z);
                    UpdateMap();
                    break;
                case "doorRight":
                    collision.transform.position = new Vector3(collision.transform.position.x - playerHorizontalMove, collision.transform.position.y, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x - roomHorizontalMove, rooms.localPosition.y, rooms.localPosition.z);
                    UpdateMap();
                    break;
                case "doorLeft":
                    collision.transform.position = new Vector3(collision.transform.position.x + playerHorizontalMove, collision.transform.position.y, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x + roomHorizontalMove, rooms.localPosition.y, rooms.localPosition.z);
                    UpdateMap();
                    break;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Transform rooms;
    float roomVerticalMove, roomHorizontalMove, playerVerticalMove, playerHorizontalMove;
    // Use this for initialization
    void Start ()
    {
        rooms = transform.parent.parent;
        roomVerticalMove = 208 * transform.parent.localScale.x;
        roomHorizontalMove = 416 * transform.parent.localScale.x;
        playerVerticalMove = 96;
        playerHorizontalMove = 224;
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            switch(name)
            {
                case "doorTop":
                    collision.enabled = false;
                    Debug.Log("moving up");
                    collision.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y - playerVerticalMove, collision.transform.position.z);
                    Debug.Log("done moving up");
                    rooms.localPosition = new Vector3(rooms.localPosition.x, rooms.localPosition.y - roomVerticalMove, rooms.localPosition.z);
                    Debug.Log("Room moved");
                    collision.enabled = true;
                    break;
                case "doorBot":
                    collision.enabled = false;
                    collision.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + playerVerticalMove, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x, rooms.localPosition.y + roomVerticalMove, rooms.localPosition.z);
                    collision.enabled = true;
                    break;
                case "doorRight":
                    collision.transform.position = new Vector3(collision.transform.position.x - playerHorizontalMove, collision.transform.position.y, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x - roomHorizontalMove, rooms.localPosition.y, rooms.localPosition.z);
                    break;
                case "doorLeft":
                    collision.transform.position = new Vector3(collision.transform.position.x + playerHorizontalMove, collision.transform.position.y, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x + roomHorizontalMove, rooms.localPosition.y, rooms.localPosition.z);
                    break;
            }
        }
    }
}

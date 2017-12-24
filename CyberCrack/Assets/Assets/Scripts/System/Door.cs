using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    Transform rooms;
    GameObject minimapIcon, player;
    float roomVerticalMove, roomHorizontalMove, playerVerticalMove, playerHorizontalMove, mMapIconVerticalMove, mMapIconHorizonalMove;
    // Use this for initialization
    void Start ()
    {
        rooms = transform.parent.parent;

        minimapIcon = GameController.instance.GetComponent<LevelGeneration>().mapRoot.transform.GetChild(0).gameObject;
        player = GameController.instance.playerCharacter;

        roomVerticalMove = 208 * transform.parent.localScale.x;
        roomHorizontalMove = 416 * transform.parent.localScale.x;

        playerVerticalMove = 96;
        playerHorizontalMove = 224;

        mMapIconVerticalMove = 53f;
        mMapIconHorizonalMove = 107f;
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
                    collision.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y - playerVerticalMove, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x, rooms.localPosition.y - roomVerticalMove, rooms.localPosition.z);
                    minimapIcon.transform.localPosition = new Vector3(minimapIcon.transform.localPosition.x, minimapIcon.transform.localPosition.y + mMapIconVerticalMove, minimapIcon.transform.localPosition.z);
                    break;
                case "doorBot":
                    collision.transform.position = new Vector3(collision.transform.position.x, collision.transform.position.y + playerVerticalMove, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x, rooms.localPosition.y + roomVerticalMove, rooms.localPosition.z);
                    minimapIcon.transform.localPosition = new Vector3(minimapIcon.transform.localPosition.x, minimapIcon.transform.localPosition.y - mMapIconVerticalMove, minimapIcon.transform.localPosition.z);
                    break;
                case "doorRight":
                    collision.transform.position = new Vector3(collision.transform.position.x - playerHorizontalMove, collision.transform.position.y, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x - roomHorizontalMove, rooms.localPosition.y, rooms.localPosition.z);
                    minimapIcon.transform.localPosition = new Vector3(minimapIcon.transform.localPosition.x + mMapIconHorizonalMove, minimapIcon.transform.localPosition.y, minimapIcon.transform.localPosition.z);
                    break;
                case "doorLeft":
                    collision.transform.position = new Vector3(collision.transform.position.x + playerHorizontalMove, collision.transform.position.y, collision.transform.position.z);
                    rooms.localPosition = new Vector3(rooms.localPosition.x + roomHorizontalMove, rooms.localPosition.y, rooms.localPosition.z);
                    minimapIcon.transform.localPosition = new Vector3(minimapIcon.transform.localPosition.x - mMapIconHorizonalMove, minimapIcon.transform.localPosition.y, minimapIcon.transform.localPosition.z);
                    break;
            }
        }
    }
}

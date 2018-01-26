using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    bool isActive = false;
    public Color32 startColor;

    Transform rooms;
    GameObject minimapIcon, player;
    float roomVerticalMove, roomHorizontalMove, playerVerticalMove, playerHorizontalMove, mMapIconVerticalMove, mMapIconHorizonalMove;
    // Use this for initialization
    void Start ()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
        GetComponent<Collider2D>().isTrigger = false;

        rooms = transform.parent.parent;

        minimapIcon = GameController.instance.GetComponent<LevelGeneration>().mapRoot.transform.GetChild(0).gameObject;
        player = PlayerCharacter.instance.gameObject;

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

    public void ToggleActive(bool active)
    {
        if (active)
        {
            GetComponent<SpriteRenderer>().color = startColor;
            GetComponent<Collider2D>().isTrigger = true;
            isActive = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = Color.gray;
            GetComponent<Collider2D>().isTrigger = false;
            isActive = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isActive)
        {
            if (collision.tag == "Player")
            {
                switch (name)
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
}

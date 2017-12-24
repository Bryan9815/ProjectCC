using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour
{
	
	public Sprite 	spU, spD, spR, spL,
			spUD, spRL, spUR, spUL, spDR, spDL,
			spULD, spRUL, spDRU, spLDR, spUDRL;

	public bool up, down, left, right, playerInside;
    public Room.roomType type;
    public Color emptyColor, activeColor;
	Color mainColor;
	SpriteRenderer rend;

	public void Start ()
    {
		rend = GetComponent<SpriteRenderer>();
		mainColor = emptyColor;
		PickSprite();
		PickColor();
	}

	void PickSprite()
    { 
        //picks correct sprite based on the four door bools
		if (up)
        {
			if (down)
            {
				if (right)
                {
					if (left)
                    {
						rend.sprite = spUDRL;
					}
                    else
                    {
						rend.sprite = spDRU;
					}
				}
                else if (left)
                {
					rend.sprite = spULD;
				}
                else
                {
					rend.sprite = spUD;
				}
			}
            else
            {
				if (right)
                {
					if (left)
                    {
						rend.sprite = spRUL;
					}
                    else
                    {
						rend.sprite = spUR;
					}
				}
                else if (left)
                {
					rend.sprite = spUL;
				}
                else
                {
					rend.sprite = spU;
				}
			}
			return;
		}

		if (down)
        {
			if (right)
            {
				if(left)
                {
					rend.sprite = spLDR;
				}
                else
                {
					rend.sprite = spDR;
				}
			}
            else if (left)
            {
				rend.sprite = spDL;
			}
            else
            {
				rend.sprite = spD;
			}
			return;
		}

		if (right)
        {
			if (left)
            {
				rend.sprite = spRL;
			}
            else
            {
				rend.sprite = spR;
			}
		}
        else
        {
			rend.sprite = spL;
		}
	}

	void PickColor()
    {
        //changes color based on what type the room is
        //switch(type)
        //{
        //    case Room.roomType.normal:
        //        mainColor = emptyColor;
        //        break;
        //    case Room.roomType.enter:
        //        mainColor = activeColor;
        //        break;
        //    case Room.roomType.item:
        //        break;
        //    default:
        //        mainColor = emptyColor;
        //        break;
        //}

        if (playerInside)
            mainColor = activeColor;
        else
            mainColor = emptyColor;

        rend.color = mainColor;
    }
}
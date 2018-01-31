using System;
using UnityEngine;

public class MapSpriteSelector : MonoBehaviour
{
	
	public Sprite 	spU, spD, spR, spL,
			spUD, spRL, spUR, spUL, spDR, spDL,
			spULD, spRUL, spDRU, spLDR, spUDRL;

	public bool up, down, left, right, playerInside;
    public Room.roomType type;
	Color mainColor;
	SpriteRenderer rend;

	public void Start ()
    {
		rend = GetComponent<SpriteRenderer>();
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
        switch (type)
        {
            case Room.roomType.normal:
                mainColor = Color.white;
                break;
            case Room.roomType.enter:
                mainColor = Color.white;
                break;
            case Room.roomType.boss:
                mainColor = Color.red;
                break;
            case Room.roomType.upgrade:
                mainColor = Color.cyan;
                break;
            case Room.roomType.shop:
                mainColor = Color.yellow;
                break;
            default:
                mainColor = Color.white;
                break;
        }
    }

    public void ChangeColor(bool active)
    {
        try
        {
            if (active)
                rend.color = Color.green;
            else
                rend.color = mainColor;
        }
        catch(Exception e) { Debug.Log("MapSpriteSelector ChangeColor: " + e); }
    }
}
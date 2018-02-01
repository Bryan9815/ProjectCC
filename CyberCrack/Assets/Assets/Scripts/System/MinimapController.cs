using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapController : MonoBehaviour
{
    List<MapSpriteSelector> minimapSprites = new List<MapSpriteSelector>();
    bool childTooBig = false;
	// Use this for initialization
	void Start ()
    {

	}

    public void RefreshMiniMapSpriteList()
    {
        childTooBig = false;

        if (minimapSprites.Count > 0)
            minimapSprites.Clear();

        for(int i = 0; i < transform.GetChild(1).childCount; i++)
        {
            minimapSprites.Add(transform.GetChild(1).GetChild(i).GetComponent<MapSpriteSelector>());

            if (transform.GetChild(1).GetChild(i).localPosition.x > 500 || transform.GetChild(1).GetChild(i).localPosition.x < -500)
            {
                Debug.Log("minimap minimized");
                childTooBig = true;
            }
        }
    }

    public void RefreshMiniMapColors()
    {
        RefreshMiniMapSpriteList();
        for (int i = 0; i < GameController.instance.GetComponent<SheetAssigner>().roomList.Count; i++)
        {
            minimapSprites[i].ChangeColor(GameController.instance.GetComponent<SheetAssigner>().roomList[i].playerInside, GameController.instance.GetComponent<SheetAssigner>().roomList[i].playerDiedHere);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (minimapSprites.Count > GameController.instance.GetComponent<SheetAssigner>().roomList.Count)
            RefreshMiniMapSpriteList();

        if (childTooBig)
            transform.localScale = new Vector3(0.75f, 0.75f, 0.75f);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
}

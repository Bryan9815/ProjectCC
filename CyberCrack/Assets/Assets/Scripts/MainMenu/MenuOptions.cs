using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuOptions : MonoBehaviour
{
    Transform selector;
    int selectNum;
	// Use this for initialization
	void Start ()
    {
        selector = transform.GetChild(1);
        selectNum = 0;
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}
}

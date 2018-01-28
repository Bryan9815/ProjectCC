using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    Vector3 original;
	// Use this for initialization
	void Start ()
    {

	}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            transform.localPosition = new Vector3(0, 2000, transform.localPosition.z);
        else if (Input.GetKeyUp(KeyCode.Space))
            transform.localPosition = new Vector3(0, 0, transform.localPosition.z);
    }
}

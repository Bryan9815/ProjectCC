using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(moveCam());
	}
	
	IEnumerator moveCam()
    {
        yield return new WaitForSeconds(0.1f);
        transform.position += new Vector3(0, 2000, 0);
    }
}

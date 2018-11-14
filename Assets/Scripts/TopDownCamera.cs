using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x + (30.0f * Time.deltaTime), Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x - (30.0f * Time.deltaTime), Camera.main.transform.position.y, Camera.main.transform.position.z);
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + (30.0f * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z - (30.0f * Time.deltaTime));
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y - (20.0f * Time.deltaTime), Camera.main.transform.position.z);
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y + (20.0f * Time.deltaTime), Camera.main.transform.position.z);
        }
    }
}

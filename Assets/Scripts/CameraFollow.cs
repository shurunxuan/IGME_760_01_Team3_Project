using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    GameObject player;
    bool follow;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        follow = true;
        Reset();
        transform.LookAt(player.transform);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            follow = false;
        }

        if (follow)
        {
            Reset();
        } else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z - 25.0f * Time.deltaTime);
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + 25.0f * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.position = new Vector3(this.transform.position.x - 25.0f * Time.deltaTime, this.transform.position.y, this.transform.position.z);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.position = new Vector3(this.transform.position.x + 25.0f * Time.deltaTime, this.transform.position.y, this.transform.position.z);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            follow = true;
            Reset();
        }
	}

    private void Reset()
    {
        Vector3 camPos = player.transform.position;
        camPos.x += 20.0f;
        camPos.y += 20.0f;
        this.transform.position = camPos;
    }
}

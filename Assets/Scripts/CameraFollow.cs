using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject Player;
    public float Speed;
    private bool follow;
    private Vector3 offset;
    private Vector3 localRight;
    private Vector3 localForward;

    // Use this for initialization
    void Start () {
        follow = true;
        localRight = transform.right;
        localForward = Vector3.Cross(transform.right, Vector3.up);
        offset = transform.position - Player.transform.position;
        //transform.LookAt(Player.transform);
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

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
                this.transform.position -= localRight * Speed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                this.transform.position += localRight * Speed;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.position += localForward * Speed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.position -= localForward * Speed;
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
        transform.position = Player.transform.position + offset;
    }
}

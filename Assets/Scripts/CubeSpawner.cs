using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour {

    public GameObject Black;
    public GameObject Yellow;
    public GameObject Blue;
    public GameObject White;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        // green side
        // q - black
        // w - yellow
        // e - blue
        // r - white
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Black;
                    spawn.transform.position = hit.point;
                    Instantiate(spawn);
                    
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Yellow;
                    spawn.transform.position = hit.point;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Blue;
                    spawn.transform.position = hit.point;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = White;
                    spawn.transform.position = hit.point;
                    Instantiate(spawn);
                }
            }
        }

        // red side
        // a - black
        // s - yellow
        // d - blue
        // f - white
        if (Input.GetKeyDown(KeyCode.A))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Black;
                    spawn.transform.position = hit.point;
                    spawn.GetComponent<Influence>().side = true;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Yellow;
                    spawn.transform.position = hit.point;
                    spawn.GetComponent<Influence>().side = true;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = Blue;
                    spawn.transform.position = hit.point;
                    spawn.GetComponent<Influence>().side = true;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray.origin, ray.direction, out hit, Camera.main.farClipPlane))
            {
                if (hit.collider.gameObject.CompareTag("Terrain"))
                {
                    GameObject spawn = White;
                    spawn.transform.position = hit.point;
                    spawn.GetComponent<Influence>().side = true;
                    Instantiate(spawn);
                }
            }
        }
        if (Input.GetKey(KeyCode.C))
        {
            GameObject[] all = GameObject.FindGameObjectsWithTag("Influence");
            foreach(GameObject g in all)
            {
                Destroy(g);
            }
        }

        if (Input.GetKey(KeyCode.I))
        {
            // generate influence map
        }
    }
}

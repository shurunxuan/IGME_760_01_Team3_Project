using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTrim : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        GameObject unit = other.gameObject;
        if (unit.tag != "FlockingUnit") return;
        unit = unit.transform.parent.gameObject;

        if (unit.transform.position.x < -24) unit.transform.position += new Vector3(48, 0, 0);
        if (unit.transform.position.x > 24) unit.transform.position -= new Vector3(48, 0, 0);
        if (unit.transform.position.z < -24) unit.transform.position += new Vector3(0, 0, 48);
        if (unit.transform.position.z > 24) unit.transform.position -= new Vector3(0, 0, 48);
    }

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

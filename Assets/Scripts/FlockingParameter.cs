using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingParameter : MonoBehaviour {

    public float SeparationRadius;
    public float CohesionRadius;
    public float VelocityMatchRadius;
    public float SeparationWeight;
    public float CohesionWeight;
    public float VelocityMatchWeight;
    public bool SeparationEnabled;
    public bool CohesionEnabled;
    public bool VelocityMatchEnabled;
    public GameObject FlockingUnitTemplate;
    public uint UnitCount;

    // Use this for initialization
    void Start () {
        for (int i = 0; i < UnitCount; ++i)
        {
            GameObject newUnit = Instantiate(FlockingUnitTemplate,
                new Vector3(Random.Range(-25.0f, 25.0f), 1, Random.Range(-25.0f, 25.0f)), FlockingUnitTemplate.transform.rotation);
            newUnit.SetActive(true);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}

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
    public float MaxAcceleration;

    // Use this for initialization
    void Start () {
        //for (int i = 0; i < UnitCount; ++i)
        //{
        //    GameObject newUnit = Instantiate(FlockingUnitTemplate,
        //        new Vector3(Random.Range(-25.0f, 25.0f), 1, Random.Range(-25.0f, 25.0f)), FlockingUnitTemplate.transform.rotation);
        //    newUnit.name = FlockingUnitTemplate.name + i;
        //    float randomAngle = Random.Range(0, Mathf.PI);
        //    float randomSpeed = Random.Range(0, 30.0f);
        //    newUnit.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle)) * randomSpeed;
        //    newUnit.SetActive(true);
        //}
    }
	
	// Update is called once per frame
	void Update ()
	{
	    int count = GameObject.FindGameObjectsWithTag("FlockingUnit").Length / 3;

	    if (count < UnitCount)
	    {
	        GameObject newUnit = Instantiate(FlockingUnitTemplate);
	        newUnit.name = FlockingUnitTemplate.name + count;
            float randomAngle = Random.Range(0, Mathf.PI);
            float randomSpeed = Random.Range(0, 30.0f);
            newUnit.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle)) * randomSpeed;
            newUnit.SetActive(true);

        }
    }
}

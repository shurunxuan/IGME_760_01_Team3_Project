using System;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using Random = UnityEngine.Random;

public class FlockingParameter : MonoBehaviour
{

    public String SeparationRadius
    {
        get { return _separationRadius.ToString(CultureInfo.InvariantCulture); }
        set { _separationRadius = float.Parse(value); }
    }
    public String CohesionRadius
    {
        get { return _cohesionRadius.ToString(CultureInfo.InvariantCulture); }
        set { _cohesionRadius = float.Parse(value); }
    }
    public String VelocityMatchRadius
    {
        get { return _velocityMatchRadius.ToString(CultureInfo.InvariantCulture); }
        set { _velocityMatchRadius = float.Parse(value); }
    }
    public String SeparationWeight
    {
        get { return _separationWeight.ToString(CultureInfo.InvariantCulture); }
        set { _separationWeight = float.Parse(value); }
    }
    public String CohesionWeight
    {
        get { return _cohesionWeight.ToString(CultureInfo.InvariantCulture); }
        set { _cohesionWeight = float.Parse(value); }
    }
    public String VelocityMatchWeight
    {
        get { return _velocityMatchWeight.ToString(CultureInfo.InvariantCulture); }
        set { _velocityMatchWeight = float.Parse(value); }
    }
    public bool SeparationEnabled { get; set; }
    public bool CohesionEnabled { get; set; }
    public bool VelocityMatchEnabled { get; set; }

    private float _separationRadius;
    private float _cohesionRadius;
    private float _velocityMatchRadius;
    private float _separationWeight;
    private float _cohesionWeight;
    private float _velocityMatchWeight;


    public GameObject FlockingUnitTemplate;
    public uint UnitCount;
    public float MaxAcceleration;

    private int _count;

    // Use this for initialization
    void Start()
    {
        SeparationRadius = "5";
        CohesionRadius = "10";
        VelocityMatchRadius = "15";
        SeparationWeight = "3";
        CohesionWeight = "1";
        VelocityMatchWeight = "3";
        SeparationEnabled = true;
        CohesionEnabled = true;
        VelocityMatchEnabled = true;

        _count = 0;
        FlockingUnitTemplate.SetActive(false);
        while (_count < UnitCount) Spawn();
        FlockingUnitTemplate.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        if (_count < UnitCount)
        {
            Spawn();
        }
    }

    void Spawn()
    {
        GameObject newUnit = Instantiate(FlockingUnitTemplate);
        newUnit.name = FlockingUnitTemplate.name + _count;
        float randomAngle = Random.Range(0, Mathf.PI);
        float randomSpeed = Random.Range(0, 30.0f);
        Vector3 randomPosition = new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-21.0f, 21.0f)) + FlockingUnitTemplate.transform.position;
        newUnit.GetComponent<Rigidbody>().velocity = new Vector3(Mathf.Sin(randomAngle), 0, Mathf.Cos(randomAngle)) * randomSpeed;
        newUnit.transform.position = randomPosition;
        newUnit.SetActive(true);
        ++_count;
    }
}

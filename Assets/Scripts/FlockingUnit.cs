using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlockingUnit : Unit
{
    public GameObject ParameterController; 

    private Rigidbody _rigidbody;
    private FlockingParameter _parameters;

    // Use this for initialization
    void Start()
    {
        // Get Rigidbody
        _rigidbody = GetComponent<Rigidbody>();
        _parameters = ParameterController.GetComponent<FlockingParameter>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveUnit(_rigidbody);
    }

    // This is the direction the unit will head to when GetMoveDirection().x > 0
    protected override void SetLocalRight()
    {
        localRight = Vector3.right;
    }

    // This is the direction the unit will head to when GetMoveDirection().y > 0
    protected override void SetLocalForward()
    {
        localForward = Vector3.forward;
    }

    protected override Vector2 GetMoveDirection()
    {
        // TODO: implement the algorithm
        List<GameObject> separationNeighborhood = GetNeighborhood(_parameters.SeparationRadius, -1.0f);
        List<GameObject> cohesionNeighborhood = GetNeighborhood(_parameters.CohesionRadius, 0.0f);
        List<GameObject> velocityMatchNeighborhood = GetNeighborhood(_parameters.VelocityMatchRadius, 0.0f);

        Vector3 separationCenter = GetNeighborhoodCenter(separationNeighborhood);
        Vector3 cohesionCenter = GetNeighborhoodCenter(cohesionNeighborhood);
        Vector3 velocityAverage = GetNeighborhoodAverageVelocity(velocityMatchNeighborhood);

        Vector3 separationSteer = transform.position - separationCenter;
        Vector3 cohesionSteer = cohesionCenter - transform.position;
        Vector3 velocitySteer = velocityAverage - _rigidbody.velocity;

        Vector3 finalSteer = separationSteer * (_parameters.SeparationEnabled ? _parameters.SeparationWeight : 0) +
                             cohesionSteer * (_parameters.CohesionEnabled ? _parameters.CohesionWeight : 0) +
                             velocitySteer * (_parameters.VelocityMatchEnabled ? _parameters.VelocityMatchWeight : 0);

        Vector2 direction = new Vector2(finalSteer.x, finalSteer.z);

        return direction;
    }


    List<GameObject> GetNeighborhood(float radius, float minDotProduct)
    {
        List<GameObject> result = new List<GameObject>();
        var colliders = Physics.OverlapSphere(transform.position, radius);
        Vector3 look = Vector3.zero;
        foreach (var collider in colliders)
        {
            if (collider.gameObject.name != "Body") continue;
            GameObject obj = collider.gameObject.transform.parent.gameObject;
            if (obj == gameObject) continue;
            if (minDotProduct > -1.0f)
            {
                look = GetOrientation();
                Vector3 offset = obj.transform.position - transform.position;
                if (Vector3.Dot(look, offset.normalized) < minDotProduct) continue;
            }
            if (obj.tag == "FlockingUnit")
            {
                result.Add(obj);
            }
        }

        return result;
    }

    Vector3 GetNeighborhoodCenter(List<GameObject> neighborhood)
    {
        Vector3 result = Vector3.zero;

        foreach (var neighbor in neighborhood)
        {
            result += neighbor.transform.position;
        }

        result /= neighborhood.Count;

        return result;
    }

    Vector3 GetNeighborhoodAverageVelocity(List<GameObject> neighborhood)
    {
        Vector3 result = Vector3.zero;

        foreach (var neighbor in neighborhood)
        {
            Rigidbody bodyRigidbody = neighbor.GetComponent<Rigidbody>();
            result += bodyRigidbody.velocity;
        }

        result /= neighborhood.Count;

        return result;
    }

}

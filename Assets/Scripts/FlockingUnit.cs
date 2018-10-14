using System.Collections.Generic;
using UnityEngine;

public class FlockingUnit : Unit
{
    public GameObject ParameterController;
    public bool DrawGizmo;

    public float RejectionFactor;
    public float RejectionRadius;

    public float MinVelocity;

    private Rigidbody _rigidbody;
    private FlockingParameter _parameters;

    private Vector3 _separationCenter;
    private Vector3 _cohesionCenter;
    private Vector3 _velocityAverage;

    private Vector2 _lastDirection;

    private List<GameObject> _separationNeighborhood;
    private List<GameObject> _cohesionNeighborhood;
    private List<GameObject> _velocityMatchNeighborhood;

    private int _requestCount;
    // Use this for initialization
    void Start()
    {
        // Get Rigidbody
        _rigidbody = GetComponent<Rigidbody>();
        _parameters = ParameterController.GetComponent<FlockingParameter>();
        useVelocityAsDirection = true;
        _requestCount = 0;
        _lastDirection = Vector2.zero;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveUnit(_rigidbody);
        if (_rigidbody.velocity.magnitude < MinVelocity)
        {
            _rigidbody.velocity = _rigidbody.velocity.normalized * MinVelocity;
        }
    }

    void OnDrawGizmos()
    {
        if (!DrawGizmo) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, float.Parse(_parameters.SeparationRadius));
        //Gizmos.DrawSphere(_separationCenter, 0.3f);
        foreach (var o in _separationNeighborhood)
            Gizmos.DrawSphere(o.transform.position + Vector3.up * 1.15f, 0.3f);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, float.Parse(_parameters.CohesionRadius));
        //Gizmos.DrawSphere(_cohesionCenter, 0.3f);
        foreach (var o in _cohesionNeighborhood)
            Gizmos.DrawSphere(o.transform.position + Vector3.up * 1.75f, 0.3f);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, float.Parse(_parameters.VelocityMatchRadius));
        //Gizmos.DrawLine(transform.position, transform.position + _velocityAverage);
        foreach (var o in _velocityMatchNeighborhood)
            Gizmos.DrawSphere(o.transform.position + Vector3.up * 2.35f, 0.3f);
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
        ++_requestCount;
        if (_requestCount != 10)
            return _lastDirection;
        _requestCount = 0;
        _separationNeighborhood = GetNeighborhood(float.Parse(_parameters.SeparationRadius), -1.0f);
        _cohesionNeighborhood = GetNeighborhood(float.Parse(_parameters.CohesionRadius), 0.0f);
        _velocityMatchNeighborhood = GetNeighborhood(float.Parse(_parameters.VelocityMatchRadius), 0.0f);


        _separationCenter = GetNeighborhoodCenter(_separationNeighborhood);
        _cohesionCenter = GetNeighborhoodCenter(_cohesionNeighborhood);
        _velocityAverage = GetNeighborhoodAverageVelocity(_velocityMatchNeighborhood);

        Vector3 separationSteer = transform.position - _separationCenter;
        Vector3 cohesionSteer = _cohesionCenter - transform.position;
        Vector3 velocitySteer = _velocityAverage - _rigidbody.velocity;
        //if (separationSteer.magnitude > _parameters.MaxAcceleration)
        //    separationSteer = separationSteer.normalized * _parameters.MaxAcceleration;
        //if (cohesionSteer.magnitude > _parameters.MaxAcceleration)
        //    cohesionSteer = cohesionSteer.normalized * _parameters.MaxAcceleration;
        //if (velocitySteer.magnitude > _parameters.MaxAcceleration)
        //    velocitySteer = velocitySteer.normalized * _parameters.MaxAcceleration;

        var rejectionNeighborhood = GetNeighborhood(RejectionRadius, -1.0f);
        Vector3 rejectionForce = Vector3.zero;
        foreach (var o in rejectionNeighborhood)
        {
            Vector3 rejectionDirection = (o.transform.position - transform.position);
            float rad = rejectionDirection.magnitude;
            //rad = (rad - 0.5f) < 0 ? 0 : (rad - 0.5f);
            rejectionForce -= RejectionFactor / rad / rad * rejectionDirection.normalized;
        }

        Vector3 finalSteer = separationSteer * (_parameters.SeparationEnabled ? float.Parse(_parameters.SeparationWeight) : 0) +
                             cohesionSteer * (_parameters.CohesionEnabled ? float.Parse(_parameters.CohesionWeight) : 0) +
                             velocitySteer * (_parameters.VelocityMatchEnabled ? float.Parse(_parameters.VelocityMatchWeight) : 0) +
                             rejectionForce;

        //finalSteer /= _parameters.SeparationWeight + _parameters.CohesionWeight + _parameters.VelocityMatchWeight;
        //finalSteer.Normalize();

        Vector2 direction = new Vector2(finalSteer.x, finalSteer.z);
        _lastDirection = direction;
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
                //look = GetOrientation();
                look = _rigidbody.velocity.normalized;
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

        if (neighborhood.Count == 0) return transform.position;

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

        if (neighborhood.Count == 0) return _rigidbody.velocity;

        foreach (var neighbor in neighborhood)
        {
            Rigidbody bodyRigidbody = neighbor.GetComponent<Rigidbody>();
            result += bodyRigidbody.velocity;
        }

        result /= neighborhood.Count;

        return result;
    }

}

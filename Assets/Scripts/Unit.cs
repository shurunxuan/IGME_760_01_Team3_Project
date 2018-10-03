using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    public float Speed;
    public GameObject DirectionPointer;
    
    protected Vector3 localRight;
    protected Vector3 localForward;

    private Vector3 _lastForceDirection;
    // Use this for initialization
    void Start()
    {
        
    }

    protected void MoveUnit(Rigidbody rigidbody)
    {
        SetLocalRight();
        SetLocalForward();
        //float horizontal = Input.GetAxis("Horizontal");
        //float vertical = Input.GetAxis("Vertical");
        Vector2 direction = GetMoveDirection();
        direction.Normalize();
        float horizontal = direction.x;
        float vertical = direction.y;


        // Calculate the force from user input, and normalize it (making it just a direction)
        Vector3 force = horizontal * localRight + vertical * localForward;
        force.Normalize();
        if (force.magnitude > 0.1f) _lastForceDirection = force;

        DirectionPointer.transform.LookAt(DirectionPointer.transform.position + _lastForceDirection);


        // Create a ray from bottom of the object to the direction of force
        Vector3 originPoint = transform.position - transform.localScale.y * Vector3.up;
        Ray ray = new Ray(originPoint + 0.05f * Vector3.up, force);
        RaycastHit raycastHit;

        // Cast a ray toward the direction of force
        bool hit = Physics.Raycast(ray, out raycastHit, 1.0f);
        // If it hits nothing then open the gravity of object
        rigidbody.useGravity = true;
        if (hit)
        {
            if (raycastHit.transform.name == "Terrain")
            {
                // If raycast hit the terrain then turn off the gravity of object
                rigidbody.useGravity = false;
                // Make the force go through the hit point (and normalize it)
                force = raycastHit.point - originPoint;
                force.Normalize();
            }
        }

        // Apply the force
        rigidbody.AddForce(force * Speed);

        //DirectionPointer.transform.position = transform.position + _directionPointerOffset;
    }

    // This is the direction the unit will head to when GetMoveDirection().x > 0
    protected abstract void SetLocalRight();
    // This is the direction the unit will head to when GetMoveDirection().y > 0
    protected abstract void SetLocalForward();
    protected abstract Vector2 GetMoveDirection();
}

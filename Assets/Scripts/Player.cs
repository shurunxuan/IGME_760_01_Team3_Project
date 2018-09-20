using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed;
    private Rigidbody _rigidbody;
    // Use this for initialization
    void Start()
    {
        // Get Rigidbody
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        // Calculate the force from user input, and normalize it (making it just a direction)
        Vector3 force = horizontal * Vector3.forward + vertical * Vector3.left;
        force.Normalize();

        // Create a ray from bottom of the object to the direction of force
        Vector3 originPoint = transform.position - transform.localScale.y * Vector3.up;
        Ray ray = new Ray(originPoint + 0.1f * Vector3.up, force);
        RaycastHit raycastHit;

        // Cast a ray toward the direction of force
        bool hit = Physics.Raycast(ray, out raycastHit, 1.0f);
        // If it hits nothing then open the gravity of object
        _rigidbody.useGravity = true;
        if (hit)
        {
            if (raycastHit.transform.name == "Terrain")
            {
                // If raycast hit the terrain then turn off the gravity of object
                _rigidbody.useGravity = false;
                // Make the force go through the hit point (and normalize it)
                force = raycastHit.point - originPoint;
                force.Normalize();
            }
        }

        // Apply the force
        _rigidbody.AddForce(force * Speed);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarUnit : Unit
{
    private Rigidbody _rigidbody;
    // Use this for initialization
    void Start()
    {
        // Get Rigidbody
        _rigidbody = GetComponent<Rigidbody>();
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
        return new Vector2(-1, 0);
    }
}

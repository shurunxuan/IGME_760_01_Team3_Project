using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Unit
{

    // Use this for initialization
    void Start()
    {
        _rigidbody = gameObject.GetComponentInChildren<Rigidbody>();
        localRight = MainCamera.transform.right;
        localForward = Vector3.Cross(MainCamera.transform.right, Vector3.up);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveUnit();
    }

    protected override void SetLocalRight()
    {
        
    }

    protected override void SetLocalForward()
    {
        
    }

    protected override Vector2 GetMoveDirection()
    {
        // TODO: implement the algorithm
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

}

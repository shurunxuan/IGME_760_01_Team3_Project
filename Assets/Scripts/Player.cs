using UnityEngine;

public class Player : Unit
{
    public Camera MainCamera;
    private Rigidbody _rigidbody;
    // Use this for initialization
    void Start()
    {
        // Get Rigidbody
        _rigidbody = GetComponent<Rigidbody>();
        useVelocityAsDirection = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MoveUnit(_rigidbody);
    }

    // This is the direction the unit will head to when GetMoveDirection().x > 0
    protected override void SetLocalRight()
    {
        localRight = MainCamera.transform.right;
    }

    // This is the direction the unit will head to when GetMoveDirection().y > 0
    protected override void SetLocalForward()
    {
        localForward = Vector3.Cross(MainCamera.transform.right, Vector3.up);
    }

    protected override Vector2 GetMoveDirection()
    {
        // TODO: implement the algorithm
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

}

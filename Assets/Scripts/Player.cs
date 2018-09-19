using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	// Use this for initialization

    private Vector3 _targetPosition;
    private Rigidbody _rigidbody;

	void Start ()
	{
	    _targetPosition = transform.position;
	    _rigidbody = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
	{
	    _targetPosition.y = transform.position.y;
        if (Input.GetKey(KeyCode.A))
        {
            _targetPosition = new Vector3(_targetPosition.x, transform.position.y, _targetPosition.z - 15.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            _targetPosition = new Vector3(_targetPosition.x, transform.position.y, _targetPosition.z + 15.0f * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            _targetPosition = new Vector3(_targetPosition.x + 15.0f * Time.deltaTime, transform.position.y, _targetPosition.z);
        }
        if (Input.GetKey(KeyCode.W))
        {
            _targetPosition = new Vector3(_targetPosition.x - 15.0f * Time.deltaTime, transform.position.y, _targetPosition.z);
        }
    }

    void FixedUpdate()
    {
        _rigidbody.MovePosition(_targetPosition);
    }
}

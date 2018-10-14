using System.Collections.Generic;
using System.Configuration;
using UnityEngine;
using UnityEngine.UI;

public class CameraFollow : MonoBehaviour
{

    public GameObject[] FollowingGameObject;
    private GameObject _currentFollowingGameObject;
    public Dropdown FollowingObjectDropdown;
    public Toggle FollowingToggle;
    private bool _follow;
    public bool Follow
    {
        get { return _follow; }
        set
        {
            FollowingToggle.isOn = value;
            _follow = value;
        }
    }

    public float Speed;

    private int _dropdownValue;
    public int DropdownValue
    {
        set
        {
            _dropdownValue = value;
            _currentFollowingGameObject = FollowingGameObject[value];
        }
        get { return _dropdownValue; }
    }

    private Vector3 _targetPosition;
    private Quaternion _targetRotation;

    private float _offset;
    private Vector3 _localRight;
    private Vector3 _localForward;

    // Use this for initialization
    void Start()
    {
        Follow = true;

        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>(FollowingGameObject.Length);
        foreach (var o in FollowingGameObject)
        {
            options.Add(new Dropdown.OptionData(o.name));
        }
        FollowingObjectDropdown.ClearOptions();
        FollowingObjectDropdown.AddOptions(options);

        _currentFollowingGameObject = FollowingGameObject[0];
        _localRight = transform.right;
        _localForward = Vector3.Cross(transform.right, Vector3.up);
        _offset = 50.0f;
        //transform.LookAt(FollowingGameObject.transform);
        reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }




        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            Follow = false;
        }

        if (Follow)
        {
            reset();
        }
        else
        {
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                _targetPosition -= _localRight * Speed;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _targetPosition += _localRight * Speed;
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                _targetPosition += _localForward * Speed;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                _targetPosition -= _localForward * Speed;
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Follow = true;
            reset();
        }
    }

    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, _targetPosition, 5.0f * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetRotation, 5.0f * Time.deltaTime);
    }

    private void reset()
    {
        _targetPosition = _currentFollowingGameObject.transform.position - _offset * transform.forward;
        _targetRotation = Quaternion.LookRotation(_currentFollowingGameObject.transform.position - _targetPosition, Vector3.up);
    }
}

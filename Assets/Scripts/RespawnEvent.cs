using UnityEngine;

public class RespawnEvent : MonoBehaviour
{
    public bool RespawnTriggered;
    private Vector3 _respawnPosition;

	// Use this for initialization
	void Start ()
	{
	    RespawnTriggered = false;
	    _respawnPosition = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if (RespawnTriggered)
	    {
	        transform.position = _respawnPosition;
	        RespawnTriggered = false;
	    }
	}
}

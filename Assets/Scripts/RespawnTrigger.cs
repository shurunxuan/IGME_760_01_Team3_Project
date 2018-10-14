using UnityEngine;

public class RespawnTrigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        GameObject mainGameObject = other.gameObject.transform.parent.gameObject;
        if (mainGameObject == null) return;
        RespawnEvent respawnEvent = mainGameObject.GetComponent<RespawnEvent>();
        if (respawnEvent == null) return;
        respawnEvent.RespawnTriggered = true;
        
    }
}

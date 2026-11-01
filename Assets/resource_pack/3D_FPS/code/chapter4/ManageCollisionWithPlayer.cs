using UnityEngine;
using System.Collections;

public class ManageCollisionWithPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		transform.GetChild(0).GetComponent<ManageWeapons>().manageCollisions(hit);
	}

}

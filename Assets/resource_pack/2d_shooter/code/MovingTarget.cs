using UnityEngine;
using System.Collections;

public class MovingTarget : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Rigidbody2D> ().linearVelocity = Vector2.down * 10;

	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

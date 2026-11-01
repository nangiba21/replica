using UnityEngine;
using System.Collections;

public class CollisionWithProjectile : MonoBehaviour 
{
	//public GameObject explosion;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnCollisionEnter (Collision collision)
	{
		if (collision.gameObject.tag == "Player") collision.gameObject.transform.position = GameObject.Find ("start").transform.position;
		//Destroy (collision.gameObject, 1);

	
		Destroy (gameObject);
	}

}

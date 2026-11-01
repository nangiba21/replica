using UnityEngine;
using System.Collections;

public class SpawnMovingTargets : MonoBehaviour {
	float timer = 0;
	public GameObject newObject;
	public GameObject bonus;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		float range = Random.Range (-10, 10);//Screen.width);
		Vector3 newPosition = new Vector3 (GameObject.Find("player").transform.position.x + range, transform.position.y, 0);
		//if (timer >= 1) 
		float respawnTime = 5/GameObject.Find("gameManager").GetComponent<ManageShooterGame>().difficulty;

		if (timer >= respawnTime)			
		{
			float typeOfObjectSpwan = Random.Range(0,100);
			GameObject t;
			if (typeOfObjectSpwan >= 50) 
			{
				t = (GameObject)(Instantiate (newObject, newPosition, Quaternion.identity));
				t.GetComponent<ManageTargetHealth> ().type = ManageTargetHealth.TARGET_BOULDER;
			}
			else t = (GameObject)(Instantiate (bonus, newPosition, Quaternion.identity));
			//GameObject t = (GameObject)(Instantiate (newObject, newPosition, Quaternion.identity));
			//t.GetComponent<ManageTargetHealth> ().type = ManageTargetHealth.TARGET_BOULDER;
			timer = 0;
		}

	
	}
}

using UnityEngine;
using System.Collections;

public class NPCSpawner : MonoBehaviour {
	public GameObject npc1;
	private float timer, respawnTime;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		//if (timer >= 1 ) 
		float respawnTime = 5/GameObject.Find("gameManager").GetComponent<ManageShooterGame>().difficulty;
		if (timer >= respawnTime)
		{
			timer = 0;
			SpawnNPC (npc1);

		}

	
	}
	void  SpawnNPC(GameObject typeOfNPC)
	{
		float range = Random.Range (-10, 10);//Screen.width);
		Vector3 newPosition = new Vector3 (GameObject.Find("player").transform.position.x + range, transform.position.y, 0);
		GameObject newNPC = (GameObject)(Instantiate (npc1, newPosition,  Quaternion.identity));
		newNPC.transform.Rotate (new Vector3 (0, 0, 180));
		newNPC.name = "npc1";

	}

}

using UnityEngine;
using System.Collections;

public class ManageShooterGame : MonoBehaviour {
	public float timer;
	public float difficulty;
	public float timerThresold;


	// Use this for initialization
	void Start () {
		timer = 0;
		difficulty = 1;
		timerThresold = 5;//difficulty increases after 5 seconds

	
	}
	
	// Update is called once per frame
	void Update () {

		timer += Time.deltaTime;
		if (timer >= timerThresold) 
		{
			difficulty++;
			print ("Difficulty level: " + difficulty);
			timer = 0;
		}

	
	}
}

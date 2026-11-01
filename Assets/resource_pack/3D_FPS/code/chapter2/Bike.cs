using UnityEngine;
using System.Collections;

public class Bike 
{
	private string name;
	private float speed;
	private int nbWheels;	

	//First Constructor
	public Bike()
	{	
		name = "Just another bike";
		speed = 0.0f;
	}
	//Second Constructor
	public Bike(string newName)
	{
		name = newName;
		speed = 0.0f;
		Debug.Log ("Just created a new bike with the name " + name);
	}	
	public void accelerate ()
	{
		speed+=1;
		Debug.Log ("Our new speed is now" + speed);
	}

}

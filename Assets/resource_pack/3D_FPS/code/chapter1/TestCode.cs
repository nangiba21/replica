using UnityEngine;
using System.Collections;

public class TestCode : MonoBehaviour {
	public class Bike
	{
		private float speed;
		private int color;

		private void accelerate()
		{
			speed++;
		}
		public void setSpeed (float newSpeed)
		{
			speed = newSpeed;
		}
		public float getSpeed ()
		{
			return (speed);
		}

		private void turnRight()
		{
		}
	}
	public void Start ()
	{
		Bike myBike = new Bike();
		myBike.setSpeed (23.0f);
		print (myBike.getSpeed());
	}
}

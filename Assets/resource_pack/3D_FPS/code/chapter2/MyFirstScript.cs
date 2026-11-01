using UnityEngine;
using System.Collections;

public class MyFirstScript : MonoBehaviour {


	private int number;
	private string myName;
	private int counter;	

	// Use this for initialization
	void Start () 
	{
		number = 1;
		myName = "Patrick";

		//print("Hello " + myName + " your number is "+number);
		int localVariable = 3;
		print("local variable: "+localVariable);
		counter = 0;	
		myFirstMethod();	
		mySecondMethod("Patrick");
		int myAge = calculateAge(1998);
		print("Your age is " + myAge);	
		Bike b1 = new Bike ("My First Bike");
		b1.accelerate();
		b1.accelerate();
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//print (myName);
		//print("local variable: "+localVariable);
		counter = counter + 1;
		//print ("counter="+counter);		
	}

	public void myFirstMethod() 
	{
		print ("Hello World");
	}

	public void mySecondMethod(string name) 
	{
		print ("Hello, your name is " +name);
	}

	public void myThirdMethod(string fName, string lName) 
	{
		print ("Hello, your name is " +fName+" "+lName);
	}
	public int calculateAge(int YOB) 
	{
		int age;
		age = 2016 - YOB; return (age);
	}


}

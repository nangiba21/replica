using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ManageCards : MonoBehaviour {
	private bool firstCardSelected, secondCardSelected;
	private GameObject card1, card2;

	// Use this for initialization
	public GameObject card;
	private string rowForCard1, rowForCard2;


	bool timerHasElapsed, timerHasStarted;
	float timer;

	int nbMatch = 0;

	void Start () {
		displayCards ();
	}
	
	// Update is called once per frame
	void Update () {


		if (timerHasStarted) 
		{
			timer += Time.deltaTime;
			print (timer);
			if (timer >= 1) 
			{
				timerHasElapsed = true;
				timerHasStarted = false;
				if (card1.tag == card2.tag) {
					Destroy (card1);
					Destroy (card2);
					nbMatch++;
					if (nbMatch == 10)
						SceneManager.LoadScene (SceneManager.GetActiveScene ().name);


				} 
				else {
					card1.GetComponent<Tile> ().hideCard ();
					card2.GetComponent<Tile> ().hideCard ();



				}
				firstCardSelected = false;
				secondCardSelected = false;
				card1 = null;
				card2 = null;
				rowForCard1 = "";
				rowForCard2 = "";
				timer = 0;



			}


		}
	
	}
	public void displayCards()
	{

		//Instantiate (card, new Vector3 (0, 0, 0), Quaternion.identity);
		//addACard(0);
		int [] shuffledArray = createShuffledArray();
		int [] shuffledArray2 = createShuffledArray();
		for (int i = 0; i < 10; i++) 
		{
			//addACard (i);
			addACard(0, i,shuffledArray[i]);
			addACard(1, i,shuffledArray2[i]);

		}
	}
	void addACard(int row, int rank, int value)
	{

		float cardOriginalScale = card.transform.localScale.x;
		float scaleFactor = (500 * cardOriginalScale) / 100.0f;
		float yScaleFactor = (725 * cardOriginalScale) / 100.0f;
		GameObject cen = GameObject.Find ("centerOfScreen");
		Vector3 newPosition = new Vector3 (cen.transform.position.x + ((rank - 10 / 2) * scaleFactor), cen.transform.position.y + ((row-2/2)*yScaleFactor), cen.transform.position.z);
		GameObject c = (GameObject)(Instantiate (card, newPosition, Quaternion.identity));

		//GameObject c = (GameObject)(Instantiate (card, new Vector3 (0, 0, 0), Quaternion.identity));
		//GameObject c = (GameObject)(Instantiate (card, new Vector3 (rank*3.0f, 0, 0), Quaternion.identity));
		c.tag = ""+(value + 1);
		//c.name = "" + value;
		c.name = "" + row+"_"+value;
		string nameOfCard = "";
		string cardNumber = "";
		if (value == 0)
			cardNumber = "ace";
		else
			cardNumber = "" + (value + 1);
		nameOfCard = cardNumber + "_of_hearts";
		Sprite s1 = (Sprite)(Resources.Load<Sprite> (nameOfCard));
		print ("S1" + s1);//testing
		//GameObject.Find(""+value).GetComponent<Tile>().setOriginalSprite(s1);
		GameObject.Find(""+row+"_"+value).GetComponent<Tile>().setOriginalSprite(s1);
	}

	public int [] createShuffledArray ()
	{

		int[] newArray = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
		int tmp;
		for (int t = 0; t < 10; t++) 
		{
			tmp = newArray [t];
			int r = Random.Range (t, 10);
			newArray [t] = newArray [r];
			newArray [r] = tmp;

		}
		return newArray;


	}

	public void cardSelected(GameObject card)
	{

		if (!firstCardSelected) {
			firstCardSelected = true;
			string row = card.name.Substring (0, 1);
			rowForCard1 = row;
			card1 = card;
			card.GetComponent<Tile> ().revealCard ();
		} else if (firstCardSelected && !secondCardSelected) 
		{
			string row = card.name.Substring (0, 1);
			rowForCard2 = row;
			if (rowForCard2 != rowForCard1) 
			{
				card2 = card;
				secondCardSelected = true;
				card2.GetComponent<Tile> ().revealCard ();
				checkCards ();

			}

		}

	}

	void checkCards()
	{
		runTimer ();

	}

	public void runTimer()
	{
		timerHasElapsed = false;
		timerHasStarted = true;

	}
}

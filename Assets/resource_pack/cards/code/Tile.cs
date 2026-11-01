using UnityEngine;
using System.Collections;

public class Tile : MonoBehaviour {
	private bool tileRevealed = false;
	public Sprite originalSprite;
	public Sprite hiddenSprite;


	// Use this for initialization
	void Start () {
		hideCard ();
	
	}
		
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnMouseDown()
	{

		print ("You pressed on tile");
		/*if (tileRevealed) hideCard();
		else revealCard ();*/
		GameObject.Find ("gameManager").GetComponent<ManageCards> ().cardSelected (gameObject);
	}

	public void hideCard()
	{

		GetComponent<SpriteRenderer> ().sprite = hiddenSprite;
		tileRevealed = false;
	}
	public void revealCard()
	{
		GetComponent<SpriteRenderer> ().sprite = originalSprite;
		tileRevealed = true;
	}

	public void setOriginalSprite(Sprite newSprite)
	{
		originalSprite = newSprite;
	}
}

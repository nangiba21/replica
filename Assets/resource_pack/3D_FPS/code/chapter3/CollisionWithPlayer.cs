
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CollisionWithPlayer : MonoBehaviour
{
    [HideInInspector] public int score = 0;
    [HideInInspector] public int targetScore = 4;

    [Header("UI References")]
    public Text messageText;       
    public GameObject nextButton;  

    void Start()
    {
        score = 0;

        if (messageText != null)
        {
            messageText.text = "";
        }

        if (nextButton != null)
        {
            nextButton.SetActive(false);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.CompareTag("pick_me"))
        {
            Destroy(collision.collider.gameObject);
            score++;
            Debug.Log("Score: " + score);
        }

        if (collision.collider.gameObject.name == "end" && score >= targetScore)
        {
            TriggerWin();
        }
    }

    void TriggerWin()
    {
        Debug.Log("Congratulations! You won!");

        if (messageText != null)
        {
            messageText.text = "You Won!";
        }
        else
        {
            GameObject msgObj = GameObject.Find("message");
            if (msgObj != null && msgObj.GetComponent<Text>() != null)
            {
                msgObj.GetComponent<Text>().text = "You Won!";
            }
        }

        if (nextButton != null)
        {
            nextButton.SetActive(true);
        }

        
        Time.timeScale = 0f;
    }
}
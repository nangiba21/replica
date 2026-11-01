using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public LaunchProjectile[] projectileLaunchers;
    public CollisionWithPlayer playerCollision;

    public Slider intensitySlider;
    public Slider spawnTimeSlider;
    public Dropdown levelDropdown;
    public Text scoreText;
    public Text timeText;
    public Text pauseInfoText;

   
    public Text winText;               
    public GameObject winButton;       

    private bool isPaused = true;
    private float runningTime = 0f;
    private bool hasWon = false;

    void Start()
    {
        Time.timeScale = 0f;
        isPaused = true;

        if (pauseInfoText != null)
            pauseInfoText.text = "Press esc or click Resume to Start";

        
        if (winText != null) winText.text = "";
        if (winButton != null) winButton.SetActive(false);
    }

    void Update()
    {
        if (hasWon) return; 

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (!isPaused)
        {
            runningTime += Time.deltaTime;
            if (timeText != null)
                timeText.text = "Time: " + runningTime.ToString("F1") + "s";
        }

        if (projectileLaunchers != null)
        {
            foreach (var launcher in projectileLaunchers)
            {
                if (launcher != null)
                {
                    if (intensitySlider != null)
                        launcher.launchForce = intensitySlider.value;

                    if (spawnTimeSlider != null)
                        launcher.spawnInterval = spawnTimeSlider.value;
                }
            }
        }

        if (levelDropdown != null && playerCollision != null)
        {
            string selectedOption = levelDropdown.options[levelDropdown.value].text;
            int targetVal;
            if (int.TryParse(selectedOption, out targetVal))
            {
                playerCollision.targetScore = targetVal;
            }
        }

        if (scoreText != null && playerCollision != null)
        {
            scoreText.text = "Score: " + playerCollision.score;

            
            if (playerCollision.score >= playerCollision.targetScore && playerCollision.targetScore > 0)
            {
                TriggerWin();
            }
        }
    }

    void TriggerWin()
    {
        hasWon = true;
        Time.timeScale = 0f; 

        if (winText != null)
            winText.text = "You Won!";

        if (winButton != null)
            winButton.SetActive(true); 
    }

    public void TogglePause()
    {
        if (hasWon) return; 

        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;

        if (pauseInfoText != null)
        {
            pauseInfoText.text = isPaused ? "Game Paused (Press Esc to Resume)" : "";
        }
    }
    public void GoToScene(string training_camp)
    {
        SceneManager.LoadScene(training_camp);
    }

    public void GoToScene2(string simple_ai)
    {
        SceneManager.LoadScene(simple_ai);
    }
}

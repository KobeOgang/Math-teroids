using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public DepartmentLevel currentDepartment;

    public GameObject gameOverUI_Parent;

    [Header("Game Settings")]
    public int earthHealth = 10;
    public float asteroidSpawnRate = 2f;
    public float asteroidMinSpeed = 1f;
    public float asteroidMaxSpeed = 3f;


    private int currentProblemAnswer;
    public string currentProblemText;

    [Header("Score System")]
    public int playerScore = 0;

    [Header("Currency System")]
    public int playerMoney = 0;

    [Header("Dynamic Difficulty AI")]
    public float difficultyScore = 0f; // The master score for our AI
    public int correctAnswersForLevelUp = 3; // Correct answers in a row to make game harder
    public int incorrectAnswersForLevelDown = 2; // Incorrect answers in a row to make game easier
    public float difficultyIncreaseAmount = 1.5f; // How much harder it gets
    public float difficultyDecreaseAmount = 1.0f; // How much easier it gets
    public float minDifficultyScore = 0f; // The easiest the game can be
    public float maxDifficultyScore = 10f; // The hardest the game can be

    public PerformanceTracker playerPerformance;

    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerPerformance = new PerformanceTracker();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SpawnAsteroids());
    }

    //how fast the asteroids spawn
    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(asteroidSpawnRate);
            
        }
    }

    //score tracking
    public void AddScore(int points)
    {
        playerScore += points;

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateScoreUI(playerScore);
        }
    }


    public void UpdateDifficulty()
    {
        // Check if player's correct streak is high enough to level up
        if (playerPerformance.correctStreak >= correctAnswersForLevelUp)
        {
            // Increase the difficulty score
            difficultyScore += difficultyIncreaseAmount;
            // "Consume" the streak so it resets to 0
            playerPerformance.ConsumeCorrectStreak();
            Debug.Log($"LEVEL UP! New Difficulty Score: {difficultyScore}");
        }
        // Check if player's incorrect streak is high enough to level down
        else if (playerPerformance.incorrectStreak >= incorrectAnswersForLevelDown)
        {
            // Decrease the difficulty score
            difficultyScore -= difficultyDecreaseAmount;
            // "Consume" the streak so it resets to 0
            playerPerformance.ConsumeIncorrectStreak();
            Debug.Log($"LEVEL DOWN! New Difficulty Score: {difficultyScore}");
        }

        // Make sure the difficulty score stays within our min/max bounds
        difficultyScore = Mathf.Clamp(difficultyScore, minDifficultyScore, maxDifficultyScore);
    }

    public bool CheckAnswer(int playerAnswer)
    {
        if (playerAnswer == currentProblemAnswer)
        {
            return true;
        }
        return false;
    }

    public void DamageEarth()
    {
        earthHealth--;
        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateHealthBar(earthHealth);
        }


        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.TriggerEarthHitAnimation();
        }

        if (earthHealth <= 0)
        {
            Debug.Log("GameOver");
            Time.timeScale = 0f;

            // Find the TimeTracker and get the final time as a string
            TimeTracker timer = FindObjectOfType<TimeTracker>();
            string finalTimeValue = "00:00"; // Default value in case timer isn't found
            if (timer != null)
            {
                finalTimeValue = timer.GetCurrentTime();
                timer.StopTimer();
            }

            // Activate the parent UI object and get the manager script
            if (gameOverUI_Parent != null)
            {
                gameOverUI_Parent.SetActive(true);
                GameOverUIManager gameOverManager = gameOverUI_Parent.GetComponent<GameOverUIManager>();
                if (gameOverManager != null)
                {
                    // Call the setup method, passing in the final score and time
                    gameOverManager.Setup(playerScore, finalTimeValue);
                }
            }
        }
    }

    public void AddMoney(int amount)
    {
        playerMoney += amount;

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateMoneyUI(playerMoney);
        }

        Debug.Log($"Money earned! Current balance: {playerMoney}");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    public enum Difficulty { Easy, Normal, Hard }
    public Difficulty currentDifficulty; 

    [Header("Game Settings")]
    public int earthHealth = 10;
    public float asteroidSpawnRate = 2f;
    public float asteroidMinSpeed = 1f;
    public float asteroidMaxSpeed = 3f;
    public GameObject gameOverPanel;

    [Header("Math Problem Settings")]
    public int minNumber = 1;
    public int maxNumber = 10;

    public int easyMinNumber = 1, easyMaxNumber = 6;
    public int normalMinNumber = 1, normalMaxNumber = 10;
    public int hardMinNumber = 10, hardMaxNumber = 50;

    private int currentProblemAnswer;
    public string currentProblemText;

    [Header("Score System")]
    public int playerScore = 0;

    [Header("Currency System")]
    public int playerMoney = 0;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameOverPanel.SetActive(false);
        GenerateMathProblem();
        StartCoroutine(SpawnAsteroids());
    }

    private IEnumerator SpawnAsteroids()
    {
        while (true)
        {
            yield return new WaitForSeconds(asteroidSpawnRate);
            
        }
    }

    public void AddScore(int points)
    {
        playerScore += points;

        UIManager uiManager = FindObjectOfType<UIManager>();
        if (uiManager != null)
        {
            uiManager.UpdateScoreUI(playerScore);
        }
    }

    public void GenerateMathProblem()
    {
        int num1 = 0, num2 = 0;
        int operation = Random.Range(0, currentDifficulty == Difficulty.Easy ? 2 : 4); 

        
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                num1 = Random.Range(easyMinNumber, easyMaxNumber);
                num2 = Random.Range(easyMinNumber, easyMaxNumber);
                break;
            case Difficulty.Normal:
                num1 = Random.Range(normalMinNumber, normalMaxNumber);
                num2 = Random.Range(normalMinNumber, normalMaxNumber);
                break;
            case Difficulty.Hard:
                num1 = Random.Range(hardMinNumber, hardMaxNumber);
                num2 = Random.Range(hardMinNumber, hardMaxNumber);
                break;
        }

        switch (operation)
        {
            case 0: 
                currentProblemAnswer = num1 + num2;
                currentProblemText = $"{num1} + {num2} = ?";
                break;
            case 1: 
                currentProblemAnswer = num1 - num2;
                currentProblemText = $"{num1} - {num2} = ?";
                break;
            case 2: 
                currentProblemAnswer = num1 * num2;
                currentProblemText = $"{num1} × {num2} = ?";
                break;
            case 3: 
                
                num1 = num1 * num2; 
                currentProblemAnswer = num1 / num2;
                currentProblemText = $"{num1} ÷ {num2} = ?";
                break;
        }

        Debug.Log($"Generated new math problem: {currentProblemText} (Answer: {currentProblemAnswer})");
    }

    public bool CheckAnswer(int playerAnswer)
    {
        if (playerAnswer == currentProblemAnswer)
        {
            GenerateMathProblem();
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
            uiManager.UpdateHealthSlider(earthHealth);
        }

        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.TriggerEarthHitAnimation();
        }

        if (earthHealth <= 0)
        {
            Debug.Log($"GameOver");
            Time.timeScale = 0f;
            TimeTracker timer = FindObjectOfType<TimeTracker>();
            if (timer != null)
            {
                timer.StopTimer();
            }
            gameOverPanel.SetActive(true);
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

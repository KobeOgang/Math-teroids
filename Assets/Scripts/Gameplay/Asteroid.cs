using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Asteroid : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public int points = 10;
    public int money = 20;
    public GameObject explosionEffect;

    [Header("Math Problem Settings")]
    public TMP_Text mathProblemText;
    private int problemAnswer;
    private string problemString;

    private void Start()
    {
        GenerateMathProblem();
    }

    private void GenerateMathProblem()
    {
        if (GameController.Instance == null) return; // Safety check

        int num1 = 0, num2 = 0;
        int operation = Random.Range(0, GameController.Instance.currentDifficulty == GameController.Difficulty.Easy ? 2 : 4);

        // Adjust number ranges based on difficulty
        switch (GameController.Instance.currentDifficulty)
        {
            case GameController.Difficulty.Easy:
                num1 = Random.Range(GameController.Instance.easyMinNumber, GameController.Instance.easyMaxNumber);
                num2 = Random.Range(GameController.Instance.easyMinNumber, GameController.Instance.easyMaxNumber);
                break;
            case GameController.Difficulty.Normal:
                num1 = Random.Range(GameController.Instance.normalMinNumber, GameController.Instance.normalMaxNumber);
                num2 = Random.Range(GameController.Instance.normalMinNumber, GameController.Instance.normalMaxNumber);
                break;
            case GameController.Difficulty.Hard:
                num1 = Random.Range(GameController.Instance.hardMinNumber, GameController.Instance.hardMaxNumber);
                num2 = Random.Range(GameController.Instance.hardMinNumber, GameController.Instance.hardMaxNumber);
                break;
        }

        // Generate problem based on operation type
        switch (operation)
        {
            case 0:
                problemAnswer = num1 + num2;
                problemString = $"{num1} + {num2} = ?";
                break;
            case 1:
                problemAnswer = num1 - num2;
                problemString = $"{num1} - {num2} = ?";
                break;
            case 2:
                problemAnswer = num1 * num2;
                problemString = $"{num1} × {num2} = ?";
                break;
            case 3:
                num1 = num1 * num2;
                problemAnswer = num1 / num2;
                problemString = $"{num1} ÷ {num2} = ?";
                break;
        }

        if (mathProblemText != null)
        {
            mathProblemText.text = problemString;
        }

        Debug.Log($"Asteroid spawned with difficulty-based problem: {problemString} (Answer: {problemAnswer})");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missile"))
        {
            Destroy(other.gameObject);
            DestroyAsteroidWithScore();
        }
        else if (other.CompareTag("Earth"))
        {
            Debug.Log("Asteroid collided with Earth");
            GameController.Instance.DamageEarth();
            DestroyAsteroid();
        }
        else if (other.CompareTag("ForceField"))
        {
            Debug.Log("Asteroid blocked by Force Field!");
            Destroy(gameObject);
            Destroy(other.gameObject);
            AudioManager.instance.PlayForceFieldHitSFX();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Earth"))
        {
            Debug.Log("Asteroid collided with Earth via collision");
            GameController.Instance.DamageEarth();
            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        Destroy(gameObject);
        AudioManager.instance.PlayEarthImpactSFX();
    }

    private void DestroyAsteroidWithScore()
    {
        GameController.Instance.AddScore(points); // Add score when destroyed
        GameController.Instance.AddMoney(money);
        AudioManager.instance.PlayAsteroidExplosionSFX();
        Destroy(gameObject);
    }

    public int GetProblemAnswer()
    {
        return problemAnswer;
    }

}

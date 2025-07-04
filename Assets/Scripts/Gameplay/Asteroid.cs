using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class Asteroid : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public int points;
    public int money = 20;
    public GameObject explosionEffect;

    [Header("Math Problem Settings")]
    public TMP_Text mathProblemText;
    private int problemAnswer;
    private string problemString;

    private bool isTargeted = false;

    private void Start()
    {
        if (GameController.Instance != null)
        {
            switch (GameController.Instance.currentDepartment)
            {
                case DepartmentLevel.Elementary:
                    points = 10;
                    break;
                case DepartmentLevel.HighSchool:
                    points = 30;
                    break;
                case DepartmentLevel.SeniorHighSchool:
                    points = 50;
                    break;
                default:
                    points = 10;
                    break;
            }
        }

        GenerateMathProblem();
    }

    private void GenerateMathProblem()
    {
        if (GameController.Instance == null) return; // Safety check

        // Get the current state from the GameController
        float difficulty = GameController.Instance.difficultyScore;
        DepartmentLevel department = GameController.Instance.currentDepartment;

        int num1 = 0, num2 = 0, num3 = 0;
        int operation1 = 0, operation2 = 0;

        // The main logic switches based on the chosen department
        switch (department)
        {
            // ===============================================
            // ELEMENTARY SCHOOL LOGIC
            // ===============================================
            case DepartmentLevel.Elementary:
                // Operations are only Addition (0) and Subtraction (1)
                operation1 = Random.Range(0, 2);

                // Difficulty scales the size of the numbers
                int maxNumberElementary = 5 + (int)(difficulty * 1.5f); // e.g., at score 10, max number is 20
                num1 = Random.Range(1, maxNumberElementary);
                num2 = Random.Range(1, maxNumberElementary);

                if (operation1 == 0) // Addition
                {
                    problemAnswer = num1 + num2;
                    problemString = $"{num1} + {num2} = ?";
                }
                else // Subtraction
                {
                    // *** IMPORTANT: To avoid negative answers for Elementary level ***
                    // We ensure the first number is always the biggest.
                    int tempNum1 = Mathf.Max(num1, num2);
                    int tempNum2 = Mathf.Min(num1, num2);
                    problemAnswer = tempNum1 - tempNum2;
                    problemString = $"{tempNum1} - {tempNum2} = ?";
                }
                break;

            // ===============================================
            // HIGH SCHOOL LOGIC
            // ===============================================
            case DepartmentLevel.HighSchool:
                // All four operations are available
                operation1 = Random.Range(0, 4);

                int maxNumberHighSchool = 10 + (int)(difficulty * 2.5f); // Numbers scale faster
                num1 = Random.Range(1, maxNumberHighSchool);
                num2 = Random.Range(2, maxNumberHighSchool); // Start at 2 to avoid weird divisions

                switch (operation1)
                {
                    case 0: // Addition
                        problemAnswer = num1 + num2;
                        problemString = $"{num1} + {num2} = ?";
                        break;
                    case 1: // Subtraction (negatives are allowed)
                        problemAnswer = num1 - num2;
                        problemString = $"{num1} - {num2} = ?";
                        break;
                    case 2: // Multiplication
                        problemAnswer = num1 * num2;
                        problemString = $"{num1} × {num2} = ?";
                        break;
                    case 3: // Division
                            // To ensure whole numbers, we create the answer first
                        int product = num1 * num2;
                        problemAnswer = num1;
                        problemString = $"{product} ÷ {num2} = ?";
                        break;
                }
                break;

            // ===============================================
            // SENIOR HIGH SCHOOL LOGIC (PEMDAS)
            // ===============================================
            case DepartmentLevel.SeniorHighSchool:
                // At low difficulty, it's the same as High School.
                // At high difficulty, we introduce a chance for PEMDAS problems.
                float pemdasChance = (difficulty - 4f) / 6f; // Starts at diff 4, 100% chance at diff 10

                if (Random.value < pemdasChance)
                {
                    // *** GENERATE A PEMDAS PROBLEM ***
                    int maxNumberPemdas = 10 + (int)difficulty;
                    num1 = Random.Range(1, maxNumberPemdas / 2); // Keep numbers smaller for multi-step
                    num2 = Random.Range(1, 10);
                    num3 = Random.Range(1, 10);
                    operation1 = Random.Range(2, 4); // Multiplication or Division first
                    operation2 = Random.Range(0, 2); // Addition or Subtraction second

                    if (operation1 == 2) // (num1 * num2) +/- num3
                    {
                        problemAnswer = (num1 * num2) + (operation2 == 0 ? num3 : -num3);
                        problemString = $"({num1} × {num2}) {(operation2 == 0 ? "+" : "-")} {num3} = ?";
                    }
                    else // (num1 * num2) --> use for division, then +/- num3
                    {
                        int product = num1 * num2;
                        problemAnswer = num1 + (operation2 == 0 ? num3 : -num3);
                        problemString = $"({product} ÷ {num2}) {(operation2 == 0 ? "+" : "-")} {num3} = ?";
                    }
                }
                else
                {
                    // *** GENERATE A NORMAL HIGH SCHOOL PROBLEM ***
                    // (This is a copy of the High School logic for when PEMDAS doesn't trigger)
                    operation1 = Random.Range(0, 4);
                    int maxNum = 15 + (int)(difficulty * 2.5f);
                    num1 = Random.Range(1, maxNum);
                    num2 = Random.Range(2, maxNum);
                    switch (operation1)
                    {
                        case 0: problemAnswer = num1 + num2; problemString = $"{num1} + {num2} = ?"; break;
                        case 1: problemAnswer = num1 - num2; problemString = $"{num1} - {num2} = ?"; break;
                        case 2: problemAnswer = num1 * num2; problemString = $"{num1} × {num2} = ?"; break;
                        case 3: int product = num1 * num2; problemAnswer = num1; problemString = $"{product} ÷ {num2} = ?"; break;
                    }
                }
                break;
        }

        // Update the text on the asteroid prefab
        if (mathProblemText != null)
        {
            mathProblemText.text = problemString;
        }

        Debug.Log($"New Problem ({department}): {problemString} (Answer: {problemAnswer}) | Difficulty: {difficulty}");
    }

    //collision handlers
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
        GameController.Instance.AddScore(points); //adds score
        GameController.Instance.AddMoney(money);
        AudioManager.instance.PlayAsteroidExplosionSFX();
        Destroy(gameObject);
    }

    public int GetProblemAnswer()
    {
        return problemAnswer;
    }
    public bool IsTargeted()
    {
        return isTargeted;
    }

    public void SetAsTargeted()
    {
        isTargeted = true;
    }

}

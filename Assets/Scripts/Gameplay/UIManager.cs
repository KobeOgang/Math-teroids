using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text problemText;
    public TMP_InputField answerInput;
    public Slider healthSlider;
    public TMP_Text scoreText;
    public TMP_Text moneyText;

    public static UIManager Instance;

    private void Start()
    {

        answerInput.onEndEdit.AddListener((value) =>
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CheckAnswer();
            }
        });


        //UpdateProblemDisplay();
    }


    private void UpdateProblemDisplay()
    {
        
        problemText.text = GameController.Instance.currentProblemText;
        answerInput.text = "";
        answerInput.Select();
    }

    private void CheckAnswer()
    {
        if (int.TryParse(answerInput.text, out int playerAnswer))
        {
            Asteroid[] asteroids = FindObjectsOfType<Asteroid>();
            foreach (Asteroid asteroid in asteroids)
            {
                if (asteroid.GetProblemAnswer() == playerAnswer)
                {
                    Debug.Log($"Correct answer found for asteroid: {asteroid.gameObject.name}");
                    FireMissileAtAsteroid(asteroid);
                    return;
                }
            }

            Debug.Log("No matching asteroid found.");
            answerInput.text = "";
            answerInput.Select();
        }
        else
        {
            Debug.Log("Invalid input format - not an integer.");
        }
    }

    private void FireMissileAtAsteroid(Asteroid targetAsteroid)
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.FireMissile(targetAsteroid);
        }
    }



    private void FireMissile()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            //playerController.FireMissile();
        }
        else
        {
            
        }
    }

    public void UpdateHealthSlider(int currentHealth)
    {
        healthSlider.value = currentHealth;
        Debug.Log($"Health Slider updated: {currentHealth}");
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void UpdateMoneyUI(int amount)
    {
        moneyText.text = $"Money: {amount}";
    }

}

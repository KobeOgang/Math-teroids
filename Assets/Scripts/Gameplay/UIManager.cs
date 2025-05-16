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

    private void Start()
    {
        
        answerInput.onEndEdit.AddListener((value) => {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                CheckAnswer();
            }
        });

        UpdateProblemDisplay();
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
            if (GameController.Instance.CheckAnswer(playerAnswer))
            {
                FireMissile();
                UpdateProblemDisplay();
            }
            else
            {
                
                
                answerInput.text = "";
                answerInput.Select();
            }
        }
        else
        {
            Debug.Log("Invalid input format - not an integer");
        }
    }

    private void FireMissile()
    {
        PlayerController playerController = FindObjectOfType<PlayerController>();
        if (playerController != null)
        {
            playerController.FireMissile();
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
}

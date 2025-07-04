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
    public GameObject closedShopPanel;
    public GameObject openedShopPanel;
    public TMP_Text wrongAnswerText;
    public TMP_Text feedbackText;

    [Header("Health Bar Segments")]
    public List<GameObject> healthSegments;

    public static UIManager Instance;

    private bool shopIsOpen = false;

    private void Start()
    {
        // ensuring input field already selected at start
        answerInput.ActivateInputField();
        answerInput.Select();

        answerInput.onSubmit.AddListener(delegate { CheckAnswer(); }); //when pressing enter

        closedShopPanel.SetActive(true);
        openedShopPanel.SetActive(false);
        wrongAnswerText.gameObject.SetActive(false);
    }

    private void Update()
    {
        //opens and closes the shop
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleShopUI();
        }
    }

    //checks if the answer is correct
    private void CheckAnswer()
    {
        if (int.TryParse(answerInput.text, out int playerAnswer))
        {
            Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

            Asteroid closestMatchingAsteroid = null;
            float closestDistance = Mathf.Infinity;

            foreach (Asteroid asteroid in asteroids)
            {
                if (asteroid.GetProblemAnswer() == playerAnswer && !asteroid.IsTargeted()) //when its still not targeted
                {
                    float distance = Vector2.Distance(transform.position, asteroid.transform.position);

                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        closestMatchingAsteroid = asteroid;
                    }
                }
            }

            if (closestMatchingAsteroid != null)
            {
                GameController.Instance.playerPerformance.RecordCorrectAnswer();
                GameController.Instance.UpdateDifficulty();

                closestMatchingAsteroid.SetAsTargeted();
                //Debug.Log($"Correct answer found for closest asteroid: {closestMatchingAsteroid.gameObject.name}");
                FireMissileAtAsteroid(closestMatchingAsteroid);

                answerInput.text = ""; //clears input field
                answerInput.ActivateInputField(); //auto-select
            }
            else
            {
                GameController.Instance.playerPerformance.RecordIncorrectAnswer();
                GameController.Instance.UpdateDifficulty();

                Debug.Log("No matching asteroid found.");
                answerInput.text = ""; 
                answerInput.ActivateInputField(); 
                AudioManager.instance.PlayWrongAnswerSFX();
                ShowWrongAnswerFeedback(); //feedback
            }
        }
        else
        {
            GameController.Instance.playerPerformance.RecordIncorrectAnswer();
            GameController.Instance.UpdateDifficulty();

            Debug.Log("Invalid input format - not an integer.");
            answerInput.text = ""; 
            answerInput.ActivateInputField(); 
            AudioManager.instance.PlayWrongAnswerSFX();
            ShowWrongAnswerFeedback(); //feedback
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

    public void UpdateHealthSlider(int currentHealth)
    {
        healthSlider.value = currentHealth;
        Debug.Log($"Health Slider updated: {currentHealth}");
    }

    public void UpdateHealthBar(int currentHealth)
    {
        for (int i = 0; i < healthSegments.Count; i++)
        {
            healthSegments[i].SetActive(i < currentHealth);
        }
    }

    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"{score}";
    }

    public void UpdateMoneyUI(int amount)
    {
        moneyText.text = $"{amount}";
    }

    public void ToggleShopUI()
    {
        shopIsOpen = !shopIsOpen;

        closedShopPanel.SetActive(!shopIsOpen);
        openedShopPanel.SetActive(shopIsOpen);
    }
    private void ShowWrongAnswerFeedback()
    {
        wrongAnswerText.gameObject.SetActive(true); 
        StartCoroutine(HideText()); 
    }

    public void ShowFeedback()
    {
        feedbackText.gameObject.SetActive(true); 
        StartCoroutine(HideText()); 
    }

    private IEnumerator HideText()
    {
        yield return new WaitForSeconds(1f); 
        wrongAnswerText.gameObject.SetActive(false); 
        feedbackText.gameObject.SetActive(false); 
    }

}

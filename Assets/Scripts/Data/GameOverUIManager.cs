using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class GameOverUIManager : MonoBehaviour
{
    [Header("Panel References")]
    public GameObject resultsPanel;
    public GameObject leaderboardPanel;

    [Header("Results Panel UI")]
    public TMP_Text finalScoreText;
    public TMP_Text finalTimeText;
    public Button nextButton;

    [Header("Leaderboard Panel UI")]
    public TMP_InputField playerKeyInput;
    public Button saveButton;
    public GameObject leaderboardContentParent; 
    public GameObject leaderboardEntryPrefab; 

    private int finalScore;
    private string finalTime;

    void Start()
    {
        // Add listeners for your buttons here
        nextButton.onClick.AddListener(OnNextButtonClicked);
        saveButton.onClick.AddListener(OnSaveButtonClicked);

    }

    public void Setup(int score, string time)
    {
        finalScore = score;
        finalTime = time;

        // Show the initial results
        resultsPanel.SetActive(true);
        leaderboardPanel.SetActive(false);

        finalScoreText.text = "Score: " + finalScore;
        finalTimeText.text = "Time: " + finalTime;
    }

    
    private void OnNextButtonClicked()
    {
        // Switch to the leaderboard view
        resultsPanel.SetActive(false);
        leaderboardPanel.SetActive(true);

        PopulateLeaderboard();
    }

    private void PopulateLeaderboard()
    {
        // Clear any old entries
        foreach (Transform child in leaderboardContentParent.transform)
        {
            Destroy(child.gameObject);
        }

        LeaderboardData leaderboard = SaveManager.Instance.LoadLeaderboard();

        if (leaderboard.allEntries.Count == 0)
        {
            return;
        }

        // Create a UI element for each entry
        for (int i = 0; i < leaderboard.allEntries.Count; i++)
        {
            GameObject entryGO = Instantiate(leaderboardEntryPrefab, leaderboardContentParent.transform);
            LeaderboardEntry entryData = leaderboard.allEntries[i];

            // text fields 
            entryGO.transform.Find("RankText").GetComponent<TMP_Text>().text = (i + 1).ToString();
            entryGO.transform.Find("KeyText").GetComponent<TMP_Text>().text = entryData.playerKey;
            entryGO.transform.Find("ScoreText").GetComponent<TMP_Text>().text = entryData.score.ToString();
            entryGO.transform.Find("TimeText").GetComponent<TMP_Text>().text = entryData.survivalTime;
            entryGO.transform.Find("DepartmentText").GetComponent<TMP_Text>().text = entryData.department.ToString();
        }
    }

    public void OnSaveButtonClicked()
    {
        string playerKey = playerKeyInput.text.ToUpper();
        if (string.IsNullOrEmpty(playerKey)) { return; }

        LeaderboardEntry newEntry = new LeaderboardEntry
        {
            playerKey = playerKey,
            score = finalScore,
            survivalTime = finalTime,
            department = GameController.Instance.currentDepartment
        };

        LeaderboardData leaderboard = SaveManager.Instance.LoadLeaderboard();
        leaderboard.allEntries.Add(newEntry);
        leaderboard.allEntries = leaderboard.allEntries.OrderByDescending(x => x.score).ToList();

        if (leaderboard.allEntries.Count > 10)
        {
            leaderboard.allEntries = leaderboard.allEntries.GetRange(0, 10);
        }

        SaveManager.Instance.SaveLeaderboard(leaderboard);

        // Disable button and input field
        saveButton.interactable = false;
        playerKeyInput.interactable = false;

        // Refresh the leaderboard display to show the new score immediately
        PopulateLeaderboard();
    }
}

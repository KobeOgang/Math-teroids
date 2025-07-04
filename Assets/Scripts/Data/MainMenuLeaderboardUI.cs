using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenuLeaderboardUI : MonoBehaviour
{
    [Header("UI References")]
    public GameObject leaderboardContentParent; // The "Content" object in your Scroll View
    public GameObject leaderboardEntryPrefab;   // The prefab for a single row

    public void Start()
    {
        // First, activate our own panel GameObject
        gameObject.SetActive(true);
        // Then, populate the list with the latest scores
        PopulateLeaderboard();
    }

   
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void PopulateLeaderboard()
    {
        // Clear any old entries that might be there from last time
        foreach (Transform child in leaderboardContentParent.transform)
        {
            Destroy(child.gameObject);
        }

        LeaderboardData leaderboard = SaveManager.Instance.LoadLeaderboard();

        if (leaderboard.allEntries.Count == 0)
        {
            
            return;
        }

        // Create a UI element for each saved entry
        for (int i = 0; i < leaderboard.allEntries.Count; i++)
        {
            GameObject entryGO = Instantiate(leaderboardEntryPrefab, leaderboardContentParent.transform);
            LeaderboardEntry entryData = leaderboard.allEntries[i];

            // Text components
            entryGO.transform.Find("RankText").GetComponent<TMP_Text>().text = (i + 1).ToString();
            entryGO.transform.Find("KeyText").GetComponent<TMP_Text>().text = entryData.playerKey;
            entryGO.transform.Find("ScoreText").GetComponent<TMP_Text>().text = entryData.score.ToString();
            entryGO.transform.Find("TimeText").GetComponent<TMP_Text>().text = entryData.survivalTime;
            entryGO.transform.Find("DepartmentText").GetComponent<TMP_Text>().text = entryData.department.ToString();
        }
    }
}

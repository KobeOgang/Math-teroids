using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string saveFilePath;

    private void Awake()
    {
        // --- Singleton Pattern ---
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Make it persistent across scenes
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // --- Define the path for our save file ---
        // Application.persistentDataPath is a special, safe folder on any device
        // where your game is allowed to save data permanently.
        saveFilePath = Path.Combine(Application.persistentDataPath, "leaderboard.json");
    }

    /// <summary>
    /// Loads the leaderboard data from the JSON file on disk.
    /// </summary>
    /// <returns>A LeaderboardData object with all saved entries.</returns>
    public LeaderboardData LoadLeaderboard()
    {
        // Check if the save file actually exists.
        if (File.Exists(saveFilePath))
        {
            // Read the entire file content as a single string.
            string json = File.ReadAllText(saveFilePath);

            // Convert the JSON string back into our C# LeaderboardData object.
            LeaderboardData loadedData = JsonUtility.FromJson<LeaderboardData>(json);
            return loadedData;
        }
        else
        {
            // If no save file exists yet, return a new, empty LeaderboardData object.
            Debug.Log("No leaderboard file found. Creating a new one.");
            return new LeaderboardData();
        }
    }

    /// <summary>
    /// Saves the provided leaderboard data to the JSON file on disk.
    /// </summary>
    /// <param name="dataToSave">The LeaderboardData object to save.</param>
    public void SaveLeaderboard(LeaderboardData dataToSave)
    {
        // Convert our C# LeaderboardData object into a JSON formatted string.
        // The 'true' argument makes the JSON output nicely formatted and human-readable.
        string json = JsonUtility.ToJson(dataToSave, true);

        // Write the JSON string to the file, overwriting it if it already exists.
        File.WriteAllText(saveFilePath, json);

        Debug.Log("Leaderboard data saved to: " + saveFilePath);
    }
}

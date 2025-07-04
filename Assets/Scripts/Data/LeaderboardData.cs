using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> allEntries;

    // This is a constructor that creates a new empty list when a new LeaderboardData object is made.
    public LeaderboardData()
    {
        allEntries = new List<LeaderboardEntry>();
    }
}

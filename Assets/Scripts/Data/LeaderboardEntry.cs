using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DepartmentLevel { Elementary, HighSchool, SeniorHighSchool }

[System.Serializable]
public class LeaderboardEntry
{
    public string playerKey;      // The 3-5 character key the player enters
    public int score;             // The player's final score
    public string survivalTime;   // The final survival time, formatted as "MM:SS"
    public DepartmentLevel department;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerformanceTracker : MonoBehaviour
{
    // These will track the player's current streaks.
    public int correctStreak { get; private set; }
    public int incorrectStreak { get; private set; }

    // We can also store totals for more complex future logic.
    public int totalCorrectAnswers { get; private set; }
    public int totalIncorrectAnswers { get; private set; }

    // Constructor to initialize the values when a new tracker is made.
    public PerformanceTracker()
    {
        correctStreak = 0;
        incorrectStreak = 0;
        totalCorrectAnswers = 0;
        totalIncorrectAnswers = 0;
    }

    /// Call this method when the player gets an answer right.
    
    public void RecordCorrectAnswer()
    {
        correctStreak++;
        incorrectStreak = 0; // A correct answer breaks the incorrect streak.
        totalCorrectAnswers++;
    }

    /// 
    /// Call this method when the player gets an answer wrong.
    /// 
    public void RecordIncorrectAnswer()
    {
        incorrectStreak++;
        correctStreak = 0; // A wrong answer breaks the correct streak.
        totalIncorrectAnswers++;
    }

    public void ConsumeCorrectStreak()
    {
        correctStreak = 0;
    }

    public void ConsumeIncorrectStreak()
    {
        incorrectStreak = 0;
    }
}

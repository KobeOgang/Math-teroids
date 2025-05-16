using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    public GameController.Difficulty selectedDifficulty; 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu") 
        {
            ReassignButtonFunctions();
        }
        else if (scene.name == "GameScene") 
        {
            GameController.Instance.currentDifficulty = selectedDifficulty;
            Debug.Log($"Difficulty set to: {selectedDifficulty}");
        }
    }

    private void ReassignButtonFunctions()
    {
        Debug.Log("Reassigning button functions...");

        Button[] buttons = FindObjectsOfType<Button>(true);

        foreach (Button button in buttons)
        {
            if (button.name == "EasyButton") button.onClick.AddListener(SetEasyDifficulty);
            if (button.name == "NormalButton") button.onClick.AddListener(SetNormalDifficulty);
            if (button.name == "HardButton") button.onClick.AddListener(SetHardDifficulty);
        }

    }

    public void SetDifficultyAndLoadGameScene(GameController.Difficulty difficulty)
    {
        selectedDifficulty = difficulty;
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayGameBGM();
        SceneManager.LoadScene("GameScene");
    }

    public void SetEasyDifficulty()
    {
        SetDifficultyAndLoadGameScene(GameController.Difficulty.Easy);
    }

    public void SetNormalDifficulty()
    {
        SetDifficultyAndLoadGameScene(GameController.Difficulty.Normal);
    }

    public void SetHardDifficulty()
    {
        SetDifficultyAndLoadGameScene(GameController.Difficulty.Hard);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player quit");
    }


}

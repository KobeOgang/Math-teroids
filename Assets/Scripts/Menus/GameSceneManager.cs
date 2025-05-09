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

        Button easyButton = GameObject.Find("EasyButton")?.GetComponent<Button>();
        Button normalButton = GameObject.Find("NormalButton")?.GetComponent<Button>();
        Button hardButton = GameObject.Find("HardButton")?.GetComponent<Button>();

        if (easyButton) easyButton.onClick.AddListener(SetEasyDifficulty);
        if (normalButton) normalButton.onClick.AddListener(SetNormalDifficulty);
        if (hardButton) hardButton.onClick.AddListener(SetHardDifficulty);
    }

    public void SetDifficultyAndLoadGameScene(GameController.Difficulty difficulty)
    {
        selectedDifficulty = difficulty;
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

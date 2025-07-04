using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    public DepartmentLevel selectedDepartment;

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
        // Check if the reloaded scene is the Main Menu
        if (scene.name == "MainMenu")
        {
            // If it is, re-run the logic to find the new buttons and assign their functions
            ReassignDepartmentButtonFunctions();
        }
        else if (scene.name == "GameScene")
        {
            // Find the GameController in the newly loaded scene and pass the choice to it
            GameController gc = FindObjectOfType<GameController>();
            if (gc != null)
            {
                gc.currentDepartment = selectedDepartment;
                Debug.Log($"Department Level set to: {selectedDepartment}");
            }
        }
    }

    // === THIS IS THE RESTORED AND UPDATED METHOD ===
    private void ReassignDepartmentButtonFunctions()
    {
        Debug.Log("Finding new buttons in Main Menu and reassigning functions...");

        Button[] buttons = FindObjectsOfType<Button>(true); // Find all buttons in the scene

        foreach (Button button in buttons)
        {
            // IMPORTANT: Clear any old listeners first to prevent duplicates!
            button.onClick.RemoveAllListeners();

            if (button.name == "ElementaryButton")
            {
                button.onClick.AddListener(SelectElementary);
            }
            else if (button.name == "HighSchoolButton")
            {
                button.onClick.AddListener(SelectHighSchool);
            }
            else if (button.name == "SeniorHighSchoolButton")
            {
                button.onClick.AddListener(SelectSeniorHighSchool);
            }
        }
    }

    public void SelectDepartmentAndLoadGame(DepartmentLevel department)
    {
        selectedDepartment = department;
        AudioManager.instance.StopMusic();
        AudioManager.instance.PlayGameBGM();
        SceneManager.LoadScene("GameScene");
    }

    public void SelectElementary()
    {
        SelectDepartmentAndLoadGame(DepartmentLevel.Elementary);
    }

    public void SelectHighSchool()
    {
        SelectDepartmentAndLoadGame(DepartmentLevel.HighSchool);
    }

    public void SelectSeniorHighSchool()
    {
        SelectDepartmentAndLoadGame(DepartmentLevel.SeniorHighSchool);
    }
}

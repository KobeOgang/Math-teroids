using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject forceFieldPrefab; // Assign the ForceField prefab in the Inspector
    private GameObject activeForceField;
    public void BuyHealth(int cost)
    {
        if (GameController.Instance.playerMoney >= cost)
        {
            GameController.Instance.playerMoney -= cost;
            GameController.Instance.earthHealth += 5;

            AudioManager.instance.PlayEarthHealSFX();

            // Update UI immediately
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.UpdateMoneyUI(GameController.Instance.playerMoney);
                uiManager.UpdateHealthSlider(GameController.Instance.earthHealth);
            }

            Debug.Log("Purchased Health!");

        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    public void BuyForceField(int cost)
    {
        if (GameController.Instance.playerMoney >= cost)
        {
            GameController.Instance.playerMoney -= cost;
            AudioManager.instance.PlayForceFieldOnSFX();
            UIManager uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.UpdateMoneyUI(GameController.Instance.playerMoney);
            }

            // Check if a force field already exists
            if (activeForceField == null)
            {
                SpawnForceField();
            }
            else
            {
                Debug.Log("Force Field is already active!");
            }
        }
        else
        {
            Debug.Log("Not enough money!");
        }
    }

    private void SpawnForceField()
    {
        Vector3 spawnPosition = new Vector3(0, 0, 0); // Adjust as needed for center placement
        activeForceField = Instantiate(forceFieldPrefab, spawnPosition, Quaternion.identity);
        activeForceField.tag = "ForceField"; // Ensure the tag is set correctly
        Debug.Log("Force Field deployed!");
    }
}

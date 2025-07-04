using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public GameObject forceFieldPrefab;
    private GameObject activeForceField;
    UIManager uiManager;

    private void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }
    //health buff
    public void BuyHealth(int cost)
    {
        if (GameController.Instance.earthHealth >= 10)
        {
            Debug.Log("Health is already at max! Purchase denied.");
            if (uiManager != null)
            {
                uiManager.answerInput.ActivateInputField();
            }
            return;
        }

        if (GameController.Instance.playerMoney >= cost)
        {
            GameController.Instance.playerMoney -= cost;

            //healing does not exceed max health
            GameController.Instance.earthHealth = Mathf.Min(GameController.Instance.earthHealth + 5, 10);

            AudioManager.instance.PlayEarthHealSFX();
            
            if (uiManager != null)
            {
                uiManager.UpdateMoneyUI(GameController.Instance.playerMoney);
                uiManager.UpdateHealthBar(GameController.Instance.earthHealth);
                uiManager.answerInput.ActivateInputField();
            }

            Debug.Log("Purchased Health!");
        }
        else
        {
            if (uiManager != null)
            {
                uiManager.answerInput.ActivateInputField();
            }
            Debug.Log("Not enough money!");
            uiManager.ShowFeedback();
        }
    }

    //Shield Buff
    public void BuyForceField(int cost)
    {
        if (GameController.Instance.playerMoney >= cost)
        {
            GameController.Instance.playerMoney -= cost;
            AudioManager.instance.PlayForceFieldOnSFX();
            if (uiManager != null)
            {
                uiManager.UpdateMoneyUI(GameController.Instance.playerMoney);
                uiManager.answerInput.ActivateInputField();
            }

            if (activeForceField == null)
            {
                SpawnForceField();
            }
            else
            {
                Debug.Log("Force Field is already active!");
                uiManager.answerInput.ActivateInputField();
            }
        }
        else
        {
            if (uiManager != null)
            {
                uiManager.answerInput.ActivateInputField();
            }
            Debug.Log("Not enough money!");
            uiManager.ShowFeedback();
        }
    }

    //Instantiate shield
    private void SpawnForceField()
    {
        Vector3 spawnPosition = new Vector3(0, 0, 0); 
        activeForceField = Instantiate(forceFieldPrefab, spawnPosition, Quaternion.identity);
        activeForceField.tag = "ForceField";
        Debug.Log("Force Field deployed!");
    }

}

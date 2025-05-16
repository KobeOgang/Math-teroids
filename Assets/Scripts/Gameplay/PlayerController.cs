using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;

    public void FireMissile()
    {
        if (missilePrefab != null && missileSpawnPoint != null)
        {

            GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
            Rigidbody2D rb = missile.GetComponent<Rigidbody2D>();
            AudioManager.instance.PlayMissileLaunchSFX();

            if (rb != null)
            {
            }
        }
        else
        {
            
            if (missilePrefab == null) Debug.LogError("Missile prefab is null");
            if (missileSpawnPoint == null) Debug.LogError("Missile spawn point is null");
        }
    }
}

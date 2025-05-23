using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject missilePrefab;
    public Transform missileSpawnPoint;

    private Animator animator;
    private SpriteRenderer spriteRenderer;


    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    public void FireMissile(Asteroid targetAsteroid)
    {
        if (missilePrefab != null && missileSpawnPoint != null && targetAsteroid != null)
        {
            GameObject missile = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
            Missile missileScript = missile.GetComponent<Missile>();

            if (missileScript != null)
            {
                missileScript.SetTarget(targetAsteroid);
            }

            AudioManager.instance.PlayMissileLaunchSFX();
        }
        else
        {
            Debug.LogError("Missile or target asteroid missing.");
        }
    }

    public void TriggerEarthHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("isHit"); // Trigger the animation
            StartCoroutine(ResetAnimationAfterDelay(.4f));
        }
        else
        {
            Debug.LogError("Animator not found on Earth object.");
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRedEffect()); // Trigger color effect
        }

    }

    public void ResetEarthHitAnimation()
    {
        if (animator != null)
        {
            animator.Play("Idle"); // Ensure it transitions back to the Idle state
        }
    }

    private IEnumerator ResetAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play("anim_EarthIdle"); // Reset animation to Idle state
    }

    private IEnumerator FlashRedEffect()
    {
        spriteRenderer.color = Color.red; // Change color to red
        yield return new WaitForSeconds(0.2f); // Short impact duration
        spriteRenderer.color = Color.white; // Revert to normal
    }
}

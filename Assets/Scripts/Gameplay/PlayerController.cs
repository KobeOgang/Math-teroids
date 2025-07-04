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

    //fires the missile
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

    //when earth gets hit by asteroid
    public void TriggerEarthHitAnimation()
    {
        if (animator != null)
        {
            animator.SetTrigger("isHit"); 
            StartCoroutine(ResetAnimationAfterDelay(.4f));
        }
        else
        {
            Debug.LogError("Animator not found on Earth object.");
        }

        if (spriteRenderer != null)
        {
            StartCoroutine(FlashRedEffect()); //red color effect
        }

    }

    private IEnumerator ResetAnimationAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.Play("anim_EarthIdle"); //reset animation
    }

    private IEnumerator FlashRedEffect()
    {
        spriteRenderer.color = Color.red; 
        yield return new WaitForSeconds(0.2f); 
        spriteRenderer.color = Color.white; 
    }
}

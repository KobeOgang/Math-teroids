using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("Asteroid Settings")]
    public int points = 10;
    public GameObject explosionEffect;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Missile"))
        {
            Destroy(other.gameObject);
            DestroyAsteroid();
        }
        else if (other.CompareTag("Earth"))
        {
            Debug.Log("Asteroid collided with Earth");
            GameController.Instance.DamageEarth();
            DestroyAsteroid();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Earth"))
        {
            Debug.Log("Asteroid collided with Earth via collision");
            GameController.Instance.DamageEarth();
            DestroyAsteroid();
        }
    }

    private void DestroyAsteroid()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [Header("Missile Settings")]
    public float speed = 10f;
    public GameObject explosionEffect;
    public float rotationSpeed = 5f;

    private Transform target;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //FindNearestAsteroid();
    }
    public void SetTarget(Asteroid targetAsteroid)
    {
        target = targetAsteroid.transform;
    }


    private void FixedUpdate()
    {
        if (target == null) return;

        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
        direction.Normalize();

        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));
    }




    private void FindNearestAsteroid()
    {
        GameObject[] asteroids = GameObject.FindGameObjectsWithTag("Asteroid");

        if (asteroids.Length == 0)
        {
            target = null; 
            return;
        }

        float closestDistance = Mathf.Infinity;
        Transform closestAsteroid = null;

        foreach (GameObject asteroid in asteroids)
        {
            float distance = Vector2.Distance(transform.position, asteroid.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestAsteroid = asteroid.transform;
            }
        }

        target = closestAsteroid;

        if (target != null)
        {
            
        }
    }



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Asteroid") && other.transform == target)
        {
            DestroyAsteroid(other.gameObject);
            DestroyMissile();
        }
    }

    private void DestroyAsteroid(GameObject asteroid)
    {
        Destroy(asteroid);
    }

    private void DestroyMissile()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;

    public float projectileLifetime = 8.0f;
    private float projectileSpeed;
    public float minProjSpeed = 5.0f;
    public float maxProjSpeed = 15.0f;
    public Vector3 direction;
    public int damage = 10;

    public float rotationSpeed = 200f;
    public float forwardSpeed = 5f; 
    public float waveSpeed = 1f;
    public float amplitude = 0.05f;
    public float frequency = 5f;
    private float startTime;

    private BulletTravelType type;
    public enum BulletTravelType
    {
        STRAIGHT,
        WAVE,
        SPIRAL,
    }
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        projectileSpeed = Random.Range(minProjSpeed, maxProjSpeed);
        direction = transform.right;
        if(projectileLifetime <= 0)
        {
            projectileLifetime = 8.0f;
        }
        int randomNum = Random.Range(0, 2); // random number for length of bullet travel type
        switch (randomNum)
        {
            case 0:
                type = BulletTravelType.STRAIGHT;          
                break;
            case 1:
                type = BulletTravelType.WAVE;
                break;
            case 2:
                type = BulletTravelType.SPIRAL;
                break;
            default:
                type = BulletTravelType.STRAIGHT;
                break;
        }

        startTime = Time.time;
        Destroy(gameObject, projectileLifetime);
    }
    
    private void Update()
    {
        switch (type)
        {
            case BulletTravelType.STRAIGHT:
                transform.position += direction * projectileSpeed * Time.deltaTime;
                break;
            case BulletTravelType.WAVE:
                float waveOffset = Mathf.Sin((Time.time - startTime) * frequency) * amplitude;

                // Move the projectile forward with the wave motion
                transform.Translate(transform.up * (waveSpeed * Time.deltaTime + waveOffset));
                transform.position += direction * projectileSpeed * Time.deltaTime;
                break;
            case BulletTravelType.SPIRAL:
                // Move the projectile forward
                transform.Translate(Vector3.right * forwardSpeed * Time.deltaTime);

                // Rotate the projectile in a circular motion
                transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
                transform.position += direction * projectileSpeed * Time.deltaTime;
                break;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check the player is in the correct dimension layer
        if (gameObject.layer == LayerMask.NameToLayer("RedDimension") && collision.gameObject.layer == LayerMask.NameToLayer("RedDimension") && collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            playerHealth.DealDamage(damage);
            Destroy(gameObject);
        }
        else if (gameObject.layer == LayerMask.NameToLayer("BlueDimension") && collision.gameObject.layer == LayerMask.NameToLayer("BlueDimension") && collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            playerHealth.DealDamage(damage);
            Destroy(gameObject);
        }

    }
}

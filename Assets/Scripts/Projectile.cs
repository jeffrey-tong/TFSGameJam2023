using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float projectileLifetime = 4.0f;
    public int damage = 10;

    private void Start()
    {
        if(projectileLifetime <= 0)
        {
            projectileLifetime = 4.0f;
        }

        Destroy(gameObject, projectileLifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Health playerHealth = collision.GetComponent<Health>();
            playerHealth.DealDamage(damage);
            Destroy(gameObject);
        }
    }
}

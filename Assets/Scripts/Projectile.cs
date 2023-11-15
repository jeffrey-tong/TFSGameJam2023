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

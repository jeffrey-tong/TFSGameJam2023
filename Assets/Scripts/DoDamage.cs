using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoDamage : MonoBehaviour
{
    [SerializeField] private int damage;
    [SerializeField] private Collider2D doDamageTo;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision == doDamageTo) 
        { 
            Health health = collision.GetComponent<Health>();
            health.DealDamage(damage);

        }

    }
}

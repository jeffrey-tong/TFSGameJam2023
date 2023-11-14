using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    public float moveSpeed = 2f;
    public float attackRange = 10.0f;
    public float attackRangeBuffer = 1.0f;
    public float attackSpeed = 1.0f;
    private bool isAttacking = false;
    private bool canAttack = true;
    public float projectileSpeed = 10.0f;

    private Transform player;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player == null)
        {
            Debug.Log("Player not found");
        }

        if (moveSpeed <= 0)
        {
            moveSpeed = 2f;
        }
        if(attackRange <= 0)
        {
            attackRange = 10.0f;
        }
        if(attackRangeBuffer <= 0) 
        {
            attackRangeBuffer = 1.0f;
        }
        if(attackSpeed <= 0)
        {
            attackSpeed = 1.0f;
        }
        if(projectileSpeed <= 0)
        {
            projectileSpeed = 10.0f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAttacking)
        {
            MoveTowardsPlayer();
            // Check if the player is within attack range
            if (Vector3.Distance(transform.position, player.position) <= attackRange + attackRangeBuffer && canAttack)
            {
                StartAttack();
                StartCoroutine(AttackCooldown());
            }
        }

        if (isAttacking)
        {
            if (Vector3.Distance(transform.position, player.position) > attackRange + attackRangeBuffer)
            {
                isAttacking = false;
            }
            if (Vector3.Distance(transform.position, player.position) <= attackRange + attackRangeBuffer && canAttack)
            {
                StartAttack();
                StartCoroutine(AttackCooldown());
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction to the player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();

        // Move towards the player
        transform.position += direction * moveSpeed * Time.deltaTime;
    }

    void StartAttack()
    {

    }

    System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //do later
    }
}

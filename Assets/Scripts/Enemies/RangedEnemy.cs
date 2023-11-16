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

    private Rigidbody2D rb;
    private Transform player;

    [Header("Bullet Data")]
    [SerializeField] private GameObject greenProjectilePrefab;
    [SerializeField] private GameObject purpleProjectilePrefab;
    [SerializeField] private GameObject projectileSpawnPoint;
    [SerializeField] private float projectileSpeed = 10.0f;
  
    private GameObject currentProjectilePrefab;
    private int currentLayer;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        int randomLayerIndex = Random.Range(0, 2);

        if (randomLayerIndex == 0 )
        {
            gameObject.layer = LayerMask.NameToLayer("PurpleDimension");
            currentLayer = gameObject.layer;
            currentProjectilePrefab = purpleProjectilePrefab;
        }
        else if (randomLayerIndex == 1 )
        {
            gameObject.layer = LayerMask.NameToLayer("GreenDimension");
            currentLayer = gameObject.layer;
            currentProjectilePrefab = greenProjectilePrefab;
        }

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
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

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
        isAttacking = true;
        AudioManager.Instance.Play("EnemyShoot");
        GameObject projectileInstance = Instantiate(currentProjectilePrefab, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation);
        //Set projectile layer = to enemy layer
        projectileInstance.layer = currentLayer;

        Vector2 shootDirection = new Vector2(Mathf.Cos(rb.rotation * Mathf.Deg2Rad), Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
        projectileInstance.GetComponent<Rigidbody2D>().velocity = shootDirection * projectileSpeed;
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

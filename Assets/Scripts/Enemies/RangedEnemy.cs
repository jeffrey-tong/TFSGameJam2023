using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private float moveSpeed;
    public float minMoveSpeed = 2.0f;
    public float maxMoveSpeed = 10.0f;

    private float attackRange = 10.0f;
    public float attackRangeBuffer = 1.0f;

    private float attackSpeed;
    public float minAttackSpeed = 1.0f;
    public float maxAttackSpeed = 2.5f;
    private bool canAttack = false;

    private Rigidbody2D rb;
    private Transform player;

    [Header("Bullet Data")]
    [SerializeField] private GameObject greenProjectilePrefab;
    [SerializeField] private GameObject purpleProjectilePrefab;
    [SerializeField] private GameObject projectileSpawnPoint;
    private float projectileSpeed;
    public float minProjSpeed = 8.0f;
    public float maxProjSpeed = 14.0f;
  
    private GameObject currentProjectilePrefab;
    private int currentLayer;

    private EnemySpawner spawner;

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

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
        if(attackRangeBuffer <= 0) 
        {
            attackRangeBuffer = 1.0f;
        }
        attackSpeed = Random.Range(minAttackSpeed, maxAttackSpeed);
        projectileSpeed = Random.Range(minProjSpeed, maxProjSpeed);

        spawner = GameObject.FindObjectOfType<EnemySpawner>();

        StartCoroutine(AttackCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > attackRange)
        {
            MoveTowardsPlayer();
        }

        if (distance < attackRange)
        {
            if (canAttack)
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
        AudioManager.Instance.Play("EnemyShoot");
        GameObject projectileInstance = Instantiate(currentProjectilePrefab, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation);
        Debug.Log(projectileSpawnPoint.transform.rotation);
        //Set projectile layer = to enemy layer
        projectileInstance.layer = currentLayer;
    }

    System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            spawner.EnemyDestroyed();
            Destroy(gameObject);
        }
    }
}

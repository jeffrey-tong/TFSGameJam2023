using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private float moveSpeed;
    public float minMoveSpeed = 12f;
    public float maxMoveSpeed = 25f;

    public float orbitRadius = 10f;

    private float attackSpeed;
    public float minAttackSpeed = 1.0f;
    public float maxAttackSpeed = 3.0f;
    private bool canAttack = false;

    private Rigidbody2D rb;
    private Transform player;

    [Header("Bullet Data")]
    [SerializeField] private GameObject projectileSpawnPoint;
    [SerializeField] private GameObject greenProjectilePrefab;
    [SerializeField] private GameObject purpleProjectilePrefab;
    private float projectileSpeed;
    public float minProjSpeed = 8f;
    public float maxProjSpeed = 15f;

    private GameObject currentProjectilePrefab;
    private int currentLayer;

    private EnemySpawner spawner;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();

        // Place enemy in random layer
        int randomLayerIndex = Random.Range(0, 2);
        if (randomLayerIndex == 0)
        {
            gameObject.layer = LayerMask.NameToLayer("PurpleDimension");
            currentLayer = gameObject.layer;
            currentProjectilePrefab = purpleProjectilePrefab;
        }
        else if (randomLayerIndex == 1)
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
        if (orbitRadius <= 0)
        {
            orbitRadius = 10.0f;
        }
        attackSpeed = Random.Range(minAttackSpeed, maxAttackSpeed);
        projectileSpeed = Random.Range(minProjSpeed, maxProjSpeed);
        
        spawner = GameObject.FindObjectOfType<EnemySpawner>();

        MoveToClosestPointOnCircle();
        StartCoroutine(AttackCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        //rotate around 0,0
        transform.RotateAround(Vector3.zero, Vector3.forward, moveSpeed*Time.deltaTime);

        //lookat player
        Vector3 direction = player.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);


        if (canAttack)
        {
            StartAttack();
            StartCoroutine(AttackCooldown());
        }
            

    }

    void MoveToClosestPointOnCircle()
    {
        float angle = CalculateClosestAngle();
        Vector3 newPosition = GetPositionOnCircle(angle);
        transform.position = newPosition;
    }

    float CalculateClosestAngle()
    {
        Vector3 currentPosition = transform.position;
        float angle = Mathf.Atan2(currentPosition.y, currentPosition.x) * Mathf.Rad2Deg;
        return Mathf.Round(angle / 360f) * 360f;
    }

    Vector3 GetPositionOnCircle(float angle)
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;
        return new Vector3(x, y, 0f);
    }

    void StartAttack()
    {
        AudioManager.Instance.Play("EnemyShoot");
        GameObject projectileInstance = Instantiate(currentProjectilePrefab, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation);
        //Set projectile layer = to enemy layer
        projectileInstance.layer = currentLayer;
    }

    System.Collections.IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackSpeed);
        canAttack = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            spawner.EnemyDestroyed();
            Destroy(gameObject);
        }
    }
}

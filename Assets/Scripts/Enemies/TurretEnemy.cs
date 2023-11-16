using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private Rigidbody2D rb;
    public float minAttackSpeed = 1;
    public float maxAttackSpeed = 3;
    public float minProjSpeed = 5;
    public float maxProjSpeed = 15;

    private float attackSpeed;
    private bool canAttack = false;

    [Header("Bullet Data")]
    [SerializeField] private GameObject greenProjectilePrefab;
    [SerializeField] private GameObject purpleProjectilePrefab;
    [SerializeField] private GameObject projectileSpawnPoint;
    private float projectileSpeed;

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
        attackSpeed = Random.Range(minAttackSpeed, maxAttackSpeed);
        projectileSpeed = Random.Range(minProjSpeed, maxProjSpeed);

        if(transform.position.x > 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        }
        spawner = GameObject.FindObjectOfType<EnemySpawner>();
        StartCoroutine(AttackCooldown());
    }

    // Update is called once per frame
    void Update()
    {
        if (canAttack)
        {
            StartAttack();
            StartCoroutine(AttackCooldown());
        }
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

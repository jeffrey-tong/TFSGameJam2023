using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private Rigidbody2D rb;
    public float attackSpeed = 2.0f;
    private bool canAttack = true;

    [Header("Bullet Data")]
    [SerializeField] private GameObject greenProjectilePrefab;
    [SerializeField] private GameObject purpleProjectilePrefab;
    [SerializeField] private GameObject projectileSpawnPoint;
    [SerializeField] private float projectileSpeed = 15.0f;

    private GameObject currentProjectilePrefab;
    private int currentLayer;

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
        if (attackSpeed <= 0)
        {
            attackSpeed = 1.0f;
        }
        if (projectileSpeed <= 0)
        {
            projectileSpeed = 10.0f;
        }

        if(transform.position.x > 0)
        {
            transform.rotation = Quaternion.AngleAxis(180, Vector3.forward);
        }
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

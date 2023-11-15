using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    public float moveSpeed = 15f;
    public float orbitRadius = 10f;
    public float attackSpeed = 1.0f;
    private bool canAttack = true;

    private Rigidbody2D rb;
    private Transform player;

    [Header("Bullet Data")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectileSpawnPoint;
    [SerializeField] private float projectileSpeed = 10.0f;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
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
            moveSpeed = 15f;
        }
        if (orbitRadius <= 0)
        {
            orbitRadius = 10.0f;
        }
        if (attackSpeed <= 0)
        {
            attackSpeed = 1.0f;
        }
        if (projectileSpeed <= 0)
        {
            projectileSpeed = 10.0f;
        }

        MoveToClosestPointOnCircle();
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
        GameObject projectileInstance = Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation);
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

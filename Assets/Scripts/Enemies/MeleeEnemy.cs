using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    private float moveSpeed;
    public float minMoveSpeed = 2;
    public float maxMoveSpeed = 5;
    private Transform player;
    private EnemySpawner spawner;
    private int currentLayer;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();

        // Place enemy in random layer
        int randomLayerIndex = Random.Range(0, 2);
        if (randomLayerIndex == 0)
        {
            gameObject.layer = LayerMask.NameToLayer("BlueDimension");
            currentLayer = gameObject.layer;
        }
        else if (randomLayerIndex == 1)
        {
            gameObject.layer = LayerMask.NameToLayer("RedDimension");
            currentLayer = gameObject.layer;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawner = GameObject.FindObjectOfType<EnemySpawner>();
        if(player == null){
            Debug.Log("Player not found");
        }

        moveSpeed = Random.Range(minMoveSpeed, maxMoveSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null)
        {
            Vector3 direction = player.position - transform.position;
            direction.Normalize();

            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spawner.EnemyDestroyed();
            Destroy(gameObject);
        }
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            spawner.EnemyDestroyed();
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : MonoBehaviour
{
    private CircleCollider2D circleCollider2D;
    public float moveSpeed = 2f;
    private Transform player;
    private EnemySpawner spawner;

    private void Awake()
    {
        circleCollider2D = GetComponent<CircleCollider2D>();   
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawner = GameObject.FindObjectOfType<EnemySpawner>();
        if(player == null){
            Debug.Log("Player not found");
        }

        if(moveSpeed <= 0)
        {
            moveSpeed = 2f;
        }
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
            //do damage
            spawner.EnemyDestroyed();
            Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Camera cam;

    [Header("Player Data")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float fireRate;

    [Header("Map Objects")]
    [SerializeField] private GameObject dimensionMap1;
    [SerializeField] private GameObject dimensionMap2;
    [SerializeField] private bool isAnotherDimension;

    private float horizontalInput;
    private float verticalInput;
    private Vector2 playerMovement;
    private Vector2 mousePos;
    private float lastBulletFiredTime;

    private void Start()
    {
        if (rb == null) 
            rb = GetComponent<Rigidbody2D>();

        if (cam == null) 
            cam = Camera.main;

        if (playerSpeed <= 0) 
            playerSpeed = 6.0f;

        if (fireRate <= 0)
            fireRate = 1.0f;

        lastBulletFiredTime = 1.0f;
        isAnotherDimension = false;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        lastBulletFiredTime += Time.deltaTime;

        // Fire
        if (Input.GetButtonDown("Fire1") && lastBulletFiredTime >= fireRate)
        {
            Shoot();
            
            lastBulletFiredTime = 0.0f;
        }

        // Split Dimension
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            SplitDimension();
        }

        
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        playerMovement.x = horizontalInput;
        playerMovement.y = verticalInput;

        playerMovement.Normalize();

        // Player movement
        rb.MovePosition(rb.position + playerSpeed * Time.fixedDeltaTime * playerMovement);

        // Calculates the vector from player position to mouse position
        Vector2 lookDir = mousePos - rb.position;

        // calculates the angle between the player and the mouse cursor 
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    private void Shoot()
    {

        Debug.Log("shootiiiinnngggg");
    }

    private void SplitDimension()
    {
        if (!isAnotherDimension) 
        { 
            dimensionMap1.SetActive(false);
            dimensionMap2.SetActive(true);
            isAnotherDimension = true;
        }
        else
        {
            dimensionMap1.SetActive(true);
            dimensionMap2.SetActive(false);
            isAnotherDimension = false;
        }
    }

    private void Health()
    {

    }

}

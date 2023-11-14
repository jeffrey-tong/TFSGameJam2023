using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float playerSpeed = 6f;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Camera cam;
    private float horizontalInput;
    private float verticalInput;
    private Vector2 playerMovement;
    private Vector2 mousePos;

   
    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
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

    }
}

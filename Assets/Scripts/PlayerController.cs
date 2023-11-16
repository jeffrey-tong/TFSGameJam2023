using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private Camera cam;
    [SerializeField] private Health health;

    [Header("Player Data")]
    [SerializeField] private float playerSpeed;
    [SerializeField] private float fireRate;

    [Header("Map Objects")]
    [SerializeField] private GameObject dimensionMap1;
    [SerializeField] private GameObject dimensionMap2;
    [SerializeField] private bool isAnotherDimension;

    [Header("Bullet Data")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject projectileSpawnPoint;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float projectileLifeTime;

    [Header("Player Health")]
    [SerializeField] private Slider slider;
    [SerializeField] private Shake shake;

    [Header("Dimensions Data")]
    [SerializeField] private LayerMask redDimensionLayer;
    [SerializeField] private LayerMask blueDimensionLayer;
    [SerializeField] private float dimensionSwitchRate;
    [SerializeField] private Slider dimensionSlider;

    private float horizontalInput;
    private float verticalInput;
    private Vector2 playerMovement;
    private Vector2 mousePos;
    private float lastBulletFiredTime;
    private float dimensionSwitchCooldownTime;

    private void Start()
    {
        health.OnTakeDamage += HandleTakeDamage;
        health.OnDie += HandleDie;

        if (rb == null) 
            rb = GetComponent<Rigidbody2D>();

        if (cam == null) 
            cam = Camera.main;

        if (health == null)
            health = GetComponent<Health>();

        if (playerSpeed <= 0) 
            playerSpeed = 6.0f;

        if (fireRate <= 0)
            fireRate = 1.0f;

        if (projectileSpeed <= 0)
            projectileSpeed = 8.0f;

        if (projectileLifeTime <= 0)
            projectileLifeTime = 2.0f;

        if (dimensionSwitchRate <= 0)
            dimensionSwitchRate = 3.0f;

        lastBulletFiredTime = 1.0f;
        isAnotherDimension = false;

        dimensionSwitchCooldownTime = 3.0f;

    }

    private void OnDisable()
    {
        health.OnTakeDamage -= HandleTakeDamage;
        health.OnDie -= HandleDie;
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        lastBulletFiredTime += Time.deltaTime;
        dimensionSwitchCooldownTime += Time.deltaTime;

        // Fire
        if (Input.GetButtonDown("Fire1") && lastBulletFiredTime >= fireRate)
        {
            Shoot();
            
            lastBulletFiredTime = 0.0f;
        }

        // Split Dimension
        if (Input.GetKeyDown(KeyCode.Space) && dimensionSwitchCooldownTime >= dimensionSwitchRate) 
        {
            StartCoroutine(DimensionDelay());
            AudioManager.Instance.Play("SplitDimension");
            dimensionSwitchCooldownTime = 0.0f;
        }

    }

    private void FixedUpdate() 
    {
        Move();
        UpdateDimensionBar(dimensionSwitchCooldownTime);
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
        AudioManager.Instance.Play("PlayerShoot");
        GameObject projectileInstance =  Instantiate(projectilePrefab, projectileSpawnPoint.transform.position, projectileSpawnPoint.transform.rotation);
        Vector2 shootDirection = new Vector2(Mathf.Cos(rb.rotation * Mathf.Deg2Rad), Mathf.Sin(rb.rotation * Mathf.Deg2Rad));
        projectileInstance.GetComponent<Rigidbody2D>().velocity = shootDirection * projectileSpeed;
        Destroy(projectileInstance, projectileLifeTime);
    }

    private void SplitDimension()
    {
        if (!isAnotherDimension) 
        { 
            dimensionMap1.SetActive(false);
            dimensionMap2.SetActive(true);
            gameObject.layer = LayerMask.NameToLayer("BlueDimension");
            isAnotherDimension = true;
        }
        else
        {
            dimensionMap1.SetActive(true);
            dimensionMap2.SetActive(false);
            gameObject.layer = LayerMask.NameToLayer("RedDimension");
            isAnotherDimension = false;
        }
    }



    private IEnumerator DimensionDelay()
    {
        shake.DimensionShake();

        yield return new WaitForSeconds(0.7f);

        SplitDimension();

    }


    private void HandleTakeDamage()
    {
        AudioManager.Instance.Play("Hit");
        UpdateHealthBar(health.health);
        shake.CamShake();
        // spawn particles
      
    }

    private void HandleDie()
    {

    }

    public void UpdateHealthBar(float curenthealth)
    {
        slider.value = curenthealth;
    }

    private void UpdateDimensionBar(float currentTime)
    {
        dimensionSlider.value = currentTime;
    }
}

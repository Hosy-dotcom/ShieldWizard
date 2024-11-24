using UnityEngine;

public class UnityChanCombat : MonoBehaviour
{
    public GameObject bulletPrefab; // Bullet prefab for shooting
    public Transform firePoint; // Point from where Unity-Chan shoots
    public float bulletSpeed = 10f; // Speed of the bullet
    public float defendDuration = 1.0f; // Duration of defense animation
    private Animator animator;

    private bool isDefending = false; // Prevent multiple triggers during defense

    void Start()
    {
        // Get the Animator component attached to Unity-Chan
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Placeholder for triggering defend or shoot manually for testing
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Defend();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Shoot();
        }
    }

    // Trigger Defend Animation
    public void Defend()
    {
        if (isDefending) return; // Prevent overlapping defenses
        isDefending = true;

        // Play defend animation
        animator.SetTrigger("Defend");

        // Use a coroutine to reset defense state after animation ends
        Invoke(nameof(ResetDefend), defendDuration);
    }

    // Trigger Shoot Animation
    public void Shoot()
    {
        // Play shoot animation
        animator.SetTrigger("Shoot");

        // Instantiate a bullet and shoot
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;

        // Destroy bullet after a certain time to avoid clutter
        Destroy(bullet, 3f);
    }

    // Reset defending state
    private void ResetDefend()
    {
        isDefending = false;
    }

    // Handle collisions with bullets (for defending)
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bullet")) // Assume enemy bullets have the tag "Bullet"
        {
            // Trigger defense when hit
            Defend();

            // Destroy or reflect the incoming bullet
            Destroy(other.gameObject);
        }
    }
}

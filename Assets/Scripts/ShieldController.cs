using System.Collections;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    public GameObject shield;  // Assign the shield GameObject in the inspector
    public float shieldActiveTime = 5f;  // Duration the shield stays active
    public float shieldCooldownTime = 10f;  // Time between shield activations
    private bool isShieldActive = false;
    private int bulletsBlocked = 0;

    private void Start()
    {
        // Start the shield activation process
        StartCoroutine(ShieldActivationRoutine());
    }

    private void Update()
    {
        // Debugging: Display the bullets blocked count
        Debug.Log($"Bullets Blocked: {bulletsBlocked}");
    }

    private IEnumerator ShieldActivationRoutine()
    {
        while (true) // Infinite loop to keep the routine running
        {
            // Wait for a random cooldown time before activating the shield
            float randomCooldown = Random.Range(shieldCooldownTime / 2, shieldCooldownTime);
            yield return new WaitForSeconds(randomCooldown);

            // Activate the shield
            ActivateShield();

            // Wait for the shield to remain active
            yield return new WaitForSeconds(shieldActiveTime);

            // Deactivate the shield
            DeactivateShield();
        }
    }

    private void ActivateShield()
    {
        isShieldActive = true;
        shield.SetActive(true);  // Enable the shield GameObject
        Debug.Log("Shield Activated!");
    }

    private void DeactivateShield()
    {
        isShieldActive = false;
        shield.SetActive(false);  // Disable the shield GameObject
        Debug.Log("Shield Deactivated!");
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the shield is active and the incoming object is a bullet
        if (isShieldActive && other.CompareTag("Bullet"))
        {
            bulletsBlocked++;  // Increment the bullet block count

            // Destroy the bullet
            Destroy(other.gameObject);

            Debug.Log("Bullet Blocked!");
        }
    }
}

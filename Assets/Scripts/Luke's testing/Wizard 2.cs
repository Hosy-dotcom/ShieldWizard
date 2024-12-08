using UnityEngine;

public class Wizard : MonoBehaviour
{
    public Transform agent;         // Reference to the agent (center)
    public float orbitRadius = 5f;  // Distance from the agent
    public float orbitSpeed = 30f;  // Degrees per second
    public GameObject bulletPrefab; // Prefab for the bullet
    public float minFireDelay = 0.5f; // Minimum delay between shots
    public float maxFireDelay = 2f;  // Maximum delay between shots

    private float angle;            // Current angle of the wizard in the orbit
    private float nextFireTime;

    void Start()
    {
        // Initialize the wizard's position at the start of the orbit
        if (agent != null)
        {
            transform.position = agent.position + new Vector3(orbitRadius, 0, 0);
        }

        // Set the initial fire time
        ScheduleNextFire();
    }

    void Update()
    {
        if (agent != null)
        {
            OrbitAroundAgent();
        }

        // Check if it's time to fire
        if (Time.time >= nextFireTime)
        {
            Shoot();
            ScheduleNextFire();
        }
    }

    void OrbitAroundAgent()
    {
        // Increment angle based on orbit speed
        angle += orbitSpeed * Time.deltaTime;

        // Convert angle to radians and calculate the new position
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * orbitRadius;
        float z = Mathf.Sin(angle * Mathf.Deg2Rad) * orbitRadius;

        // Update wizard's position relative to the agent
        transform.position = new Vector3(agent.position.x + x, transform.position.y, agent.position.z + z);

        // Make the wizard face toward the agent
        transform.LookAt(agent);
    }

    void Shoot()
    {
        if (bulletPrefab != null)
        {
            // Instantiate a bullet
            GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        }
    }

    void ScheduleNextFire()
    {
        // Schedule the next fire time using a random delay
        float fireDelay = Random.Range(minFireDelay, maxFireDelay);
        nextFireTime = Time.time + fireDelay;
    }
}

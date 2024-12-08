using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 10f;        // Speed of the bullet
    public float lifeTime = 5f;     // Time after which the bullet will be destroyed

    void Start()
    {
        // Destroy the bullet after its lifetime expires
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        // Move the bullet forward in a straight line
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the bullet collides with the agent or the shield
        if (other.CompareTag("Agent") || other.CompareTag("Shield"))
        {
            // Destroy the bullet on collision
            Destroy(gameObject);
        }
    }
}

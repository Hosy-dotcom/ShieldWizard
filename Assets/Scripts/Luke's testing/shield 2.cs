using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour
{
    public float activeDuration = 3f;    // Duration for which the shield is active
    public float cooldownDuration = 5f; // Cooldown before the shield can reappear

    private Renderer shieldRenderer;
    private Collider shieldCollider;
    private bool isActive;

    void Start()
    {
        shieldRenderer = GetComponent<Renderer>();
        shieldCollider = GetComponent<Collider>();
        DeactivateShield();
        StartCoroutine(ShieldCycle());
    }

    IEnumerator ShieldCycle()
    {
        while (true)
        {
            ActivateShield();
            yield return new WaitForSeconds(activeDuration);
            DeactivateShield();
            yield return new WaitForSeconds(cooldownDuration);
        }
    }

    void ActivateShield()
    {
        isActive = true;
        shieldRenderer.enabled = true;
        shieldCollider.enabled = true;
    }

    void DeactivateShield()
    {
        isActive = false;
        shieldRenderer.enabled = false;
        shieldCollider.enabled = false;
    }
}

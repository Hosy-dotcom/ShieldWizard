using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;
using System.Collections;

public class ShieldAgent : Agent 
{
    [SerializeField] private GameObject shield;
    [SerializeField] private Transform wizard;
    [SerializeField] private float shieldDuration = 2f;
    private bool shieldActive = false;
    private float shieldCooldown = 0f;

    public override void OnEpisodeBegin()
    {
        // Reset agent and environment
        transform.position = Vector3.zero;
        shield.SetActive(false);
        shieldCooldown = 0f;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Relative position of the wizard
        sensor.AddObservation(transform.position - wizard.position);

        // Shield state (0 = inactive, 1 = active)
        sensor.AddObservation(shieldActive ? 1f : 0f);

        // Cooldown state
        sensor.AddObservation(shieldCooldown);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // Action: 0 = do nothing, 1 = activate shield
        int action = actions.DiscreteActions[0];

        if (action == 1 && !shieldActive && shieldCooldown <= 0f)
        {
            StartCoroutine(ActivateShield());
        }

        // Reward or penalize based on environment state (handled in collision scripts)
    }

    private IEnumerator ActivateShield()
    {
        shieldActive = true;
        shield.SetActive(true);
        yield return new WaitForSeconds(shieldDuration);
        shield.SetActive(false);
        shieldActive = false;
        shieldCooldown = 5f; // Cooldown period
    }

    private void Update()
    {
        // Reduce cooldown
        if (shieldCooldown > 0f)
        {
            shieldCooldown -= Time.deltaTime;
        }
    }

    public void RewardForBlocking()
    {
        AddReward(1.0f); // Reward for successfully blocking a bullet
    }

    public void PenalizeForGettingHit()
    {
        AddReward(-1.0f); // Penalty for getting hit
        EndEpisode(); // Optionally end the episode
    }
}

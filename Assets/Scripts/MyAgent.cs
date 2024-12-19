using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class MyAgent : Agent
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private float bulletSpeed=100f;
    [SerializeField] private Transform wizard;
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private Transform shieldSpawnPoint;
    [SerializeField] private CircleCenterObj circle;
    [SerializeField] private Transform Ember;

    //private bool shieldActivated = false;
    private float gunHeat;
    private const float TimeBetweenShots = 0.2f;
    private float shieldHeat;
    private float respawnTime = 1f;

    


    public override void OnEpisodeBegin()
    {
        
        circle.ResetWizardPosition();
        transform.rotation = Quaternion.Euler(0, 0, 0);

        //Radomize bulletSpeed
        bulletSpeed = Random.Range(100f, 150f);
        
        //Debug.Log("Episode Begins");

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.localPosition.x);
        sensor.AddObservation(transform.localPosition.z);

        sensor.AddObservation(wizard.localPosition.x);
        sensor.AddObservation(wizard.localPosition.z);

        //sensor.AddObservation(Ember.transform.localPosition.x);
        //sensor.AddObservation(Ember.transform.localPosition.z);
        

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.ContinuousActions;
        float actionSteering = actionTaken[0];

        //rotate the agent
        transform.rotation = Quaternion.Euler(0, actionSteering * 180, 0);



        

        float shootAction = actionTaken[1];
        if (shootAction > 0)
        {
            Shooting();
        }
        else if (shootAction<0)
        {
            Shield();
        }
        
        
        

        
    }

    public void Shooting()
    {


        if (gunHeat > 0)
        {
            gunHeat -= Time.deltaTime;
        }


        if (gunHeat <= 0)
        {
            // heat the gun up so we have to wait a bit before shooting again
            gunHeat = TimeBetweenShots;


            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.OnBulletHit += HandleBulletHit;
            }

            Rigidbody rb = bullet.GetComponent<Rigidbody>();


            rb.velocity = bulletSpawnPoint.transform.forward * bulletSpeed;
            Destroy(bullet, 2);
        }

    }
    

    public void Shield()
    {
        if (shieldHeat > 0)
        {
            shieldHeat -= Time.deltaTime;
        }


        if (shieldHeat <= 0)
        {

            shieldHeat = respawnTime;

            var shield = Instantiate(shieldObject, shieldSpawnPoint.position, shieldSpawnPoint.rotation);
            ShieldCollision shieldCollision = shield.GetComponent<ShieldCollision>();
            
            //Debug.Log("Shield Created");
            if (shieldCollision != null)
            {
                shieldCollision.OnShieldHit += HandleShieldHit;
            }
            Destroy(shield, 0.5f);
            //Debug.Log("Shield Destory");

        }

    }


    void HandleBulletHit(GameObject hitObject)
    {
        
        // Example: Check if it's an enemy
        if (hitObject.CompareTag("Wizard"))
        {
            Debug.Log("Enemy was hit by the bullet!");
            
            AddReward(2f);
            EndEpisode();
        }
        else
        {
            //Debug.Log($"Agent detected the bullet hit: {hitObject.name}");
            AddReward(-0.01f);
        }
    }
    void HandleShieldHit(GameObject hitObject)
    {

        // Example: Check if it's an enemy
        if (hitObject.CompareTag("Ember"))
        {
            Debug.Log("Shield Hit By Ember");
            Destroy(hitObject);
            AddReward(1f);
           
        }
        else
        {
            //Debug.Log($"Shield detected the bullet hit: {hitObject.name}");
            
            
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;

        actions[0] = 0;
        actions[1] = 0;

        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        if (horizontal == -1)
        {
            actions[0] = -1;
        }
        else if (horizontal == 1)
        {
            actions[0] = 1;
        }
        if (vertical == -1)
        {
            actions[1] = -1;
        }
        else if(vertical==1)
        {
            actions[1] = 1;
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ember")
        {
            Debug.Log("Unity Chan Was Shot by " + other.name);
            Destroy(other.gameObject);
            AddReward(-1f);
            EndEpisode();
        }

        
    }
}

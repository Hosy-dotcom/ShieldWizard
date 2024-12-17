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
    [SerializeField] private Transform wizardStartingPoint;
    [SerializeField] private GameObject shieldObject;
    [SerializeField] private Transform shieldSpawnPoint;
    

    //private bool shieldActivated=false;
    //private float gunHeat;
    //private const float TimeBetweenShots = 0.2f;
    private float shieldHeat;
    private const float respawnTime = 2f;




    public override void OnEpisodeBegin()
    {
        wizardStartingPoint.localPosition=new Vector3(-265f,0,-11f);
        transform.rotation = Quaternion.Euler(0, 0, 0);

        //Radomize bulletSpeed
        bulletSpeed = Random.Range(100f, 150f);
        Debug.Log("Episode Begins");

    }
    public override void CollectObservations(VectorSensor sensor)
    {
        
    }
    public override void OnActionReceived(ActionBuffers actions)
    {
        var actionTaken = actions.ContinuousActions;
        float actionSteering = actionTaken[0];

        //rotate the agent
        transform.rotation = Quaternion.Euler(0, actionSteering * 180, 0);

        //shoot
        
        Shooting();
        
        

        //Reward if wizard is killed
        //Shield();
    }

    public void Shooting()
    {


        //if (gunHeat > 0)
        //{
        //    gunHeat -= Time.deltaTime;
        //}


        //if (gunHeat <= 0)
        //{
            // heat the gun up so we have to wait a bit before shooting again
            //gunHeat = TimeBetweenShots;


            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            Bullet bulletScript = bullet.GetComponent<Bullet>();
            if (bulletScript != null)
            {
                bulletScript.OnBulletHit += HandleBulletHit;
            }

            Rigidbody rb = bullet.GetComponent<Rigidbody>();


            rb.velocity = bulletSpawnPoint.transform.forward * bulletSpeed;
            Destroy(bullet, 2);
        //}

    }
    

    public void Shield()
    {
        if (shieldHeat > 0)
        {
            shieldHeat -= Time.deltaTime;
        }


        if (shieldHeat <= 0)
        {
            // heat the gun up so we have to wait a bit before shooting again
            shieldHeat = respawnTime;
            var shield = Instantiate(shieldObject, shieldSpawnPoint.position, shieldSpawnPoint.rotation);
            ShieldCollision shieldCollision = shield.GetComponent<ShieldCollision>();
            
            Debug.Log("Shield Created");
            if (shieldCollision != null)
            {
                shieldCollision.OnShieldHit += HandleShieldHit;
            }
            Destroy(shield, 0.3f);
            Debug.Log("Shield Destory");
            
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
            
            AddReward(0.1f);
           
        }
        else
        {
            Debug.Log($"Shield detected the bullet hit: {hitObject.name}");
            
        }
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actions = actionsOut.ContinuousActions;

        actions[0] = 0;

        var horizontal = Input.GetAxisRaw("Horizontal");
        if (horizontal == -1)
        {
            actions[0] = -1;
        }
        else if (horizontal == 1)
        {
            actions[0] = 1;
        }

    }
    private void OnTriggerEnter(Collider other)
    {
    if (other.tag == "Ember") {
        Debug.Log("Unity Chan Was Shot by " + other.name);
        Destroy(other.gameObject);
            AddReward(-0.1f);
            EndEpisode();
    }

        
    }
}

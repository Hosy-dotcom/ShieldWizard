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
    [SerializeField] private float bulletSpeed=50;
    //private Vector3 startingPosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void OnEpisodeBegin()
    {
        //transform.localPosition = startingPosition;
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
        //Quaternion rotation = Quaternion.Euler(-5, 0, 0);
        //Vector3 direction = rotation * bulletSpawnPoint.transform.forward;
        //var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
        //Rigidbody rb = bullet.GetComponent<Rigidbody>();
        //rb.velocity = direction * bulletSpeed;
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        
            Debug.Log("Unity Chan Was Shot by "+other.name);
            Destroy(other.gameObject);

        
    }
}

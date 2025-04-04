using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CircleCenterObj : MonoBehaviour
{
    
    [SerializeField] private float speed = 0.1f;
    [SerializeField] private GameObject unityChan;
    [SerializeField] private GameObject wizard;
    [SerializeField] private bool moveClockwise = true;
    [SerializeField] private float radiusOffset = -20f;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 100f;
    private float angle = 0f;
    private float directon = -1;
    private Vector3 unityChanPosition;    
    private float gunHeat;
    private const float TimeBetweenShots = 1f; //seconds


    public void ResetWizardPosition()
    {
        //transform.localPosition = new Vector3(-265f, 0, -11f);
        angle = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        //random speed and bulletspeed
        speed = Random.Range(0.1f, 0.3f);
        bulletSpeed = Random.Range(50f, 75f);

        unityChanPosition =unityChan.transform.position;


        directon=moveClockwise ? -1 : 1;
        angle += Time.deltaTime * directon * speed;

        float x=Mathf.Cos(angle)*radiusOffset;
        float z= Mathf.Sin(angle)*radiusOffset;

        wizard.transform.position = new Vector3(unityChanPosition.x + x, 0, unityChanPosition.z + z);
        transform.LookAt(unityChanPosition);


        if (gunHeat > 0)
        {
            gunHeat -= Time.deltaTime;
        }


        if (gunHeat <= 0)
        {
            // heat the gun up so we have to wait a bit before shooting again
            gunHeat = TimeBetweenShots;

            var bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            bulletSpawnPoint.transform.LookAt(unityChanPosition);
            rb.velocity = bulletSpawnPoint.transform.forward * bulletSpeed;
            Destroy(bullet, 4);
        }


    }

}

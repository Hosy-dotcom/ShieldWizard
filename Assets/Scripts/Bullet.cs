using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("Unity Chan Was Shot");
        Destroy(bulletPrefab);

    }


}

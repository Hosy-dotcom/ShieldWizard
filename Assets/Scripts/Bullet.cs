using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject bulletPrefab;

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(bulletPrefab);
        Debug.Log("Unity Chan Was Shot");

    }


}

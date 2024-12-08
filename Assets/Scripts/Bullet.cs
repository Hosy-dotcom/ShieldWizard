using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public delegate void BulletHitEvent(GameObject hitObject);
    public event BulletHitEvent OnBulletHit;

    private void OnCollisionEnter(Collision collision)
    {
        
        Debug.Log("wizard is killed");
        OnBulletHit?.Invoke(collision.gameObject);
        Destroy(gameObject);
        

    }


}

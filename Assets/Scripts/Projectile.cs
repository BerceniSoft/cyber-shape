using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector3 firingPoint;

    [SerializeField]
    float bulletSpeed;

    [SerializeField]
    private float maxBulletDistance;


    
    void Start()
    {
        firingPoint = transform.position;
    }

    void Update()
    {
        MoveProjectile();
    }

    void MoveProjectile() {
        if (Vector3.Distance(firingPoint, transform.position) > maxBulletDistance){
            Destroy(this.gameObject);
        } else {
            transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
        }
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGun : MonoBehaviour
{
    [SerializeField]
    Transform firingPoint;

    [SerializeField]
    GameObject projectile;

    [SerializeField]
    float firingSpeed;

    public static PlayerGun Instance;

    private float lastTimeShot = 0;

    void Start()
    {
        Instance = GetComponent<PlayerGun>();
    }


    void Update()
    {
        RotationInput();
    }

    void RotationInput()
    {
        // rotim efectiv firePointul catre locul unde playerul da click. Se actualizeaza automat in functie de pozitia cursorului pe ecran.
        RaycastHit hit; 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out hit) ) // si && PlayerPrefs.GetInt("pause") == 0 cand facem meniu pauza.
        {
            transform.LookAt(new Vector3(hit.point.x, transform.position.y, hit.point.z));
        }
    }

    public void Shoot()
    {
        if (lastTimeShot + firingSpeed < Time.time ) //(lastTimeShot + firingSpeed < Time.time && PlayerPrefs.GetInt("pause") == 0
        {
            lastTimeShot = Time.time;
            Instantiate(projectile, firingPoint.position, firingPoint.rotation);
        }
    }
}

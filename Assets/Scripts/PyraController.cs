using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PyraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 5.0f;

    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        HandleShootInput();
        MovementControl();
               
    }

    void MovementControl()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        _rigidbody.AddTorque(new Vector3(vertical/3, 0, -horizontal/3) * speed);

    }

    void HandleShootInput() 
    {
            if (Input.GetButton("Fire1")) 
            {
                    PlayerGun.Instance.Shoot();
            }
    }

}

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













   
    // bool isRolling;
    // public float rotationSpeed;

    // Bounds bound;
    // Vector3 left, right, up, down;
    
    // // Start is called before the first frame update
    // void Start()
    // {
    //     bound = GetComponent<MeshCollider>().bounds;
    //     left = new Vector3(-bound.size.x , -bound.size.y , 0);
    //     right = new Vector3(bound.size.x , -bound.size.y , 0);
    //     up = new Vector3(0 , -bound.size.y , bound.size.z );
    //     down = new Vector3(0 , -bound.size.y , -bound.size.z );
    // }

    // // Update is called once per frame
    // void Update()
    // {
    //     // Move Forward
    //     if (Input.GetKey(KeyCode.UpArrow) && !isRolling)
    //     {
    //         StartCoroutine(Roll(up));
    //     }

    //     // Move Backwards
    //     else if (Input.GetKey(KeyCode.DownArrow) && !isRolling)
    //     {
    //         StartCoroutine(Roll(down));
    //     }

    //     // Move Right
    //     else if (Input.GetKey(KeyCode.RightArrow) && !isRolling)
    //     {
    //         StartCoroutine(Roll(right));
    //     }

    //     // Move Left
    //     else if (Input.GetKey(KeyCode.LeftArrow) && !isRolling)
    //     {
    //         StartCoroutine(Roll(left));
    //     }
    // }

    // IEnumerator Roll(Vector3 positionToRotation)
    // {
    //     isRolling = true;
    //     float angle = 0;
    //     Vector3 point = transform.position + positionToRotation;
    //     Vector3 axis = Vector3.Cross(Vector3.up, positionToRotation).normalized;

    //     while (angle < 60f)
    //     {
    //         float angleSpeed = Time.deltaTime + rotationSpeed;
    //         transform.RotateAround(point, axis, angleSpeed);
    //         angle += angleSpeed;
    //         yield return null;
    //     }

    //     transform.RotateAround(point, axis, 60 - angle);
    //     isRolling = false;
    // }
}

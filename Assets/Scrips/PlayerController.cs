using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float distanceToGround;


    // Variables player
    public int playerRunning = 10;
    public int forceJump = 10;




    // Variables componentes

    private Animator animator;
    new Rigidbody rigidbody;

    //variables camara
    public float cameraVertical;
    public Camera camPlayer;
    Vector3 RotationPlayer = Vector3.zero;
    public float rotationSensibility = 10f;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rigidbody = GetComponent<Rigidbody>();
       
        distanceToGround = GetComponent<Collider>().bounds.extents.y;


    }


    private void FixedUpdate()
    {
       
    }


    // Update is called once per frame
    void Update()
    {

        Move();

        Jump();

        Look();


    }

    private bool IsGrounded()
    {
        return Physics.BoxCast(this.transform.position, new Vector3(0.4f, 0f, 0.4f), Vector3.down, Quaternion.identity, distanceToGround + 0.1f);

    }

    private void Move()
    {

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        Vector3 velocity = Vector3.zero;

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 dirrection = (transform.forward * vertical + transform.right * horizontal).normalized;

            velocity = dirrection * playerRunning;
        }
        velocity.y = rigidbody.velocity.y;
        rigidbody.velocity = velocity;

        animator.SetFloat("velX", horizontal);
        animator.SetFloat("velY", vertical);

    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            rigidbody.AddForce(Vector3.up * forceJump, ForceMode.Impulse);
        }
    }

    void Look()
    {
        RotationPlayer.x = Input.GetAxis("Mouse X") * rotationSensibility + Time.deltaTime;
        RotationPlayer.y = Input.GetAxis("Mouse Y") * rotationSensibility + Time.deltaTime;


        cameraVertical = cameraVertical + RotationPlayer.y;
        cameraVertical = Mathf.Clamp(cameraVertical, -70, 70);

        transform.Rotate(Vector3.up * RotationPlayer.x);
        camPlayer.transform.localRotation = Quaternion.Euler(-cameraVertical, 0f, 0f);


    }

}


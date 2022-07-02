using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    float distanceToGround;


    // Variables player
    public float playerRunning = 10;
    public float RUNNING_FORCE = 2f;
    public int forceJump = 10;


    // Variables vida y energia

    public int maxHealth { get; private set; }
    public int currentHealth { get; private set; }
    public float healthRange { get { return (float)currentHealth / (float)maxHealth; } }

    public float maxEnergy { get; private set; }

    public float currentEnergy;
    
    public float energyRange { get { return (float)currentEnergy / (float)maxEnergy; } }

    public PlayerStatus_UI playerStatus;


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

        maxHealth = 100;
        currentHealth = maxHealth;
        maxEnergy = 100;
        currentEnergy = maxEnergy;
        
        rigidbody = GetComponent<Rigidbody>();
       
        distanceToGround = GetComponent<Collider>().bounds.extents.y;


    }





    // Update is called once per frame
    void Update()
    {
        if (GameManager.current.currentGameState == GameState.inGame)
        {
            if (Input.GetKey(KeyCode.LeftShift) && IsGrounded())
            {
                Move(true);

            }

            else
            {

                
                Move(false);
              
            }


            Jump();

            Look();


           
        }




        if (Input.GetKeyDown(KeyCode.F))
        {
            takeDamage(5);
        }

     
    

    }


    public void takeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        playerStatus.SetHealth(healthRange);

        if (currentHealth <= 0)
        {
            Died();
        }

       
    }


    public void Energy(float decrease)
    {
        currentEnergy -= decrease;
        currentEnergy = Mathf.Clamp(currentEnergy, 0, maxEnergy);
        playerStatus.SetEnergy(energyRange);

    }


    public void IncreaseLife(float IncreaseEnergy)
    {
        if (currentEnergy < maxEnergy)
        {
            currentEnergy += IncreaseEnergy;
            playerStatus.SetEnergy(energyRange);

            
        }
        
    }
    private bool IsGrounded()
    {
        return Physics.BoxCast(this.transform.position, new Vector3(0.4f, 0f, 0.4f), Vector3.down, Quaternion.identity, distanceToGround + 0.1f);

    }

    private void Move(bool Isrunning)
    {

        float runningSpeedFactor = playerRunning;
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");


        if (Isrunning && currentEnergy > 0 && vertical == 1)
        {


            Energy(0.5f);
            runningSpeedFactor *= RUNNING_FORCE;
        }


        if (!Isrunning)
        {
            IncreaseLife(1f);
        }
     

        Vector3 velocity = Vector3.zero;

        if (horizontal != 0 || vertical != 0)
        {
            Vector3 dirrection = (transform.forward * vertical + transform.right * horizontal).normalized;

            velocity = dirrection * runningSpeedFactor;
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


   public void Died()
    {
        Debug.Log("personaje muere");
        GameManager.current.currentGameState = GameState.InGameOver;
    }

}


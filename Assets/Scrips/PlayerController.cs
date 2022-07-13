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

    public int MaxHealth { get; private set; }
    public int CurrentHealth { get; private set; }
    public float HealthRange { get { return (float)CurrentHealth / (float)MaxHealth; } }

    public float MaxEnergy { get; private set; }

    public float CurrentEnergy { get; private set; }
    
    public float EnergyRange { get { return (float)CurrentEnergy / (float)MaxEnergy; } }

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

        MaxHealth = 100;
        CurrentHealth = MaxHealth;
        MaxEnergy = 100;
        CurrentEnergy = MaxEnergy;
        
        rigidbody = GetComponent<Rigidbody>();
       
        distanceToGround = GetComponent<Collider>().bounds.extents.y;


    }





    // Update is called once per frame
    void Update()
    {
        if (GameManager.current.currentGameState == GameState.inGame)
        {
            if (Input.GetButton("SuperRunning") && IsGrounded())
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
            TakeDamage(5);
        }

     
    

    }


    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        playerStatus.SetHealth(HealthRange);

        if (CurrentHealth <= 0)
        {
            Died();
        }

       
    }


    public void Energy(float decrease)
    {
        CurrentEnergy -= decrease;
        CurrentEnergy = Mathf.Clamp(CurrentEnergy, 0, MaxEnergy);
        playerStatus.SetEnergy(EnergyRange);

    }


    public void IncreaseEnergy(float IncreaseEnergy)
    {
        if (CurrentEnergy < MaxEnergy)
        {
            CurrentEnergy += IncreaseEnergy;
            playerStatus.SetEnergy(EnergyRange);

            
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


        if (Isrunning && CurrentEnergy > 0 && vertical == 1)
        {


            Energy(0.3f);
            runningSpeedFactor *= RUNNING_FORCE;
        }


        if (!Isrunning)
        {
            IncreaseEnergy(0.2f);
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


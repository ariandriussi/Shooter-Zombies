using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{


    public float LifeEnemgy;
    private Rigidbody enemyRB;
   
    private GameObject player;
 

    
   private void Awake()
    {

        enemyRB = GetComponent<Rigidbody>();

    }
    
    
    // Start is called before the first frame update
    
 
    void Start()
    {
        player = GameObject.Find("player");
        LifeEnemgy = 2;
       
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.LookAt(player.transform);
        }

 


      

     }

    private void FixedUpdate()
    {
      //FollowingPlayer(200);
    }

    void KillingEnemgy()
    {
        GameManager.current.DefeatedEnemgys++;
        Debug.Log("Muerte");
        Destroy(gameObject);
    }

    public void DamageReceivded(float damageAmount)
    {
        LifeEnemgy -= damageAmount;

        if (LifeEnemgy <= 0)
        {
            KillingEnemgy();
        }


    }

    private void FollowingPlayer(float speed)
    {
        enemyRB.AddForce((player.transform.position - this.transform.position).normalized * speed *10* Time.deltaTime);
    }

   
}

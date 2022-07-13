using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletHIt : MonoBehaviour
{

    float damageAmount = 1f;
    EnemyControler enemy;




    private void OnTriggerEnter(Collider other)
    {
       


        enemy = other.transform.GetComponent<EnemyControler>();

        if (enemy != null)
        {
            enemy.DamageReceivded(damageAmount);
            Destroy(gameObject);
        }


    }
}

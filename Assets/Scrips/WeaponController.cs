using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("General")]
    Transform cameraPlayerTransform;
    public LayerMask hittableLayers;
    public GameObject bulletHolePrefab;

    [Header("Shoot Parametrs")]
    public float fireRange = 200;


    private void Update()
    {
        HandlShoot();
    }


    private void Start()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private void HandlShoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            RaycastHit hit;
         if (Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, fireRange, hittableLayers))
            {
                GameObject bulletHoleCLone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleCLone, 4f);
            }
            
        }
    }


}


   

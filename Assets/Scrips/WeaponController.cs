using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("references")]

    public Transform weaponMuzzle;
    
    [Header("General")]

    Transform cameraPlayerTransform;
    public LayerMask hittableLayers;
    public GameObject bulletHolePrefab;

    [Header("Shoot Parametrs")]
    public float fireRange = 200;
    public float recoilForce = 4f; // Fuerza de retroceso del arma

    [Header("Sounds & Visuals")]
    public GameObject flashEffect;
    
    private void Update()
    {
        HandlShoot();

        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }


    private void Start()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }
    private void HandlShoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {

           GameObject flashClone = Instantiate(flashEffect, weaponMuzzle.position, Quaternion.Euler(weaponMuzzle.forward),transform);
            Destroy(flashClone, 1f);
            AddRecoil();
            RaycastHit hit;
         if (Physics.Raycast(cameraPlayerTransform.position, cameraPlayerTransform.forward, out hit, fireRange, hittableLayers))
            {
                GameObject bulletHoleCLone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleCLone, 4f);
            }
            
        }
    }

    private void AddRecoil()
    {
        transform.Rotate(-recoilForce, 0, 0);
        transform.position = transform.position - transform.forward * (recoilForce / 50f);
    }


}


   

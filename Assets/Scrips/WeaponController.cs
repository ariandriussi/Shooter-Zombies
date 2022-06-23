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
    public float fireRate = 0.6f;
    public int maxAmmo = 8;

    public int currentAmmo { get; private set; }

    private float LasTimeShoot = Mathf.NegativeInfinity;

    [Header("Sounds & Visuals")]
    public GameObject flashEffect;


    [Header ("Reload Parameters")]
    public float reloadTime = 1.5f;

    public GameObject owner { set; get; }


    private void Awake()
    {
        currentAmmo = maxAmmo;
        EventManager.current.updateBulletsEvents.Invoke(currentAmmo, maxAmmo);
    }


    private void Start()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            TryShoot();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(Reload());
        }


        transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * 5f);
    }

    private bool TryShoot()
    {

        if (LasTimeShoot + fireRate < Time.time)
        {
            if (currentAmmo >= 1)
            {
                HandlShoot();
                currentAmmo -= 1;
                EventManager.current.updateBulletsEvents.Invoke(currentAmmo, maxAmmo);
                return true;
            }
        }
        return false;
    }

    private void HandlShoot()
    {
        
        

           GameObject flashClone = Instantiate(flashEffect, weaponMuzzle.position, Quaternion.Euler(weaponMuzzle.forward),transform);
            Destroy(flashClone, 1f);
            AddRecoil();
      

        RaycastHit[] hits;
        hits = Physics.RaycastAll(cameraPlayerTransform.position, cameraPlayerTransform.forward, fireRange, hittableLayers);

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject != owner)
            {
                GameObject bulletHoleCLone = Instantiate(bulletHolePrefab, hit.point + hit.normal * 0.001f, Quaternion.LookRotation(hit.normal));
                Destroy(bulletHoleCLone, 4f);
            }
        }

         LasTimeShoot = Time.time;
            
        
    }

    private void AddRecoil()
    {
        transform.Rotate(-recoilForce, 0, 0);
        transform.position = transform.position - transform.forward * (recoilForce / 50f);
    }


    IEnumerator Reload()
    {
        Debug.Log("Recargando...");
        yield return new WaitForSeconds(reloadTime);
        currentAmmo = maxAmmo;
        EventManager.current.updateBulletsEvents.Invoke(currentAmmo, maxAmmo);
        Debug.Log("recargada");
    }

}


   

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ShootType
{
    Manual,
    Automatic
}

public class WeaponController : MonoBehaviour
{
    [Header("references")]

    public Transform weaponMuzzle;
    
    [Header("General")]

    Transform cameraPlayerTransform;
    public LayerMask hittableLayers;
    public GameObject bulletHolePrefab;
 

    [Header("Shoot Parametrs")]
    public ShootType shootType;
    public float fireRange = 200;
    public float recoilForce = 4f; // Fuerza de retroceso del arma
    public float fireRate = 0.6f; // Retraso de cada disparo
    public int maxAmmo = 8;
    public int totalMagazine = 20;
   
   int remainingBullets;

    public int currentmagazine { get; set; }
    public int currentAmmo { get; set; }

    private float LasTimeShoot = Mathf.NegativeInfinity;

    [Header("Sounds & Visuals")]
    public GameObject flashEffect;


    [Header ("Reload Parameters")]
    public float reloadTime = 1.5f;

    public GameObject owner { set; get; }


    private void Awake()
    {
        currentAmmo = maxAmmo;
        currentmagazine = totalMagazine;
       
    }


    private void Start()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
      
    }

    private void Update()
    {

        EventManager.current.updateBulletsEvents.Invoke(currentAmmo, currentmagazine);


        if (Input.GetKeyUp(KeyCode.E))
        {
            currentmagazine = totalMagazine;
        }
        if (shootType == ShootType.Manual)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                TryShoot();
            }
        }
         else if (shootType == ShootType.Automatic)
        {
            if (Input.GetButton("Fire1"))
            {
                TryShoot();
            }
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
                remainingBullets++;
                EventManager.current.updateBulletsEvents.Invoke(currentAmmo, currentmagazine);
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
        // si la cantidad de balas intanciadas es mayor a 1 y el cargador de balas es mayor a 0 puede recargar
        if (remainingBullets >= 1 && currentmagazine > 0)
        {
            Debug.Log("Recargando...");
            yield return new WaitForSeconds(reloadTime);
            if (remainingBullets < currentmagazine)
            {                                            //En el caso que la cantidad de balas instanciadas sea menor que la cantidad de balas dentro del cargador, cargara el arma completa.
                currentmagazine -= remainingBullets;
                remainingBullets = 0;
                currentAmmo = maxAmmo;
            }
            else
            {
                currentAmmo += currentmagazine;        //En el caso que la cantidad de balas istanciadas sea mayor a la cantidad dentro del cargador, el arma cargara sólo lo que resta en el cargador.
                currentmagazine = 0;
            }
            
          
             EventManager.current.updateBulletsEvents.Invoke(currentAmmo, currentmagazine);
            Debug.Log("recargada");
        } 
    }
       

}


   

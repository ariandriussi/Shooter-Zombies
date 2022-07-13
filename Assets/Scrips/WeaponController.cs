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
    public GameObject BulletModel;
    
 

    [Header("Shoot Parametrs")]
    public ShootType shootType;
    public float fireForce = 2;
    public float fireRange = 200;
    public float recoilForce = 4f; // Fuerza de retroceso del arma
    public float fireRate = 0.6f; // Retraso de cada disparo
    public int maxAmmo = 8;
    public int totalMagazine = 20;
   
   int remainingBullets;

    public int Currentmagazine { get; set; }
    public int CurrentAmmo { get; set; }

    private float LasTimeShoot = Mathf.NegativeInfinity;

    [Header("Sounds & Visuals")]
    public GameObject flashEffect;


    [Header ("Reload Parameters")]
    public float reloadTime = 1.5f;

    public GameObject Owner { set; get; }


    private void Awake()
    {
        CurrentAmmo = maxAmmo;
        Currentmagazine = totalMagazine;
       
    }


    private void Start()
    {
        cameraPlayerTransform = GameObject.FindGameObjectWithTag("MainCamera").transform;
        
      
    }

    private void Update()
    {

        EventManager.current.updateBulletsEvents.Invoke(CurrentAmmo, Currentmagazine);

        
        if (Input.GetKeyUp(KeyCode.E))
        {
            Currentmagazine = totalMagazine;
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
            if (CurrentAmmo >= 1)
            {
                HandlShoot();
                CurrentAmmo -= 1;
                remainingBullets++;
                EventManager.current.updateBulletsEvents.Invoke(CurrentAmmo, Currentmagazine);
                return true;
            }
        }
        return false;
    }

    private void HandlShoot()
    {
        
        

      GameObject flashClone = Instantiate(flashEffect, weaponMuzzle.position, Quaternion.Euler(weaponMuzzle.forward),transform);
       Destroy(flashClone, 1f);



        IstanceBullet();
        AddRecoil();
        RaycastHit[] hits;
        hits = Physics.RaycastAll(cameraPlayerTransform.position, cameraPlayerTransform.forward, fireRange, hittableLayers);
      

        foreach (RaycastHit hit in hits)
        {
            if (hit.collider.gameObject != Owner)
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

    public void IstanceBullet()
    {

        GameObject bulletCLone = Instantiate(BulletModel, weaponMuzzle.position, Quaternion.identity);
        bulletCLone.GetComponent<Rigidbody>().AddForce(weaponMuzzle.forward*fireForce);
        Destroy(bulletCLone, 20f);
    }


    IEnumerator Reload()
    {
        // si la cantidad de balas intanciadas es mayor a 1 y el cargador de balas es mayor a 0 puede recargar
        if (remainingBullets >= 1 && Currentmagazine > 0)
        {
            Debug.Log("Recargando...");
            yield return new WaitForSeconds(reloadTime);
            if (remainingBullets < Currentmagazine)
            {                                            //En el caso que la cantidad de balas instanciadas sea menor que la cantidad de balas dentro del cargador, cargara el arma completa.
                Currentmagazine -= remainingBullets;
                remainingBullets = 0;
                CurrentAmmo = maxAmmo;
            }
            else
            {
                CurrentAmmo += Currentmagazine;        //En el caso que la cantidad de balas istanciadas sea mayor a la cantidad dentro del cargador, el arma cargara sólo lo que resta en el cargador.
                Currentmagazine = 0;
            }
            
          
             EventManager.current.updateBulletsEvents.Invoke(CurrentAmmo, Currentmagazine);
            Debug.Log("recargada");
        } 
    }
       

}


   

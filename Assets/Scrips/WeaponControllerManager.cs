using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponControllerManager : MonoBehaviour
{
    public List<WeaponController> startingWeapons = new List<WeaponController>();

    public Transform weaponParentSocket;
    public Transform defaultWeaponPosition;
    public Transform aimingPosition;

 

    public int ActiveWeaponIndex { get; private set; }

    private WeaponController[] weaponSlots = new WeaponController[2];

    // Start is called before the first frame update
    void Start()
    {
        ActiveWeaponIndex = -1;

        foreach (WeaponController startingWeapon in startingWeapons)
        {
            AddWeapon(startingWeapon);
        }

        EventManager.current.newGunEvent.Invoke();
        SwitchWeapon(0);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(0);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(1);
        }
    }

    private void SwitchWeapon(int index)
    {

     
        int tempIndex = index;

        if (weaponSlots[tempIndex] == null) { return; }
          

        foreach(WeaponController weapon in weaponSlots)
        {
            if (weapon != null) { weapon.gameObject.SetActive(false); }
        }
            weaponSlots[tempIndex].gameObject.SetActive(true);
        ActiveWeaponIndex = tempIndex;
        EventManager.current.newGunEvent.Invoke();
    }

    private void AddWeapon(WeaponController p_weaponPrefab)
    {
        weaponParentSocket.position = defaultWeaponPosition.position;

        //Añadir arma al jugador pero no mostrarla
        for (int i = 0; i < weaponSlots.Length; i++)
        {
            if (weaponSlots[i] == null)
            {
                WeaponController weaponClone = Instantiate(p_weaponPrefab, weaponParentSocket);
                weaponClone.Owner = gameObject;
                weaponClone.gameObject.SetActive(false);

               

                weaponSlots[i] = weaponClone;
                return;
            }
        }
    }
}

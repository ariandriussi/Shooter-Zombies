using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus_UI : MonoBehaviour
{
    public float barSpeed = 1f;
    public Image healthBar;
    public Image healthBarShadow;
    public Image energyBar;
    public Image energyBarShadow;
    private float nextHealth;
    private float nextEnergy;


    private bool activeShadowHealth = false;
    private bool activeShadowEnegy = false;
    private void Awake()
    {
        healthBar.fillAmount = 1f;
        energyBar.fillAmount = 1f;
        nextHealth = healthBar.fillAmount;
        nextEnergy = energyBar.fillAmount;
    }

  

    public void Update()
    {
        
        if (healthBar.fillAmount != nextHealth)
        {
            healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, nextHealth, Time.deltaTime * barSpeed);
        }

        if (activeShadowHealth)
        {
            healthBarShadow.fillAmount = Mathf.MoveTowards(healthBarShadow.fillAmount, nextHealth, Time.deltaTime * barSpeed)
;       }

        if (healthBarShadow.fillAmount == nextHealth)
        {
            activeShadowHealth = false;
        }


        //energy
        if (energyBar.fillAmount != nextEnergy)
        {

            energyBar.fillAmount = Mathf.MoveTowards(energyBar.fillAmount, nextEnergy, Time.deltaTime * barSpeed);
            
        }

        if (activeShadowEnegy)
        {
            energyBarShadow.fillAmount = Mathf.MoveTowards(energyBarShadow.fillAmount, nextEnergy, Time.deltaTime * barSpeed)
;
        }

        if (energyBarShadow.fillAmount == nextEnergy)
        {
            activeShadowEnegy = false;
        }

        

    }


    IEnumerator ActiveShadowHealth()
    {
        yield return new WaitForSeconds(0.2f);
        activeShadowHealth = true;
    }


    IEnumerator ActiveShadowEnergy()
    {
        yield return new WaitForSeconds(0.2f);
        activeShadowEnegy = true;
    }




    public void SetHealth(float healthPercentage)
    {
        nextHealth = healthPercentage;
        StartCoroutine(ActiveShadowHealth());
    }

    public void SetEnergy(float energyPercentage)
    {
        nextEnergy = energyPercentage;
        StartCoroutine(ActiveShadowEnergy());


    }
}

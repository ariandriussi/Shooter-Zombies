using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus_UI : MonoBehaviour
{
    public float barSpeed = 1f;
    public Image healthBar;
    public Image healthBarShadow;
    private float nextHealth;


    private bool activeShadow = false;
    private void Awake()
    {
        healthBar.fillAmount = 1f;
        nextHealth = healthBar.fillAmount;
    }

  
    public void Update()
    {
        
        if (healthBar.fillAmount != nextHealth)
        {
            healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, nextHealth, Time.deltaTime * barSpeed);
        }

        if (activeShadow)
        {
            healthBarShadow.fillAmount = Mathf.MoveTowards(healthBarShadow.fillAmount, nextHealth, Time.deltaTime * barSpeed)
;       }

        if (healthBarShadow.fillAmount == nextHealth)
        {
            activeShadow = false;
        }

       
    }


    IEnumerator ActiveShadowHealth()
    {
        yield return new WaitForSeconds(0.2f);
        activeShadow = true;
    }




    public void SetHealth(float healthPercentage)
    {
        nextHealth = healthPercentage;
        StartCoroutine(ActiveShadowHealth());
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[Serializable]

public class WeaponInfo_UI : MonoBehaviour
{
    public TMP_Text currentBullets, totalBullets;
  

    private void OnEnable()
    {
        EventManager.current.updateBulletsEvents.AddListener(UpdateCurrent);
    }

    private void OnDisable()
    {
        EventManager.current.updateBulletsEvents.RemoveListener(UpdateCurrent);
    }

    public void UpdateCurrent(int newCurrentBullets, int newTotalBullets)
    {
        if (newCurrentBullets <= 0)
        {
            currentBullets.color = Color.red;
        }
        else
        {
            currentBullets.color = Color.white;
        }
        currentBullets.text = newCurrentBullets.ToString();
        totalBullets.text = newTotalBullets.ToString();
     


    }
}

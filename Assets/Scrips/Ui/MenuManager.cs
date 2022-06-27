using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{

    #region singleton

    public static MenuManager instance;

    public void Awake()
    {
        if (instance == null )
        {
            instance = this;
        }
    }


    #endregion



    public Canvas Game, Pause, menu;

  public  void ShowMenuPause()
    {
        Pause.enabled = true;
    }

 public   void HideMenuPause()
    {
        Pause.enabled = false;
    }

   public void ShowMenuGame()
    {
        Game.enabled = true;
    }

    public void HideMenuGame()
    {
        Game.enabled = false;
    }

    public void ShowMenu()
    {
        menu.enabled = true;
    }

    public void HideMenu()
    {
        menu.enabled = false;
    }
}

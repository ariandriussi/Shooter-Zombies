using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum GameState{
    inGame,
    inMenu,
    InGameOver,
    inPause
}
public class GameManager : MonoBehaviour
{

    #region singleton 
    public static GameManager current;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
    }

    #endregion

    public GameState currentGameState = GameState.inMenu;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && currentGameState == GameState.inGame)
        {

            // Al tocar la P si el estado del juego está en inGame el estado de juego pasara a pause.
      PauseGame();

        }
        else if (Input.GetKeyDown(KeyCode.P) && currentGameState == GameState.inPause)
        {
            // Lo mismo que el anterior pero a la inversa
        currentGameState = GameState.inGame;



        }


    }

    
    public void StartGame()
    {

        SetGameState(GameState.inGame);

    }

    public void EndGame()
    {
        SetGameState(GameState.InGameOver);
    }

    public void BackToMenu()
    {

        SetGameState(GameState.inMenu);

    }

    public void PauseGame()
    {
        SetGameState(GameState.inPause);
    }






    public void SetGameState(GameState newGameState)
    {
       if (newGameState == GameState.inGame)
        {
    
            MenuManager.instance.ShowMenuGame();
            MenuManager.instance.HideMenuPause();
            MenuManager.instance.HideMenu();
            Cursor.lockState = CursorLockMode.Locked;
        }

       else if (newGameState == GameState.inMenu)
        {
            MenuManager.instance.ShowMenu();
            MenuManager.instance.HideMenuGame();
            MenuManager.instance.HideMenuPause();
            Cursor.lockState = CursorLockMode.None;
        }

       else if (newGameState == GameState.inPause)
        {

            MenuManager.instance.ShowMenuPause();
            MenuManager.instance.HideMenuGame();
            MenuManager.instance.HideMenu();
            Cursor.lockState = CursorLockMode.None;
        }

       else if (newGameState == GameState.InGameOver)
        {

        }


        this.currentGameState = newGameState;
    }
}

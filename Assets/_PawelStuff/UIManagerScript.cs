//using UnityEngine;
//using System.Collections;
//using System.Globalization;
//using UnityEngine.SceneManagement;
//using UnityEngine.UI;
//using UnityStandardAssets.ImageEffects;
//
//public class UIManagerScript : Singleton<UIManagerScript>
//{
//
//    public GameObject EndGamePanel, GameplayPanel, MainMenu;
//    public Text[] playerScores;
//    //private StartInfoOptionsScript startInfoScript;
//    public Camera camera;
//    private BlurOptimized blur;
//    public Text EndText;
// 
//    private int PlayersAmount = 3;
//
//    void OnEnable()
//    {
//        blur = camera.GetComponent<BlurOptimized>();
//    }
//
//    public void QuitApplication()
//    {
//        Application.Quit();
//    }
//    public void TransitionToTitle()
//    {
//        blur = camera.GetComponent<BlurOptimized>();
//        blur.enabled = true;
//        MainMenu.SetActive(true);
//        GameplayPanel.SetActive(false);
//        EndGamePanel.SetActive(false);
//    }
//
//    public void TransitionToAvatarSelection()
//    {
//        //transition to avatar selection here
//        GameplayPanel.SetActive(false);
////        SelectAvatarPanel.SetActive(true);
////        GameManager.Instance.SetPlayersAmount(PlayersAmount);
//    }
//
//    public void TransitionToGameplay()
//    {
//        //transition to gameplay here
//        MainMenu.SetActive(false);
//        blur.enabled = false;
//        GameplayPanel.SetActive(true);
//        EndGamePanel.SetActive(false);
//        //        SelectAvatarPanel.SetActive(false);
//    }
//
//    public void TransitionToEndGame(int num)
//    {
//        //transition to gameplay here
//        MainMenu.SetActive(false);
//        blur.enabled = true;
//        GameplayPanel.SetActive(false);
//        EndGamePanel.SetActive(true);
//        EndText.text = "Player " + num + " Wins!";
//
//        //        SelectAvatarPanel.SetActive(false);
//    }
//
//    public void ChangeScore(int playerNumber, int amount)
//    {
//        playerScores[playerNumber].text = amount.ToString();
//    }
//
//    public void ClickPlay()
//    {
//        //GameManager.Instance.ResetLifesOfPlayers();
//        GameManager.Instance.StartGame();
//    }
//
//    public void RestartGame()
//    {
//        GameManager.Instance.BackToTitleScreen();
//        ;
//    }
//}

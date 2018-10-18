using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Lifetime;
using UnityEngine;
using UnityEngine.Assertions;
//using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    public Character[] characters;
    public GameStates state = GameStates.TitleScreen;
    public ObjectSpawner objectSpawner;
    public static int numberOfLifes = 1;
    public bool[] arrayOfLoosers;

    private int[] scores = new int[3];

    private void Awake()
    {
//        Assert.AreEqual(arrayOfLoosers.Length, characters.Length);
        arrayOfLoosers = new bool[characters.Length];
        //Debug.Log(arrayOfLoosers[1]);
        ChangeState(GameStates.TitleScreen);
    }

    private void Update()
    {
        if (state == GameStates.Endgame && Input.GetKeyDown(KeyCode.R))
        {
            ChangeState(GameStates.TitleScreen);
            BackToTitleScreen();
            objectSpawner.Reload();
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].lifes = numberOfLifes;
                StartCoroutine(SpawnPlayerImmediately(i));
            }
            
            // Application.LoadLevel(0);
        }
    }

    private void ChangeState(GameStates newState)
    {
        switch (newState)
        {
            case GameStates.TitleScreen:
                ResetScores();
                Time.timeScale = 0.001f;
                UIManagerScript.Instance.TransitionToTitle();
                break;

            case GameStates.Gameplay:
                Time.timeScale = 1;
                UIManagerScript.Instance.TransitionToGameplay();
                StartCoroutine(DelayedStartGameplay());
                break;

            case GameStates.Endgame:
                objectSpawner.IsActive = false;
                Time.timeScale = 0.0001f;
                //                for (int i = 0; i < characters.Length; i++)
                //                {
                //                    //characters[i].gameObject.SetActive(false);
                ////                    for (int j = 0; j < arrayOfLoosers.Length; j++)
                ////                    {
                ////                        Debug.Log(arrayOfLoosers[j]);
                ////                    }
                //                    Debug.Log("The Winner is.... " + GetWinner());
                //                }
                UIManagerScript.Instance.TransitionToEndGame(WinnerPlayer()+1);
                
                break;
        }

        state = newState;
    }

    private IEnumerator DelayedStartGameplay()
    {
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < characters.Length; i++)
        {
            characters[i].gameObject.SetActive(true);
        }
    }

    public void OnPlayerDead(int playerNumber)
    {
        scores[playerNumber]--;
        UIManagerScript.Instance.ChangeScore(playerNumber, scores[playerNumber]);
        //        Character _character = characters[playerNumber];
        //        _character.lifes -= 1;
        //Debug.Log("Last? " + IsLastPlayer());
        if (IsLastPlayer())
        {
            ChangeState(GameStates.Endgame);
        }
        else if (scores[playerNumber] > 0)
        {
            StartCoroutine(SpawnPlayer(playerNumber));
        }
        //        else
        //        {
        //            arrayOfLoosers[characters[playerNumber].playerNumber] = true;
        ////            Debug.Log("Looser: " + characters[playerNumber].playerNumber);
        ////            Debug.Log("array: " + arrayOfLoosers.Length);
        //        }
        //Debug.Log("P1" + scores[0]);
        //Debug.Log("p2" + scores[1]);

        //Debug.Log("p3" + scores[2]);

        int onePlayerLive = 0;
        int playerId = 0;
        for ( playerId = 0; playerId<scores.Length;playerId++ )
        {
            if (scores[playerId] == 0)
            {
                onePlayerLive++;
            }

        }

        if (onePlayerLive == 2)
        {
            //Debug.Log("GAME OVER!");
            ChangeState(GameStates.Endgame);
        }
    }

    //public int sum()
    //{
    //    int k = 0;
    //    for (int i = 0; i < 3; i++)
    //    {
    //        k += scores[i];

    //    }
    //    return k;
    //}

    //public int GetWinner()
    //{
    //    /*
    //    for (int i = 0; i < characters.Length; i++)
    //    {
    //        if (characters[i].lifes > 0) return characters[i].playerNumber;
    //    }

    //    return -1;
    //    */
    //}

    public bool IsLastPlayer()
    {
        
        int _k = 2;
        for(int i = 0; i < 3; i++)
        {
            if (characters[i].lifes <= 0) _k--;
        }
        return _k <= 0;
    }

//    public int WinnerPlayer()
//    {
//        int id = 2;
//        for (int i = 0; i < characters.Length; i++)
//        {
//            if (characters[i].lifes > 0)
//            {
//                id = i;
//            }
//        }
//        return id;
//    }
    public int WinnerPlayer()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (scores[i] != 0) return i;
        }
        
        return -1;
    }

    private IEnumerator SpawnPlayer(int playerNumber)
	{
		yield return new WaitForSeconds(1f);
		characters[playerNumber].Respawn();
	}

    private IEnumerator SpawnPlayerImmediately(int playerNumber)
    {
        characters[playerNumber].Respawn();
        yield return null;
    }

    public void BackToTitleScreen()
    {
        ChangeState(GameStates.TitleScreen);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void StartGame()
    {
        ChangeState(GameStates.Gameplay);
    }

    //	public void ChangeRounds(BaseEventData data)
    //	{
    //		AxisEventData axisData = data as AxisEventData;
    //
    //		if(axisData.moveDir == MoveDirection.Left)
    //		{
    //			winScore -= 2;
    //		}
    //		if(axisData.moveDir == MoveDirection.Right)
    //		{
    //			winScore += 2;
    //		}
    //
    //		winScore = Mathf.Clamp(winScore, 1, 7);
    //	}

	public enum GameStates
	{
		TitleScreen,
		Gameplay,
		Endgame
	}

    public void ResetLifesOfPlayers()
    {

        foreach(Character character in characters)
        {
            character.lifes = numberOfLifes;
        }
    }

    private void ResetScores()
    {
        for (int i = 0; i < 3; i++)
        {
            scores[i] = 3;
            UIManagerScript.Instance.ChangeScore(i, scores[i]);
        }
        //GameManager.Instance.SetPlayersAmount(PlayersAmount);
        //GameManager.Instance.StartGame();
    }
}

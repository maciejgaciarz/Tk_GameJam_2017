using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : Singleton<UIManager>
{
	public Image titleLogo;
	//public Text[] playerScores;
	public Text roundsLabel;
	public Transform mainCamera;
	public CanvasGroup mainTitleCanvas, instructionsCanvas, buttonsCanvas, inGameUICanvas, endGameCanvas;
	public GameObject playButton, instructionsBackButton, backToTitleScreenButton, instructionsButton;

//	public void TransitionToTitle()
//	{
		//DOTween.KillAll(false);
//
//		endGameCanvas.interactable = false;
		//endGameCanvas.DOFade(0f, .5f);

//		mainTitleCanvas.interactable = true;
//		mainTitleCanvas.alpha = 1f;

		//mainCamera.DOMove(new Vector3(3.2f, 26f, -25.5f), 1f, false);
		//titleLogo.DOFade(1f, .5f).SetDelay(1f);
		//buttonsCanvas.DOFade(1f, .5f).SetDelay(.5f);
		//inGameUICanvas.DOFade(0f, .5f);

		//EventSystem.current.SetSelectedGameObject(playButton);
//	}

	public void TransitionToGameplay()
	{
		//DOTween.KillAll(false);

//		mainTitleCanvas.interactable = false;

		//mainCamera.DOMove(new Vector3(3.2f, 19.3f, -29.3f), 1f, false);
		//titleLogo.DOFade(0f, .5f);
		//buttonsCanvas.DOFade(0f, .5f);
		//inGameUICanvas.DOFade(1f, .5f);
	}

	public void TransitionToEndGame()
	{
		//DOTween.KillAll(false);

		endGameCanvas.interactable = true;
		//EventSystem.current.SetSelectedGameObject(backToTitleScreenButton);

		//mainCamera.DOMove(new Vector3(3.2f, 26f, -25.5f), 1f, false);
		//endGameCanvas.DOFade(1f, 1f);
	}

	public void ShowInstructions()
	{
		//DOTween.KillAll(false);

//		mainTitleCanvas.interactable = false;
//		mainTitleCanvas.DOFade(0f, 0.5f);

		instructionsCanvas.interactable = true;
		//instructionsCanvas.DOFade(1f, 0.5f).SetDelay(.5f);

		//EventSystem.current.SetSelectedGameObject(instructionsBackButton);
	}

	public void HideInstructions()
	{
		//DOTween.KillAll(false);

		instructionsCanvas.interactable = false;
		//instructionsCanvas.DOFade(0f, 0.5f);

//		mainTitleCanvas.interactable = true;
//		mainTitleCanvas.DOFade(1f, 0.5f).SetDelay(.5f);

		//EventSystem.current.SetSelectedGameObject(instructionsButton);
	}

	public void DisplayNumberOfRounds(int howMany)
	{
		roundsLabel.text = "Rounds: " + howMany;
	}

//	public void ChangeScore(int playerNumber, int amount)
//	{
//		playerScores[playerNumber].text = amount.ToString();
//	}
}

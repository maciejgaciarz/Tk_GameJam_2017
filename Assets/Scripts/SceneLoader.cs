using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using DG.Tweening;

public class SceneLoader : MonoBehaviour {
	public Image bg;


	void Start ()
	{
		//bg.DOFade(1f, .3f);
		//bg.DOFade(0f, .3f).SetDelay(1.7f);

		StartCoroutine(LoadScene());
	}

	private IEnumerator LoadScene()
	{
		yield return new WaitForSeconds(2f);

		SceneManager.LoadScene("Waves");
	}
}

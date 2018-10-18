using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoiceRecorder : MonoBehaviour {
	AudioClip recordedClip;
	string micName;

	void Start ()
	{
		micName = Microphone.devices[0];
	}
	
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			StartCoroutine(StartRecording());
		}

		if(Input.GetKeyUp(KeyCode.Space))
		{
			EndRecording();
		}
	}

	private IEnumerator StartRecording()
	{
		Debug.Log("Start Recording");
		recordedClip = Microphone.Start(micName, true, 4, 44100);

		yield return new WaitForSeconds(4f);

		EndRecording();
	}

	private void EndRecording()
	{
		if(Microphone.IsRecording(micName))
		{
			Microphone.End(micName);
			Debug.Log("Stop Recording");

			GetComponent<AudioSource>().clip = recordedClip;
			GetComponent<AudioSource>().Play();

			float[] dataArray = new float[32];
			GetComponent<AudioSource>().GetSpectrumData(dataArray, 1, FFTWindow.Rectangular);
			for(int i = 0; i<recordedClip.length; i++)
			{
				Debug.Log(dataArray[i]);
			}
		}
	}
}

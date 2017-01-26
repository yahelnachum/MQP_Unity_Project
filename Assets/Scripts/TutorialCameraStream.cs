using UnityEngine;
using System.Collections;

public class TutorialCameraStream : MonoBehaviour {

	private float gopherIntroductionWaitTime = 3f; // in seconds
	private float gopherPinchAndZoomExplainationWaitTime = 3f; // in seconds
	private float gopherAnalyzeButtonExplainationWaitTime = 4f; // in seconds


	public AudioClip sound;
	private AudioSource source;
	public AudioClip sound2;
	private AudioSource source2;
	public AudioClip sound3;

	public void Play1 (){
		Debug.Log ("PLAYING");
		source.PlayOneShot(sound,1.0f);
	}
	public void Play2 (){
		Debug.Log ("PLAYING");
		source.PlayOneShot(sound2,1.0f);
	}
	public void Play3(){
		Debug.Log ("PLAYING");
		source.PlayOneShot(sound3,1.0f);
	}


	void Start(){
		Debug.Log ("Initializing sound object for cams");
		source = GetComponent<AudioSource>();
		StartCoroutine (StartTutorialTimer ());
	}

	private IEnumerator StartTutorialTimer(){
		if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 0) {
			Play1 ();
			yield return new WaitForSeconds (gopherIntroductionWaitTime);
			SwitchPanels.changePanelStatic ("pCameraStreamTutorial:activate");
			Play2 ();
			yield return new WaitForSeconds (gopherPinchAndZoomExplainationWaitTime);
			SwitchPanels.changePanelStatic ("pPinchAndZoomTutorial:deactivate,pAnalyzeButtonTutorial:activate");
			Play3 ();
			yield return new WaitForSeconds (gopherAnalyzeButtonExplainationWaitTime);
			SwitchPanels.changePanelStatic ("pCameraStreamTutorial:deactivate");
		}
	}
}

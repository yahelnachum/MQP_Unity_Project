using UnityEngine;
using System.Collections;

public class TutorialMainMenu : MonoBehaviour {


	private float gopherIntroductionWaitTime = 5f; // in seconds
	private float gopherObjectListExplainationWaitTime = 5f; // in seconds
	private float gopherCameraButtonExplainationWaitTime = 4f; // in seconds

	//private float splashScreenWaitTime = 5f; // in seconds
	//private float gopherIntroductionWaitTime = 1f; // in seconds

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



//	IEnumerator SwitchToMainMenu(){
//		Debug.Log ("splash screen \"loading\"");
//		Play ();
//
//		yield return new WaitForSeconds (splashScreenWaitTime);
//		Debug.Log ("splash screen \"loaded\"");
//		SwitchPanels.changePanelStatic ("pSplash:deactivate,pMain:activate");
//	}
//
	void Start(){
		Debug.Log ("Initializing sound object for tuts");
		source = GetComponent<AudioSource>();
		StartCoroutine (StartTutorialTimer ());

	}

	private IEnumerator StartTutorialTimer(){
		if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 0) {
			Debug.Log ("before play1");

			Play1 ();
			Debug.Log ("after play1");

			yield return new WaitForSeconds (gopherIntroductionWaitTime);
			SwitchPanels.changePanelStatic ("pMainTutorial:activate");
			Debug.Log ("before play2");

			Play2 ();
			Debug.Log ("after play2");

			yield return new WaitForSeconds (gopherObjectListExplainationWaitTime);
			SwitchPanels.changePanelStatic ("pObjectListTutorial:deactivate,pCameraButtonTutorial:activate");
			Debug.Log ("before play3");

			Play3 ();
			Debug.Log ("after play3");

			yield return new WaitForSeconds (gopherCameraButtonExplainationWaitTime);
			SwitchPanels.changePanelStatic ("pMainTutorial:deactivate");
		}
	}
}

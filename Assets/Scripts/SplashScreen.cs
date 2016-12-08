using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	private float splashScreenWaitTime = 5f; // in seconds
	private float gopherIntroductionWaitTime = 1f; // in seconds
	// Use this for initialization

	public AudioClip sound;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		Debug.Log ("Initializing sound object");
		source = GetComponent<AudioSource>();
	}
		

	public void Play (){
		Debug.Log ("PLAYING");
		source.PlayOneShot(sound,1.0f);
	}



	IEnumerator SwitchToMainMenu(){
		Debug.Log ("splash screen \"loading\"");
		Play ();

		yield return new WaitForSeconds (splashScreenWaitTime);
		Debug.Log ("splash screen \"loaded\"");
		SwitchPanels.changePanelStatic ("pSplash:deactivate,pMain:activate");
	}

	bool first = true;
	// Update is called once per frame
	void Update () {

		if (first && !Application.isShowingSplashScreen) {
			first = false;
			StartCoroutine (SwitchToMainMenu ());
		}
		//Debug.Log (Input.gyro.attitude);
	}

}

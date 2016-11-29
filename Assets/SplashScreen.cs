using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	private float splashScreenWaitTime = 1f; // in seconds
	private float gopherIntroductionWaitTime = 1f; // in seconds
	// Use this for initialization
	void Start () {
		StartCoroutine (SwitchToMainMenu ());
	}

	IEnumerator SwitchToMainMenu(){
		Debug.Log ("splash screen \"loading\"");
		yield return new WaitForSeconds (splashScreenWaitTime);
		Debug.Log ("splash screen \"loaded\"");
		SwitchPanels.changePanelStatic ("pSplash:deactivate,pMain:activate");

		if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 0) {
			Debug.Log ("Loading tutorial");
			yield return new WaitForSeconds (gopherIntroductionWaitTime);
			Debug.Log ("Activating tutorial");
			SwitchPanels.changePanelStatic ("pMainTutorialObjectList:activate");
		}
	}
	
	// Update is called once per frame
	void Update () {
		//Debug.Log (Input.gyro.attitude);
	}
}

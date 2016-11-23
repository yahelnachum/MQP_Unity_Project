using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	private float waitTime = 1f; // in seconds

	// Use this for initialization
	void Start () {
		StartCoroutine (SwitchToMainMenu ());
	}

	IEnumerator SwitchToMainMenu(){
		Debug.Log ("splash screen \"loading\"");
		yield return new WaitForSeconds (waitTime);
		Debug.Log ("splash screen \"loaded\"");
		SwitchPanels.changePanelStatic ("pSplash:deactivate,pMain:activate");
	}
	
	// Update is called once per frame
	void Update () {
		Debug.Log (Input.gyro.attitude);
	}
}

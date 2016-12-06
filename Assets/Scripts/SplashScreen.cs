﻿using UnityEngine;
using System.Collections;

public class SplashScreen : MonoBehaviour {

	private float splashScreenWaitTime = 1f; // in seconds
	private float gopherIntroductionWaitTime = 1f; // in seconds
	// Use this for initialization
	void Start () {
		
	}

	IEnumerator SwitchToMainMenu(){
		Debug.Log ("splash screen \"loading\"");
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
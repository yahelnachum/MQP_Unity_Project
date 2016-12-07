﻿using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class CheatsController : MonoBehaviour {

	public void deletePlayerData(){
		File.Delete (PlayerData.getInstance ().getFilePath ());
	}

	bool textSwitched = false;
	string originalButtonText = null;
	public void getCurrentNarrativeChunk(Text buttonText){
		if (textSwitched) {
			buttonText.text = originalButtonText;
			textSwitched = false;
		} else {
			originalButtonText = buttonText.text;
			buttonText.text = "Current Narrative Chunk: " + PlayerData.getInstance ().getCurrentNarrativeChunk ();
			textSwitched = true;
		}
	}

	public void goUpdate(){
		SwitchPanels.changePanelStatic ("pUpdate:activate,pCheats:deactivate");

		UpdatePanel.progress (0.1f);
		Debug.Log("update 1");

		UpdatePanel.progress (0.2f);
		Debug.Log("update 1");

		UpdatePanel.progress (0.3f);
		Debug.Log("update 1");


		UpdatePanel.progress (0.4f);
		Debug.Log("update 1");


		UpdatePanel.progress (0.5f);
		Debug.Log("update 1");

		UpdatePanel.progress (0.6f);
		Debug.Log("update 1");


		UpdatePanel.progress (0.7f);
		Debug.Log("update 7");



	
	}
}

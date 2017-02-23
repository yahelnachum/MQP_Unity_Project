﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARImageGallery : MonoBehaviour {

	private static ARImageGallery arImageGallery = null;
	private int[] narrativeNumbers = {3, 5, 6, 7, 8, 9};
	private List<GameObject> panels;

	private ARImageGallery(){}

	public void initialize(){
		panels = new List<GameObject> ();

		for (int i = 0; i < narrativeNumbers.Length; i++) {
			GameObject obj = StartGame.findInactive ("pARImage0" + narrativeNumbers [i], "vMenu") [0];
			Debug.Log ("PanelPosition: " + obj.transform.position);
			panels.Add(obj);
		}
	}

	public static ARImageGallery getInstance(){
		if (arImageGallery == null) {
			GameObject singleton = new GameObject ();
			arImageGallery = singleton.AddComponent<ARImageGallery> ();
		}
		return arImageGallery;
	}

	public void toggleUnlockedARImages(bool enable){
		string activation = "deactivate";
		if (enable) {
			activation = "activate";
		}

		for(int i = 0; i < narrativeNumbers.Length; i++){
			if(narrativeNumbers[i] < PlayerData.getInstance().getCurrentNarrativeChunk()){
				SwitchPanels.changePanelStatic("pARImage0"+narrativeNumbers[i]+":"+activation);
			}
		}
	}

	public void pinchAndZoom(){
		// check that the player is using only two fingers
		if (Input.touchCount == 1) {
			//Tutorial obj = new Tutorial ();
			//obj.changePanel ("pCameraStreamTutorial:deactivate,pAnalyzeButtonTutorial:activate");

			// Store both touches.
			Touch touchZero = Input.GetTouch (0);
		
			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Debug.Log ("TouchZeroPosition: "+touchZero.position);
			Vector2 vector = new Vector2 (touchZero.position.x / 230f, touchZero.position.y / 420f);
			for (int i = 0; i < panels.Count; i++) {
				panels [i].GetComponent<RectTransform> ().anchorMin = vector;
				panels [i].GetComponent<RectTransform> ().anchorMax = vector;
			}

		}
	}
}

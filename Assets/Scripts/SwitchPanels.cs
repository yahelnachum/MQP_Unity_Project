﻿using UnityEngine;
using System.Collections;

public class SwitchPanels : MonoBehaviour {

	const string ACTIVATE = "activate";
	const string DE_ACTIVATE = "deactivate";

	public void changePanel(string str){
		changePanelStatic (str);
	}

	public static void changePanelStatic(string str){

		char[] panelSplits = { ',' };
		char[] instructionsSplits = { ':' };

		string[] panels = str.Split (panelSplits);

		for (int i = 0; i < panels.Length; i++) {

			string[] panelInstruction = panels [i].Split (instructionsSplits);
			Debug.Log ("tutorial obj name: " + panelInstruction [0]);
			GameObject panel = (GameObject) StartGame.findInactive (panelInstruction [0], "vMenu")[0];

			if (panelInstruction [1].CompareTo (ACTIVATE) == 0) {
				Debug.Log ("activate");
				panel.SetActive (true);
			} else {
				Debug.Log ("deactivate");
				panel.SetActive (false);
			}
		}
	}
}

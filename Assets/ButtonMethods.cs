using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ButtonMethods : MonoBehaviour {

	public void changePanel(string panels){

		char[] splits = new char[1];
		splits [0] = ',';

		string[] panelsSplit = panels.Split(splits);

		GameObject oldPanelObj = StartGame.findInactive (panelsSplit[0],"vMenu");
		GameObject newPanelObj = StartGame.findInactive (panelsSplit[1],"vMenu");

		oldPanelObj.SetActive (false);
		newPanelObj.SetActive (true);
	}

	public void startCamera(){
		Webcam.getInstance ().startCamera ();
	}

	public void takeSnapshot(){
		Webcam.getInstance ().TakeSnapShot ();
	}

	public void stopCamera(){
		Webcam.getInstance ().stopCamera ();
	}

	public void deletePlayerData(){
		File.Delete (PlayerData.getInstance ().getFilePath ());
	}

	public void activateNextNarrativePanel(){

		GameObject pAugmentedReality = GameObject.Find ("pAugmentedReality");
		GameObject pMain = pAugmentedReality.transform.parent.FindChild ("pMain").gameObject;

		pAugmentedReality.SetActive (false);

		GameObject nextNarrative = StartGame.findInactive("pNarrative"+(PlayerData.getInstance ().getCurrentNarrativeChunk()+1),"vMenu");
		if (nextNarrative != null) {
			nextNarrative.SetActive (true);

			PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
			PlayerData.getInstance ().saveData ();
		} else {
			pMain.SetActive (true);
		}
	}
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ButtonMethods : MonoBehaviour {

	public void changePanel(string panels){

		char[] splits = new char[1];
		splits [0] = ',';

		string[] panelsSplit = panels.Split(splits);

		GameObject oldPanelObj = StartGame.findInactive (panelsSplit[0]);
		GameObject newPanelObj = StartGame.findInactive (panelsSplit[1]);

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
}

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BarGraphPanel : MonoBehaviour {

	public void switchToCorrectPanelFromBarGraph(){
		GameObject pBarGraph = GameObject.Find ("pBarGraph");
		RectTransform rect = pBarGraph.GetComponent<RectTransform> ();
		rect.anchorMin = new Vector2 (rect.anchorMin.x, 0.05f);

		GameObject obj1 = StartGame.findInactive ("pCameraAnalyzing", "vMenu")[0];
		GameObject obj2 = StartGame.findInactive ("bNext", "pCameraAnalyzing")[0];

		obj1.SetActive (false);
		obj2.SetActive (false);

		switchToCorrectPanel ();
	}

	public static void switchToCorrectPanel(){
		

		ObjectList.getInstance ().pickCurrentObjects ();
		GameObject pCamera = GameObject.Find ("pCamera");
		Webcam.getInstance ().resetCameraZoom ();
		Webcam.getInstance ().stopCamera ();

		pCamera.SetActive (false);

		if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 9) {
			AugmentedRealityGyro.getInstance ().reInitializeForNarrativeChunk ();
			GameObject pAugmentedReality = pCamera.transform.parent.FindChild ("pAugmentedReality").gameObject;
			pAugmentedReality.SetActive (true);
			AugmentedReality.getInstance ().setNewImage ();

			GameObject obj = StartGame.findInactive ("bCamera", "vMenu")[0];
			obj.GetComponent<Button> ().interactable = false;
		}
		else if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 3 ||
			PlayerData.getInstance ().getCurrentNarrativeChunk () > 4) {

			AugmentedRealityGyro.getInstance ().reInitializeForNarrativeChunk ();
			GameObject pAugmentedReality = pCamera.transform.parent.FindChild ("pAugmentedReality").gameObject;
			pAugmentedReality.SetActive (true);
			AugmentedReality.getInstance ().setNewImage ();
		} else if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 4) {
			Rewards.PrepareRewards ();
			SwitchPanels.changePanelStatic ("pUpdate:activate");
			UpdatePanel.startUpdate ();
		} else {
			GameObject pRewards = pCamera.transform.parent.FindChild ("pRewardsCongrats").gameObject;
			pRewards.SetActive (true);

			GameObject confetti = StartGame.findInactive ("confetti", "vMenu") [0];
			Animation anim = confetti.GetComponent<Animation> ();
			anim.Play ();

			Rewards.PrepareRewards ();
		}
	
	}
}

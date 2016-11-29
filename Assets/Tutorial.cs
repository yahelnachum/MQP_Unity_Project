using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {


	private float gopherIntroductionWaitTime = 1f; // in seconds

	void Start(){
		StartCoroutine (StartTutorialTimer ());
	}

	IEnumerator StartTutorialTimer(){
		if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 0) {
			Debug.Log ("Loading tutorial");
			yield return new WaitForSeconds (gopherIntroductionWaitTime);
			Debug.Log ("Activating tutorial");
			SwitchPanels.changePanelStatic ("pMainTutorialObjectList:activate");
		}
	}
}

using UnityEngine;
using System.Collections;

public class TutorialMainMenu : MonoBehaviour {


	private float gopherIntroductionWaitTime = 1f; // in seconds
	private float gopherObjectListExplainationWaitTime = 3f; // in seconds
	private float gopherCameraButtonExplainationWaitTime = 3f; // in seconds

	void Start(){
		StartCoroutine (StartTutorialTimer ());
	}

	private IEnumerator StartTutorialTimer(){
		if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 0) {
			yield return new WaitForSeconds (gopherIntroductionWaitTime);
			SwitchPanels.changePanelStatic ("pMainTutorial:activate");
			yield return new WaitForSeconds (gopherObjectListExplainationWaitTime);
			SwitchPanels.changePanelStatic ("pObjectListTutorial:deactivate,pCameraButtonTutorial:activate");
			yield return new WaitForSeconds (gopherCameraButtonExplainationWaitTime);
			SwitchPanels.changePanelStatic ("pMainTutorial:deactivate");
		}
	}
}

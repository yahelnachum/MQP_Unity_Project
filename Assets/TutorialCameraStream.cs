using UnityEngine;
using System.Collections;

public class TutorialCameraStream : MonoBehaviour {

	private float gopherIntroductionWaitTime = 1f; // in seconds
	private float gopherPinchAndZoomExplainationWaitTime = 3f; // in seconds
	private float gopherAnalyzeButtonExplainationWaitTime = 3f; // in seconds

	void Start(){
		StartCoroutine (StartTutorialTimer ());
	}

	private IEnumerator StartTutorialTimer(){
		if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 0) {
			yield return new WaitForSeconds (gopherIntroductionWaitTime);
			SwitchPanels.changePanelStatic ("pCameraStreamTutorial:activate");
			yield return new WaitForSeconds (gopherPinchAndZoomExplainationWaitTime);
			SwitchPanels.changePanelStatic ("pPinchAndZoomTutorial:deactivate,pAnalyzeButtonTutorial:activate");
			yield return new WaitForSeconds (gopherAnalyzeButtonExplainationWaitTime);
			SwitchPanels.changePanelStatic ("pCameraStreamTutorial:deactivate");
		}
	}
}

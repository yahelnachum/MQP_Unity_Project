using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraAnalysisFailedPanel : MonoBehaviour {

	public void yes(){
		/*List<HttpRequest> cloudsightRequests = HttpRequestManager.getInstance ().getCloudsightRequests ();
		StartCoroutine(cloudsightRequests[cloudsightRequests.Count - 1].postAnalyze());*/

		HttpRequestManager.getInstance ().runLastCloudsightRequestPostAnalyze ();

		SwitchPanels.changePanelStatic ("pCameraAnalysisFailed:deactivate,pCameraAnalyzing:deactivate");
		Webcam.getInstance ().resetCameraZoom ();
		Webcam.getInstance ().startCamera ();
	}

	public void no(){

		SwitchPanels.changePanelStatic ("pCameraAnalysisFailed:deactivate,pCameraAnalyzing:deactivate");
		Webcam.getInstance ().resetCameraZoom ();
		Webcam.getInstance ().startCamera ();
	}
}

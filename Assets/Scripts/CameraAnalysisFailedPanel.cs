using UnityEngine;
using System.Collections;

public class CameraAnalysisFailedPanel : MonoBehaviour {

	public void yes(){
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

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class HttpRequestManager : MonoBehaviour {

	private HttpRequest clarifaiRequest = null;
	private List<HttpRequest> cloudsightRequests = new List<HttpRequest>();
	private string foundObject = "";

	private bool simulateClarifai = false;
	private bool simulateCloudSight = false;

	private static HttpRequestManager instance = null;

	private HttpRequestManager(){

	}

	public static HttpRequestManager getInstance(){
		if (instance == null) {
			GameObject singleton = new GameObject ();
			instance = singleton.AddComponent<HttpRequestManager> ();
		}
		return instance;
	}

	public void makeRequest(byte[] imageByte){
		GameObject dummy = new GameObject ();

		if (simulateClarifai) {
			SimulatedClarifaiRequest simRequest = dummy.AddComponent<SimulatedClarifaiRequest> ();
			simRequest.initialize (imageByte, 0.5f, 4.0f, AnalysisResponseEnum.Incorrect);
			clarifaiRequest = simRequest;
		} else {
			clarifaiRequest = dummy.AddComponent<ClarifaiRequest> ();
			clarifaiRequest.initialize (imageByte);
		}

		Debug.Log ("HttpRequestManager: Starting Clarifai Analyze");
		StartCoroutine(clarifaiRequest.analyze ());

		if (simulateCloudSight) {
			SimulatedCloudSightRequest simRequest = dummy.AddComponent<SimulatedCloudSightRequest> ();
			simRequest.initialize (imageByte, 3.0f, 25.0f, cloudsightRequests.Count, AnalysisResponseEnum.Random);
			cloudsightRequests.Add (simRequest);
		} else {
			CloudSightRequest cloudRequest = dummy.AddComponent<CloudSightRequest> ();
			cloudRequest.initialize (imageByte, cloudsightRequests.Count);
			cloudsightRequests.Add (cloudRequest);
		}

		StartCoroutine(cloudsightRequests[cloudsightRequests.Count - 1].analyze());

		StartCoroutine (BarGraph.getInstance ().start ());
		Webcam.getInstance ().stopCamera ();
		SwitchPanels.changePanelStatic ("pCameraAnalyzing:activate");
	}

	public void runLastCloudsightRequestPostAnalyze(){
		StartCoroutine (cloudsightRequests [cloudsightRequests.Count - 1].postAnalyze ());
	}

	public void clearRequests(){
		if (clarifaiRequest.hasAnalysisSucceeded ()) {
			foundObject =  clarifaiRequest.getFoundObject ();
		} else {

			for (int i = 0; i < cloudsightRequests.Count; i++) {
				if (cloudsightRequests [i].hasAnalysisSucceeded ()) {
					foundObject = cloudsightRequests [i].getFoundObject ();
				}
			}
		}

		Debug.Log ("Found Object: " + foundObject);

		clarifaiRequest = null;

		for (int i = 0; i < cloudsightRequests.Count; i++) {
			cloudsightRequests [i].setShowDeepAnalysis (false);
		}

		cloudsightRequests.Clear ();
	}

	public HttpRequest getClarifaiRequest(){
		return clarifaiRequest;
	}

	public List<HttpRequest> getCloudsightRequests(){
		return cloudsightRequests;
	}

	public string getFoundObject(){
		return foundObject;
	}
}

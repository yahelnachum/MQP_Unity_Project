using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SimulatedCloudSightRequest : HttpRequest {

	private float minAnalysisTime;
	private float maxAnalysisTime;
	private int requestIndex;

	private AnalysisResponseEnum response;

	public SimulatedCloudSightRequest(){
	}

	public void initialize(byte[] imageByte, float minAnalysisTime, float maxAnalysisTime, int requestIndex, AnalysisResponseEnum response){
		base.initialize (imageByte);

		this.minAnalysisTime = minAnalysisTime;
		this.maxAnalysisTime = maxAnalysisTime;
		this.requestIndex = requestIndex;
		this.response = response;
	}

	public override IEnumerator analyze(){
		Debug.Log ("SimulatedCloudsight: Entering analyze()");
		yield return new WaitForSeconds (Random.value * (maxAnalysisTime - minAnalysisTime) + minAnalysisTime);

		List<Text>[] currentObjects = ObjectList.getInstance ().getCurrentObjects ();

		switch(response){
		case AnalysisResponseEnum.Random:
			if (Random.value > 0.5) {
				setAnalysisSucceeded (true);
				setFoundObj (currentObjects [Random.Range (0, currentObjects.Length)] [0].text);
				saveJPG (getImageByte ());
			} else {
				setAnalysisSucceeded (false);
			}
			break;
		case AnalysisResponseEnum.Correct:
			setAnalysisSucceeded (true);
			setFoundObj (currentObjects [Random.Range (0, currentObjects.Length)] [0].text);
			saveJPG (getImageByte ());
			break;
		case AnalysisResponseEnum.Incorrect:
			setAnalysisSucceeded (false);
			break;
		}

		setAnalyzing (false);
		Debug.Log ("SimulatedCloudsight: Exiting analyze()");
	}

	public override IEnumerator postAnalyze(){
		Debug.Log ("SimulatedCloudsight: Entering postAnalyze()");
		while (isAnalyzing ()) {
			Debug.Log ("SimulatedCloudsight postAnalyse: busy waiting");
			yield return new WaitForSeconds (1f);
		}

		if (shouldShowDeepAnalysis ()) {
			GameObject pDeepAnalysis = StartGame.findInactive ("pDeepAnalysis", "vMenu") [0];
			GameObject pCamera = StartGame.findInactive ("pCamera", "vMenu") [0];

			GameObject pDeepAnalysisCopy = Instantiate<GameObject> (pDeepAnalysis);
			pDeepAnalysisCopy.name = "pDeepAnalysis" + requestIndex;

			pDeepAnalysisCopy.transform.SetParent (pCamera.transform);
			RectTransform rect = pDeepAnalysisCopy.GetComponent<RectTransform> ();
			rect.sizeDelta = new Vector3 (1f, 1f, 1f);
			rect.localScale = new Vector3 (1f, 1f, 1f);
			rect.localPosition = new Vector3 (0f, 0f, 0f);
			rect.offsetMax = new Vector3 (0f, 0f, 0f);

			pDeepAnalysisCopy.SetActive (true);

			GameObject messageTitle = StartGame.findInactive ("tDeepAnalysisMessageTitle", "pDeepAnalysis" + requestIndex) [0];
			GameObject messageContent = StartGame.findInactive ("tDeepAnalysisMessageContent", "pDeepAnalysis" + requestIndex) [0];
			if (hasAnalysisSucceeded ()) {
				messageTitle.GetComponent<Text> ().text = "Deep Analysis" + '\u2122' + " Succeeded!";
				messageContent.GetComponent<Text> ().text = "Your Deep Analysis" + '\u2122' + " request has succeeded.";
			} else {
				messageTitle.GetComponent<Text> ().text = "Deep Analysis" + '\u2122' + " Failed!";
				messageContent.GetComponent<Text> ().text = "Please try analyzing another picture.";
			}
		}
		Debug.Log ("SimulatedCloudsight: Exiting postAnalyze()");
	}
}
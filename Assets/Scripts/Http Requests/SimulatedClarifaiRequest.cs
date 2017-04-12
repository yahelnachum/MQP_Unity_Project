using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SimulatedClarifaiRequest : HttpRequest {

	private float minAnalysisTime;
	private float maxAnalysisTime;

	private AnalysisResponseEnum response;

	public SimulatedClarifaiRequest(){
	}

	public void initialize(byte[] imageByte, float minAnalysisTime, float maxAnalysisTime, AnalysisResponseEnum response){
		base.initialize (imageByte);

		this.minAnalysisTime = minAnalysisTime;
		this.maxAnalysisTime = maxAnalysisTime;

		this.response = response;
	}

	public override IEnumerator analyze(){
		Debug.Log ("Starting Simulated Clarifai Analyze");
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

		Debug.Log ("Ending Simulated Clarifai Analyze");
		setAnalyzing (false);
	}
}
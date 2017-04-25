using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SimpleJSON;

public class CloudSightRequest : HttpRequest {

	public const string HEADER_AUTHORIZATION_CLOUDSIGHT = "CloudSight ";
	private const string CLOUDSIGHT_TOKEN = "4pOKD3RAvMEhUCDBkExAhA";

	private int requestIndex;

	public CloudSightRequest(){
		
	}

	public void initialize(byte[] imageByte, int requestIndex){
		base.initialize (imageByte);
		this.requestIndex = requestIndex;
	}

	public override IEnumerator analyze()
	{
		byte[] image = getImageByte();

		HttpConfiguration[] configurations = new HttpConfiguration[2];
		configurations[0] = new HttpConfiguration("image_request[locale]", BODY_CONTENT_TYPE_TEXT_PLAIN, stringToBytes("en"));
		configurations[1] = new HttpConfiguration("image_request[image]", "unityWebcam.jpg", BODY_CONTENT_TYPE_IMAGE_JPEG, image);

		byte[] entireData = htmlPostBody(configurations);
		Debug.Log(entireData.Length);

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", "CloudSight "+CLOUDSIGHT_TOKEN);
		headers.Add("Content-Length", "" + (entireData.Length));
		headers.Add("Content-Type","multipart/form-data; boundary=" + BODY_BOUNDARY);

		WWW www = new WWW("https://api.cloudsightapi.com/image_requests", entireData, headers);

		yield return www;
		checkWWWForError(www);

		string token = JSON.Parse(www.text)["token"].Value;
		Debug.Log("token: " + token);

		string status = "not completed";
		string response = "";
		while(status.Equals("not completed"))
		{
			status = "";

			headers = new Dictionary<string, string>();
			headers.Add("Authorization", "CloudSight "+CLOUDSIGHT_TOKEN);

			www = new WWW("http://api.cloudsightapi.com/image_responses/" + token, null, headers);
			yield return www;
			checkWWWForError(www);

			Debug.Log("Cloudsight Response Text: " + www.text);

			status = JSON.Parse(www.text)["status"].Value;
			response = JSON.Parse(www.text)["name"].Value;
			//txtCloud.text = response;
			Debug.Log("CloudSight Response: "+response);

			yield return new WaitForSeconds(5);
		}

		Debug.Log(response);

		char[] spaceSplitter = { ' ' };
		response = response.Replace ("'s", "");
		string[] responses = response.Split (spaceSplitter);

		checkIfFoundObjects (responses, image);

		setAnalyzing (false);
	}

	public override IEnumerator postAnalyze(){
		Debug.Log ("Cloudsight: Entering postAnalyze()");
		while (isAnalyzing ()) {
			Debug.Log ("Cloudsight postAnalyse: busy waiting");
			yield return new WaitForSeconds (1f);
		}

		if (shouldShowDeepAnalysis ()) {
			GameObject pDeepAnalysis = StartGame.findInactive ("pDeepAnalysis", "vMenu") [0];
			GameObject pCamera = StartGame.findInactive ("pCamera", "vMenu") [0];

			GameObject pDeepAnalysisCopy = Instantiate<GameObject> (pDeepAnalysis);
			pDeepAnalysisCopy.name = "pDeepAnalysis" + requestIndex;

			pDeepAnalysisCopy.transform.SetParent (pCamera.transform);
			pDeepAnalysisCopy.transform.SetSiblingIndex (1);
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
		Debug.Log ("Cloudsight: Exiting postAnalyze()");
	}
}

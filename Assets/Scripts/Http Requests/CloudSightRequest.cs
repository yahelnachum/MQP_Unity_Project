using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class CloudSightRequest : HttpRequest {

	public const string HEADER_AUTHORIZATION_CLOUDSIGHT = "CloudSight ";

	private const string CLOUDSIGHT_TOKEN = "4pOKD3RAvMEhUCDBkExAhA";

	public CloudSightRequest(){
		
	}

	public void initialize(byte[] imageByte){
		base.initialize (imageByte);
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
}

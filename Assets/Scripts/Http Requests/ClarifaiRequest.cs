using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;

public class ClarifaiRequest : HttpRequest {

	public const string HEADER_AUTHORIZATION_CLARIFAI = "Bearer ";

	private const string CLARIFAI_TOKEN = "";
	private const string CLARIFAI_CLIENT_ID = "j7yHzbxOlue-Q4NkEXTIl1UHllT3_UerH8TLn2Cu";
	private const string CLARIFAI_CLIENT_SECRET = "dbNK8HdXVqGl4NbN-8U0v-KAJbPh40idRSvCd8vI";

	public ClarifaiRequest(){
		
	}

	public void initialize(byte[] imageByte){
		base.initialize (imageByte);
	}

	public override IEnumerator analyze()
	{

		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters.Add("client_id", CLARIFAI_CLIENT_ID);
		parameters.Add("client_secret", CLARIFAI_CLIENT_SECRET);
		parameters.Add("grant_type", "client_credentials");

		WWW www = new WWW("https://api.clarifai.com/v1/token/", htmlPostBody(parameters));

		yield return www;
		checkWWWForError(www);

		string token = JSON.Parse(www.text)["access_token"].Value;
		Debug.Log("access_token: " + token);

		byte[] image = getImageByte();

		HttpConfiguration[] configurations = new HttpConfiguration[1];
		configurations[0] = new HttpConfiguration("encoded_data", "unityWebcam.jpg", BODY_CONTENT_TYPE_IMAGE_JPEG, image);

		byte[] entireData = htmlPostBody(configurations);

		Debug.Log(entireData.Length);
		Debug.Log("Entire data: \r\n"+ System.Text.Encoding.UTF8.GetString(entireData));

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", "Bearer " + token);
		headers.Add("Content-Length", "" + (entireData.Length));
		headers.Add("Content-Type", "multipart/form-data; boundary=" + BODY_BOUNDARY);

		www = new WWW("https://api.clarifai.com/v1/tag", entireData, headers);
		yield return www;
		checkWWWForError(www);

		//JSONNode array = JSON.Parse (www.text) ["meta"].AsObject;
		JSONNode array1 = JSON.Parse(www.text)["results"].AsArray[0];
		JSONNode array2 = array1 ["result"].AsObject;
		JSONNode array3 = array2 ["tag"].AsObject;
		JSONArray array4 = array3["classes"].AsArray;

		string s = "";
		for (int i = 0; i < array4.Count; i++) {
			s += array4 [i].Value + "|";
		}

		//txtClarifai.text = s;
		Debug.Log("Clarifai Response: "+s);

		string response = array4.ToString ();
		response = response.Replace ("\"", "");
		response = response.Replace ("[", "");
		response = response.Replace ("]", "");
		response = response.Replace (" ", "");

		char[] spaceSplitter = { ',' };
		string[] responses = response.Split (spaceSplitter);

		checkIfFoundObjects (responses, image);

		setAnalyzing (false);
	}
}

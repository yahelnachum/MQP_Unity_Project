using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Text;
using System.Linq;
using UnityEngine.UI;

public class HttpRequest : MonoBehaviour {

	public const string EQUALS = "=";
	public const string AND = "&";

	public const string HEADER_AUTHORIZATION = "Authorization";
	public const string HEADER_AUTHORIZATION_CLOUDSIGHT = "CloudSight ";
	public const string HEADER_AUTHORIZATION_CLARIFAI = "Bearer ";

	public const string HEADER_CONTENT_TYPE = "Content-Type";
	public const string HEADER_CONTENT_TYPE_MULTIPART_FORM_DATA = "multipart/form-data; ";
	public const string HEADER_BOUDARY = "boundary=";

	public const string BODY_BOUNDARY = "------------------------aa2af2f6902e7855";
	public const string BODY_TWO_HYPHENS = "--";
	public const string BODY_CR_LF = "\r\n";
	public const string BODY_DOUBLE_QUOTES = "\"";

	public const string BODY_CONTENT_TYPE = "Content-Type: ";
	public const string BODY_CONTENT_DISPOSITION = "Content-Disposition: ";
	public const string BODY_FORM_DATA = "form-data; ";
	public const string BODY_NAME = "name=";
	public const string BODY_FILE_NAME = "filename=";
	public const string BODY_SEMICOLON_SPACE = "; ";

	public const string BODY_CONTENT_TYPE_IMAGE_JPEG = "image/jpeg";
	public const string BODY_CONTENT_TYPE_TEXT_PLAIN = "text/plain";
	public const string BODY_CONTENT_PLACEHOLDER = "aoidfngoiefoi12097445";

	// Use this for initialization
	void Start () { 
		//StartCoroutine(postWatson(Application.dataPath + "/unityWebcam.jpg"));
		//StartCoroutine(postCloudSight(Application.dataPath + "/unityWebcam.jpg"));
		//StartCoroutine(postClarifai(Application.dataPath + "/unityWebcam.jpg"));
	}

	/*
     * Build the html post body for a post without any image data.
     */
	public static byte[] htmlPostBody(Dictionary<string, string> paramaters)
	{
		StringBuilder sb = new StringBuilder();

		for(int i = 0; i < paramaters.Count; i++)
		{
			var element = paramaters.ElementAt(i);

			sb.Append(element.Key);
			sb.Append(EQUALS);
			sb.Append(element.Value);

			if(i < paramaters.Count - 1)
			{
				sb.Append(AND);
			}
		}

		return stringToBytes(sb.ToString());
	}

	/*
     * Build a html post body for a post with text data (0..n) and image data (0..n)
     */
	public static byte[] htmlPostBody(HttpConfiguration[] configurations)
	{
		if(configurations.Length == 0)
		{
			return null;
		}

		StringBuilder sb = new StringBuilder();

		// combine all UTF-8 encoded data
		int contentSize = 0;
		int[] placeHolderIndexes = new int[configurations.Length];
		for(int i = 0; i < configurations.Length; i++)
		{
			HttpConfiguration config = configurations[i];

			// boundary
			sb.Append(BODY_TWO_HYPHENS);
			sb.Append(BODY_BOUNDARY);
			sb.Append(BODY_CR_LF);

			// content disposition
			sb.Append(BODY_CONTENT_DISPOSITION);
			sb.Append(BODY_FORM_DATA);

			// name
			sb.Append(BODY_NAME);
			sb.Append(BODY_DOUBLE_QUOTES);
			sb.Append(config.getName());
			sb.Append(BODY_DOUBLE_QUOTES);
			sb.Append(BODY_SEMICOLON_SPACE);

			// optional file name
			if (config.hasFileName())
			{
				sb.Append(BODY_FILE_NAME);
				sb.Append(BODY_DOUBLE_QUOTES);
				sb.Append(config.getFileName());
				sb.Append(BODY_DOUBLE_QUOTES);
			}
			sb.Append(BODY_CR_LF);

			// content type
			sb.Append(BODY_CONTENT_TYPE);
			sb.Append(config.getContentType());
			sb.Append(BODY_CR_LF);
			sb.Append(BODY_CR_LF);

			// content placeholder
			placeHolderIndexes[i] = sb.ToString().Length;
			sb.Append(BODY_CONTENT_PLACEHOLDER);
			sb.Append(BODY_CR_LF);
			contentSize += config.getContent().Length;

			// end entire body
			if(i == configurations.Length - 1)
			{
				sb.Append(BODY_TWO_HYPHENS);
				sb.Append(BODY_BOUNDARY);
				sb.Append(BODY_TWO_HYPHENS);
				sb.Append(BODY_CR_LF);
			}
		}

		Debug.Log("Part body: \r\n" + sb.ToString());
		byte[] bodyPart = stringToBytes(sb.ToString());

		int neededSize = bodyPart.Length - (BODY_CONTENT_PLACEHOLDER.Length * configurations.Length) + contentSize;
		byte[] wholeBody = new byte[neededSize];

		// insert content byte[] data
		int currentIndexSrc = 0;
		int currentIndexDest = 0;
		for(int i = 0; i < configurations.Length; i++)
		{
			byte[] content = configurations[i].getContent();

			Debug.Log("Block copy 1");
			Debug.Log("Current Index Src: " + currentIndexSrc);
			Debug.Log("Current Index Dest: " + currentIndexDest);
			Debug.Log("Count: " + (placeHolderIndexes[i] - currentIndexSrc));

			// before placeholder
			System.Buffer.BlockCopy(bodyPart, currentIndexSrc, wholeBody, currentIndexDest, placeHolderIndexes[i] - currentIndexSrc);
			currentIndexDest += placeHolderIndexes[i] - currentIndexSrc;
			currentIndexSrc = placeHolderIndexes[i] + BODY_CONTENT_PLACEHOLDER.Length;

			Debug.Log("Block copy 2");
			Debug.Log("Current Index Src: " + currentIndexSrc);
			Debug.Log("Current Index Dest: " + currentIndexDest);
			Debug.Log("Count: " + (content.Length));

			// insert content instead of placeholder
			System.Buffer.BlockCopy(content, 0, wholeBody, currentIndexDest, content.Length);
			currentIndexDest += content.Length;

			Debug.Log("Block copy 3");
			Debug.Log("Current Index Src: " + currentIndexSrc);
			Debug.Log("Current Index Dest: " + currentIndexDest);
			Debug.Log("Count: " + (bodyPart.Length - currentIndexSrc));

			// insert rest of data if its the last configuration
			if (i == configurations.Length - 1)
			{
				System.Buffer.BlockCopy(bodyPart, currentIndexSrc, wholeBody, currentIndexDest, bodyPart.Length - currentIndexSrc);
			}

		}

		Debug.Log("Whole body: \r\n" + System.Text.Encoding.UTF8.GetString(wholeBody));

		return wholeBody;
	}

	public static byte[] stringToBytes(string str)
	{
		return System.Text.Encoding.UTF8.GetBytes(str);
	}

	public static IEnumerator postClarifai(byte[] imageByte, Text text)
	{

		Dictionary<string, string> parameters = new Dictionary<string, string>();
		parameters.Add("client_id", "j7yHzbxOlue-Q4NkEXTIl1UHllT3_UerH8TLn2Cu");
		parameters.Add("client_secret", "dbNK8HdXVqGl4NbN-8U0v-KAJbPh40idRSvCd8vI");
		parameters.Add("grant_type", "client_credentials");

		WWW www = new WWW("https://api.clarifai.com/v1/token/", htmlPostBody(parameters));

		yield return www;
		checkWWWForError(www);

		string token = JSON.Parse(www.text)["access_token"].Value;
		Debug.Log("access_token: " + token);

		byte[] image = imageByte;//System.IO.File.ReadAllBytes(pathToImage);

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

		Text[] currentObjects = ObjectList.getInstance ().getCurrentObjects ();
		AcceptedTags[] acceptedTags = ObjectList.getInstance ().getAcceptedTags ();
		bool foundCurrentObjects = false;

		for (int i = 0; i < currentObjects.Length; i++) {
			string currentObj = currentObjects [i].text;
			for (int j = 0; j < acceptedTags.Length; j++) {
				string acceptedTag = acceptedTags [j].getAcceptedTag();
				string[] similarTags = acceptedTags [j].getSimilarTags ();

				if (acceptedTag.CompareTo (currentObj) == 0) {

					for(int k = 0; k < similarTags.Length; k++){
						string similarTag = similarTags [k];

						for (int l = 0; l < array4.Count; l++) {
							string tagAnalyzed = array4 [l].Value;

							if (tagAnalyzed.CompareTo (similarTag) == 0) {
								foundCurrentObjects = true;

								l = array4.Count;
								k = similarTags.Length;
								j = acceptedTags.Length;
								i = currentObjects.Length;
							}
						}
					}
				}
			}
		}

		string s = "";
		for (int i = 0; i < array4.Count; i++) {
			s += array4 [i].Value + "|";
		}
			
		text.text = s;

		if (foundCurrentObjects) {
			ObjectList.getInstance ().pickCurrentObjects ();
			GameObject pCamera = GameObject.Find ("pCamera");
			GameObject pMain = pCamera.transform.parent.FindChild ("pMain").gameObject;
			Debug.Log (pMain);
			Debug.Log (pCamera);

			Webcam.getInstance().stopCamera ();
			pCamera.SetActive (false);

			GameObject nextNarrative = StartGame.findInactive("pNarrative"+(PlayerData.getInstance ().getCurrentNarrativeChunk()+1));
			if (nextNarrative != null) {
				nextNarrative.SetActive (true);

				PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
				PlayerData.getInstance ().saveData ();
			} else {
				pMain.SetActive (true);
			}
		}

		Debug.Log(www.text);
	}

	public static IEnumerator postCloudSight(byte[] imageByte, Text text)
	{
		byte[] image = imageByte;// System.IO.File.ReadAllBytes(pathToImage);

		HttpConfiguration[] configurations = new HttpConfiguration[2];
		configurations[0] = new HttpConfiguration("image_request[locale]", BODY_CONTENT_TYPE_TEXT_PLAIN, stringToBytes("en"));
		configurations[1] = new HttpConfiguration("image_request[image]", "unityWebcam.jpg", BODY_CONTENT_TYPE_IMAGE_JPEG, image);

		byte[] entireData = htmlPostBody(configurations);
		Debug.Log(entireData.Length);

		Dictionary<string, string> headers = new Dictionary<string, string>();
		headers.Add("Authorization", "CloudSight PK2x9PRpXn-3BSAh6QRdpA");
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
			Debug.Log("hello");
			status = "";

			headers = new Dictionary<string, string>();
			headers.Add("Authorization", "CloudSight PK2x9PRpXn-3BSAh6QRdpA");

			www = new WWW("http://api.cloudsightapi.com/image_responses/" + token, null, headers);
			yield return www;
			checkWWWForError(www);

			Debug.Log(www.text);

			status = JSON.Parse(www.text)["status"].Value;
			response = JSON.Parse(www.text)["name"].Value;
			text.text = response;

			yield return new WaitForSeconds(2);
		}

		Debug.Log(response);
	}

	public static IEnumerator postWatson(byte[] imageByte, Text text)
	{
		WWWForm form = new WWWForm();
		WWW www = new WWW("https://gateway-a.watsonplatform.net/visual-recognition/api/v3/classify?api_key=68afcccf311899e6f6cc6064de624901456c180a&version=2016-05-20", imageByte);// form);

		yield return www;
		checkWWWForError(www);

		JSONNode array = JSON.Parse (www.text) ["images"].AsArray [0];
		JSONNode array1 = array ["classifiers"].AsArray [0];
		JSONArray array2 = array1 ["classes"].AsArray;

		string s = "";
		for (int i = 0; i < array2.Count; i++) {
			s += array2 [i].AsObject ["class"].Value;
		}
		text.text = s;
		Debug.Log(www.text);
	}

	public static void checkWWWForError(WWW www)
	{
		if (www.error == null)
		{
			Debug.Log("WWW Ok!: " + www.text);
		}
		else
		{
			Debug.Log("WWW Error: " + www.error);
		}
	}

	// Update is called once per frame
	void Update () {

	}

	public WWW GET(string url)
	{
		WWW www = new WWW(url);
		//StartCoroutine(WaitForRequest(www));
		return www;
	}
}
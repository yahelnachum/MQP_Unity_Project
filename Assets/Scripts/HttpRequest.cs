using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SimpleJSON;
using System.Text;
using System.Linq;
using UnityEngine.UI;

public class HttpRequest : MonoBehaviour {

	private const string EQUALS = "=";
	private const string AND = "&";

	private const string HEADER_AUTHORIZATION = "Authorization";
	private const string HEADER_AUTHORIZATION_CLOUDSIGHT = "CloudSight ";
	private const string HEADER_AUTHORIZATION_CLARIFAI = "Bearer ";

	private const string HEADER_CONTENT_TYPE = "Content-Type";
	private const string HEADER_CONTENT_TYPE_MULTIPART_FORM_DATA = "multipart/form-data; ";
	private const string HEADER_BOUDARY = "boundary=";

	private const string BODY_BOUNDARY = "------------------------aa2af2f6902e7855";
	private const string BODY_TWO_HYPHENS = "--";
	private const string BODY_CR_LF = "\r\n";
	private const string BODY_DOUBLE_QUOTES = "\"";

	private const string BODY_CONTENT_TYPE = "Content-Type: ";
	private const string BODY_CONTENT_DISPOSITION = "Content-Disposition: ";
	private const string BODY_FORM_DATA = "form-data; ";
	private const string BODY_NAME = "name=";
	private const string BODY_FILE_NAME = "filename=";
	private const string BODY_SEMICOLON_SPACE = "; ";

	private const string BODY_CONTENT_TYPE_IMAGE_JPEG = "image/jpeg";
	private const string BODY_CONTENT_TYPE_TEXT_PLAIN = "text/plain";
	private const string BODY_CONTENT_PLACEHOLDER = "aoidfngoiefoi12097445";

	private const string CLOUDSIGHT_TOKEN = "sSXwCsxn6h6KDlhv4e9iBw";
	private const string CLARIFAI_TOKEN = "";
	private const string CLARIFAI_CLIENT_ID = "j7yHzbxOlue-Q4NkEXTIl1UHllT3_UerH8TLn2Cu";
	private const string CLARIFAI_CLIENT_SECRET = "dbNK8HdXVqGl4NbN-8U0v-KAJbPh40idRSvCd8vI";

	private static HttpRequest httpRequestInstance = null;

	private bool cloudsightRequestInProgress = false;
	private Coroutine[] coroutines = new Coroutine[2]; 

	private Text txtClarifai = null;
	private Text txtCloud = null;
	private string foundObj = "";

	private HttpRequest(){
	}

	public static HttpRequest getInstance(){
		if (httpRequestInstance == null) {
			GameObject singleton = new GameObject ();
			httpRequestInstance = singleton.AddComponent<HttpRequest> ();
			//httpRequestInstance.setUpTextObjects ();
		}

		return httpRequestInstance;
	}

	public void setUpTextObjects(){

		// get the text objects for output of http POST responses
		//GameObject textWatson = GameObject.Find ("Watson");
		//Text txtWatson = textWatson.GetComponent<Text> ();
		//GameObject textClarifai = StartGame.findInactive ("ClarifaiOutput", "vMenu");
		//txtClarifai = textClarifai.GetComponent<Text> ();
		//GameObject textCloud = StartGame.findInactive ("CloudSightOutput", "vMenu");
		//txtCloud = textCloud.GetComponent<Text> ();

	}

	public void startVisibleProcessDelay(){
		// TODO: TJ's code here

		GameObject anal = StartGame.findInactive ("bAnalyze", "vMenu") [0];
		anal.GetComponent<Button> ().interactable = false;

		GameObject can = StartGame.findInactive ("bCancel", "vMenu") [0];
		can.GetComponent<Button> ().interactable = false;

		Webcam.getInstance ().stopCamera ();
		SwitchPanels.changePanelStatic ("pCameraAnalyzing:activate");
	}

	public void endVisibleProcessDelayFailed(){
		// TODO: TJ's code here

		GameObject anal = StartGame.findInactive ("bAnalyze", "vMenu") [0];
		anal.GetComponent<Button> ().interactable = true;

		GameObject can = StartGame.findInactive ("bCancel", "vMenu") [0];
		can.GetComponent<Button> ().interactable = true;

		Webcam.getInstance ().startCamera ();
		SwitchPanels.changePanelStatic ("pCameraAnalyzing:deactivate");
	}

	public void endVisibleProcessDelaySuccessful(){
		// TODO: TJ's code here

		GameObject anal = StartGame.findInactive ("bAnalyze", "vMenu") [0];
		anal.GetComponent<Button> ().interactable = true;

		GameObject can = StartGame.findInactive ("bCancel", "vMenu") [0];
		can.GetComponent<Button> ().interactable = true;

		Webcam.getInstance ().startCamera ();
		SwitchPanels.changePanelStatic ("pCameraAnalyzing:deactivate");
	}

	public void makeRequest(byte[] imageData){
		startVisibleProcessDelay ();

		coroutines[0] = StartCoroutine(postClarifai(imageData));

		if (cloudsightRequestInProgress == false) {
			coroutines[1] = StartCoroutine(postCloudSight(imageData));
			cloudsightRequestInProgress = true;
		}
	}
	/*
     * Build the html post body for a post without any image data.
     */
	private byte[] htmlPostBody(Dictionary<string, string> paramaters)
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
	private byte[] htmlPostBody(HttpConfiguration[] configurations)
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

			// before placeholder
			System.Buffer.BlockCopy(bodyPart, currentIndexSrc, wholeBody, currentIndexDest, placeHolderIndexes[i] - currentIndexSrc);
			currentIndexDest += placeHolderIndexes[i] - currentIndexSrc;
			currentIndexSrc = placeHolderIndexes[i] + BODY_CONTENT_PLACEHOLDER.Length;

			// insert content instead of placeholder
			System.Buffer.BlockCopy(content, 0, wholeBody, currentIndexDest, content.Length);
			currentIndexDest += content.Length;

			// insert rest of data if its the last configuration
			if (i == configurations.Length - 1)
			{
				System.Buffer.BlockCopy(bodyPart, currentIndexSrc, wholeBody, currentIndexDest, bodyPart.Length - currentIndexSrc);
			}

		}

		Debug.Log("Whole body: \r\n" + System.Text.Encoding.UTF8.GetString(wholeBody));

		return wholeBody;
	}

	private byte[] stringToBytes(string str)
	{
		return System.Text.Encoding.UTF8.GetBytes(str);
	}

	private IEnumerator postClarifai(byte[] imageByte)
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

		checkIfFoundObjects (responses, imageByte);
	}

	private IEnumerator postCloudSight(byte[] imageByte)
	{
		byte[] image = imageByte;// System.IO.File.ReadAllBytes(pathToImage);

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

			Debug.Log(www.text);

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

		checkIfFoundObjects (responses, imageByte);

		cloudsightRequestInProgress = false;
	}

	public void checkIfFoundObjects(string[] responses, byte[] imageByte){
		List<Text>[] currentObjects = ObjectList.getInstance ().getCurrentObjects ();
		AcceptedTags[] acceptedTags = ObjectList.getInstance ().getAcceptedTags ();
		bool foundCurrentObjects = false;


		for (int i = 0; i < currentObjects.Length; i++) {
			string currentObj = currentObjects [i][0].text;
			for (int j = 0; j < acceptedTags.Length; j++) {
				string acceptedTag = acceptedTags [j].getAcceptedTag();
				string[] similarTags = acceptedTags [j].getSimilarTags ();

				if (acceptedTag.CompareTo (currentObj) == 0) {

					for(int k = 0; k < similarTags.Length; k++){
						string similarTag = similarTags [k];

						for (int l = 0; l < responses.Length; l++) {
							string tagAnalyzed = responses[l];

							if (tagAnalyzed.CompareTo (similarTag) == 0) {
								foundCurrentObjects = true;
								foundObj = currentObj;

								l = responses.Length;
								k = similarTags.Length;
								j = acceptedTags.Length;
								i = currentObjects.Length;
							}
						}
					}
				}
			}
		}

		if (foundCurrentObjects) {

			endVisibleProcessDelaySuccessful ();

			saveJPG(imageByte);

			switchToCorrectPanel ();

		} else {
			endVisibleProcessDelayFailed ();
		}
	}

	private void switchToCorrectPanel(){
		ObjectList.getInstance ().pickCurrentObjects ();
		GameObject pCamera = GameObject.Find ("pCamera");
		Webcam.getInstance ().stopCamera ();

		pCamera.SetActive (false);

		// go to augmented reality panel for chunks 3,5,6,7,8,9
		if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 3 ||
			PlayerData.getInstance ().getCurrentNarrativeChunk () > 4) {

			GameObject pAugmentedReality = pCamera.transform.parent.FindChild ("pAugmentedReality").gameObject;
			pAugmentedReality.SetActive (true);
			AugmentedReality.getInstance ().setNewImage ();
		} else if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 4) {
			//
			SwitchPanels.changePanelStatic ("pUpdate:activate");
			UpdatePanel.startUpdate ();
		} else {
			GameObject pRewards = pCamera.transform.parent.FindChild ("pRewardsCongrats").gameObject;
			pRewards.SetActive (true);

			GameObject confetti = StartGame.findInactive ("confetti", "vMenu") [0];
			Animation anim = confetti.GetComponent<Animation> ();
			anim.Play ();

			Rewards.PrepareRewards ();
		}
	}

	private void saveJPG(byte[] imageByte){
		Debug.Log (Application.persistentDataPath);
		System.IO.File.WriteAllBytes (Application.persistentDataPath +
			"/image" + PlayerData.getInstance ().getCurrentNarrativeChunk () + ".jpg", imageByte);
	}

	public string getFoundObj(){
		return foundObj;
	}

	private IEnumerator postWatson(byte[] imageByte, Text text)
	{
		//WWWForm form = new WWWForm();
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

	private void checkWWWForError(WWW www)
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
}
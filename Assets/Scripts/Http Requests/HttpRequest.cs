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

	private byte[] imageByte;
	private bool analyzing;
	private bool analysisSucceeded;
	private bool showDeepAnalysis;
	private string foundObject;

	public HttpRequest(){
	
	}

	public void initialize(byte[] imageByte){
		this.imageByte = imageByte;
		analyzing = true;
		analysisSucceeded = false;
		showDeepAnalysis = true;
		foundObject = "";
	}

	public byte[] getImageByte (){
		return imageByte;
	}

	public bool isAnalyzing(){
		return analyzing;
	}

	public bool hasAnalysisSucceeded(){
		return analysisSucceeded;
	}

	public bool shouldShowDeepAnalysis(){
		return showDeepAnalysis;
	}

	public string getFoundObject(){
		return foundObject;
	}

	public void setAnalyzing(bool analyzing){
		this.analyzing = analyzing;
	}

	public void setAnalysisSucceeded(bool analysisSucceeded){
		this.analysisSucceeded = analysisSucceeded;
	}

	public void setShowDeepAnalysis(bool showDeepAnalysis){
		this.showDeepAnalysis = showDeepAnalysis;
	}

	public void setFoundObj(string foundObject){
		this.foundObject = foundObject;
	}

	public virtual IEnumerator analyze(){
		Debug.Log ("HttpRequest: Analyze");
		yield return 0;
	}

	public virtual IEnumerator postAnalyze(){
		Debug.Log ("HttpRequest: PostAnalyze");
		yield return 0;
	}
		
	public void makeRequest(byte[] imageData){
		StartCoroutine (BarGraph.getInstance ().start ());
		Webcam.getInstance ().stopCamera ();
		SwitchPanels.changePanelStatic ("pCameraAnalyzing:activate");
	}

	private void getDeepAnalysisMessageReady (){
		GameObject messageTitle = StartGame.findInactive ("tDeepAnalysisMessageTitle", "pCamera")[0];
		GameObject messageContent= StartGame.findInactive ("tDeepAnalysisMessageContent", "pCamera")[0];
		if (foundObject == "") {
			messageTitle.GetComponent<Text>().text = "Deep Analysis"+'\u2122'+" Failed!";
			messageContent.GetComponent<Text>().text = "Please try analyzing another picture.";
		} else {
			messageTitle.GetComponent<Text>().text = "Deep Analysis"+'\u2122'+" Succeeded!";
			messageContent.GetComponent<Text>().text = "Your Deep Analysis"+'\u2122'+" request has succeeded.";
		}
	}
		
	/*
     * Build the html post body for a post without any image data.
     */
	public byte[] htmlPostBody(Dictionary<string, string> paramaters)
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
	public byte[] htmlPostBody(HttpConfiguration[] configurations)
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

	public byte[] stringToBytes(string str)
	{
		return System.Text.Encoding.UTF8.GetBytes(str);
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
								foundObject = currentObj;

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
			saveJPG(imageByte);
		}
	}

	private void switchToCorrectPanel(){
		ObjectList.getInstance ().pickCurrentObjects ();
		GameObject pCamera = GameObject.Find ("pCamera");
		Webcam.getInstance ().resetCameraZoom ();
		Webcam.getInstance ().stopCamera ();

		pCamera.SetActive (false);

		if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 9) {
			AugmentedRealityGyro.getInstance ().reInitializeForNarrativeChunk ();
			GameObject pAugmentedReality = pCamera.transform.parent.FindChild ("pAugmentedReality").gameObject;
			pAugmentedReality.SetActive (true);
			AugmentedReality.getInstance ().setNewImage ();

			GameObject obj = StartGame.findInactive ("bCamera", "vMenu")[0];
			obj.GetComponent<Button> ().interactable = false;
		}
		else if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 3 ||
			PlayerData.getInstance ().getCurrentNarrativeChunk () > 4) {

			AugmentedRealityGyro.getInstance ().reInitializeForNarrativeChunk ();
			GameObject pAugmentedReality = pCamera.transform.parent.FindChild ("pAugmentedReality").gameObject;
			pAugmentedReality.SetActive (true);
			AugmentedReality.getInstance ().setNewImage ();
		} else if (PlayerData.getInstance ().getCurrentNarrativeChunk () == 4) {
			Rewards.PrepareRewards ();
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

	public void saveJPG(byte[] imageByte){
		Debug.Log (Application.persistentDataPath);
		System.IO.File.WriteAllBytes (Application.persistentDataPath +
			"/image" + PlayerData.getInstance ().getCurrentNarrativeChunk () + ".jpg", imageByte);
	}

	public void checkWWWForError(WWW www)
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
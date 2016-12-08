using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ButtonMethods : MonoBehaviour {

	/*public void activateNextNarrativePanel(){

		GameObject pAugmentedReality = GameObject.Find ("pAugmentedReality");
		GameObject pMain = pAugmentedReality.transform.parent.FindChild ("pMain").gameObject;

		pAugmentedReality.SetActive (false);

		GameObject nextNarrative = (GameObject)StartGame.findInactive("pNarrative"+(PlayerData.getInstance ().getCurrentNarrativeChunk()+1),"vMenu")[0];
		if (nextNarrative != null) {
			nextNarrative.SetActive (true);

			PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
			PlayerData.getInstance ().saveData ();
		} else {
			pMain.SetActive (true);
		}
	}*/

	private static ButtonMethods instance = new ButtonMethods();


	private ButtonMethods () {}

	public static ButtonMethods getInstance(){
		if (instance == null) {
			GameObject singleton = new GameObject ();
			instance = singleton.AddComponent<ButtonMethods> ();
		}
		return instance;
	}



	public string current;
	public string next;

	public AudioClip sound;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		ButtonMethods.getInstance();

		Debug.Log ("Initializing sound object");
		source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void HelpPlay(){
		sPlay ();
	}

	public void coPlay(){
		Debug.Log ("ENTER CO");

		Play ();

		Debug.Log ("LEAVE CO");

	}
	public void NormalPlay(){
		Play ();
		Switch();
	}

	public void sPlay(){
		source.PlayOneShot (sound, 1.0f);
	}

	public void Play (){
		Debug.Log ("PLAYING");
		source.PlayOneShot (sound, 1.0f);
		Debug.Log ("FINISH PLAYING");

	}

	public void Switch(){

		Debug.Log ("Going from " + current+ " to " +next);

		SwitchPanels.changePanelStatic (next+":activate," +current+":deactivate");

	}

	public void Please(){
		Debug.Log ("PLEASE");

		PlayChange ();

		Debug.Log ("POST PLEASE");

	}

	public IEnumerator PlayChange(){
		Debug.Log ("PRE PLAYING");

		//ButtonMethods.getInstance().StartCoroutine(Play ());

		Play ();
		Debug.Log ("EXIT PLAYING");

		yield return new WaitForSeconds (5.0f);

		//Play ();
		Switch ();

	}

	public void soundChange(){

		Debug.Log ("PRE PLAYING");

		source.PlayOneShot (sound, 1.0f);

		Debug.Log ("POST PLAYING");

		Debug.Log ("Going from " + current+ " to " +next);

		SwitchPanels.changePanelStatic (next+":activate," +current+":deactivate");

		Debug.Log ("CHANGED?");


	}

	public void Play_Switch(){

		Debug.Log ("START FIR");

		float len = source.clip.length;

		source.PlayOneShot (sound, 1.0f);

		StartCoroutine(delayAct(len));

		Debug.Log ("END FIR");

	}


	//TODO
	//Shorten files so that delay is less apparent


	IEnumerator delayAct (float timeToWait)
	{

		Debug.Log ("START SEC");

		yield return new WaitForSeconds(timeToWait);

		Debug.Log ("Going from " + current+ " to " +next);

		SwitchPanels.changePanelStatic (next+":activate," +current+":deactivate");

		Debug.Log ("END SEC");
	}

}

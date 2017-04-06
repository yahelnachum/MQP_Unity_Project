using UnityEngine;
using System.Collections;

public class SecureCall : MonoBehaviour {

	public AudioClip sound1;
	public AudioClip sound2;
	public AudioClip sound3;


	private AudioSource SecSource;

	private static SecureCall secureCallInstance = null;

	private SecureCall(){}

	static bool curPlay = false;

	public static SecureCall getInstance(){
		Debug.Log ("call get instance of SecureCall" );


		if (secureCallInstance == null) {
			Debug.Log ("First secure Call!!!!" );

			GameObject singleton = new GameObject ();
			secureCallInstance = singleton.AddComponent<SecureCall> ();
		}
		Debug.Log ("ret secure call instance" );


		return secureCallInstance;
	}


	void Start () {
		Debug.Log ("Start Sec Call" );

		SecureCall.getInstance();

		Debug.Log ("Initializing SecSound object");
		SecSource = GetComponent<AudioSource>();
		Debug.Log ("check if playing");


		Debug.Log ("Play status: " + curPlay);


		if (!curPlay) {
			curPlay = true;
			Debug.Log ("Play status: " + curPlay);


			Debug.Log ("Start CallTime");

			//source = GetComponent<AudioSource>();
			//StartCoroutine (StartCallTime ());

			Debug.Log ("End CallTime");


		} else {

			Debug.Log ("already playing, chill");

		}

	}

	public void clickAn (){

		StartCoroutine (StartCallTime ());

	}

	public void PlayConvo (){
		Debug.Log ("START play convo");

		Debug.Log ("setting SecSource");
		SecSource = GetComponent<AudioSource>();

		float len;

		switch (PlayerData.getInstance ().getCurrentNarrativeChunk ()) {

		case 6:

			Debug.Log ("Case 6");


			Debug.Log ("trying to get sound clip?");

			SecSource.clip = sound1;
			Debug.Log ("found sound clip");


			len = SecSource.clip.length;
			Debug.Log ("length is " + len);


			SecSource.PlayOneShot (sound1, 1.0f);
			Debug.Log ("after make sound");
			break;

		case 8:

			Debug.Log ("Case 8");


			Debug.Log ("trying to get sound clip?");

			SecSource.clip = sound2;
			Debug.Log ("found sound clip");


			len = SecSource.clip.length;
			Debug.Log ("length is " + len);


			SecSource.PlayOneShot (sound2, 1.0f);
			Debug.Log ("after make sound");
			break;

		case 9:

			Debug.Log ("Case 9");

			Debug.Log ("trying to get sound clip?");

			SecSource.clip = sound3;
			Debug.Log ("found sound clip");


			len = SecSource.clip.length;
			Debug.Log ("length is " + len);


			SecSource.PlayOneShot (sound3, 1.0f);
			Debug.Log ("after make sound");
			break;

		default:

			Debug.Log ("ERROR DEFAULT SECURE CALL REACHED NO BUENO");
			break;
			
		}

	}

	private IEnumerator StartCallTime(){
		Debug.Log ("before play1");

		//Play1 ();
		PlayConvo();
		Debug.Log ("after play1");

		Debug.Log ("about to yield");
		Debug.Log ("Play status: " + curPlay);


		yield return new WaitForSeconds (SecSource.clip.length);

		Debug.Log ("post yield");
		curPlay = false;

		//SwitchPanels.changePanelStatic ("pMainTutorial:activate");

		Debug.Log ("proceed");

		SwitchPanels.changePanelStatic ("pMain:activate,pSecureCall:deactivate");
		PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
		PlayerData.getInstance ().saveData ();
	}

	public void Play1 (){
		Debug.Log ("PLAYING_THE_SECURE_CALL? I GUESS?");
		SecSource = GetComponent<AudioSource>();
		Debug.Log ("source set?");

		SecSource.PlayOneShot(sound1,1.0f);
		Debug.Log ("SOUND PLAYED?");

	}

	public IEnumerator goodPlay (){
		Debug.Log ("good start?");

		gPlayer ();
		Debug.Log ("good end?");

		yield return new WaitForSeconds (46f);

		proceed ();


	}

	public void gPlayer(){

		Debug.Log ("PLAYING_THE_SECURE_CALL? I GUESS?");
		SecSource = GetComponent<AudioSource>();
		Debug.Log ("source set?");

		SecSource.PlayOneShot(sound1,1.0f);
		Debug.Log ("SOUND PLAYED?");
	}






	public void startTimer(){
		//StartCoroutine (startSecureCallPanel());


			Debug.Log ("START FIR");



		Debug.Log ("PLAYING_THE_SECURE_CALL? I GUESS?");
		SecSource = GetComponent<AudioSource>();
		Debug.Log ("source set?");

			SecSource.clip = sound1;

			float len = SecSource.clip.length;

			SecSource.PlayOneShot (sound1, 1.0f);

			StartCoroutine(delayAct(len));

			Debug.Log ("END FIR");

		
	}

	IEnumerator delayAct (float timeToWait)
	{

		Debug.Log ("START SEC");

		yield return new WaitForSeconds(timeToWait);

		Debug.Log ("after wait");

		proceed ();

		//SwitchPanels.changePanelStatic (next+":activate," +current+":deactivate");

		Debug.Log ("END SEC");
	}

	public IEnumerator startSecureCallPanel(){



		Debug.Log ("Initializing sound object for call");
		//source = GetComponent<AudioSource>();
		Debug.Log ("good start?");

		gPlayer ();
		Debug.Log ("good end?");

		yield return new WaitForSeconds (46f);

		proceed ();






	}

	public void proceed() {
		Debug.Log ("proceed");

		SwitchPanels.changePanelStatic ("pMain:activate,pSecureCall:deactivate");
		PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
		PlayerData.getInstance ().saveData ();
	}

}

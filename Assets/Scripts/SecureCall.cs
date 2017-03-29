using UnityEngine;
using System.Collections;

public class SecureCall : MonoBehaviour {

	public AudioClip sound1;
	public AudioClip sound2;
	public AudioClip sound3;


	private AudioSource source;

	public void Play1 (){
		Debug.Log ("PLAYING");
		source.PlayOneShot(sound1,1.0f);
	}

	void Start(){

		startTimer ();
	}

	private static SecureCall secureCallInstance = null;

	private SecureCall(){
	}

	public static SecureCall getInstance(){

		if (secureCallInstance == null) {
			GameObject singleton = new GameObject ();
			secureCallInstance = singleton.AddComponent<SecureCall> ();
		}

		return secureCallInstance;
	}

	public void startTimer(){
		StartCoroutine (startSecureCallPanel());
	}

	public IEnumerator startSecureCallPanel(){

		Debug.Log ("Initializing sound object for call");
		source = GetComponent<AudioSource>();
		Play1 ();
		yield return new WaitForSeconds (46f);
<<<<<<< Updated upstream

=======
		//Play1 ();
>>>>>>> Stashed changes

		SwitchPanels.changePanelStatic ("pMain:activate,pSecureCall:deactivate");
		PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
		PlayerData.getInstance ().saveData ();
	}
}

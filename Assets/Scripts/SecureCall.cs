using UnityEngine;
using System.Collections;

public class SecureCall : MonoBehaviour {

	public AudioClip sound;
	private AudioSource source;

	public void Play1 (){
		Debug.Log ("PLAYING");
		source.PlayOneShot(sound,1.0f);
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
		yield return new WaitForSeconds (46f);
		Play1 ();

		SwitchPanels.changePanelStatic ("pMain:activate,pSecureCall:deactivate");
		PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
		PlayerData.getInstance ().saveData ();
	}
}

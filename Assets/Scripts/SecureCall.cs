using UnityEngine;
using System.Collections;

public class SecureCall : MonoBehaviour {

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
		yield return new WaitForSeconds (3f);
		SwitchPanels.changePanelStatic ("pMain:activate,pSecureCall:deactivate");
		PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
		PlayerData.getInstance ().saveData ();
	}
}

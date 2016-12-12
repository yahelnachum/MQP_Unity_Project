using UnityEngine;
using System.Collections;

public class PlaySound : MonoBehaviour {


	private static PlaySound instance = null;

	/// <summary>
	/// Initializes a new instance of the <see cref="UpdatePanel"/> class.
	/// Made private so that it can be a singleton.
	/// </summary>
	private PlaySound(){}

	/// <summary>
	/// Returns the single instance of the UpdatePanel.
	/// </summary>
	/// <returns>The single instance of the UpdatePanel class.</returns>
	public static PlaySound getInstance(){
		if (instance == null) {
			GameObject singleton = new GameObject ();
			instance = singleton.AddComponent<PlaySound> ();
		}
		return instance;
	}

	public void Start(){
		PlaySound.getInstance();
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	//used by button to call sound function
	public void startSound( string sound){

		//UpdatePanel.getInstance().StartCoroutine(progress (0.1f));
		//PlaySound.getInstance().StartCoroutine(begin(sound));

		Debug.Log ("NOW PLAYING" + sound);


	}



}

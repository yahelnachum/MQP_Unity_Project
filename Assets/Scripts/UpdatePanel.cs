using UnityEngine;
using System.Collections;
using UnityEngine.UI; 

public class UpdatePanel : MonoBehaviour {

	private static UpdatePanel instance = new UpdatePanel();
	public AudioClip newClip;


	/// <summary>
	/// Initializes a new instance of the <see cref="UpdatePanel"/> class.
	/// Made private so that it can be a singleton.
	/// </summary>
	private UpdatePanel(){}

	/// <summary>
	/// Returns the single instance of the UpdatePanel.
	/// </summary>
	/// <returns>The single instance of the UpdatePanel class.</returns>
	public static UpdatePanel getInstance(){
		if (instance == null) {
			GameObject singleton = new GameObject ();
			instance = singleton.AddComponent<UpdatePanel> ();
		}
		return instance;
	}

	public void Start(){
		UpdatePanel.getInstance();
	}


	/// <summary>
	/// Increase the progress bar element on the update panel by looping, adjusting anchors, and waiting.
	/// </summary>
	/// <returns>An IEnumerator for the sake of waiting</returns>
	/// <param name="val">Optional parameter to adjust fill rate.</param>
	public  IEnumerator progress(float val){

		GameObject bar = GameObject.Find ("pLoadingBar");
		RectTransform rect = bar.GetComponent<RectTransform> ();

		GameObject title = GameObject.Find ("tUpdate");
		Text word = title.GetComponent<Text> (); 

		GameObject button = GameObject.Find ("bProceed");
		button.SetActive(false);

		GameObject panel = GameObject.Find ("pUpdate");
		AudioSource song = panel.GetComponent<AudioSource> ();

		GameObject icon = GameObject.Find ("iLoading");


		for (int i = 0; i < 100; i++) {
			
			float temp = rect.anchorMax.x;
			val = temp + 0.01f;

			rect.anchorMax = new Vector2 (val, 1);
			//rect.position = new Vector2 (0, 0);
			//rect.anchorMax = new Vector2 (0.9f, 1);

			if (i == 20) {
				word.text = "Isn't this fun?";
			}

			if (i == 40) {
				word.text = "Just a bit longer";
			}

			if (i == 60) {
				word.text = "Don't worry, all your files are exactly where you left them";
			}

			if (i == 80) {
				word.text = ";)";
			}

			if (i == 85) {
				word.text = "Almost there!";
			}

			spin (icon);
				
			yield return new WaitForSeconds (0.3f);

		}

		word.text = "Done!";

		icon.SetActive(false);
		button.SetActive(true);

		//song.Stop():

		song.Stop ();

		end (song);

	}

	//used by button to call progress function
	public static void startUpdate(){

		UpdatePanel.getInstance().StartCoroutine(instance.progress (0.1f));

	}

	public void end(AudioSource song){

		AudioClip crap = Resources.Load ("ding") as AudioClip;

		float len = crap.length;

		Debug.Log ("SONG IS " + len + "LONG");

		song.PlayOneShot (crap, 1.0f);	
	}


	public void spin(GameObject icon){

		//rotate object some amount

	}

}



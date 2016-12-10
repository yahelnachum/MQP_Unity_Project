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

	float fps = 60;

	public  IEnumerator load(float start, float end, float seconds, RectTransform rect){
		for (float current = start; current < end; current += (end - start) / (seconds * fps)) {
			float init = Time.realtimeSinceStartup;
			rect.anchorMax = new Vector2 (current, 1);

			float delta = Time.realtimeSinceStartup - init;
			if (delta < 1f / fps) {
				yield return new WaitForSeconds (1f / fps - delta);
			}
		}
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

		Debug.Log ("loading 8888888888");

		yield return StartCoroutine(load (0f, 0.2f, 1f, rect));
		word.text = "Isn't this fun?";
		yield return StartCoroutine(load (0.2f, 0.4f, 2f, rect));
		word.text = "Just a bit longer";
		yield return StartCoroutine(load (0.4f, 0.6f, 1f, rect));
		word.text = "Don't worry, all your files are exactly where you left them";
		yield return StartCoroutine(load (0.6f, 0.8f, 4f, rect));
		word.text = "Almost there!";
		yield return StartCoroutine(load (0.8f, 1.0f, 1f, rect));

		//spin (icon);

		word.text = "Done!";

		icon.SetActive(false);
		button.SetActive(true);

		//song.Stop():

		song.Stop ();

		end (song);

		yield return null;

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



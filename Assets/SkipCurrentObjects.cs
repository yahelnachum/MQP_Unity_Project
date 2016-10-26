using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SkipCurrentObjects : MonoBehaviour {

	GameObject bSkip;
	static float initialTime = 0f;
	const float waitSeconds = 1f; // 5 mins is 300f.
	// Use this for initialization
	void Start () {

		// find button and set it un-interactable and set up the inital time
		bSkip = GameObject.Find ("bSkip");
		bSkip.GetComponent<Selectable> ().interactable = false;
		initialTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		// if the wait seconds has passed then make the button interactable again
		if (bSkip.GetComponent<Selectable> ().interactable == false && Time.time - initialTime > waitSeconds) {
			bSkip.GetComponent<Selectable> ().interactable = true;
		}
	}

	/// <summary>
	/// Skips the current list of objects to a new set of objects.
	/// </summary>
	public void skipCurrentObjects(){

		// pick new objects, reset initial time, and set button to un-interactable
		ObjectList.getInstance ().pickCurrentObjects ();
		initialTime = Time.time;
		bSkip.GetComponent<Selectable> ().interactable = false;
	}
}

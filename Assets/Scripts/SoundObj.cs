using UnityEngine;
using System.Collections;

public class SoundObj : MonoBehaviour {

	public AudioClip shootSound;
	private AudioSource source;

	// Use this for initialization
	void Start () {
		Debug.Log ("Initializing sound object");
		source = GetComponent<AudioSource>();
	}

	// Update is called once per frame
	void Update () {

	}

	public void Play (){
		Debug.Log ("PLAYING");
		source.PlayOneShot(shootSound,1.0f);
	}
}

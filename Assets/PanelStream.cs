using UnityEngine;
using System.Collections;

public class PanelStream : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
		Webcam.pinchAndZoom ();
	}
}

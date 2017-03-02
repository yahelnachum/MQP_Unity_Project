using UnityEngine;
using System.Collections;

public class WebcamButtonController : MonoBehaviour {

	public void startCamera(){
		Webcam.getInstance ().startCamera ();
	}

	public void takeSnapshot(){
		Webcam.getInstance ().TakeSnapShot ();
	}

	public void stopCamera(){
		Webcam.getInstance ().stopCamera ();
		Webcam.getInstance ().resetCameraZoom ();
	}
}

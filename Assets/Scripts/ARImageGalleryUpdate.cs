using UnityEngine;
using System.Collections;

public class ARImageGalleryUpdate : MonoBehaviour {
	
	// Update is called once per frame
	void Update () {
		ARImageGallery.getInstance ().pinchAndZoom ();
	}
}

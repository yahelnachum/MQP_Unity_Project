using UnityEngine;
using System.Collections;

public class ARImageGalleryPanel : MonoBehaviour {

	void Start(){
		ARImageGallery.getInstance ().initialize ();
	}

	public void enableARImages(){
		ARImageGallery.getInstance ().toggleUnlockedARImages (true);
	}

	public void disableARImages(){
		ARImageGallery.getInstance ().toggleUnlockedARImages (false);
	}
}

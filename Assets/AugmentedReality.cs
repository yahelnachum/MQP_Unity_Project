using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AugmentedReality : MonoBehaviour {

	private static AugmentedReality augmentedRealityInstance = null;
	private GameObject pARImage;
	private RawImage img;

	private AugmentedReality(){
	}

	public static AugmentedReality getInstance(){

		if (augmentedRealityInstance == null) {
			GameObject singleton = new GameObject ();
			augmentedRealityInstance = singleton.AddComponent<AugmentedReality> ();

			augmentedRealityInstance.pARImage = StartGame.findInactive ("pARImage", "vMenu");

			int cwCamera = Webcam.getInstance ().cwCamera;
			augmentedRealityInstance.pARImage.transform.localRotation = Quaternion.AngleAxis (-cwCamera * 1f, Vector3.back);

			// scale to fit screen
			RectTransform rectT = (RectTransform)augmentedRealityInstance.pARImage.transform;
			float width = rectT.rect.width;
			float height = rectT.rect.height;

			rectT.localScale = new Vector3 (height / width, width / height, 1f);

			augmentedRealityInstance.img = augmentedRealityInstance.pARImage.AddComponent<RawImage> ();
			augmentedRealityInstance.img.uvRect = new Rect (1f, 0f, -1f, -1f);

		}

		return augmentedRealityInstance;
	}

	public void setNewImage(){

		byte[] imageData = System.IO.File.ReadAllBytes (Application.persistentDataPath + "/image" + PlayerData.getInstance ().getCurrentNarrativeChunk () + ".jpg");


		Texture2D tex = new Texture2D (1, 1);
		tex.LoadImage (imageData);
		img.texture = tex;

	}
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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

			augmentedRealityInstance.pARImage = (GameObject)StartGame.findInactive ("pARImageBackground", "vMenu")[0];

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

		byte[] imageData = System.IO.File.ReadAllBytes (Application.persistentDataPath + 
														"/image" + PlayerData.getInstance ().getCurrentNarrativeChunk () + ".jpg");
		Texture2D tex = new Texture2D (1, 1);
		tex.LoadImage (imageData);
		img.texture = tex;

		startTimer ();
	}

	public void startTimer(){
		StartCoroutine (WaitForPanelSwitch ());
	}

	private float minWaitTime = 10f; // in seconds
	private float maxWaitTime = 2f; // in seconds
	private float firstWaitTime = 5f; // in seconds

	IEnumerator WaitForPanelSwitch(){
		if (PlayerData.getInstance().getCurrentNarrativeChunk() == 3) {
			yield return new WaitForSeconds (firstWaitTime);
			List<GameObject> panel = StartGame.findInactive ("pError", "pAugmentedReality");
			panel [0].SetActive (true);

			PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
			PlayerData.getInstance ().saveData ();

		} else {
			yield return new WaitForSeconds (minWaitTime);
			SwitchPanels.changePanelStatic ("pAugmentedReality:deactivate,pRewards:activate");
		}
	}
}

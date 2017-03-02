using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ARImageGallery : MonoBehaviour {

	private static ARImageGallery arImageGallery = null;
	private int[] narrativeNumbers = {3, 5, 6, 7, 8, 9};
	private List<GameObject> panels;
	private int ScreenWidth = Screen.width;
	private int ScreenHeight = Screen.height;

	private ARImageGallery(){}

	public void initialize(){
		panels = new List<GameObject> ();

		for (int i = 0; i < narrativeNumbers.Length; i++) {
			GameObject obj = StartGame.findInactive ("pARImage0" + narrativeNumbers [i], "vMenu") [0];
			Debug.Log ("PanelPosition: " + obj.transform.position);
			panels.Add(obj);
		}
	}

	public static ARImageGallery getInstance(){
		if (arImageGallery == null) {
			GameObject singleton = new GameObject ();
			arImageGallery = singleton.AddComponent<ARImageGallery> ();
		}
		return arImageGallery;
	}

	public void toggleUnlockedARImages(bool enable){
		string activation = "deactivate";
		if (enable) {
			activation = "activate";
		}

		for(int i = 0; i < narrativeNumbers.Length; i++){
			if(narrativeNumbers[i] < PlayerData.getInstance().getCurrentNarrativeChunk()){
				SwitchPanels.changePanelStatic("pARImage0"+narrativeNumbers[i]+":"+activation);
			}
		}
	}

	private float screenPercent = 0.8f;
	private float screenOffset = 0.2f;

	public void pinchAndZoom(){
		// check that the player is using only two fingers
		if (Input.touchCount == 1) {
			//Tutorial obj = new Tutorial ();
			//obj.changePanel ("pCameraStreamTutorial:deactivate,pAnalyzeButtonTutorial:activate");

			// Store both touches.
			Touch touchZero = Input.GetTouch (0);
			float touchZeroXPercent = touchZero.position.x / ScreenWidth;
			float touchZeroYPercent = touchZero.position.y / ScreenHeight;
			if (touchZeroYPercent >= screenOffset) {
				// Find the position in the previous frame of each touch.
				Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
				Debug.Log ("TouchZeroPosition: " + touchZero.position);
				//Vector2 vector = new Vector2 (touchZero.position.x / 230f, touchZero.position.y / 420f);
				Vector2 vector = new Vector2 (touchZeroXPercent, (touchZeroYPercent - screenOffset) / screenPercent);
				int i = getClosestPanelToVector (vector);
				changeMinAndMaxAnchors (panels [i], vector);
			}
		} else if (Input.touchCount == 2) {
			// Store both touches.
			Touch touchZero = Input.GetTouch (0);
			Touch touchOne = Input.GetTouch (1);

			// Find the position in the previous frame of each touch.
			Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
			Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

			// Find the magnitude of the vector (the distance) between the touches in each frame.
			float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
			float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

			// Find the difference in the distances between each frame.
			float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

			for (int i = 0; i < panels.Count; i++) {
				RectTransform rectT = (RectTransform)panels[i].transform;

				// get current x and y
				float currentX = rectT.localScale.x;
				float currentY = rectT.localScale.y;

				// get ratio of sides
				float width = rectT.rect.width;
				float height = rectT.rect.height;
				float ratioXY = height / width;

				// used to make it easier to zoom as the picture gets bigger
				float mult = 0.001f + ((currentX - 0.0f) * 0.002f);

				// calculate difference
				float diffX = -deltaMagnitudeDiff * mult * ratioXY;
				float diffY = -deltaMagnitudeDiff * mult * (1 / ratioXY);

				// calculate new x and y
				float newX = rectT.localScale.x + diffX;
				float newY = rectT.localScale.y + diffY;

				rectT.localScale = new Vector3 (newX, newY, 1f);

				float xDiff = newX / currentX;
				float yDiff = newY / currentY;

				Vector2 vector = new Vector2 (rectT.anchorMin.x * xDiff, rectT.anchorMin.y * yDiff);
				changeMinAndMaxAnchors (panels [i], vector);
			}
		}
	}

	public void changeMinAndMaxAnchors(GameObject obj, Vector2 vector){
		obj.GetComponent<RectTransform> ().anchorMin = vector;
		obj.GetComponent<RectTransform> ().anchorMax = vector;
	}

	public int getClosestPanelToVector(Vector2 vector){
		int closestIndex = 0;
		float closestMag = 2f;
		for (int i = 0; i < panels.Count; i++) {
			if (narrativeNumbers [i] < PlayerData.getInstance ().getCurrentNarrativeChunk ()) {
				Vector2 vector1 = panels [i].GetComponent<RectTransform> ().anchorMin;
				float mag = (vector - vector1).sqrMagnitude;
				Debug.Log ("Panel: " + i + " Magnetude: " + mag);
				if (mag < closestMag) {
					closestMag = mag;
					closestIndex = i;
				}
			} 
		}

		return closestIndex;
	}
}

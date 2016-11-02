using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Webcam : MonoBehaviour{

    public WebCamTexture mCamera = null;
	public GameObject pCameraStream;
  
	private float maxZoom = 10.0f;
	private float minX;
	private float minY;
	private float maxX;
	private float maxY;

	private static Webcam webcam = null;

	private Webcam(){}

	public void Start(){
		Webcam.getInstance();
	}

	public static Webcam getInstance(){
		if (webcam == null) {
			GameObject singleton = new GameObject ();
			webcam = singleton.AddComponent<Webcam> ();
		}
		return webcam;
	}

    /// <summary>
    /// Starts the camera
    /// </summary>
	public void startCamera() {
		
		if (mCamera == null) {

			// get available webcam devices
			WebCamDevice[] devices = WebCamTexture.devices;

			if (devices.Length > 0) {


				mCamera = new WebCamTexture (devices [0].name, 1920, 1920);
				string pCameraStreamName = "pCameraStream";
				pCameraStream = GameObject.Find (pCameraStreamName);


				RawImage img = pCameraStream.GetComponent<RawImage> ();
				img.texture = mCamera;
				mCamera.Play ();

				// rotate panel to match mobile orientation
				int cwCamera = mCamera.videoRotationAngle;
				pCameraStream.transform.localRotation = Quaternion.AngleAxis (-cwCamera * 1f, Vector3.back);
				img.uvRect = new Rect (1f, 0f, -1f, 1f);

				// scale to fit screen
				RectTransform rectT = (RectTransform)pCameraStream.transform;
				float width = rectT.rect.width;
				float height = rectT.rect.height;

				rectT.localScale = new Vector3 (height / width, width / height, 1f);

				// set up zoom limits
				minX = rectT.localScale.x;
				minY = rectT.localScale.y;
				maxX = maxZoom * minX;
				maxY = maxZoom * minY;
			}
		} else {
            mCamera.Play();
        }
	}

	/// <summary>
	/// Pinches and zooms the panel.
	/// </summary>
	public void pinchAndZoom(){

		if (mCamera != null) {

			// check that the player is using only two fingers
			if (Input.touchCount == 2) {
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

				RectTransform rectT = (RectTransform)pCameraStream.transform;

				// get current x and y
				float currentX = rectT.localScale.x;
				float currentY = rectT.localScale.y;

				// get ratio of sides
				float width = rectT.rect.width;
				float height = rectT.rect.height;
				float ratioXY = height / width;

				// used to make it easier to zoom as the picture gets bigger
				float mult = 0.001f + ((currentX - minX) * 0.002f);

				// calculate difference
				float diffX = -deltaMagnitudeDiff * mult * ratioXY;
				float diffY = -deltaMagnitudeDiff * mult * (1 / ratioXY);
			
				// calculate new x and y
				float newX = rectT.localScale.x + diffX;
				float newY = rectT.localScale.y + diffY;

				// keep panel within min and max limits
				if( minX <= newX && newX <= maxX &&  minY <= newY && newY <= maxY){
					rectT.localScale = new Vector3 (newX, newY, 1f);
				}
			}
		}
	}

	/// <summary>
	/// Stops the camera.
	/// </summary>
	public void stopCamera(){

        if (mCamera == null) return;

		// reset the pinch and zoom of the camera and stop streaming it
		RectTransform rectT = (RectTransform)pCameraStream.transform;
		rectT.localScale = new Vector3 (minX, minY, 1f);
		mCamera.Stop ();
	}
		
	/// <summary>
	/// Takes a snap shot of the camera stream
	/// </summary>
	public void TakeSnapShot()
    {

        if (mCamera == null) return;

		// get the text objects for output of http POST responses
		GameObject textCloud = GameObject.Find ("Cloud");
		Text txtCloud = textCloud.GetComponent<Text> ();
		GameObject textWatson = GameObject.Find ("Watson");
		Text txtWatson = textWatson.GetComponent<Text> ();
		GameObject textClarifai = GameObject.Find ("Clarifai");
		Text txtClarifai = textClarifai.GetComponent<Text> ();
        Debug.Log("Taking a snapshot");

		// crop the picture to fit with the scale of the pinch and zoom
		RectTransform rectT = (RectTransform)pCameraStream.transform;

		float width = mCamera.width;
		float height = mCamera.height;

		float currentX = rectT.localScale.x;
		float percentCrop = ((currentX - minX) / (maxX - minX)) * ((maxZoom - 1)/maxZoom);

		float newWidth = width * (1f - percentCrop);
		float newHeight = height * (1f - percentCrop);

		float midX = width / 2f;
		float midY = height / 2f;

		float newX = midX - newWidth / 2f;
		float newY = midY - newHeight / 2f;

		Color[] array = mCamera.GetPixels (Mathf.RoundToInt(newX), Mathf.RoundToInt(newY), Mathf.RoundToInt(newWidth), Mathf.RoundToInt(newHeight));
		Texture2D snap = new Texture2D (Mathf.RoundToInt(newWidth), Mathf.RoundToInt(newHeight));//mCamera.width, mCamera.height);
        
		snap.SetPixels(array);
        snap.Apply();

		//Debug.Log (Application.persistentDataPath);
		//System.IO.File.WriteAllBytes(Application.persistentDataPath+"/unityWebcam.jpg", snap.EncodeToJPG());

		//StartCoroutine(HttpRequest.postCloudSight(snap.EncodeToJPG(), txtCloud));//Application.dataPath + "/unityWebcam.jpg", txt));
		//StartCoroutine(HttpRequest.postWatson(snap.EncodeToJPG(), txtWatson));
		StartCoroutine(HttpRequest.postClarifai(snap.EncodeToJPG(), txtClarifai));
    }
}

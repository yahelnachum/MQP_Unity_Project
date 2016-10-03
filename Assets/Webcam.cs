using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Webcam : MonoBehaviour {

    public WebCamTexture mCamera = null;
	GameObject obj;
   // public GameObject ImageOnPanel;

    float sizeMultiplier = -0.001F;
    // Use this for initialization
    void Start () {

        Debug.Log("Script has been started");
		GameObject panel = GameObject.Find("Image");
        //plane = panel.geComponent<RawImage>();
		//plane = panel.GetComponentInChildren<RawImage> ();
		//plane = panel.AddComponent<RawImage>();
		WebCamDevice[] devices = WebCamTexture.devices;

		int i = 0;
		while (i < devices.Length)
		{
			Debug.Log(devices[i].name);
			i++;
		}

		mCamera = new WebCamTexture(devices[0].name, 1920,1920);
		string objectName = "pCameraStream";
		obj = GameObject.Find (objectName);
		Debug.Log (obj);


		RawImage img = obj.GetComponent<RawImage> ();
		Debug.Log (img);
		img.texture = mCamera;
		mCamera.Play ();

		obj.transform.localScale = new Vector3 (-1f*2f, 1f*0.5f, 1f);
		int cwCamera = mCamera.videoRotationAngle;
		obj.transform.localRotation = Quaternion.AngleAxis (-cwCamera*1f, Vector3.back);
		//obj.transform.localScale.Scale (new Vector3 (-1f, 1f, 1f));

	}

    // Update is called once per frame
    bool first = true;
    void Update () {
		
		/*
//        if (first)
  //      {
            
            Debug.Log("Webcam: " + mCamera == null);
            Debug.Log("RawImage: " + plane == null);
            Debug.Log("RawImage Texture: " + plane.texture == null);

            plane.texture = mCamera;
            mCamera.Play();

            int width = mCamera.width;
            int height = mCamera.height;

            Debug.Log("Width : " + width);
            Debug.Log("Height: " + height);
            Debug.Log("Requested Width : " + mCamera.requestedHeight);
            Debug.Log("Requested Height: " + mCamera.requestedWidth);

            //plane.transform.localScale = new Vector3(sizeMultiplier * width, 1F, sizeMultiplier * height);

		int cwNeeded = 90;//mCamera.videoRotationAngle;
		int ccwNeeded = -cwNeeded;
		float videoRatio = (float)mCamera.width / (float)mCamera.height;

		//if (mCamera.videoVerticallyMirrored) {
			plane.uvRect = new Rect (1, 0, -1, 1);
			//ccwNeeded += 180;
		//}

		Debug.Log ("Video Ratio: "+videoRatio);
		//if (ccwNeeded != 0 && ccwNeeded % 90==1) {
			//plane.transform.localScale = new Vector3 (videoRatio, videoRatio, 1);
		//plane.transform.rotation = plane.transform.rotation * Quaternion.AngleAxis (180f, Vector3.back);
		//lane.transform.localScale= new Vector3(3f,-unity 5f,10f);
		//}

            first = false;

            Debug.Log("DataPath: " + Application.dataPath);
        //}*/
    }

    /*void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
            TakeSnapShot();

    }

    void TakeSnapShot()
    {
        Debug.Log("Taking a snapshot");
        Texture2D snap = new Texture2D(mCamera.width, mCamera.height);
        snap.SetPixels(mCamera.GetPixels());
        snap.Apply();

        System.IO.File.WriteAllBytes(Application.dataPath+"/unityWebcam.jpg", snap.EncodeToJPG());
    }*/
}

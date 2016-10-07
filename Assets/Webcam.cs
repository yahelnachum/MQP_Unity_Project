﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Webcam : MonoBehaviour {

    public WebCamTexture mCamera = null;
	GameObject obj;
   // public GameObject ImageOnPanel;

    float sizeMultiplier = -0.001F;
    // Use this for initialization
    void Start () {
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

		Debug.Log (obj.transform.localScale);
		int cwCamera = mCamera.videoRotationAngle;
		obj.transform.localRotation = Quaternion.AngleAxis (-cwCamera*1f, Vector3.back);
		img.uvRect = new Rect(1f,0f,-1f,1f);

		RectTransform rectT = (RectTransform) obj.transform;
		float width = rectT.rect.width;
		float height = rectT.rect.height;

		rectT.localScale = new Vector3 (height / width, width / height, 1f);
		Debug.Log (((RectTransform)obj.transform).rect.width);
	}

	public void stopCamera(){
		mCamera.Stop ();
	}
		
    void Update () {

    }

    /*void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 50, 30), "Click"))
            TakeSnapShot();

    }*/

	public void TakeSnapShot()
    {
		GameObject background = GameObject.Find ("Text");
		Text txt = background.GetComponent<Text> ();
        Debug.Log("Taking a snapshot");
        Texture2D snap = new Texture2D(mCamera.width, mCamera.height);
        snap.SetPixels(mCamera.GetPixels());
        snap.Apply();

        //System.IO.File.WriteAllBytes(Application.dataPath+"/unityWebcam.jpg", snap.EncodeToJPG());

		StartCoroutine(HttpRequest.postCloudSight(snap.EncodeToJPG(), txt));//Application.dataPath + "/unityWebcam.jpg", txt));
    }
}

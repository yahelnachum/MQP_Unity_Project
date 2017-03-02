using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class AugmentedRealityGyro : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
		
	// Update is called once per frame
	void Update () {
		GameObject obj = GameObject.Find ("pARImageForeground0" + PlayerData.getInstance ().getCurrentNarrativeChunk ());
		Vector3 init = obj.transform.position;
		Vector3 input = Input.gyro.rotationRateUnbiased;
		input = new Vector3 (-1f * input.y * multiplier, input.x * multiplier, input.z);
		Vector3 current = obj.transform.position;
		obj.transform.position = new Vector3 (input.x + current.x + 0.01f * (init.x - current.x), 
																			 	 input.y + current.y + 0.01f * (init.y - current.y), 
																			 	 current.z);
	}
	static float multiplier = 0.1f;
}

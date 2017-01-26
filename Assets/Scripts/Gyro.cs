using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Gyro : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
		
	// Update is called once per frame
	void Update () {
		if (first) {
			first = false;
			init = GameObject.Find ("pARImageForeground").transform.position;
		}
		Vector3 input = Input.gyro.rotationRateUnbiased;
		input = new Vector3 (-1f * input.y * multiplier, input.x * multiplier, input.z);
		Vector3 current = GameObject.Find ("pARImageForeground").transform.position;
		GameObject.Find ("pARImageForeground").transform.position = new Vector3 (input.x + current.x + 0.01f * (init.x - current.x), 
																			 	 input.y + current.y + 0.01f * (init.y - current.y), 
																			 	 current.z);
	}
	static Vector3 init;
	static bool first = true;
	static float multiplier = 0.1f;
}

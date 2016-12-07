using UnityEngine;
using System.Collections;

public class UpdatePanel : MonoBehaviour {
	public static IEnumerator progress(float val){

		Debug.Log("enter func val is" + val);


		GameObject bar = GameObject.Find ("pLoadingBar");

		RectTransform rect = bar.GetComponent<RectTransform> ();

		//float temp = rect.anchorMax.x;

		rect.anchorMax = new Vector2 (val, 1);
		rect.position = new Vector2(0,0);

		//rect.anchorMax = new Vector2 (0.9f, 1);

		Debug.Log("update before panel val is" + val);

		yield return new WaitForSeconds(1);

		Debug.Log("update after panel val is" + val);



	}

}

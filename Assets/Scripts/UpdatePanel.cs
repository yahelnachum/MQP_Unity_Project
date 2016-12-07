using UnityEngine;
using System.Collections;

public class UpdatePanel : MonoBehaviour {
	public static IEnumerator progress(float val){

		GameObject bar = GameObject.Find ("pLoadingBar");

		RectTransform rect = bar.GetComponent<RectTransform> ();

		//float temp = rect.anchorMax.x;

		rect.anchorMax = new Vector2 (val, 1);
		rect.position = new Vector2(0,0);

		//rect.anchorMax = new Vector2 (0.9f, 1);

		yield return new WaitForSeconds(1);


	}


}

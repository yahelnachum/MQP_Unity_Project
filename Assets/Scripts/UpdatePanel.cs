using UnityEngine;
using System.Collections;

public class UpdatePanel : MonoBehaviour {


	/// <summary>
	/// Increase the progress bar element on the update panel by looping, adjusting anchors, and waiting.
	/// </summary>
	/// <returns>An IEnumerator for the sake of waiting</returns>
	/// <param name="val">Optional parameter to adjust fill rate.</param>
	public static IEnumerator progress(float val){

		for (int i = 0; i < 100; i++) {
			
			GameObject bar = GameObject.Find ("pLoadingBar");
			RectTransform rect = bar.GetComponent<RectTransform> ();
			float temp = rect.anchorMax.x;
			val = temp + 0.01f;

			rect.anchorMax = new Vector2 (val, 1);
			//rect.position = new Vector2 (0, 0);
			//rect.anchorMax = new Vector2 (0.9f, 1);

			yield return new WaitForSeconds (0.02f);

		}

	}

	//used by button to call progress function
	public void startUpdate(){

		StartCoroutine (UpdatePanel.progress (0.1f));

	}

}

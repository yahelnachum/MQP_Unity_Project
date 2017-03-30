using UnityEngine;
using System.Collections;
using UnityEngine.UI; 
using System.Collections.Generic;

public class BarGraph : MonoBehaviour {

	private GameObject pBarGraph;

	private GameObject pBarFill0;
	private GameObject pBarFill1;
	private GameObject pBarFill2;

	private GameObject tBarPercent0;
	private GameObject tBarPercent1;
	private GameObject tBarPercent2;

	private GameObject tBarObject0;
	private GameObject tBarObject1;
	private GameObject tBarObject2;

	private bool[] animating = new bool[3];

	private Color red = new Color (1f, 0f, 0f);
	private Color green = new Color (0f, 1f, 0f);

	private bool timerExpired = true;

	private static BarGraph barGraphInstance = null;

	private BarGraph(){
	}

	public static BarGraph getInstance(){
		if (barGraphInstance == null) {
			GameObject singleton = new GameObject ();
			barGraphInstance = singleton.AddComponent<BarGraph> ();
			barGraphInstance.initialize ();
		}

		return barGraphInstance;
	}

	private void initialize(){
		pBarFill0 = StartGame.findInactive ("pBarFill0", "vMenu")[0];
		pBarFill1 = StartGame.findInactive ("pBarFill1", "vMenu")[0];
		pBarFill2 = StartGame.findInactive ("pBarFill2", "vMenu")[0];

		tBarPercent0 = StartGame.findInactive ("tBarPercent0", "vMenu")[0];
		tBarPercent1 = StartGame.findInactive ("tBarPercent1", "vMenu")[0];
		tBarPercent2 = StartGame.findInactive ("tBarPercent2", "vMenu")[0];

		pBarGraph = StartGame.findInactive ("pBarGraph", "vMenu") [0];
		List<GameObject> tObjects = StartGame.findObjectsContainingName ("tObject", pBarGraph);
		Debug.Log ("tobjects found: " + tObjects.Count);
		if (tObjects.Count == 3) {
			tBarObject0 = tObjects [0];
			tBarObject1 = tObjects [1];
			tBarObject2 = tObjects [2];
		}
	}

	public  IEnumerator start (){
		timerExpired = false;

		animating [0] = true;
		animating [1] = true;
		animating [2] = true;

		StartCoroutine (animateBar (pBarFill0, tBarPercent0, tBarObject0, 0));
		StartCoroutine (animateBar (pBarFill1, tBarPercent1, tBarObject1, 1));
		StartCoroutine (animateBar (pBarFill2, tBarPercent2, tBarObject2, 2));
		StartCoroutine (timer());
		yield return null;
	}

	public  IEnumerator timer (){
		yield return new WaitForSeconds (3f);
		timerExpired = true;

		while (HttpRequest.getInstance ().isClarifaiRequestInProgress () ||
			animating[0] ||
			animating[1] ||
			animating[2] ) {
			yield return new WaitForSeconds (0.5f);
		}
			
		if (HttpRequest.getInstance ().getFoundObj () == "") {
			SwitchPanels.changePanelStatic ("pCameraAnalysisFailed:activate");
		} else {
			yield return StartCoroutine(animateGraph(0.2f, 1f, pBarGraph));
			GameObject nextButton = StartGame.findInactive("bNext", "pCameraAnalyzing")[0];
			nextButton.SetActive (true);
		}
	}
		
	const float fps = 60;

	public IEnumerator animateBar(GameObject barObj, GameObject barPercent, GameObject tBarObject, int animatingIndex){
		while(HttpRequest.getInstance().isClarifaiRequestInProgress() || !timerExpired){
			yield return StartCoroutine (load (Random.value, Random.value*0.5f+0.5f, barObj, barPercent));
		}

		if (HttpRequest.getInstance ().getFoundObj () != tBarObject.GetComponent<Text>().text) {
			yield return StartCoroutine (load (Random.value * 0.2f + 0.05f, Random.value * 0.5f + 1.0f, barObj, barPercent));
		} else{
			yield return StartCoroutine (load (Random.value * 0.25f + 0.70f, Random.value * 0.5f + 1.0f, barObj, barPercent));
		}

		animating [animatingIndex] = false;
	}

	public  IEnumerator animateGraph(float end, float seconds, GameObject barGraph){
		RectTransform rect = barGraph.GetComponent<RectTransform> ();
		float start = rect.anchorMin.y;
		float currentTime = 0.0f;

		//Debug.Log ("start: " + start + " end: " + end + " seconds: " + seconds + " rect:" + rect);
		while(currentTime < seconds){
			float current = easeInOut(currentTime, start, end - start, seconds);
			float init = Time.realtimeSinceStartup;

			rect.anchorMin = new Vector2 (rect.anchorMin.x, current);

			float delta = Time.realtimeSinceStartup - init;
			if (delta < 1f / fps) {
				yield return new WaitForSeconds (1f / fps - delta);
			}
			currentTime += Time.realtimeSinceStartup - init;
		}

		rect.anchorMin = new Vector2 (rect.anchorMin.x, end);
	}

	public  IEnumerator load(float end, float seconds, GameObject barObj, GameObject barPercent){
		RectTransform barPercentRect = barPercent.GetComponent<RectTransform> ();
		Text barPercentText = barPercent.GetComponent<Text> ();

		RectTransform rect = barObj.GetComponent<RectTransform> ();
		Image img = barObj.GetComponent<Image> ();
		float start = rect.anchorMax.y;
		float currentTime = 0.0f;

		//Debug.Log ("start: " + start + " end: " + end + " seconds: " + seconds + " rect:" + rect);
		while(currentTime < seconds){
			float current = easeInOut(currentTime, start, end - start, seconds);
			float init = Time.realtimeSinceStartup;

			setGraphics (current, barPercentRect, barPercentText, rect, img);

			float delta = Time.realtimeSinceStartup - init;
			if (delta < 1f / fps) {
				yield return new WaitForSeconds (1f / fps - delta);
			}
			currentTime += Time.realtimeSinceStartup - init;
		}

		setGraphics (end, barPercentRect, barPercentText, rect, img);
	}

	public void setGraphics(float current, RectTransform barPercentRect,Text barPercentText,RectTransform rect,Image img){
		rect.anchorMax = new Vector2 (rect.anchorMax.x, current);
		barPercentRect.anchorMin = new Vector2 (barPercentRect.anchorMin.x, current);
		barPercentRect.anchorMax = new Vector2 (barPercentRect.anchorMax.x, current);

		barPercentText.text = Mathf.Round(current * 100f) + "%";
		img.color = green * current + (1f - current) * red;
	}

	public float easeInOut(float currentTime, float startValue, float changeInValue, float duration){
		currentTime /= duration/2f;
		if(currentTime < 1f) return changeInValue/2f*currentTime*currentTime + startValue;
		currentTime--;
		return -changeInValue/2f * (currentTime*(currentTime-2f) - 1f) + startValue;
	}

	public bool isBetween(float bound1, float bound2, float check){
		return  (bound1 <= check && check <= bound2) ||
			(bound2 <= check && check <= bound1);
	}
}

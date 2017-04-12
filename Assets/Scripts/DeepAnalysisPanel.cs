using UnityEngine;
using System.Collections;

public class DeepAnalysisPanel : MonoBehaviour {

	public void ok(){

		string pDeepAnalysisName = transform.parent.transform.parent.name;
		int requestIndex = int.Parse(pDeepAnalysisName.Substring("pDeepAnalysis".Length));
		if (HttpRequestManager.getInstance ().getCloudsightRequests () [requestIndex].hasAnalysisSucceeded ()) {
			HttpRequestManager.getInstance ().clearRequests ();
			BarGraphPanel.switchToCorrectPanel ();
		}

		Destroy (transform.parent.transform.parent.gameObject);
	}
}

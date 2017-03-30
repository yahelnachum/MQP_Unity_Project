using UnityEngine;
using System.Collections;

public class DeepAnalysisPanel : MonoBehaviour {

	public void ok(){
		SwitchPanels.changePanelStatic ("pDeepAnalysis:deactivate");
		if (HttpRequest.getInstance ().getFoundObj () != "") {
			BarGraphPanel.switchToCorrectPanel ();
		}
	}
}

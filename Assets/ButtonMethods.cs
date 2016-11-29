using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ButtonMethods : MonoBehaviour {

	/*public void activateNextNarrativePanel(){

		GameObject pAugmentedReality = GameObject.Find ("pAugmentedReality");
		GameObject pMain = pAugmentedReality.transform.parent.FindChild ("pMain").gameObject;

		pAugmentedReality.SetActive (false);

		GameObject nextNarrative = (GameObject)StartGame.findInactive("pNarrative"+(PlayerData.getInstance ().getCurrentNarrativeChunk()+1),"vMenu")[0];
		if (nextNarrative != null) {
			nextNarrative.SetActive (true);

			PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
			PlayerData.getInstance ().saveData ();
		} else {
			pMain.SetActive (true);
		}
	}*/
}

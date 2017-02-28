using UnityEngine;
using System.Collections;

public class AugmentedRealityPanel : MonoBehaviour {

	public void incrementChunkAndSave(){
		PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
		PlayerData.getInstance ().saveData ();
	}
}

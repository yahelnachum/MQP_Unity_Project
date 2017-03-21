using UnityEngine;
using System.Collections;

public class AugmentedRealityPanel : MonoBehaviour {

	public void incrementChunkAndSave_andResetGyro(){
		PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
		PlayerData.getInstance ().saveData ();

		AugmentedRealityGyro.getInstance ().resetGyro ();
	}
}

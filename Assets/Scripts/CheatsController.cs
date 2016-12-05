using UnityEngine;
using System.Collections;
using System.IO;

public class CheatsController : MonoBehaviour {

	public void deletePlayerData(){
		File.Delete (PlayerData.getInstance ().getFilePath ());
	}
}

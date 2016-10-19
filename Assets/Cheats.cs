using UnityEngine;
using System.Collections;
using System.IO;

public class Cheats : MonoBehaviour {

	void Start(){
	}
	void Update(){
	}
	public void deletePlayerData(){
		File.Delete (PlayerData.getInstance ().getFilePath ());
	}
}

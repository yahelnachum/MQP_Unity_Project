using UnityEngine;
using System;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class PlayerData {

	private string fileName = "playerData.dat";

	private static PlayerData data = new PlayerData();
	private int currentNarrativeChunk = 0;

	private PlayerData(){}

	public static PlayerData getInstance(){
		return data;
	}

	public int getCurrentNarrativeChunk(){
		return currentNarrativeChunk;
	}

	public void incrementCurrentNarrativeChunk(){
		currentNarrativeChunk++;
		Debug.Log ("Incrementing the current narrative chunk to chunk " + currentNarrativeChunk);
	}

	public string getFilePath(){
		return Application.persistentDataPath + "/" + fileName;
	}

	public void loadData(){
		Debug.Log ("loading data");
		if (System.IO.File.Exists (getFilePath())) {
			Debug.Log ("data file found");
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (getFilePath(), FileMode.Open);
			currentNarrativeChunk = (int)bf.Deserialize (file);
			file.Close ();
		} else {
			Debug.Log ("data file NOT found");
			currentNarrativeChunk = 0;
		}
	}

	public void saveData(){
		Debug.Log ("Saving Player Data");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Open (Application.persistentDataPath + "/" + fileName, FileMode.OpenOrCreate);
		bf.Serialize (file, currentNarrativeChunk);
		file.Close ();
	}
}

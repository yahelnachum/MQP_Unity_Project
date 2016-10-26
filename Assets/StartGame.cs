﻿using UnityEngine;
using System.Collections;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		PlayerData data = PlayerData.getInstance ();

		data.loadData ();


		GameObject pNarrative0 = findInactive ("pNarrative"+data.getCurrentNarrativeChunk());
		pNarrative0.SetActive (true);
	}

	public static GameObject findInactive(string objName){
		GameObject vMenu = GameObject.Find ("vMenu");
		Transform[] children = vMenu.GetComponentsInChildren<Transform>(true);

		for (int i = 0; i < children.Length; i++) {
			if (children [i].name.CompareTo (objName) == 0) {
				return children [i].gameObject;
			}
		}

		return null;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
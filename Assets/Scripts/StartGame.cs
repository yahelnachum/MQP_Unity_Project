using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StartGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		PlayerData data = PlayerData.getInstance ();
		data.loadData ();

        ObjectList.pickCurrentObjectsStatic();
	}

	public static List<GameObject> findInactive(string objName, string rootObjName){
		Debug.Log ("objName; " + objName + " rootObjName: " + rootObjName);
		GameObject gameObj = GameObject.Find (rootObjName);
		Transform[] children = gameObj.GetComponentsInChildren<Transform>(true);

		List<GameObject> list = new List<GameObject> ();
		for (int i = 0; i < children.Length; i++) {
			if (children [i].name.CompareTo (objName) == 0) {
				list.Add(children [i].gameObject);
			}
		}

		return list;
	}
}

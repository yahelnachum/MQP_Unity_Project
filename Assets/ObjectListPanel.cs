using UnityEngine;
using System.Collections;

public class ObjectListPanel : MonoBehaviour {

	public TextAsset asset;

	// Use this for initialization
	void Start () {
		ObjectList instance = ObjectList.getInstance ();
		instance.initialize (asset);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

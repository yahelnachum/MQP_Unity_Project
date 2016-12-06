using UnityEngine;
using System.Collections;

public class ObjectListPanel : MonoBehaviour {

	public TextAsset objectListTextAsset;
	public TextAsset acceptedTagsTextAsset;

	// Use this for initialization
	void Start () {
		Debug.Log ("Initializing List from panel start");
		ObjectList instance = ObjectList.getInstance ();
		instance.initialize (objectListTextAsset, acceptedTagsTextAsset);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

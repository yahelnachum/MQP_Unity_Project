using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Version : MonoBehaviour {

    // <branch> YYMMDD.hhmm
    public const string VersionString = "HTTPCam 161026.1611";

	// Use this for initialization
	void Start () {
        GameObject tVersionNo = GameObject.Find("tVersionNo");
        tVersionNo.GetComponent<Text>().text = VersionString;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

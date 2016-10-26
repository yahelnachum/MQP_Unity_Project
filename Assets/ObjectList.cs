using UnityEngine;
using System.Collections;
using System.Text;
using System.IO; 
using UnityEngine.UI;

public class ObjectList{

	private static ObjectList instance = new ObjectList();

	private string[] objects;
	private AcceptedTags[] acceptedTags;
	private Text[] currentObjects = new Text[3];

	/// <summary>
	/// Initializes a new instance of the <see cref="ObjectList"/> class.
	/// Made private so that it can be a singleton.
	/// </summary>
	private ObjectList(){}

	/// <summary>
	/// Returns the single instance of the ObjectList.
	/// </summary>
	/// <returns>The single instance of the ObjectList class.</returns>
	public static ObjectList getInstance(){
		return instance;
	}

	/// <summary>
	/// Initializes the instance of the ObjectList.
	/// </summary>
	/// <param name="asset">Asset.</param>
	public void initialize(TextAsset objectListTextAsset, TextAsset acceptedTagsTextAsset){

		// get array of available objects
		char[] lineSplitters = { '\n', '\r' };
		objects = objectListTextAsset.text.Split (lineSplitters, System.StringSplitOptions.RemoveEmptyEntries);

		// get text objects from game
		for (int i = 0; i < currentObjects.Length; i++) {
			GameObject text = GameObject.Find ("tObject" + i);
			currentObjects[i] = text.GetComponent<Text> ();
		}

		pickCurrentObjects ();

		char[] tagSplitters = { ',' };
		string[] lines = acceptedTagsTextAsset.text.Split (lineSplitters);
		acceptedTags = new AcceptedTags[lines.Length];

		for (int i = 0; i < lines.Length; i++) {
			string[] tags = lines [i].Split (tagSplitters);
			acceptedTags [i] = new AcceptedTags (tags [0], tags);
		}
			
	}

	/// <summary>
	/// Picks a new set of 3 objects to give to the player and displays them in the main menu panel.
	/// </summary>
	public void pickCurrentObjects(){

		int[] usedIndexs = new int[currentObjects.Length];
		for (int i = 0; i < currentObjects.Length; i++) {
			int index = -1;

			// get a unique index
			while(index == -1 || inArray(usedIndexs, index, i)){
				index = Random.Range (0, objects.Length);
			}

			currentObjects [i].text = objects [index];
			usedIndexs [i] = index;
		}
	}

	/// <summary>
	/// Checks if the number was found in the array up to a given index.
	/// </summary>
	/// <returns><c>true</c>, If number was found in array, <c>false</c> otherwise.</returns>
	/// <param name="array">The int array to look through</param>
	/// <param name="num">The number to make sure that is not already present in the array.</param>
	/// <param name="maxIndex">The maximum index to look up to in the array (exclusive)</param>
	public bool inArray(int[] array, int num, int maxIndex){
		for (int i = 0; i < maxIndex; i++) {
			if (array [i] == num) {
				return true;
			}
		}

		return false;
	}

	/// <summary>
	/// Returs the current Text objects so that you can check the current list of objects the player is tasked to find.
	/// </summary>
	/// <returns>The current text objects the player is searching for.</returns>
	public Text[] getCurrentObjects(){
		return currentObjects;
	}

	public AcceptedTags[] getAcceptedTags(){
		return acceptedTags;
	}
}

public class AcceptedTags {

	private string _acceptedTags;
	private string[] _similarTags;

	public AcceptedTags(string acceptedTags, string[] similarTags){
		_acceptedTags = acceptedTags;
		_similarTags = similarTags;
	}

	public string getAcceptedTag(){
		return _acceptedTags;
	}

	public string[] getSimilarTags(){
		return _similarTags;
	}
}
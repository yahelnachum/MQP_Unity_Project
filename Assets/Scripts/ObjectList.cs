using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO; 
using UnityEngine.UI;

public class ObjectList{

	private static ObjectList instance = new ObjectList();

	private string[] objects;
	private AcceptedTags[] acceptedTags;
	private List<Text>[] currentObjects = new List<Text>[3];
	private int[] usedIndexs;
	private int counter;
	private int subCount;

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

		counter = 0;
		subCount = 0;

		// get array of available objects
		char[] lineSplitters = { '\n', '\r' };
		objects = objectListTextAsset.text.Split (lineSplitters, System.StringSplitOptions.RemoveEmptyEntries);

		usedIndexs = new int[objects.Length];


            //initialize to 0, reperesenting nothing has been used yet
            for (int j = 0; j < usedIndexs.Length; j++)
            {
                usedIndexs[j] = 0;
            }

		// get text objects from game
		for (int i = 0; i < currentObjects.Length; i++) {
			List<GameObject> text = StartGame.findInactive ("tObject" + i, "vMenu");
			List<Text> texts = new List<Text> ();
			for(int j = 0; j < text.Count; j++){
				texts.Add (text [j].GetComponent<Text> ());
			}
			currentObjects[i] = texts;
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

    public void prepareScore()
    {
        long coins = PlayerData.getInstance().getMonies();

        string oldRank = EnumRank.getRankFromCoins(coins).name;

		Text tRank  = StartGame.findInactive("tRank_main","vMenu")[0] .GetComponent<Text>(),
		tCoins = StartGame.findInactive("tCoins_main","vMenu")[0].GetComponent<Text>();

        tRank.text = string.Concat("Rank:\n", oldRank);
        
        tCoins.text = string.Concat("$", coins.ToString());
        
    }

    public static void pickCurrentObjectsStatic()
    {
        ObjectList.getInstance().pickCurrentObjects();
    }

	/// <summary>
	/// Picks a new set of 3 objects to give to the player and displays them in the main menu panel.
	/// </summary>
	/// 
	/// Todo: get it so that the list resets immediately if exhausted
    /// Todo: update the rank and money
	public void pickCurrentObjects(){

        prepareScore();
		
		for (int i = 0; i < currentObjects.Length; i++) {

			if (subCount == 5) {
				Debug.Log ("reseting list");

				subCount = 0;
				counter = 0;
				for (int k = 0; k < usedIndexs.Length; k++) {
					usedIndexs [k] = 0;
				}
			}

			if (counter == usedIndexs.Length) {
 				//if we've exhausted the entire list, indicate as such
					Debug.Log ("object list exhausted");
					for (int j = 0; j < currentObjects [i].Count; j++) {
						currentObjects [i] [j].text = "no more unique objects";
					}
					subCount++;
					continue;

			}

			//get first random value
			int index = Random.Range (0, objects.Length);

			//if used before, keep searching for unused word
			while(usedBefore(usedIndexs, index)){
				index = Random.Range (0, objects.Length);
			}
				
			Debug.Log("object "+objects[index]);
			for (int j = 0; j < currentObjects [i].Count; j++) {
				currentObjects [i][j].text = objects [index];
			}
			usedIndexs [index] = 1;

			counter++;
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
	/// Checks if the randomly chosen index for a word has been used already.
	/// </summary>
	/// <returns><c>true</c>, If word was used already, <c>false</c> otherwise.</returns>
	/// <param name="array">The int array to look through</param>
	/// <param name="num">The Index to check against.</param>
	public bool usedBefore(int[] array, int num){
		
		if (array [num] == 1) {
			return true;
		} else {
			return false;
		}
	}

	/// <summary>
	/// Returs the current Text objects so that you can check the current list of objects the player is tasked to find.
	/// </summary>
	/// <returns>The current text objects the player is searching for.</returns>
	public List<Text>[] getCurrentObjects(){
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
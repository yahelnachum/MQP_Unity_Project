using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Rewards : MonoBehaviour {

	public AudioClip sound;
	private AudioSource source;

	private float callTime = 30f; // in seconds

	public void Play1 (){
		Debug.Log ("PLAYING");
		source.PlayOneShot(sound,1.0f);
	}


	/// <summary>
    /// Function used to set all values on the rewards page. I'm not quite sure
    /// where I should be getting these values from, so I'm sort of making them
    /// up right now. Also not sure how to get this function called.
    /// </summary>
    public static void PrepareRewards()
    {
        long coins  = PlayerData.getInstance().getMonies(),
             earned = 2500000L;

        string oldRank = EnumRank.getRankFromCoins(coins).name,
               newRank = EnumRank.getRankFromCoins(coins + earned).name;

		Text tRank       = StartGame.findInactive("tRank_rew", "vMenu")[0].GetComponent<Text>(),
			 tNewRank    = StartGame.findInactive("tNewRank_rew", "vMenu")[0].GetComponent<Text>(),
			 tCoins      = StartGame.findInactive("tCoins_rew", "vMenu")[0].GetComponent<Text>(),
			 tGetCoins   = StartGame.findInactive("tGetCoins_rew", "vMenu")[0].GetComponent<Text>(),
			 tTotalCoins = StartGame.findInactive("tTotalCoins_rew", "vMenu")[0].GetComponent<Text>();

        tRank.text    = string.Concat("Rank:\n", oldRank);
        tNewRank.text = string.Compare(oldRank, newRank) != 0 ? string.Concat("\u2193\n", newRank) : "";

        tCoins.text      = string.Concat("$", coins.ToString());
        tGetCoins.text   = string.Concat("+$", earned.ToString());
        tTotalCoins.text = string.Concat("$", PlayerData.getInstance().addToMonies(earned).ToString());

		setupCongrats ();
        // Add any SFX here.

    }

	public static void setupCongrats(){
		string foundObj = HttpRequestManager.getInstance ().getFoundObject ();
		Text tCongrats = StartGame.findInactive ("tHeaderCongrats", "vMenu") [0].GetComponent<Text> ();
		tCongrats.text = "Congratulations, you found "+getCorrectIndefiniteArticle(foundObj)+" "+foundObj+"!";
	}

	public static string getCorrectIndefiniteArticle(string obj){
		if (obj.Length > 0) {
			char[] vowels = { 'a', 'e', 'i', 'p', 'u' };
			for (int i = 0; i < vowels.Length; i++) {
				if (obj.ToCharArray () [0] == vowels [i])
					return "an";
			}

			return "a";
		}

		return "error";
	}

	void Start(){
		Debug.Log ("Initializing sound object for tuts");
		source = GetComponent<AudioSource>();
		//StartCoroutine (StartTutorialTimer ());
	}

    public void ProcessNextBit()
    {
        switch (PlayerData.getInstance().getCurrentNarrativeChunk())
        {
			case 0:
			case 1:
			case 2:
			case 4:
			case 5:
			case 7:
				Debug.Log ("Processing Next bit");
				PlayerData.getInstance ().incrementCurrentNarrativeChunk ();
				PlayerData.getInstance ().saveData ();
				SwitchPanels.changePanelStatic("pMain:activate,pRewardsStats:deactivate");
                ObjectList.pickCurrentObjectsStatic();
                break;
			case 6:
			case 8:
			case 9:
				SwitchPanels.changePanelStatic ("pSecureCall:activate,pRewardsStats:deactivate");
				//SecureCall.getInstance ().startTimer ();
                break;
            default:
                Debug.Log("Default case reached for rewards panel: this is not a good thing! (tm)");

                // Fail gracefully.
				PlayerData.getInstance ().saveData ();
                SwitchPanels.changePanelStatic("pMain:activate,pRewardsStats:deactivate");
                break;
        }
    }
}

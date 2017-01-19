using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System;

public class CheatsController : MonoBehaviour {

	public void deletePlayerData(){
		File.Delete (PlayerData.getInstance ().getFilePath ());
	}

	bool textSwitched = false;
	string originalButtonText = null;
	public void getCurrentNarrativeChunk(Text buttonText){
		if (textSwitched) {
			buttonText.text = originalButtonText;
			textSwitched = false;
		} else {
			originalButtonText = buttonText.text;
			buttonText.text = "Current Narrative Chunk: " + PlayerData.getInstance ().getCurrentNarrativeChunk ();
			textSwitched = true;
		}
	}

	//switch panels to update
	public void goUpdate(){
		SwitchPanels.changePanelStatic ("pUpdate:activate,pCheats:deactivate");
		UpdatePanel.startUpdate ();
	}

    public void goRewards()
    {
        SwitchPanels.changePanelStatic("pRewards:activate,pCheats:deactivate");
        Rewards.PrepareRewards();
    }

    public void testEnumRank()
    {
        for (long coins = 0; coins < 1000000000; coins += 25000000)
        {
            Debug.Log(coins.ToString() + EnumRank.getRankFromCoins(coins).name);
        }
    }

	public void setCurrentNarrativeChunk(Text asset){
		Debug.Log(asset.GetComponent<Text> ().text);
		PlayerData.getInstance ().setCurrentNarrativeChunk (Int32.Parse (asset.GetComponent<Text> ().text));
	}

}

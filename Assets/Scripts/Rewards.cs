﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Rewards : MonoBehaviour {

    /// <summary>
    /// Function used to set all values on the rewards page. I'm not quite sure
    /// where I should be getting these values from, so I'm sort of making them
    /// up right now. Also not sure how to get this function called.
    /// </summary>
    public void PrepareRewards()
    {
        int coins  = 50000000,
            earned = 2500000;

        string oldRank = "Blah",
               newRank = "BlahBlah";

        Text tRank       = GameObject.Find("tRank_rew")      .GetComponent<Text>(),
             tNewRank    = GameObject.Find("tNewRank_rew")   .GetComponent<Text>(),
             tCoins      = GameObject.Find("tCoins_rew")     .GetComponent<Text>(),
             tGetCoins   = GameObject.Find("tGetCoins_rew")  .GetComponent<Text>(),
             tTotalCoins = GameObject.Find("tTotalCoins_rew").GetComponent<Text>();

        tRank.text    = string.Concat("Rank:\n", oldRank);
        tNewRank.text = string.Compare(oldRank, newRank) == 0 ? string.Concat("\u2193\n", newRank) : "";

        tCoins.text      = string.Concat("$", coins.ToString());
        tGetCoins.text   = string.Concat("+$", earned.ToString());
        tTotalCoins.text = string.Concat("$", (coins + earned).ToString());

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
                SwitchPanels.changePanelStatic("pMain:activate,pRewards:deactivate");
                break;
            case 6:
            case 8:
            case 9:
                SwitchPanels.changePanelStatic("pSecureCall:activate,pRewards:deactivate");
                break;
            default:
                Debug.Log("Default case reached for rewards panel: this is not a good thing! (tm)");
                break;
        }
    }
}

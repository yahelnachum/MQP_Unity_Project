using UnityEngine;
using System.Collections;

public class Rewards : MonoBehaviour {

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

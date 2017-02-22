using UnityEngine;
using System.Collections;
using System;

public class ChatPanel : MonoBehaviour {

    public static string getMessage()
    {
        int number;
        string value;
        System.Random random = new System.Random();
        Func<string> generator = () =>
        {
            int rnum = random.Next(10);
            switch (rnum)
            {
                case 1: return "ha n00b im better than u";
                case 2: return "this game is the best";
                case 3: return "i luv this game";
                case 4: return "this is gr8";
                case 5: return "ur mom";
                case 6: return "gr8 b8 m8 i r8 8/8";
                case 7: return "im having so much fun";
                case 8: return "im at the louvre. where r u";
                case 9: return "@ the prada";
                case 10: return "i beat the round";
                default: return "";
            }
        };
        value = generator();
        number = random.Next(9999999);
        return string.Format("#%d: %s\n", number, value);
    }

    public string getMessageByInstance()
    {
        return ChatPanel.getMessage();
    }

    public void newMessage()
    {

    }

}

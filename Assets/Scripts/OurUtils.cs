using UnityEngine;
using System.Collections;

public class OurUtils {

    public static string Capitalize (string input) {
        return input[0].ToString().ToUpper() + input.Substring(1);
    }

    public static string CapitalizeEachWord(string input)
    {
        string[] words = input.Split(' '),
                 output = new string[words.Length];

        for ( int x = 0; x < words.Length; ++x )
        {
            output[x] = OurUtils.Capitalize(words[x]);
        }

        return string.Join(" ", output);
    }

}

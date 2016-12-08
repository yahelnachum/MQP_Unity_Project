using UnityEngine;
using System.Collections;

public class EnumRank
{

    public readonly string name;
    public readonly long minCoins;
    public readonly long maxCoins;

    private EnumRank(string name, long minCoins, long maxCoins)
    {
        this.name = name;
        this.minCoins = minCoins;
        this.maxCoins = maxCoins;
    }

    public static EnumRank[] Ranks = 
    {
        new EnumRank("Wimpy N00b",              0L,         0L),
        new EnumRank("Junior Seeker",           1L,         50000000L),
        new EnumRank("Mediocre Scavenger",      50000000L,  100000000L),
        new EnumRank("Assistant Investigator",  100000000L, 200000000L),
        new EnumRank("Rising Detective",        200000000L, long.MaxValue)
    };

    public static EnumRank getRankFromCoins(long numCoins)
    {
        for (int x = 0; x < EnumRank.Ranks.Length; ++x)
        {
            if (numCoins <= EnumRank.Ranks[x].maxCoins)
            {
                return EnumRank.Ranks[x];
            }
        }
        return null;
    }

}

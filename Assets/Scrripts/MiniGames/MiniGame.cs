using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame : MonoBehaviour
{
    public helpedWho RewardFrom;
    public ShopGiveReward shop;

    public void setShop()//called by miniGameManager On its start or awake
    {
        if (RewardFrom == helpedWho.Farmer)
        { shop = FindObjectOfType<FarmShop>(); }

        else if (RewardFrom == helpedWho.Blacksmith)
        { shop = FindObjectOfType<BlackSmithShop>(); }

        else if (RewardFrom == helpedWho.Sensei)
        { shop = FindObjectOfType<SenseiPanel>(); }
    }

    public virtual float CalculateScore()
    {
        Debug.LogError("called parent calculate score, doesn't do anything");
        return -1;
    }
}
public enum helpedWho { Farmer, Blacksmith, Sensei}
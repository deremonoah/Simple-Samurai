using UnityEngine;
using UnityEngine.Events;

public class ShopReward : Reward
{
    public virtual void ResolveReward()
    {
        //each reward will refrence a shop & then call a method for that shop
        //so they are sort of like method containers
    }
}

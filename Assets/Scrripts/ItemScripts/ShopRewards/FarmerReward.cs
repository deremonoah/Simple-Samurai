using UnityEngine;

[CreateAssetMenu(fileName = "New FarmReward", menuName = "Reward/FarmReward")]
public class FarmerReward : ShopReward
{
    public FarmRewardType reward;
    public override void ResolveReward()
    {
        if(reward==FarmRewardType.dicountHeal)
        {
            FindObjectOfType<FarmShop>().DiscountHeal();
        }
        else if(reward==FarmRewardType.discountMaxHP)
        {
            FindObjectOfType<FarmShop>().DiscountMaxHP();
        }
        else if (reward == FarmRewardType.discountAll)
        {
            FindObjectOfType<FarmShop>().DiscountAll();
        }
        else if (reward == FarmRewardType.improveFarm)
        {
            FindObjectOfType<FarmShop>().ImproveFarmReward();
        }
    }
}
public enum FarmRewardType { dicountHeal, discountMaxHP, discountAll, improveFarm }
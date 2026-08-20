using UnityEngine;

[CreateAssetMenu(fileName = "New BSmith reward", menuName = "Reward/BlackSmithRewrd")]
public class BlackSmithReward : ShopReward
{
    public BlackSmithRewarType reward;
    public int amount;
    public override void ResolveReward()
    {
        if (reward == BlackSmithRewarType.discountWeaponUp)
        {
            FindObjectOfType<BlackSmithShop>().DiscountWeapon(amount);
        }
        else if (reward == BlackSmithRewarType.discountArmorUp)
        {
            FindObjectOfType<BlackSmithShop>().DiscountArmor(amount);
        }
        else if (reward == BlackSmithRewarType.allDiscount)
        {
            FindObjectOfType<BlackSmithShop>().allDiscount();
        }
        else if (reward == BlackSmithRewarType.twoArmor)
        {
            FindObjectOfType<BlackSmithShop>().LearnedTwoArmor();
        }
    }
}
public enum BlackSmithRewarType { discountWeaponUp, discountArmorUp, allDiscount, twoArmor}
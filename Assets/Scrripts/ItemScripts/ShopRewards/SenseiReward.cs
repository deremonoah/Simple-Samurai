using UnityEngine;

[CreateAssetMenu(fileName = "New SenseiReward", menuName = "Reward/SenseiReward")]
public class SenseiReward : ShopReward
{
    public SenseiRewardType reward;
    public override void ResolveReward()
    {
        /*else if (reward == SenseiRewardType.twoWeaponWeilding)
        {
            FindObjectOfType<PlayerEquipedItemsManager>().UnlockTwoWeapons();
            FindObjectOfType<BuffAreaManager>().PlaceBuff(0);//places swap weapon
        } else*/
        if (reward == SenseiRewardType.enemySwap)
        {
            FindObjectOfType<BuffAreaManager>().PlaceBuff(1);//swap enemy
        }
        else if (reward == SenseiRewardType.twoStyles)
        {
            FindObjectOfType<StrikePoint>().UnlockedTwoStyleAbility();
        }
    }
}
public enum SenseiRewardType { newForm,twoWeaponWeilding,enemySwap, twoStyles}
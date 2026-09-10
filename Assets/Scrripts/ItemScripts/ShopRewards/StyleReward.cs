using UnityEngine;

[CreateAssetMenu(fileName = "New StyleReward", menuName = "Reward/StyleReward")]
public class StyleReward : ShopReward
{
    public StyleID styleToLearn;

    public override void ResolveReward()
    {
        FindObjectOfType<StrikePoint>().EquipPrimaryStyle((int)styleToLearn);
        FindObjectOfType<SenseiPanel>().newStyles(styleToLearn);
    }
}

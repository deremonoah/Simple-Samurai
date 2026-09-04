using UnityEngine;

[CreateAssetMenu(fileName = "New StyleReward", menuName = "Reward/StyleReward")]
public class StyleReward : ShopReward
{
    public StyleID styleToLearn;

    public override void ResolveReward()
    {
        FindObjectOfType<SenseiPanel>().newStyles(styleToLearn);
    }
}

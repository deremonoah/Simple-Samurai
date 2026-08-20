using UnityEngine;

public class Reward : ScriptableObject
{
    public string Name;
    public Sprite PanelIcon;
    [TextArea(3, 10)]
    public string Description;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class Event : ScriptableObject
{
    [TextArea(3,10)]
    public List<string> textStatements;
    //0 yes, 1 no if its a yes or no obviously
    public List<string> buttonOptions;
    public EventEffect myeventEffect;
    public Sprite eventImage;

}
public enum EventEffect { blackSmith, moreVillagers, defenses, invest, leader, missingFamily, damagedCity, urHurt, joinBandits,sensieUnlock,unlockDefense}
//like how the wild dog goes out with the leader guy to save a family who's house is burning down

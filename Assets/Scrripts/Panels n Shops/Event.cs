using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class Event : ScriptableObject
{
    public List<string> textStatements;
    //0 yes, 1 no if its a yes or no obviously
    public List<string> buttonOptions;
    public EventEffect myeventEffect;
}
public enum EventEffect { blackSmith, moreVillagers, defenses, invest, leader, missingFamily, damagedCity}
//like how the wild dog goes out with the leader guy to save a family who's house is burning down

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Event", menuName = "Event")]
public class Event : ScriptableObject
{
    public List<string> textStatements;
    //0 yes, 1 no if its a yes or no obviously
    public List<string> buttonOptions;
    [SerializeField] EventEffect myeventEffect;
}
public enum EventEffect { blackSmith, moreVillagers, defenses}

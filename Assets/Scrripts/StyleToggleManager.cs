using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StyleToggleManager : MonoBehaviour
{
    [SerializeField] int HowManyEnabledAtOnce = 1;
    [SerializeField] int StopTrackingInputAfter = 2;
    private List<Toggle> allMyToggles;
    private List<Toggle> lastTogglePressed;

    //get all my toggles on awake
    //pretty sture subscribe to on value change for each of them

}

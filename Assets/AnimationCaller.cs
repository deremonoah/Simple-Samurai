using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationCaller : MonoBehaviour
{
    public enemy myscript;

    public void SendAttack()
    {
        myscript.AttackUI();
    }
}

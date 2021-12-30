using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicEnmAnim : MonoBehaviour
{
    private Animator anim;
    [SerializeField] float State;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        anim.SetFloat("enmState", State);
    }

    public void setState(float state)
    {
        State = state;
    }

}

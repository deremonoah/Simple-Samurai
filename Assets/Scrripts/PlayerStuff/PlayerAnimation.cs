using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    public float atkTimer;
    public float atkTimerMax;
    [SerializeField] float playerstate;

    [SerializeField] float readyingTimer;
    [SerializeField] float readyingTimerMax;
    [SerializeField] bool ready=false;
    [SerializeField] bool postatk = false;
    // 0 = idle, 1 = readying, 2 = ready, 3 = strike
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        //if (StrikeArea.PlayerOn)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                readyingTimer = readyingTimerMax;
                ready = true;
                playerstate = 0;
            }
            if (readyingTimer > 0)
            {
                readyingTimer -= Time.deltaTime;
                playerstate = 1f;
            }

            if (ready && readyingTimer < 0)
            {
                playerstate = 2f;
                ready = false;
            }

            if (Input.GetKeyUp(KeyCode.Space))
            {
                atkTimer = atkTimerMax;
                postatk = true;
            }

            if (atkTimer > 0)
            {
                atkTimer -= Time.deltaTime;
                playerstate = 3f;
            }

            if (postatk && atkTimer < 0)
            {
                playerstate = 0;
                postatk = false;
            }

            
        }
        anim.SetFloat("playerState", playerstate);
    }
}

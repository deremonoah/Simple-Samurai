using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnsHammerGame : MonoBehaviour
{
    [Header("Player Hammer Stuff")]
    [SerializeField] Transform PlayerHammer;
    [SerializeField] Vector3 phRotationBottom;
    [SerializeField] Vector3 phRotationTop;
    [SerializeField] float SwingSpeed=5f;
    [SerializeField] float durationOfSwing;
    private Quaternion endRotation;
    private Quaternion StartRotation;
    private float lerpTimer;
    private float percentSwung;
    private float power;
    private bool backSwing;
    private bool justHit;

    [Header("Smith Hammer stuff")]
    [SerializeField] Transform SmithHammer;
    [SerializeField] Vector3 shRotationBottom;
    [SerializeField] Vector3 shRotationTop;

    [Header("Working Metal")]
    [SerializeField] Transform WorkingMetal;
    [SerializeField] SpriteRenderer metalForColor;

    // Update is called once per frame
    void Update()
    {
        //maybe get key down to see where player started in the rotation to indicate force to apply to the metal
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            //get current rotation of z, and get the difference from the bottom.z
            StartRotation = PlayerHammer.rotation;
            Vector3 startVec3 = PlayerHammer.rotation.eulerAngles;
            lerpTimer = 0;
            power = Mathf.Abs(phRotationBottom.z - startVec3.z);
            Debug.Log(power);
            durationOfSwing = power / SwingSpeed;
            endRotation= Quaternion.Euler(phRotationBottom);
            justHit = false;
            backSwing = false;
        }
        if(Input.GetKey(KeyCode.Space)||Input.GetKey(KeyCode.Mouse0))
        {
            //holding input
            lerpTimer += Time.deltaTime;
            percentSwung = lerpTimer / durationOfSwing;
            PlayerHammer.rotation = Quaternion.Slerp(StartRotation, endRotation,percentSwung);

            //check if hit the end yet
            if (Quaternion.Angle(PlayerHammer.rotation,Quaternion.Euler(phRotationBottom))<0.5 &&!justHit)//euler angles gives not the same as the vector 3 in inspector
            {
                justHit = true;
                shrinkage(power);
            }
        }
        else if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            //released input
            backSwing = true;
            lerpTimer = 0;
            Vector3 curHam = PlayerHammer.rotation.eulerAngles;
            StartRotation = Quaternion.Euler(curHam);
            endRotation = Quaternion.Euler(phRotationTop);
            durationOfSwing = Mathf.Abs( phRotationTop.z- curHam.z) / SwingSpeed;
        }

        //auto moving back up ifs
        if(backSwing)
        {
            lerpTimer += Time.deltaTime;
            percentSwung = lerpTimer / durationOfSwing;
            PlayerHammer.rotation = Quaternion.Slerp(StartRotation, endRotation, percentSwung);
        }
        if(transform.rotation.z>=phRotationTop.z)
        {
            backSwing = false;
        }
        //timer or maybe coroutine instead for the blacksmiths hammer, he should probably call out his swings

    }

    private void shrinkage(float amount)
    {
        Vector3 size = WorkingMetal.localScale;
        if(amount<40)
        {
            amount = amount / 3;
        }
        if(amount<80)
        {
            amount = amount / 2;
        }
        WorkingMetal.localScale = new Vector3(size.x, size.y - .001f*amount, size.z);

        if(WorkingMetal.localScale.y<0.3f)
        {
            Debug.Log("below 25%"); //maybe a target size instead, so there is a point to use little taps

            //tell it to move it off screen then restore it prob with a coroutine
            //prob need a variable for onAnvil so player can't cheese it off screen
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TurnsHammerGame : MonoBehaviour
{
    [Header("Player Hammer Stuff")]
    [SerializeField] Transform PlayerHammer;
    [SerializeField] Vector3 phRotationBottom;
    [SerializeField] Vector3 phRotationTop;
    [SerializeField] float SwingSpeed;
    [SerializeField] float MaxLiftUpDuration;
    private float durationOfSwing;
    private Quaternion endRotation;
    private Quaternion StartRotation;
    private float lerpTimer;
    private float percentSwung;
    private float power;
    private bool autoSwing;
    private bool justHit;

    [Header("Smith Hammer stuff")]
    [SerializeField] Transform SmithHammer;
    [SerializeField] Vector3 shRotationBottom;
    [SerializeField] Vector3 shRotationTop;

    [Header("Working Metal")]
    [SerializeField] Transform WorkingMetal;
    [SerializeField] SpriteRenderer metalForColor;
    [SerializeField] Color StartYellow;
    [SerializeField] Color EndRed;
    [SerializeField] float DurationToCool;
    private Coroutine colorRoutine;
    //then maybe grey?

    [Header("Specifications")]
    [SerializeField] Transform specificationToChange;//controles the max size the player needs
    [SerializeField] float MoveSpeedForNewPiece=5;
    //sprite needs to be the same as the metal, or the same size in order to scale and compare transforms correctly
    
    [Header("numbers for scoring")]
    [SerializeField] List<float> colorScores;
    [SerializeField] List<float> sizeScores;
    private float  colorProgress;


    private Coroutine PlayerInactiveRoutine;
    private Coroutine otherRoutine;//for don't mess it up RN routine
    private bool firstRun=true;//so it doesn't score size bandaid solutions work man

    private void OnEnable()
    {
        PlayerInactiveRoutine=StartCoroutine(GetNewMetalPieceRoutine());//so new metal comes in
    }

    // Update is called once per frame
    void Update()
    {
        //maybe get key down to see where player started in the rotation to indicate force to apply to the metal
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
        {
            //get current rotation of z, and get the difference from the bottom.z
            StartRotation = PlayerHammer.rotation;
            lerpTimer = 0;
            
            durationOfSwing = MaxLiftUpDuration;
            endRotation= Quaternion.Euler(phRotationTop);
            autoSwing = false;
        }
        else if(Input.GetKey(KeyCode.Space)||Input.GetKey(KeyCode.Mouse0))
        {
            //holding input
            lerpTimer += Time.deltaTime;
            percentSwung = lerpTimer / durationOfSwing;
            PlayerHammer.rotation = Quaternion.Slerp(StartRotation, endRotation,percentSwung);
            
        }
        else if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            //released input
            autoSwing = true;
            justHit = false;
            lerpTimer = 0;
            Vector3 curHam = PlayerHammer.rotation.eulerAngles;

            power = Mathf.Abs(phRotationBottom.z - curHam.z);

            StartRotation = Quaternion.Euler(curHam);
            endRotation = Quaternion.Euler(phRotationBottom);
            durationOfSwing = Mathf.Abs( phRotationBottom.z- curHam.z) / SwingSpeed;
        }

        //auto moving back up ifs
        if(autoSwing)
        {
            lerpTimer += Time.deltaTime;
            percentSwung = lerpTimer / durationOfSwing;
            PlayerHammer.rotation = Quaternion.Slerp(StartRotation, endRotation, percentSwung);

            //check if hit the end yet
            if (Quaternion.Angle(PlayerHammer.rotation, Quaternion.Euler(phRotationBottom)) < 0.5 && !justHit && PlayerInactiveRoutine == null)//euler angles gives not the same as the vector 3 in inspector
            {
                justHit = true;
                shrinkage(power);
            }
        }
        if(transform.rotation.z>=phRotationTop.z)
        {
            autoSwing = false;
        }
        //timer or maybe coroutine instead for the blacksmiths hammer, he should probably call out his swings
        
    }

    private void shrinkage(float amount)
    {
        Vector3 size = WorkingMetal.localScale;
        /*if(amount<40)
        {
            amount = amount / 2;
        }*/

        if(amount>100)
        {
            amount = amount * 2.5f;
        }

        float ychange = Mathf.Clamp(size.y - .001f * amount, 0.02f, 1.1f);//1.1 cause exclcusive but should never actually matter
        WorkingMetal.localScale = new Vector3(size.x, ychange, size.z);

        if(WorkingMetal.localScale.y<=specificationToChange.localScale.y && otherRoutine==null)//check if other routine cause might start 2 routine sets
        {
            otherRoutine=StartCoroutine(WaitDontMessItUpRoutine());
            //maybe not right away but with enough time for you to mess it up more
        }
    }

    private IEnumerator WaitDontMessItUpRoutine()
    {
        yield return new WaitForSeconds(1f);
        PlayerInactiveRoutine = StartCoroutine(GetNewMetalPieceRoutine());
        float colorScore = (float)Math.Round((.75f -colorProgress) * 100,2);
        colorScores.Add(colorScore);

        otherRoutine = null;
    }



    private IEnumerator GetNewMetalPieceRoutine()
    {
        float timeElapsed = 0f;
        Vector2 startPos = WorkingMetal.position;
        Vector2 targetPos = new Vector2(14, startPos.y);
        float durationOut = Mathf.Abs(startPos.x - 14) / MoveSpeedForNewPiece;

        while(WorkingMetal.position.x<14)
        {
            float t = timeElapsed / durationOut;

            WorkingMetal.position = Vector2.Lerp(startPos, targetPos, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }
        if(!firstRun)//this is to get around the initial start scoring for size
        {
            float newScore = 100 - ((float)Math.Round(specificationToChange.localScale.y - WorkingMetal.localScale.y, 2) * 100);
            sizeScores.Add(newScore);
        }
        else { firstRun = false; }
        

        //can change specifications
        RandomSpecifictation();
        WorkingMetal.localScale = new Vector3(1, 1, 1);
        colorRoutine = StartCoroutine(ColorChangeRoutine());

        //move working metal back out
        timeElapsed = 0f;
        startPos = WorkingMetal.position;
        targetPos = new Vector2(0, startPos.y);
        durationOut = Mathf.Abs(startPos.x - 0) / MoveSpeedForNewPiece;

        while (WorkingMetal.position.x > 0)
        {
            float t = timeElapsed / durationOut;

            WorkingMetal.position = Vector2.Lerp(startPos, targetPos, t);

            timeElapsed += Time.deltaTime;

            yield return null;
        }

        PlayerInactiveRoutine = null;//set to null so that the player can hit it with the hammer
        //and pretty sure its good practice from Eli
        colorRoutine = StartCoroutine(ColorChangeRoutine());
    }

    private void RandomSpecifictation()
    {
        float newSpec = UnityEngine.Random.Range(0.15f, 0.51f);
        //could be like 3 set sizes if this seems weird
        specificationToChange.localScale = new Vector3(specificationToChange.localScale.x, newSpec, 1);
        //maybe lerp it? eh for now its fine
    }

    private IEnumerator ColorChangeRoutine()//based on metal cooling
    {
        float elapsedTime=0f;
        
        while(elapsedTime<DurationToCool)
        {
            colorProgress = elapsedTime / DurationToCool;

            metalForColor.color = Color.Lerp(StartYellow, EndRed, colorProgress);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        colorRoutine = null;
    }
}

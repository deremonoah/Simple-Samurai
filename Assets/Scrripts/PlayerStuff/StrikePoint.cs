using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class StrikePoint : MonoBehaviour
{

    private Rigidbody2D rb;

    public GameObject startpoint;

    public PathCreator currentPath;
    private PathCreator equipedPath;
    public PathCreator endPath;
    
    public float baseSpeed;
    public float bonusSpeed;
    private float currentSpeed;
    [SerializeField] float endSpeed;
    [SerializeField] float distanceTravelled;

    [SerializeField] float frequency;
    [SerializeField] float magnitude;

    [SerializeField] float inbetweenTimerMax;
    [SerializeField] float inbetweenTimer;
    [SerializeField] bool inbetween;

    bool faceingRight = true;

    private Vector3 pos, localScale;
    [SerializeField] float PathTimer, InbetweenTimer;

    [SerializeField] GameObject TopBound;
    [SerializeField] GameObject BottomBound;
    [SerializeField] GameObject NormalEndBound;
    [SerializeField] GameObject SmallerEndBound;
    private GameObject CurrentEndBound;


    private bool _hasTransitionedPath;
    private Vector3 endPathPosition;
    public float mostRecentX;

    //confused style stuff
    private float oldspeed;
    private float StyleconfusedTimer;
    private bool Styleconfused;
    //could also have a stun timer that makes them unable to move pointer for a bit by disabling player for a set time


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos = transform.position;

        localScale = transform.localScale;

        CurrentEndBound = NormalEndBound;
        inbetweenTimer = inbetweenTimerMax;

        StyleconfusedTimer = 0f;
        Styleconfused = false;
    }

    void Update()
    {
        


        checkWhereToFace();

        //inbetween is to check if the player should be in between swings because of the cool down timer
        //this makes me think maybe a fun alternative mode is Hyper Samurai where all the attacks are faster maybe bigger and you pointer moves insanley fast

        if (StrikeArea.PlayerOn)
        {
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && !inbetween)
            {
                PathTimer = 0;
            }
            if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0)) && !inbetween)
            {
                PathTimer += Time.deltaTime;
                InbetweenTimer += Time.deltaTime;
                if (faceingRight)
                {
                    currentSpeed = baseSpeed + bonusSpeed;
                    moveRight();
                }
                else if (!faceingRight)
                {
                    if (!_hasTransitionedPath)
                    {
                        currentSpeed = endSpeed;
                        distanceTravelled = 0;
                        _hasTransitionedPath = true;
                    }
                    moveUPandDown();
                }
                pos = transform.position;
                mostRecentX = transform.localPosition.x;
            }
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                InbetweenTimer = 0;
            }
            //reset to start pos here
            if (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
            {
                //mostRecentX = transform.position.x;
                PointerReturnToStart();
            }
        }

        if (inbetween)
        {
            if (inbetweenTimer <= 0)
            {
                inbetween = false;
                inbetweenTimer = inbetweenTimerMax;
            }
            else { inbetweenTimer -= Time.deltaTime; }
        }

        //new confusion attack from sensie makes your speed vary from slow to really fast
        //when the player becomes confused the timer will be set to above that
        if (StyleconfusedTimer >= 0)
        {

            //negative is interesting but it might be too random and when it just slows down over the strike area you just let go and are fine
            //maybe i should try 2 random numbers it chooses between one like below and the other more regular or weighted some how idk

            StyleconfusedTimer -= Time.deltaTime;
        }else if(Styleconfused && StyleconfusedTimer <=0)
        {
            //this is so we don't keep setting base speed in above or changing
            Styleconfused = false;
            baseSpeed = oldspeed;
        }

    }

    public void PointerReturnToStart()
    {
        pos = startpoint.transform.position;
        rb.transform.position = startpoint.transform.position;
        distanceTravelled = 0;
        inbetween = true;
        inbetweenTimer = inbetweenTimerMax;
        _hasTransitionedPath = false;
        PathTimer = 0;
    }

    void checkWhereToFace()
    {
        //changing the value in the if below changes the distance it will travel over all
        if (pos.x < -2f)
        { faceingRight = true; }
        else if (pos.x > CurrentEndBound.transform.position.x)
        { faceingRight = false; }

        /*if (((faceingRight)&&(localScale.x < 0)) || ((!faceingRight)&&(localScale.x >0)))
        {
            localScale.x *= -1;
        }*/

        //transform.localScale = localScale;
    }

    void moveRight()
    {
        //old sign wave way
        //pos += transform.right * Time.deltaTime ;
        //transform.position = pos + transform.up * Mathf.Sin(InbetweenTimer * (frequency )) * magnitude;
        distanceTravelled += currentSpeed * Time.deltaTime;
        transform.position = currentPath.path.GetPointAtDistance(distanceTravelled);
    }

    void moveUPandDown()
    {
        distanceTravelled += currentSpeed * Time.deltaTime;
        var direction = currentPath.name == "Cresent Moon" ? distanceTravelled * -1 : distanceTravelled;
        transform.position = endPath.path.GetPointAtDistance(direction);
    }

    public void ChangeStrikeSprite(Sprite spt)
    {
        GetComponent<SpriteRenderer>().sprite = spt;
        var colld = GetComponent<PolygonCollider2D>();
        DestroyImmediate(colld);
        colld = gameObject.AddComponent<PolygonCollider2D>();
        colld.isTrigger = true;
    }
    public void SetBoundsSmaller()
    {
        CurrentEndBound = SmallerEndBound;
    }
    public void SetBoundsRegular()
    {
        CurrentEndBound = NormalEndBound;
    }

    public void ChangeStyle(PathCreator tempPath)
    {
        currentPath = tempPath;
        //equipedPath = tempPath;
        if (tempPath.name == "Simple Style")
        {
            baseSpeed = 7.5f;
            //was 5 am testing other numbers
        }
        else if (tempPath.name == "Mountain Path")
        {
            baseSpeed = 6f;
        }
        else if (tempPath.name == "Rushing Boar")
        {
            baseSpeed = 4f;
        }
        else if(tempPath.name == "Serpent Strike")
        {
            baseSpeed = 4.8f;
        }
        else
        {
            baseSpeed = 4.5f;
        }

    }
    

    public void ConfuseStyle(float timer)
    {
        oldspeed = baseSpeed;
        StyleconfusedTimer = timer;
    }
}

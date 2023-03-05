using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class StrikePoint : MonoBehaviour
{

    private Rigidbody2D rb;

    public GameObject startpoint;

    public PathCreator currentPath;
    public PathCreator endPath;
    public float baseSpeed;
    public float bonusSpeed;
    private float currentSpeed;
    [SerializeField] float endSpeed;
    [SerializeField] float distanceTravelled;

    [SerializeField] float frequency;
    [SerializeField] float magnitude;

    

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
    public bool pressing = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos = transform.position;

        localScale = transform.localScale;

        CurrentEndBound = NormalEndBound;
    }

    void Update()
    {

        checkWhereToFace();



        if (StrikeArea.PlayerOn)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0))
            {
                PathTimer = 0;
            }
            if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.Mouse0))
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
            if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Mouse0)) && !pressing)
            {
                InbetweenTimer = 0;
                pressing = true;
            }
            //reset to start pos here
            if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && pressing)
            {
                //mostRecentX = transform.position.x;
                PointerReturnToStart();
            }
        }

        
    }

    public void PointerReturnToStart()
    {
        pos = startpoint.transform.position;
        rb.transform.position = startpoint.transform.position;
        distanceTravelled = 0;
        pressing = false;
        _hasTransitionedPath = false;
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
        if (tempPath.name == "Simple Style")
        {
            baseSpeed = 6.5f;
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
            baseSpeed = 4f;
        }
        else
        {
            baseSpeed = 5f;
        }

    }
}

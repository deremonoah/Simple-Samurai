using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikePoint : MonoBehaviour
{
    
    private Rigidbody2D rb;
    
    public GameObject startpoint;

    
    [SerializeField] float frequency;
    [SerializeField] float magnitude;

    [SerializeField] float inbetweenTimerMax;
    [SerializeField] float inbetweenTimer;
    [SerializeField] bool inbetween;

    bool faceingRight = true;

    private Vector3 pos, localScale;
    [SerializeField] float Timer;

    [SerializeField] GameObject TopBound;
    [SerializeField] GameObject BottomBound;
    [SerializeField] GameObject NormalEndBound;
    [SerializeField] GameObject SmallerEndBound;
    private GameObject CurrentEndBound;
    [SerializeField] float upSpeed;

    public float mostRecentX;
    public bool pressing = false;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos = transform.position;

        localScale = transform.localScale;

        CurrentEndBound = NormalEndBound;
        inbetweenTimer = inbetweenTimerMax;
    }

    void Update()
    {
        
        checkWhereToFace();

        

        if (StrikeArea.PlayerOn)
        {
            if ((Input.GetKey(KeyCode.Space)||Input.GetKey(KeyCode.Mouse0) )&& !inbetween)
            {
                Timer += Time.deltaTime;
                if (faceingRight)
                {
                    moveRight();
                }
                else if(!faceingRight)
                {
                    moveUPandDown();
                }
                mostRecentX = transform.localPosition.x;
            }
            if ((Input.GetKeyDown(KeyCode.Space)|| Input.GetKeyDown(KeyCode.Mouse0))&& !pressing)
            {
                Timer = 0;
                pressing = true;
            }
            //reset to start pos here
            if ((Input.GetKeyUp(KeyCode.Space)|| Input.GetKeyUp(KeyCode.Mouse0))&& pressing)
            {
                //mostRecentX = transform.position.x;
                pos = startpoint.transform.position;
                rb.transform.position = startpoint.transform.position;
                inbetween = true;
                pressing = false;
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
        pos += transform.right * Time.deltaTime ;
        transform.position = pos + transform.up * Mathf.Sin(Timer * (frequency )) * magnitude;
    }

    void moveUPandDown()
    {
        /*if (transform.position.y == TopBound.transform.position.y)
        {
            uping = false;
        }
        if (transform.position.y == BottomBound.transform.position.y)
        {
            uping = true;
        }

        if (uping)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, TopBound.transform.position.y), upSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, BottomBound.transform.position.y), upSpeed * Time.deltaTime);
        }*/
        
        transform.position = new Vector3(transform.position.x,startpoint.transform.position.y,transform.position.z)+transform.up*Mathf.Sin(Timer*frequency)*magnitude;
        
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
}

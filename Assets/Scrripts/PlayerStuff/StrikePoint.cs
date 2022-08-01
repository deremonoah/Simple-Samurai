using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrikePoint : MonoBehaviour
{
    
    private Rigidbody2D rb;
    
    public GameObject startpoint;

    
    [SerializeField] float frequency;
    [SerializeField] float magnitude;
    
    

    bool faceingRight = true;

    private Vector3 pos, localScale;
    [SerializeField] float Timer;

    [SerializeField] GameObject TopBound;
    [SerializeField] GameObject BottomBound;
    [SerializeField] GameObject NormalEndBound;
    [SerializeField] GameObject SmallerEndBound;
    private GameObject CurrentEndBound;
    private bool uping = false;
    [SerializeField] float upSpeed;

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
            if (Input.GetKey(KeyCode.Space))
            {
                Timer += Time.deltaTime;
                if (faceingRight)
                {
                    moveRight();
                }
                else if(!faceingRight)
                {
                    moveLeft();
                }
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Timer = 0;
            }
            //reset to start pos here
            if (Input.GetKeyUp(KeyCode.Space))
            {
                
                pos = startpoint.transform.position;
                rb.transform.position = startpoint.transform.position;
                uping = false;
            }
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

    void moveLeft()
    {
        if (uping)
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, TopBound.transform.position.y), upSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, BottomBound.transform.position.y), upSpeed * Time.deltaTime);
        }
        if (transform.position.y == BottomBound.transform.position.y)
        {
            uping = true;
        }
        else if (transform.position.y == TopBound.transform.position.y)
        {
            uping = false;
        }
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

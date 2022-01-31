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
    


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pos = transform.position;

        localScale = transform.localScale;
    }

    void Update()
    {
        
        checkWhereToFace();

        

        if (Input.GetKey(KeyCode.Space))
        {
            Timer += Time.deltaTime;
            if (faceingRight)
            {
                moveRight();
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
        }

        



    }

    void checkWhereToFace()
    {
        //changing the value in the if below changes the distance it will travel over all
        if (pos.x < -2f)
        { faceingRight = true; }
        else if (pos.x > 0f)
        { faceingRight = false; }

        if (((faceingRight)&&(localScale.x < 0)) || ((!faceingRight)&&(localScale.x >0)))
        {
            localScale.x *= -1;
        }
        transform.localScale = localScale;
    }

    void moveRight()
    {
        pos += transform.right * Time.deltaTime ;
        transform.position = pos + transform.up * Mathf.Sin(Timer * (frequency )) * magnitude;
    }

    void moveLeft()
    {
        //pos -= transform.right * Time.deltaTime * moveSpeed;
        
    }

    public void ChangeStrikeSprite(Sprite spt)
    {
        GetComponent<SpriteRenderer>().sprite = spt;
        var colld = GetComponent<PolygonCollider2D>();
        DestroyImmediate(colld);
        colld = gameObject.AddComponent<PolygonCollider2D>();
        colld.isTrigger = true;
    }
   
}

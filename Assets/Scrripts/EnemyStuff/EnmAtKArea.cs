using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmAtKArea : MonoBehaviour
{

    private bool blocking;
    private enmy myenm;
    private Transform endPos;
    [SerializeField] Vector2 dir;
    [SerializeField] float movspd;
    private float multiPerry = 0;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space)&& blocking)
        {
            if (multiPerry >0)
            {
                multiPerry -= 1;
                GetComponent<SpriteRenderer>().color = Color.red;
                //move the ui back a bit to give player more time
            }
            else
            {
                myenm.Blocked();
                Destroy(gameObject);
            }
        }
        transform.Translate(dir * movspd *Time.deltaTime);
        if (this.transform.position.x < endPos.transform.position.x)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.name=="strike point")
        {
            blocking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        blocking = false;
    }

    public void Setstuff(enmy em, Transform end)
    {
        myenm = em;

        endPos = end;

        /*if (myenm.myAbility == enmy.Ability.antiarmor)
        {
            multiPerry = 1;
        }*/ 
        //could use the multi perry thing for a boss or different ability instead
    }
}

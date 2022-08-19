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
        if ((Input.GetKeyUp(KeyCode.Space)||Input.GetKeyUp(KeyCode.Mouse0))&& blocking)
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
        if ((this.transform.position.x < endPos.transform.position.x && dir.x<1) || (transform.position.y < endPos.transform.position.y && dir.x < 1))
        {
            myenm.hitNow();
            Destroy(gameObject);
        } else if ((this.transform.position.x > endPos.transform.position.x && dir.x == 1) || (this.transform.position.y < endPos.transform.position.y && dir.x == 1))
        {

            if (myenm.myAbility == enmy.Ability.steal)
            {
                myenm.IRan();
            }else if (myenm.myAbility == enmy.Ability.heal)
            {
                myenm.healAllyNow();
                //this is spiecial reverse moves
            }
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

    public void Setstuff(enmy em, Transform end, Vector2 direct)
    {
        myenm = em;
        dir = direct;
        endPos = end;

        /*if (myenm.myAbility == enmy.Ability.antiarmor)
        {
            multiPerry = 1;
        }*/ 
        //could use the multi perry thing for a boss or different ability instead
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SasumataUIScript : MonoBehaviour
{
    private enemy myenm;
    private Transform endPos;
    [SerializeField] Vector2 dir;
    [SerializeField] float movspd;
    private bool hasPointer;
    void Start()
    {
        hasPointer = false;
    }

    
    void Update()
    {
        transform.Translate(dir * movspd * Time.deltaTime);

        if (hasPointer)
        {
            FindObjectOfType<StrikePoint>().gameObject.transform.position = this.gameObject.transform.position;
            movspd = 3.5f;
        }

        if ((this.transform.position.x < endPos.transform.position.x && dir.x < 1) || (transform.position.y < endPos.transform.position.y && dir.x < 1))
        {
            StrikeArea.PlayerOn = true;
            FindObjectOfType<StrikePoint>().PointerReturnToStart();
            Destroy(gameObject);
        }
    }

    public void Setstuff(enemy em, Transform end, Vector2 direct)
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "strike point")
        {
            //now we want to move the pointer to the center of this
            hasPointer = true;
            StrikeArea.PlayerOn = false;
        }
    }
}

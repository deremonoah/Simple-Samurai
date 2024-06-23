using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class ArkingAtk : EnmAtKArea
{
    [SerializeField] float PathTimer;
    private PathCreator currentPath;
    private float distanceTravelled;
    
    
    
    private void Start()
    {
        //this is where it should get or set it's path

        currentPath = FindObjectOfType<EnemysManager>().GetRandomThrowPath();
    }


    protected override void Update()
    {
        //for blocking
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && blocking)
        {
            if (multiPerry > 0)
            {
                multiPerry -= 1;
                GetComponent<SpriteRenderer>().color = Color.red;

                var pos = this.gameObject.transform.position;
                this.gameObject.transform.position = new Vector2(pos.x + 1.5f, pos.y);

                damage = damage * .7f;
                var temp = this.gameObject.GetComponent<SpriteRenderer>().color;
                float alph = .6f;
                this.gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g + .2f, temp.b + .2f, alph);
            }
            else
            {
                myenm.Blocked(atkEef);
                FindObjectOfType<SoundManager>().PlaySound("block");
                Destroy(gameObject);
            }
        }

        //moves
        distanceTravelled += base.movespeed * Time.deltaTime;
        transform.position = currentPath.path.GetPointAtDistance(distanceTravelled);

        //once the attack is past designated area it is destoryed
        if ((this.transform.position.x < endPos.transform.position.x && dir.x < 1) || (transform.position.y < endPos.transform.position.y && dir.x < 1))
        {
            myenm.hitNow(damage, atkEef);
            Destroy(gameObject);
        }
        else if ((this.transform.position.x > endPos.transform.position.x && dir.x == 1) || (this.transform.position.y < endPos.transform.position.y && dir.x == 1))
        {

            if (myenm.myAbilities[0] == enemy.Ability.steal)
            {
                myenm.IRan();
            }
            else if (myenm.myAbilities[0] == enemy.Ability.heal)
            {
                myenm.GetComponent<Healer>().healAllyNow();
                //this is spiecial reverse moves
            }
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "strike point")
        {
            blocking = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        blocking = false;
    }
}

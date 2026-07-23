using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attack : MonoBehaviour
{
    private Transform endPos;
    private enemy myenm;
    bool blocking;

    public Vector2 dir;//could make this multiple and have the guy who spawned it look at which direction it should start from
    public float movespeed;
    public float multiPerry;
    public float damage;
    public AttackEffect atkEef;
    protected Vector2 posBlock;

    //put here how we will indicate damage, with the mask in future

    void Start()
    {
        endPos = EnemysManager.instance.getEndAttackPos();
    }

    protected virtual void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0)) && blocking)
        {
            if (multiPerry > 0)
            {
                multiPerry -= 1;
                ParticleManager.instance.BlockedHere(posBlock, damage);
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
                myenm.GetComponent<EnemyBehavior>().Blocked();
                FindObjectOfType<SoundManager>().PlaySound("block");
                ParticleManager.instance.BlockedHere(posBlock, damage);
                Destroy(gameObject);
            }
        }

        //movement
        transform.Translate(dir * movespeed * Time.deltaTime);

        if ((this.transform.position.x < endPos.transform.position.x && dir.x < 1) || (transform.position.y < endPos.transform.position.y && dir.x < 1))
        {
            myenm.hitNow(damage, atkEef);
            //particle effect for hitting player
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
            posBlock = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        blocking = false;
    }

    public void Setstuff(enemy em, Vector2 direct)
    {
        myenm = em;
        dir = new Vector2(-1,0);//for now we just need them to be flat
        GenerateDamage();
    }

    private void GenerateDamage()
    {
        var renderer = this.gameObject.GetComponent<SpriteRenderer>();
        var color = renderer.color;
        var Damgs = myenm.getRandomAttackDamage();
        float alph = Damgs[0] / Damgs[1];
        /*if (alph < .4f)
        {
            alph = 0.25f;
            renderer.color = new Color(color.r, color.g + .4f, color.b + .4f, alph);
        }
        else if (alph < .65f && alph >= .4f)
        {
            alph = .6f;
            renderer.color = new Color(color.r, color.g + .2f, color.b + .2f, alph);
        }
        else
        {
            alph = .8f;
            renderer.color = new Color(color.r, color.g, color.b, alph);
        }*/
        renderer.color = new Color(color.r, color.g, color.b, alph);

        damage = Damgs[0];


    }
}

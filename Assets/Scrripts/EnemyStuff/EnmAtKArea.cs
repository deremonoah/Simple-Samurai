using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmAtKArea : MonoBehaviour
{

    private bool blocking;
    public enemy myenm;
    public Transform endPos;
    public Vector2 dir;
    [SerializeField] float movspd;
    [SerializeField] float multiPerry;
    public float damage;
    private bool parried= false;
    private float parriedTimer;
    public AttackEffect atkEef;

    void Start()
    {
        
    }

    
    protected virtual void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Space)||Input.GetKeyUp(KeyCode.Mouse0))&& blocking)
        {
            if (multiPerry >0)
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

        //movement
        
        transform.Translate(dir * movspd * Time.deltaTime);

        /*else
        { transform.Translate(-dir * movspd * Time.deltaTime); }

        if (parriedTimer > 0)
        { parriedTimer -= Time.deltaTime; }
        else { parried = false; }*/

        if ((this.transform.position.x < endPos.transform.position.x && dir.x<1) || (transform.position.y < endPos.transform.position.y && dir.x < 1))
        {
            myenm.hitNow(damage,atkEef);
            Destroy(gameObject);
        }
        else if ((this.transform.position.x > endPos.transform.position.x && dir.x == 1) || (this.transform.position.y < endPos.transform.position.y && dir.x == 1))
        {

            if (myenm.myAbilities[0] == enemy.Ability.steal)
            {
                myenm.IRan();
            }else if (myenm.myAbilities[0] == enemy.Ability.heal)
            {
                myenm.GetComponent<Healer>().healAllyNow();
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

    public void SetDamage(float dmg, float maxDmg)
    {
        var temp = this.gameObject.GetComponent<SpriteRenderer>().color;

        float alph = dmg / maxDmg;
        if(alph < .4f)
        {
            alph = 0.25f;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g+.4f, temp.b+.4f, alph);
        }
        else if(alph < .65f && alph >= .4f)
        {
            alph = .6f;
            this.gameObject.GetComponent<SpriteRenderer>().color = new Color(temp.r, temp.g+.2f, temp.b+.2f, alph);
        }
        else
        {
            alph = .8f;
            temp = new Color(temp.r, temp.g, temp.b, alph);
        }
        
        damage = dmg;
    }
}
public enum AttackEffect
{
    none, DamageWeapon, DamageArmor, confuseStyle
}
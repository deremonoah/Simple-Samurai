using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class RotatingAtk : EnmAtKArea
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
}

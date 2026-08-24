using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    private PlayerHealthBar playerHP;
    [SerializeField] TrapEffectOnStrike TrapEffect;
    [SerializeField] protected float damageFromTrap;
    [SerializeField] protected bool BlocksStrikes;
    [SerializeField] protected bool DestroyOnBlock=true;//true by default some things won't
    protected bool PointerOnTrap;
    protected Vector2 posBlock;

    [Header("Positional fot non set spawns")]
    [SerializeField] protected Vector2 SpawnPosMinMax;
    [SerializeField] protected float SpawnPosOffset;

    protected void OnEnable()//in case I change to object pooling, which probably should
    {

        playerHP = FindObjectOfType<PlayerHealthBar>();
        List<List<Transform>> temp = EnemysManager.instance.GetTrapSpawnSpots();
        //decide between spawn spots
        int rand = Random.Range(0, temp[0].Count);
        //lerp out to it with a corotuine?
        List<Transform> trapSpots = temp[0];
        List<Transform> smokeSpots = temp[1];

        this.transform.position = trapSpots[rand].position;//should we instead use a random distance on the path the cursor will for sure travel on?
        //if you are the kind of trap that follows path
        if (SpawnPosOffset>0)
        {
            StrikePoint point = FindObjectOfType<StrikePoint>();
            float randpos = SpawnPosOffset + Random.Range(SpawnPosMinMax.x, SpawnPosMinMax.y);//idk man, just based off what I have in buff areas so just some nummbers
            transform.position = point.currentPath.path.GetPointAtDistance(randpos);
        }
        EffectOnStart(); 
    }

    protected virtual void EffectOnStart()
    {
        //overwritten by child scripts that have effects that start right away like big heal or bomb
    }

    protected void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (PointerOnTrap)
            {
                ResolveStrikeTrapEffect();
                if(DestroyOnBlock)//for things like shield & sumo hand
                {
                    ParticleManager.instance.BlockedHere(posBlock, 40f);
                    Destroy(this.gameObject);
                }
            }
        }
    }

    protected virtual void ResolveStrikeTrapEffect()
    {
        if(TrapEffect == TrapEffectOnStrike.none && damageFromTrap>0)
        { playerHP.DamagePlayer(null, damageFromTrap, 2); }//2 is anti armor. this used for 0 damage as well
        else if (TrapEffect == TrapEffectOnStrike.flame)
        {
            playerHP.DamagePlayer(null, damageFromTrap, 8);//8 is fire
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("in trigger");
        if (other.name == "strike point" && damageFromTrap>0)
        {
            PointerOnTrap = true;
        }
        if(BlocksStrikes)
        {
            //for sumo block
            FindObjectOfType<StrikeArea>().BeingBlocked(true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.name == "strike point" && damageFromTrap == 0)
        {
            FindObjectOfType<StrikeArea>().BeingBlocked(true);
            PointerOnTrap = true;
        }
        if (other.name == "strike point")
        {
            PointerOnTrap = true;
            posBlock = other.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //if its a block then it won't, I am realizing that these should probably be 2 scripts and inheerit from 1
        //and the blocking thing should only happen if its a thing to do the blocking, yes
        FindObjectOfType<StrikeArea>().BeingBlocked(false);
        PointerOnTrap = false;
    }
}
public enum TrapEffectOnStrike { none, flame, bombDisarm}

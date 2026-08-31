using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffStrikeArea : MonoBehaviour
{
    public enum Buff { swapEnemy, swapWeapon, speedUp, damageUp,weakSpot}
    public bool DestroyOnHit;
    [SerializeField] Buff mybuff;
    private StrikeArea mainStrikeArea;
    private PlayerEquipedItemsManager playerEquips;
    private bool OnThisStrikeArea;

    void Start()
    {
        mainStrikeArea = FindObjectOfType<StrikeArea>();
        playerEquips = FindObjectOfType<PlayerEquipedItemsManager>();
    }
    private void Update()
    {
        if ((Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.Mouse0))&& OnThisStrikeArea && DestroyOnHit)
        {
            StartCoroutine(DestroyRoutine());
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.name == "strike point")
        {
            mainStrikeArea.RecieveBuff((int)mybuff);
            if(mybuff==Buff.weakSpot)
            {
                mainStrikeArea.inStrikeArea = true;
                OnThisStrikeArea = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.name == "strike point")
        {
             mainStrikeArea.RecieveBuff(-1);
            if (mybuff == Buff.weakSpot)
            {
                mainStrikeArea.inStrikeArea = false;
                OnThisStrikeArea = false;
            }
        }
    }

    private IEnumerator DestroyRoutine()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(this.gameObject);
    }

    //so when should this appear?
    //and who should handle it? enemy spawner?
    //new manager? I already have the buff manager and will prob use some of the buffs
    //a new seperate thing, that has an instance to call from enemies after they flurry of attacks?
}

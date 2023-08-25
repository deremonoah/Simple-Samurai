using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armory : MonoBehaviour
{
    public List<Item> stockPile;
    [SerializeField] DropZone primaryWeaponHolder;
    [SerializeField] DropZone secondryWeaponHolder;
    [SerializeField] DropZone primaryArmorHolder;
    [SerializeField] DropZone secondaryArmorHolder;
    [SerializeField] DropZone primaryCurioHolder;
    [SerializeField] DropZone secondaryCurioHolder;
    [SerializeField] List<DropZone> InventorySlotsActive;
    [SerializeField] List<DropZone> InventorySlotsInActive;


    //ill need an event to pop up when players have 1 too many items
    //having more upgrades providing some buff? (a buff for item combos/ sets of armors)
    /*does the player just get this?
     * there will be upgrades and probably another panel
     * maybe an option to sell items?
     * also when does this unlock right when a player picks up a new item? then there is also when weapon swap will come in
     * I REALLY NEED A MANAGER FOR BUFF AREAS to decide where they need to go for each. set spots is easy to do quick but a more robust system would be a good idea
     * or even having them appear at certain times and go toward the player which could be an upgrade itself maybe a bar to tell when it will appear and come at player
     */

    //for the panel art having it start as just a dusty collset type dealio but as it upgrades it looks more like an armory

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItem(Item item)
    {
        stockPile.Add(item);
        //check if enough items or quality items to give buff
    }

    public void OpenArmoryPanel()
    {
        //load primary and secondary items
        //maybe a get playerEquipedItems that returns a list of the items
        //load list of items to inventroy slots
    }

    public void CloseArmoryPanel()
    {
        //give playerEquipedItems the loadout from slots
        //update list of items

    }
}

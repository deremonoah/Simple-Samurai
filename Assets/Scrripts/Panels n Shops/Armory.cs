using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armory : MonoBehaviour
{
    public List<Item> stockPile;
    [SerializeField] GameObject blankDragablePrefab;
    [SerializeField] DropZone primaryWeaponHolder;
    [SerializeField] DropZone secondryWeaponHolder;
    [SerializeField] DropZone primaryArmorHolder;
    [SerializeField] DropZone secondaryArmorHolder;
    [SerializeField] DropZone primaryCurioHolder;
    [SerializeField] DropZone secondaryCurioHolder;
    [SerializeField] List<GameObject> InventorySlotsActive;
    [SerializeField] List<DropZone> InventorySlotsInActive;
    private PlayerEquipedItemsManager equipManager;


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
        //this is to make sure any scriptable objects in the list are clones not the original
        equipManager = FindObjectOfType<PlayerEquipedItemsManager>();
        LoadArmoryPanel();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToArmory(Item item)
    {
        stockPile.Add(item);
        //check if enough items or quality items to give buff
    }

    public void LoadArmoryPanel()
    {
        //load primary and secondary items
        //do this by instantiating a dragable prefab and call assign item on it then move it to be the child of a dropzone
        LoadSlot(equipManager.PrimaryWeapon,primaryWeaponHolder);
        LoadSlot(equipManager.equipedArmor, primaryArmorHolder);
        if (equipManager.equipedCurio != null)
        {
            LoadSlot(equipManager.equipedCurio, primaryCurioHolder);
        }

        //maybe a get playerEquipedItems that returns a list of the items
        //load list of items to inventroy slots

        for (int lcv = 0; lcv < stockPile.Count; lcv++)
        {
            var dragerobj = Instantiate(blankDragablePrefab, InventorySlotsActive[lcv].transform.position, InventorySlotsActive[lcv].transform.rotation);
            var drager = dragerobj.GetComponent<Dragable>();
            drager.ParentToReturnTo = InventorySlotsActive[lcv].transform;
            drager.AssignItem(stockPile[lcv]);
            dragerobj.transform.parent = InventorySlotsActive[lcv].transform;
        }
    }

    private void LoadSlot(Item item, DropZone dz)
    {
        var dzobj = dz.gameObject;
        var dragerobj = Instantiate(blankDragablePrefab, dzobj.transform.position, dzobj.transform.rotation);
        Dragable drager = dragerobj.GetComponent<Dragable>();
        drager.ParentToReturnTo = dzobj.transform;
        drager.AssignItem(item);
        dragerobj.transform.parent = dzobj.transform;
        dz.heldDragable = drager;
    }

    public void CloseArmoryPanel()
    {
        //give playerEquipedItems the loadout from slots
        //update list of items
        foreach(GameObject dz in InventorySlotsActive)
        {
            dz.GetComponent<DropZone>().clearZone();
        }
    }

    public void IncreaseArmorySlots(int type)
    {
        if(type == 0)
        {
            //this is for inventroy

        }
        if(type == 1)
        {
            //this is for secondaryies maybe then also take in an item
        }
        
        //if there is room add a slot from the disabled slots to the enabled
        //and enable the drop zone
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int DefenseNum = 0;
    [SerializeField] int SpotInDefenseList;
    [SerializeField] DropZoneType Droptype;
    public Dragable heldDragable;

    public virtual void OnDrop(PointerEventData eventData)
    {
        //put the dragable.defense into this containers holder and then update the manager I have to make
        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
        if(d != null)
        {
            clearZone();
            d.ParentToReturnTo = this.transform;
            if(d.mytype == DragableType.defense)
            { 
                DefenseNum = (int)d.defense;
                FindObjectOfType<PlayerDefense>().ReadyDefense(DefenseNum, SpotInDefenseList);
            }
            else if(d.mytype == DragableType.item)
            {
                //I should probably make these 3 seperate classes one parent with 2 children for defenses and one for items
                if((int)Droptype>=3 &&(int)Droptype<=5)
                {
                    //3 to 5 are primary weapons
                    FindObjectOfType<PlayerEquipedItemsManager>().EquipItem(heldDragable.myitem,false);
                }
                else if((int)Droptype >= 6 && (int)Droptype <= 8)
                {
                    //for secondary stuff
                    FindObjectOfType<PlayerEquipedItemsManager>().EquipSecondary(heldDragable.myitem);
                }
                else if(Droptype == DropZoneType.Inventory)
                {
                    //search list and 
                }
            }
            //call and equip the right defense
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public void clearZone()
    {
        DefenseNum = 0;
        while(this.transform.childCount>0)
        {
            foreach(Transform child in this.transform)
            {
                child.GetComponent<Dragable>().ReturnToDispenser();
            }
        }
    }

}

public enum DropZoneType { none, Defense, Inventory, primaryWeapon, primaryArmor,primaryCurio, secondaryWeapon, secondaryArmor,secondaryCurio}
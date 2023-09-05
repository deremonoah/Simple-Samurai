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
    private bool rightType;

    

    public virtual void OnDrop(PointerEventData eventData)
    {
        //put the dragable.defense into this containers holder and then update the manager I have to make
        Debug.Log("onDrop");
        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
        if(d.mytype == DragableType.defense && Droptype ==DropZoneType.Defense)
        {
            //should check this before because defenses wont have any item
            rightType = true;
        }
        
        else if(d.myitem.GetType() == typeof(Armor) && ((int)Droptype == 4 || (int)Droptype == 7))
        {
            //checks if the item is armor and if this dropzone is compatible with armor
            rightType = true;
        }
        else if (d.myitem.GetType() == typeof(Weapon) && ((int)Droptype == 3 || (int)Droptype == 6))
        {
            //checks if the item is armor and if this dropzone is compatible with armor
            rightType = true;
        }
        else if (d.myitem.GetType() == typeof(Curio) && ((int)Droptype == 5 || (int)Droptype == 8))
        {
            //checks if the item is armor and if this dropzone is compatible with armor
            rightType = true;
        } else if(Droptype == DropZoneType.Inventory)
        {
            rightType = true;
        }

        if (d != null && rightType && heldDragable ==null)
        {
            clearZone();
            d.ParentToReturnTo = this.transform;
            heldDragable = d;
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
                rightType = false;
            }
            else if(d != null && rightType && heldDragable != null)
            {
                heldDragable.ParentToReturnTo = d.ParentToReturnTo;
                heldDragable.transform.SetParent(heldDragable.ParentToReturnTo);
                rightType = false;
            }
            //call and equip the right defense

            //reset so the next item has to match
            
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
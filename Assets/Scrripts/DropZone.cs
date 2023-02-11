using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public int DefenseNum = 0;
    public void OnDrop(PointerEventData eventData)
    {
        //put the dragable.defense into this containers holder and then update the manager I have to make
        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
        if(d != null && DefenseNum ==0)
        {
            d.ParentToReturnTo = this.transform;
            DefenseNum = (int)d.defense;
            FindObjectOfType<PlayerDefense>().ReadyDefense(DefenseNum);
            //call and equip the right defense
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}

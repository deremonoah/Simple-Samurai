using UnityEngine;
using UnityEngine.EventSystems;

public class Dispenser : MonoBehaviour , IDropHandler ,IPointerEnterHandler,IPointerExitHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        Dragable d = eventData.pointerDrag.GetComponent<Dragable>();
        if (d != null)
        {
            d.ParentToReturnTo = this.transform;            
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

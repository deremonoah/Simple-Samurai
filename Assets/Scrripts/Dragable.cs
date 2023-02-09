using UnityEngine;
using UnityEngine.EventSystems;


public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform ParentToReturnTo;

    public void OnBeginDrag(PointerEventData eventData)
    {
        ParentToReturnTo = this.transform.parent;
        this.transform.SetParent(this.transform.parent.parent);
        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        this.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        this.transform.SetParent(ParentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}

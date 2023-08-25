using UnityEngine;
using UnityEngine.EventSystems;


public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform ParentToReturnTo;
    private Transform DispenserToReturnTo;
    public DragableType mytype;
    public defenseKind defense;
    public Item myitem;

    public void Start()
    {
        DispenserToReturnTo = this.transform.parent;
    }

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

    public void ReturnToDispenser()
    {
        this.transform.SetParent(DispenserToReturnTo);
    }
}
public enum DragableType { defense, item }
public enum defenseKind { none, pit, palisade, spikes }

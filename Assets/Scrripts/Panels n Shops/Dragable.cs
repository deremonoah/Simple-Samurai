using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


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
        Debug.Log("onBeginDrag");
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
        Debug.Log("onEndDrag");
        this.transform.SetParent(ParentToReturnTo);
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }

    public void ReturnToDispenser()
    {
        this.transform.SetParent(DispenserToReturnTo);
    }

    public void AssignItem(Item item)
    {
        mytype = DragableType.item;
        this.gameObject.GetComponent<Image>().sprite = item.itemPanelIcon;
        myitem = item;
    }
}
public enum DragableType { defense, item }
public enum defenseKind { none, pit, palisade, spikes }

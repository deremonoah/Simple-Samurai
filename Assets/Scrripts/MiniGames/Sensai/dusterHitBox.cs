using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dusterHitBox : MonoBehaviour
{
    DustShelf manager;

    [SerializeField] int myItem;
    void Start()
    {
        manager = FindObjectOfType<DustShelf>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if(other.layer==6)//the only thing in the scene that has this layer and a hit box is the duster
        {
            manager.SetDustingItem(myItem);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        GameObject other = collision.gameObject;
        if (other.layer == 6)//the only thing in the scene that has this layer and a hit box is the duster
        {
            manager.SetDustingItem(-1);
        }
    }
}

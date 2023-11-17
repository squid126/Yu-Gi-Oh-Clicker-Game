using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public InventoryItemData item;
    public void Pickup()
    {
        Debug.Log("hello");
        InventoryManager.Instance.Add(item);
        Destroy(gameObject);
        
    }
}

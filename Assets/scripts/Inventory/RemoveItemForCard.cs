using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveItemForCard : MonoBehaviour
{
    
    public string CardName;
    public InventoryItemData card;
    public static RemoveItemForCard Instance { get; private set; }

    public void remove()
    {
       
        InventoryManager.Instance.Remove(card);
       
        Destroy(gameObject);
    }
   public  void setName(string name)
    {
        CardName = name;
    }
    public void setcard(InventoryItemData name)
    {
        card = name;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContentObject : MonoBehaviour
{
    public InventoryItemData card;
    public string CardName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void setName(string name)
    {
        CardName = name;
    }
    public void setcard(InventoryItemData name)
    {
        card = name;
    }

}

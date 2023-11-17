using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class InventoryManager : MonoBehaviour
{
    public List<InventoryItemData> Items = new List<InventoryItemData>();
    public Inventory inventory3 = new Inventory();
  
    public static InventoryManager Instance { get; private set; }
    public Transform ItemContent;
    public GameObject InventoryItem;
    public Sprite[] icons;
    private void Awake()
    {
        Instance = this;
        LoadFromJson();
    }
    public void Add(InventoryItemData item)
    {
        Items.Add(item);
        Item temp = new Item();
        temp.displayName = item.displayname;
        temp.price = item.price;
        temp.id=item.id;
        inventory3.inventoryItems.Add(temp);
        SaveToJson();   

        

    }
    private bool AreItemsEqual(Item item1, InventoryItemData item2)
    {
        return item1.displayName == item2.displayname &&
               item1.price == item2.price &&
               item1.id == item2.id;
    }
    public void Remove(InventoryItemData item)
    {
        // Debug.Log("_------------------------");
        //Debug.Log("Card thats being sold"+item.displayname);
        //remove item
        // Find the index of the item in the 'Items' list
        //int index = Items.FindIndex(i => i == item);
        //Items.Remove(item);
        if (Items.Contains(item))
        {
            Debug.Log("true");
            Items.Remove(item);
            
        }
        else
        {
            Debug.Log("False");
        }
        

       
            // Find the matching item in the 'inventory3.inventoryItems' list
         var matchingItem = inventory3.inventoryItems.Find(itemFromList => AreItemsEqual(itemFromList, item));
        if (matchingItem != null)
        {
            Debug.Log("true");
            // For example, you can remove it from the list:
            inventory3.inventoryItems.Remove(matchingItem);
        }
        else
        {
            Debug.Log("False");
        }
        



        //add price to coins
        GameManger.coins += System.Convert.ToInt32(item.price);
        PlayerPrefs.SetFloat("coins", (float)GameManger.coins);
        SaveToJson();
    }
    public void ListItems()
    {
        //clean the inventory before
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in Items)
        {
           // Debug.Log(item.displayname);
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            var itemButton = obj.transform.Find("Button").GetComponent<Button>();
            var itemIcon = obj.GetComponent<Image>();
            var itemPrice = itemButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            itemPrice.text = "Sell " + item.price;
            
            if (int.Parse(item.id) < icons.Length)
            {
                itemIcon.sprite = icons[int.Parse(item.id)];
            }
            
            //Debug.Log(item.displayname+"");
            RemoveItemForCard removeItemScript = obj.GetComponent<RemoveItemForCard>();
            if (removeItemScript != null)
            {
                
                removeItemScript.setName(item.displayname);
                removeItemScript.setcard(item);
            }


        }
    }
    public void SaveToJson()
    {
        
        string inventoryData = JsonUtility.ToJson(inventory3);
        Debug.Log(inventoryData);
        string filePath = Application.persistentDataPath + "/InventoryData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, inventoryData);
        
    }
    
    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/InventoryData.json";
        string inventoryData = System.IO.File.ReadAllText(filePath);
        inventory3 = JsonUtility.FromJson<Inventory>(inventoryData);
        //now add to list Items
        //create InventoryItemData
        // Clear the list first, to avoid duplicates
        Items.Clear();

        // Instantiate the assigned ScriptableObject instance
        
        foreach (var inventory in inventory3.inventoryItems)
        {
            // Assuming you have the asset path for the InventoryItemData ScriptableObject
            // You can change "AssetPath" to the actual asset path in your project
            InventoryItemData temp = ScriptableObject.CreateInstance<InventoryItemData>();
            temp.displayname = inventory.displayName;
            temp.price = inventory.price;
            temp.id = inventory.id;
            Items.Add(temp);
        }


    }
}
[System.Serializable]
public class Inventory
{
    public List<Item> inventoryItems = new List<Item> ();
}

[System.Serializable]
public class Item
{
    // Properties to hold the data from ItemController
    public string displayName;
    public string id;
    public double price;
}


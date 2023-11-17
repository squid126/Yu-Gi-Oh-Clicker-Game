using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PackManager : MonoBehaviour
{
    public List<ShopItemSO> Packs = new List<ShopItemSO>();
    public ListPacks packInventory = new ListPacks();
    public Transform packopener;// where the packs will be placed when opening
    public static PackManager instance { get; private set;  }
    public Transform ItemContent; //place to put the contents
    public GameObject InventoryItem; //prefab to create
    public Sprite[] icons;
    private void Awake()
    {
        instance = this;
        LoadFromJson();
    }
    public void Add(ShopItemSO item)
    {
        Packs.Add(item);
        Pack_detail temp = new Pack_detail();
        temp.displayName = item.title;
        temp.price = item.baseCost;
        temp.id = item.id;
        packInventory.inventoryItems.Add(temp);
       SaveToJson();
    }
    private bool AreItemsEqual(Pack_detail item1, ShopItemSO item2)
    {
        return item1.displayName == item2.title &&
               item1.price == item2.baseCost &&
               item1.id == item2.id;
    }
    public void Remove(ShopItemSO item)
    {
        // Debug.Log("_------------------------");
        //Debug.Log("Card thats being sold"+item.displayname);
        //remove item
        // Find the index of the item in the 'Items' list
        //int index = Items.FindIndex(i => i == item);
        //Items.Remove(item);
        if (Packs.Contains(item))
        {
            Debug.Log("true");
            Packs.Remove(item);

        }
        else
        {
            Debug.Log("False");
        }
        // Find the matching item in the 'inventory3.inventoryItems' list
        var matchingItem = packInventory.inventoryItems.Find(itemFromList => AreItemsEqual(itemFromList, item));
        if (matchingItem != null)
        {
            Debug.Log("true");
            // For example, you can remove it from the list:
            packInventory.inventoryItems.Remove(matchingItem);
        }
        else
        {
            Debug.Log("False");
        }
        //add price to coins
        //GameManger.coins += System.Convert.ToInt32(item.baseCost);
        //PlayerPrefs.SetFloat("coins", GameManger.coins);
        SaveToJson();
    }

    public void ListItems()
    {
        //clean the inventory before
        foreach (Transform item in ItemContent)
        {
            Destroy(item.gameObject);
        }
        foreach (var item in Packs)
        {
            // Debug.Log(item.displayname);
            GameObject obj = Instantiate(InventoryItem, ItemContent);
            
            var itemButton = obj.transform.Find("Button").GetComponent<Button>();
            var itemIcon = obj.GetComponent<Image>();
            var itemPrice = itemButton.GetComponentInChildren<TMPro.TextMeshProUGUI>();

            itemPrice.text = "Open";

            if (item.id < icons.Length)
            {
                itemIcon.sprite = icons[item.id];
            }

            //Debug.Log(item.displayname+"");
            PackItem removeItemScript = obj.GetComponent<PackItem>();
            if (removeItemScript != null)
            {

                removeItemScript.setName(item.title);
                removeItemScript.setcard(item);
                removeItemScript.setPack(packopener);
            }


        }
    }
    public void SaveToJson()
    {

        string inventoryData = JsonUtility.ToJson(packInventory);
        Debug.Log(inventoryData);
        string filePath = Application.persistentDataPath + "/PackData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, inventoryData);

    }
    public void LoadFromJson()
    {
        string filePath = Application.persistentDataPath + "/PackData.json";
        string inventoryData = System.IO.File.ReadAllText(filePath);
        packInventory = JsonUtility.FromJson<ListPacks>(inventoryData);
        //now add to list Items
        //create InventoryItemData
        // Clear the list first, to avoid duplicates
        Packs.Clear();

        // Instantiate the assigned ScriptableObject instance

        foreach (var inventory in packInventory.inventoryItems)
        {
            // Assuming you have the asset path for the InventoryItemData ScriptableObject
            // You can change "AssetPath" to the actual asset path in your project
            ShopItemSO temp = ScriptableObject.CreateInstance<ShopItemSO>();
            temp.title = inventory.displayName;
            temp.baseCost = inventory.price;
            temp.id = inventory.id;
            Packs.Add(temp);
        }


    }
}
[System.Serializable]
public class ListPacks
{
    public List<Pack_detail> inventoryItems = new List<Pack_detail>();
}

[System.Serializable]
public class Pack_detail
{
    // Properties to hold the data from ItemController
    public string displayName;
    public int id;
    public int price;
}

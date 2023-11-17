using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public ShopItemSO[] shopItemsSO;
    public ShopTemplate[] shopPanels;
    public GameObject[] shopPanelsGO;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanelsGO[i].SetActive(true);
            Debug.Log(shopPanelsGO[i].name);

        }
        LoadPanels();
    }
    public void LoadPanels()
    {
        for (int i = 0; i < shopItemsSO.Length; i++)
        {
            shopPanels[i].Price.text = "Buy: "+shopItemsSO[i].baseCost.ToString();
            if (shopItemsSO[i].imageComponent !=null)
            {
                shopPanels[i].image.sprite = shopItemsSO[i].imageComponent;
                //Debug.Log(item.displayname+"");
                
            }
            ShopTemplate packItem = shopPanelsGO[i].GetComponent<ShopTemplate>();
            if (packItem != null)
            {

                packItem.setName(shopItemsSO[i].title);
                packItem.setcard(shopItemsSO[i]);
            }
        }
    }
   

    
}

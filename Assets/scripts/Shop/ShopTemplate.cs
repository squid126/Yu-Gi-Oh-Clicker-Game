using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ShopTemplate : MonoBehaviour
{
    public TMP_Text Price;
    public Image image;
    public ShopItemSO pack;
    public string PackName;
    
   

    public void remove()
    {

        PackManager.instance.Remove(pack);

        Destroy(gameObject);
    }
    public void setName(string name)
    {
        PackName = name;
    }
    public void setcard(ShopItemSO name)
    {
        pack = name;
    }
    public void buy()
    {
        //send to PackManager and add it to the list so it can get save in the json file
       
        if (GameManger.coins >= pack.baseCost)
        {
            //minus the cost
            GameManger.coins -= pack.baseCost;
            //round to nearest two decimals
            GameManger.coins = Math.Round(GameManger.coins, 2);
            PlayerPrefs.SetFloat("coins", (float)GameManger.coins);
            PackManager.instance.Add(pack);

        }
        //remove money

    }
}

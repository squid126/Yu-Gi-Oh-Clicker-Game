using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManger : MonoBehaviour
{
    
    public static double coins;
    public static double multipler;
    public static int costofCoin;//variable that changes the cost up Fortunes Bounty
    public static int costofOfflineRate;//variable that changes the cost up Fortunes Bounty
    public static float offlineRate; // Coins per second


    public static float offlineMax; // limit to how much you can gain during a offline period
    public static int offlineLimitCost;//the cost to increase the limit of your offline earnings
    // Start is called before the first frame update
    void Start()
    {
        coins = PlayerPrefs.GetFloat("coins",0.00f);
        multipler = PlayerPrefs.GetFloat("multipler",1f);
       
        string savedDateQuit = PlayerPrefs.GetString("DateQuit", "0");
        offlineRate = PlayerPrefs.GetFloat("OfflineRate", 0.001f); //offinline rate
        offlineMax = PlayerPrefs.GetFloat("OfflineMax", 5f); //offinline max



        costofCoin = PlayerPrefs.GetInt("Fortune", 25);
        costofOfflineRate = PlayerPrefs.GetInt("TimeKeeper", 50);

        offlineLimitCost = PlayerPrefs.GetInt("Eternal", 75);

    }


}

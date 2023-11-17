using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Game : MonoBehaviour
{
    public static Game instance { get; private set; }
    private int panelSelector = 0 ;// 0 represents the clicker panel
    private int inventorypanelSelector = 1;// 0 represents the cards panel
    public GameObject clickPanel;//0
    public GameObject inventoryPanel;//1
    public GameObject shopPanel;//2
    public GameObject upgradePanel;//3
    private GameObject[] panels;
    public TextMeshProUGUI ui;
    public TextMeshProUGUI FortunesBountyUI;
    public TextMeshProUGUI TimeKeeperBountyUI;
    public TextMeshProUGUI EternalHarvestUI;
    public GameObject CardsPanel; //1
    public GameObject PacksPanel;//2
    private string FortuneBountyText = "Fortune's Bounty is a legendary card that ushers in great abundance and prosperity to those who possess it.It is a symbol of immense fortune and holds the promise of untold riches.";
    public GameObject scrollviewCard; 
    public GameObject scrollviewPack;

    private void Awake()
    {
        instance = this;
        //PlayerPrefs.DeleteAll();
    }
    private void OnApplicationQuit()
    {
        DateTime dateQuit = DateTime.Now;
       // Debug.Log(dateQuit.ToString()+"Quit time");
        PlayerPrefs.SetString("DateQuit", dateQuit.ToString());
    }
    public void Start()
    {
        //offline earnign rate
        
        string savedDateQuit = PlayerPrefs.GetString("DateQuit", "0");
        if (!savedDateQuit.Equals("0")){
            DateTime dateQuit =DateTime.Parse(savedDateQuit);
            DateTime dt = DateTime.Now;
            Debug.Log(dt.ToString() + "Start time");
            if (dt > dateQuit)
            {
                TimeSpan timespan = dt - dateQuit;
                float offlineEarnings = (float)timespan.TotalSeconds * GameManger.offlineRate;
                float roundedOfflineEarnings = (float)Math.Round(offlineEarnings, 2);
                if (roundedOfflineEarnings > GameManger.offlineMax)
                {
                    GameManger.coins += GameManger.offlineMax;
                    Debug.Log(GameManger.offlineMax);
                }
                else
                {
                    
                    GameManger.coins += roundedOfflineEarnings;
                    Debug.Log(roundedOfflineEarnings);
                    // Assuming GameManger.coins is a float

                }
                 GameManger.coins = Math.Round(GameManger.coins, 2);
                
                PlayerPrefs.SetFloat("coins", (float)GameManger.coins);
                
            }
        }
        


        panels = new GameObject[] { clickPanel, inventoryPanel, shopPanel, upgradePanel };
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        clickPanel.SetActive(true);
        PacksPanel.SetActive(false);
        CardsPanel.SetActive(true);
        //setting the prices
       EternalHarvestUI.text = GameManger.offlineLimitCost.ToString();
        FortunesBountyUI.text =GameManger.costofCoin.ToString();
        TimeKeeperBountyUI.text =  GameManger.costofOfflineRate.ToString();
    }
    public void Increment()
    {
        GameManger.coins += GameManger.multipler;
        PlayerPrefs.SetFloat("coins", (float)GameManger.coins);

    }
    public void Update()
    {
        ui.text = GameManger.coins.ToString();
    }

    public void BuyUpgradeOfflineLimit()
    {
        Debug.Log(GameManger.offlineMax);
        if (GameManger.coins >= GameManger.offlineLimitCost)
        {
            //minus the cost
            GameManger.coins -= GameManger.offlineLimitCost;

            //round to nearest two decimals
            GameManger.coins = Math.Round(GameManger.coins, 2);
            //incrmenet rate
            GameManger.offlineMax += 1;
            //save the rate
            PlayerPrefs.SetFloat("OfflineMax", GameManger.offlineMax);
            //change the cost
            GameManger.offlineLimitCost += 50;
            float temp = (float)(GameManger.offlineLimitCost);
            temp = (temp * 0.60f) + temp;

            GameManger.offlineLimitCost = Mathf.RoundToInt(temp);

            //save the new cost
            PlayerPrefs.SetInt("Eternal", GameManger.offlineLimitCost);
            PlayerPrefs.SetFloat("coins", (float)GameManger.coins);
            EternalHarvestUI.text = "Cost:" + GameManger.offlineLimitCost;
           
        }
    }
    public void BuyUpgradeOfflineRate()
    {
        if (GameManger.coins >= GameManger.costofOfflineRate)
        {
            //minus the cost
            GameManger.coins -= GameManger.costofOfflineRate;
            //round to nearest two decimals
            GameManger.coins = Math.Round(GameManger.coins, 2);
            //incrmenet rate
            GameManger.offlineRate += 0.01f;
            //save the rate
            PlayerPrefs.SetFloat("OfflineRate", GameManger.offlineRate);
            //change the cost
            //GameManger.costofOfflineRate += 25;
            //change the cost
            float temp = (float)(GameManger.costofOfflineRate);
            temp = (temp * 0.40f) + temp;

            GameManger.costofOfflineRate = Mathf.RoundToInt(temp);
            //save the new cost
            PlayerPrefs.SetInt("TimeKeeper", GameManger.costofOfflineRate);

            PlayerPrefs.SetFloat("coins", (float)GameManger.coins);
            TimeKeeperBountyUI.text =  "Cost:" + GameManger.costofOfflineRate;
            Debug.Log(GameManger.offlineRate);
        }
    }
    public void BuyUpgradeCoin()
    {
        Debug.Log(GameManger.costofCoin + "cost of COin");
        if ( GameManger.coins>=GameManger.costofCoin)
        {
            //minus the cost
            GameManger.coins -= GameManger.costofCoin;
            //round to nearest two decimals
            GameManger.coins = Math.Round(GameManger.coins, 2);
            //incrmenet multipler
            GameManger.multipler += 0.5;
            Debug.Log(GameManger.multipler+"Multiplier");    
            //save multipler
            PlayerPrefs.SetFloat("multipler", (float)GameManger.multipler);
            //change the cost
            float temp = (float)(GameManger.costofCoin);
            temp = (temp * 0.20f)+ temp;
            
            GameManger.costofCoin = Mathf.RoundToInt(temp);
            //save the new cost
            PlayerPrefs.SetInt("Fortune", GameManger.costofCoin);
           
            PlayerPrefs.SetFloat("coins", (float)GameManger.coins);
            FortunesBountyUI.text = FortuneBountyText + " Cost:" + GameManger.costofCoin;
        }
    }
    

    public void ActivateClickPanel()
    {
        //CHECK IF PANEL IS ON 
        //IF FALSE SET ON 
        //DEACTIVATE CURRRENT PANEL
        if (panelSelector != 0)
        {
            panels[panelSelector].SetActive(false);
            clickPanel.SetActive(true);
            panelSelector = 0;
        }
        
    }
    public void activateInventoryPanel()
    {
        //CHECK IF PANEL IS ON 
        //IF FALSE SET ON 
        //DEACTIVATE CURRRENT PANEL
        if (panelSelector != 1)
        {
            panels[panelSelector].SetActive(false);
            inventoryPanel.SetActive(true);
            panelSelector = 1;
        }
    }
    public void activateShopPanel()
    {
        //CHECK IF PANEL IS ON 
        //IF FALSE SET ON 
        //DEACTIVATE CURRRENT PANEL
        if (panelSelector != 2)
        {
            panels[panelSelector].SetActive(false);
            shopPanel.SetActive(true);
            panelSelector = 2;
        }
       
    }
    public void activateUpgradePanel()
    {
        //CHECK IF PANEL IS ON 
        //IF FALSE SET ON 
        //DEACTIVATE CURRRENT PANEL
        if (panelSelector != 3)
        {
            panels[panelSelector].SetActive(false);
            upgradePanel.SetActive(true);
            panelSelector = 3;
        }
    }
    public void activatePacksPanel()
    {
        if (inventorypanelSelector != 2)
        {
            CardsPanel.SetActive(false);
            PacksPanel.SetActive(true);
            inventorypanelSelector = 2;
        }
    }
    public void activatecardsPanel()
    {
        if (inventorypanelSelector != 1)
        {
            CardsPanel.SetActive(true);
            PacksPanel.SetActive(false);
            inventorypanelSelector = 1;
        }
    }
    //function to figure out if we should list out cards or packs panels
    //based on last opened panel
    public void cardsorPacksPanel()
    {
        if (inventorypanelSelector == 2)
        {
            //list packs
            PackManager.instance.ListItems();
        }
        else
        {
            //list cards
            InventoryManager.Instance.ListItems();
        }
    }
    
    //when a pack is open set either the card or pack scrol view off
    public void setPacksorCardsOff()
    {
        scrollviewPack.SetActive(false);
    }
    public void setPacksorCardsOn()
    {
        scrollviewPack.SetActive(true);
    }

}

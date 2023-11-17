using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PackItem : MonoBehaviour
{
    
    public GameObject myPrefab;
    public string PackName;
    public ShopItemSO pack;
    public static PackItem Instance { get; private set; }
    public Transform packOpener;
    

    private void Start()
    {
       // panel = GameObject.FindGameObjectWithTag(panelTag).transform;
    }
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
    public void setPack(Transform place)
    {
        packOpener = place;
    }
    public void buy()
    {
        Game.instance.setPacksorCardsOff();
        GameObject obj = Instantiate(myPrefab,packOpener);
        remove();

        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(fileName ="ShopMenu",menuName ="Scriptable objects/New Shop Item",order =1)]
public class ShopItemSO : ScriptableObject
{
    public string title;
    public int baseCost;
    public int id;
    public Sprite imageComponent;
}

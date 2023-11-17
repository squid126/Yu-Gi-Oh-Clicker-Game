using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollInitilazer : MonoBehaviour
{
    #region Public Properties

    /// <summary>
    /// How far apart each item is in the scroll view.
    /// </summary>
    public float ItemSpacing { get { return itemSpacing; } }

    /// <summary>
    /// How much the items are indented from left and right of the scroll view.
    /// </summary>
    public float HorizontalMargin { get { return horizontalMargin; } }

    /// <summary>
    /// How much the items are indented from top and bottom of the scroll view.
    /// </summary>
    public float VerticalMargin { get { return verticalMargin; } }

    /// <summary>
    /// Is the scroll view oriented horizontally?
    /// </summary>
    public bool Horizontal { get { return horizontal; } }

    /// <summary>
    /// Is the scroll view oriented vertically?
    /// </summary>
    public bool Vertical { get { return vertical; } }

    /// <summary>
    /// The width of the scroll content.
    /// </summary>
    public float Width { get { return width; } }

    /// <summary>
    /// The height of the scroll content.
    /// </summary>
    public float Height { get { return height; } }

    /// <summary>
    /// The width for each child of the scroll view.
    /// </summary>
    public float ChildWidth { get { return childWidth; } }

    /// <summary>
    /// The height for each child of the scroll view.
    /// </summary>
    public float ChildHeight { get { return childHeight; } }

    #endregion

    #region Private Members

    /// <summary>
    /// The RectTransform component of the scroll content.
    /// </summary>
    private RectTransform rectTransform;

    /// <summary>
    /// The RectTransform components of all the children of this GameObject.
    /// </summary>
    private RectTransform[] rtChildren;

    /// <summary>
    /// The width and height of the parent.
    /// </summary>
    private float width, height;

    /// <summary>
    /// The width and height of the children GameObjects.
    /// </summary>
    private float childWidth, childHeight;

    /// <summary>
    /// How far apart each item is in the scroll view.
    /// </summary>
    [SerializeField]
    private float itemSpacing;

    /// <summary>
    /// How much the items are indented from the top/bottom and left/right of the scroll view.
    /// </summary>
    [SerializeField]
    private float horizontalMargin, verticalMargin;

    /// <summary>
    /// Is the scroll view oriented horizontall or vertically?
    /// </summary>
    [SerializeField]
    private bool horizontal, vertical;

    #endregion

    public Sprite cardicon;
    public Sprite test;
    //all possible items you can get
    //10 items in total
    //
    //you can get one secret rare or  one ultra rare or one super rare
    //algthorim of picking which rare item to choose
    public List<InventoryItemData> RareItems = new List<InventoryItemData>();
    //rest are common
    public List<InventoryItemData> RestItems = new List<InventoryItemData>();
    //the index values whos items are rare will go here

    public  int rareItemsIndex1=-1;
    public int rareItemsIndex2=-1;
    // Start is called before the first frame update
    void Start()
    {
        // Get the InventoryManager instance and access the icons array.
        InventoryManager inventoryManager = InventoryManager.Instance;
        shuffleLists();
        rectTransform = GetComponent<RectTransform>();
        rtChildren = new RectTransform[rectTransform.childCount];
        //int j = 0;
        for (int i = 0; i < rtChildren.Length; i++)
        {
            rtChildren[i] = rectTransform.GetChild(i) as RectTransform;
            var itemIcon = rectTransform.GetChild(i).GetComponent<Image>();
            ContentObject item = rtChildren[i].GetComponent<ContentObject>();
            
            if (rareItemsIndex1==i )
            {
                //item is a special item
                
                itemIcon.sprite = inventoryManager.icons[int.Parse(RareItems[i].id)];
                item.setName(RareItems[i].name);
                item.setcard(RareItems[i]);

            }
            else if (rareItemsIndex2 == i)
            {
                itemIcon.sprite = inventoryManager.icons[int.Parse(RareItems[i].id)];
                item.setName(RareItems[i].name);
                item.setcard(RareItems[i]);
            }
            else
            {
               
                itemIcon.sprite = inventoryManager.icons[int.Parse(RestItems[i].id)];
                item.setName(RestItems[i].name);
                item.setcard(RestItems[i]);
            }
           
            
        }


       // Subtract the margin from both sides.
       width = rectTransform.rect.width - (2 * horizontalMargin);
        // Subtract the margin from the top and bottom.
        height = rectTransform.rect.height - (2 * verticalMargin);


        childWidth = rtChildren[0].rect.width;
        childHeight = rtChildren[0].rect.height;

        InitializeContentHorizontal();
        //selector();
    }
    /// <summary>
    /// beware code is really unreadble 
    /// baicalyy selects the card that will be chosen
    /// a rare item has 2.5 percent chance of being chose the rest 
    /// have 13% of being chosen
    /// </summary>
    public int selector()
    {
        //2.5% of these  items being chosen
        float lowProbabilityWeight = 0.05f / 2;
        // Calculate the weight for the remaining items (95% for all).
        float highProbabilityWeight = (1f - 0.05f) / (9 - 2);
       // Debug.Log(lowProbabilityWeight + "Low ");
        //Debug.Log(highProbabilityWeight + "High ");
        // the sum of the probabilities must be 1
        // so P(1)+p(2)+P(3)+p(4)+P(5)+p(6)+P(7)+p(8)+P(9)
        //calcualte the cumulative probabilities based on the weight
        // Create a list to store the cumulative probabilities for each item.
        List<float> cumulativeProbabilities = new List<float>();
        bool flag = false; //first flag to check if one rare item is selected
        bool flag2 = false; //second flag to check if both rare items have been selected
        // Calculate the cumulative probabilities based on the weights.
        for (int i = 0; i < 9; i++)
        {
            //condition need to check if indice is a low probabilty item
            //add highprobailty items
            // start with 1
            if (i == 0)
            {
                //first index is a rare item
                if (rareItemsIndex1 == i || rareItemsIndex2 == i)
                { 
                    //add weight and first flag is true now since one rare item is selected
                    cumulativeProbabilities.Add(lowProbabilityWeight);
                    flag = true;
                }
                else
                {
                    //if not a rare item start 
                    cumulativeProbabilities.Add(highProbabilityWeight);
                }

            }
            //check if indice is a rare item
            else if (rareItemsIndex1 == i || rareItemsIndex2 == i)
            {
                //if one rare item has been selected than this is the second rare item being selected
                
                if (flag == true)
                {
                    // add the weigth of both rare items than of the high probabilty items 
                    cumulativeProbabilities.Add(lowProbabilityWeight * 2 + highProbabilityWeight * (i - 1));
                    flag2 = true;
                }
                else
                {
                    //if the first item has not been selected than 
                    // add the low item rate + the high probabilty rate
                    // than set the flag true indicating a rare item has been selected
                    cumulativeProbabilities.Add(lowProbabilityWeight + highProbabilityWeight * (i - 1));
                    flag = true;
                }

            }
            else if (flag == true && flag2 == false)
            {
                // if one rare item has been selected 
                // and we are adding a high probabiltiy weiught
                cumulativeProbabilities.Add(lowProbabilityWeight + highProbabilityWeight * (i));
            }
            else
            {
                // both rare items have been added so now we are jsut addign the high probaiblty items weight
                cumulativeProbabilities.Add(lowProbabilityWeight * 2 + highProbabilityWeight * (i - 1));

            }
        }
        // Generate a random value between 0 and 1 to select an item.
        float randomValue = UnityEngine.Random.value;

        // Find the index of the selected item based on the random value and cumulative probabilities.
        int selectedIndex = 0;
        while (selectedIndex < 9 - 1 && randomValue > cumulativeProbabilities[selectedIndex])
        {
            selectedIndex++;
        }
        //Debug.Log(randomValue + "random value");
        int counter = 0;
        foreach (var item in cumulativeProbabilities)
        {
            //Debug.Log(item + "item in cumulative probvavilities"+ "counter: "+ counter);
            counter++;
        }
       // Debug.Log(selectedIndex + "card that the user gets");
        return selectedIndex;
    }
    //shuffle the lists using the fischer yates algorthim
    private void shuffleLists()
    {
        int k;
        for (int i = 0; i < 2; i++)
        {
            k = UnityEngine.Random.Range(0, RareItems.Count);
            if (rareItemsIndex1 == -1)
            {
                rareItemsIndex1 = k;
            }
            else if( k==rareItemsIndex1)
            {
                i--;
            }
            else
            {
                rareItemsIndex2 = k;
            }
            
           // Debug.Log(k);
         

        }
       
        
       
        int n = RestItems.Count;
        while (n > 1)
        {
            n--;
            k = UnityEngine.Random.Range(0, n + 1);
            var temp = RestItems[k];
            RestItems[k] = RestItems[n];
            RestItems[n] = temp;
        }
         n = RareItems.Count;
        while (n > 1)
        {
            n--;
            k = UnityEngine.Random.Range(0, n + 1);
            var temp = RareItems[k];
            RareItems[k] = RareItems[n];
            RareItems[n] = temp;
        }
    }
    
    // <summary>
    /// Initializes the scroll content if the scroll view is oriented horizontally.
    /// </summary>
    private void InitializeContentHorizontal()
    {
        float total = 0f;
        float originX = 0 - (width * 0.5f);
        float posOffset = childWidth * 0.5f;
        for (int i = 0; i < rtChildren.Length; i++)
        {
            Vector2 childPos = rtChildren[i].localPosition;
            childPos.x = originX + posOffset + i * (childWidth + itemSpacing);
            rtChildren[i].localPosition = childPos;
            
            total += Math.Abs(rtChildren[i].localPosition.x);
        }
       
    }
}
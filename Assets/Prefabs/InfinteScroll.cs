using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfinteScroll : MonoBehaviour
{
    #region Private Members

    /// <summary>
    /// The ScrollContent component that belongs to the scroll content GameObject.
    /// </summary>
    [SerializeField]
    private ScrollInitilazer scrollContent;

    /// <summary>
    /// How far the items will travel outside of the scroll view before being repositioned.
    /// </summary>
    [SerializeField]
    private float outOfBoundsThreshold;

    /// <summary>
    /// The ScrollRect component for this GameObject.
    /// </summary>
    private ScrollRect scrollRect;

    /// <summary>
    /// The last position where the user has dragged.
    /// </summary>
    private Vector2 lastDragPosition;

    /// <summary>
    /// Is the user dragging in the positive axis or the negative axis?
    /// </summary>
    private bool positiveDrag;

    #endregion
    [SerializeField] private float scrollSpeed = 1f;
    private bool scrollingEnabled = true;
    private float minRandomFactor = 1f;
    [SerializeField] private float maxRandomFactor = 100f;
    private float contentWidth;
    private float timer = 0f;
    private float timerDuration = 7f;
    private bool isTimerComplete = false;
    private float acceleration;
    private int counter = 0;
    //public  string[] names = { "Item 0", "Item 0 (1)", "Item 0 (2)", "Item 0 (3)", "Item 0 (4)", "Item 0 (5)", "Item 0 (6)", "Item 0 (7)", "Item 0 (8)" };
   // public int[] countervar = {0,0,0, 0, 0, 0,0,0,0 };
// Start is called before the first frame update
    void Start()
    {
        acceleration = (scrollSpeed / Random.Range(2,7));
        scrollRect = GetComponent<ScrollRect>();
        scrollRect.horizontal = scrollContent.Horizontal;
        scrollRect.movementType = ScrollRect.MovementType.Unrestricted;
        


        
    }

    // Update is called once per frame
    void Update()
    {
        if (scrollingEnabled)
        {
            HandleHorizontalScroll();
            // Update the timer
            timer += Time.deltaTime;
            
            if (Mathf.RoundToInt(timer) > 2)
            {
                float randomFactor = Random.Range(minRandomFactor, maxRandomFactor);
                float randomizedAcceleration = acceleration * randomFactor;
                scrollSpeed -= randomizedAcceleration * Time.deltaTime;
            }
           




            // Check if the timer is complete
            if (timer >= timerDuration && !isTimerComplete || scrollSpeed<0)
            {
                //356 is the middle 
                //if greater than 380 theres a chance it lands in the middle
                //nudge it forward
                
                
                var currItem = scrollRect.content.GetChild(3);
                Debug.Log(currItem);
                Vector3 worldPosition = currItem.position;
               
                //if (worldPosition.x >= 380)
                //{
                //    Debug.Log("workd");
                //    scrollRect.content.localPosition += Vector3.left * 20 * Time.deltaTime;
                //}

                

                // Perform actions when the timer is complete
                isTimerComplete = true;
                Debug.Log("Timer Complete!");
                scrollingEnabled = false;
               
               
            }
        }
        
        
    }
    

    private void HandleHorizontalScroll()
    {

        Debug.Log("horizaontal scroll");
        scrollRect.content.localPosition += Vector3.left * scrollSpeed * Time.deltaTime;
        int currItemIndex = positiveDrag ? scrollRect.content.childCount - 1 : 0;
        var currItem = scrollRect.content.GetChild(currItemIndex);
        if (!ReachedThreshold(currItem))
        {
            return;
        }

        int endItemIndex = positiveDrag ? 0 : scrollRect.content.childCount - 1;
        Transform endItem = scrollRect.content.GetChild(endItemIndex);
        Vector2 newPos = endItem.position;

        if (positiveDrag)
        {
            //newPos.x = endItem.position.x - scrollContent.ChildWidth * 1.5f + scrollContent.ItemSpacing;
            newPos.x = endItem.position.x - scrollContent.ChildWidth + scrollContent.ItemSpacing;
        }
        else
        {
            //newPos.x = endItem.position.x + scrollContent.ChildWidth * 1.5f - scrollContent.ItemSpacing;
            newPos.x = endItem.position.x + scrollContent.ChildWidth+ scrollContent.ItemSpacing;
        }

        currItem.position = newPos;
        currItem.SetSiblingIndex(endItemIndex);
    }

    /// <summary>
    /// Checks if an item has the reached the out of bounds threshold for the scroll view.
    /// </summary>
    /// <param name="item">The item to be checked.</param>
    /// <returns>True if the item has reached the threshold for either ends of the scroll view, false otherwise.</returns>
    private bool ReachedThreshold(Transform item)
    {
        Debug.Log("reached threshold");
        if (scrollContent.Vertical)
        {
            float posYThreshold = transform.position.y + scrollContent.Height * 0.5f + outOfBoundsThreshold;
            float negYThreshold = transform.position.y - scrollContent.Height * 0.5f - outOfBoundsThreshold;
            return positiveDrag ? item.position.y - scrollContent.ChildWidth * 0.5f > posYThreshold :
                item.position.y + scrollContent.ChildWidth * 0.5f < negYThreshold;
        }
        else
        {
            Debug.Log("Here");

            float posXThreshold = transform.position.x + scrollContent.Width * 0.5f + outOfBoundsThreshold;
            float negXThreshold = transform.position.x - scrollContent.Width * 0.5f - outOfBoundsThreshold;
            return positiveDrag ? item.position.x - scrollContent.ChildWidth * 0.5f > posXThreshold :
                item.position.x + scrollContent.ChildWidth * 0.5f < negXThreshold;
        }
    }
}


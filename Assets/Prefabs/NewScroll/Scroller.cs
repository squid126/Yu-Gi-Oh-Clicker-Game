using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    public Button DoneButton;
    public Button StartButton;
    //protected ScrollRect scrollRecter;
    public RectTransform contentPanel;
    //public InventoryManager inventoryList;
    private float acceleration;
    public ScrollInitilazer scrollInitializer; // Drag and drop the ScrollInitilazer script reference in the Inspector.
    public float scrollSpeed; // Adjust this value to set the scroll speed.
    public float scrollInital = 700f; // Adjust this value to set the delay before starting the scroll.
    public float timerDuration = 7f;
    public float timer = 0f;
    private RectTransform contentRect;
    private ScrollRect scrollRect;
    private bool isScrolling = true;
    private float decelerationRate;
    Vector3 startPosition;
    private RectTransform rectTransform;
    float width;
    int value;
    Transform selectedItem;
    public bool isLeft = false;
    public bool buttonPressed = false;
    public bool isDone = false;
    private void Start()
    {
        acceleration = (scrollSpeed / Random.Range(2, 7));
        // Get a reference to the RectTransform of the "Content" GameObject.
        scrollRect = GetComponent<ScrollRect>();
        contentRect = scrollRect.content;
         startPosition = contentRect.GetChild(0).position;
        // Get the RectTransform component of the GameObject.
        rectTransform = GetComponent<RectTransform>();
         value=scrollInitializer.selector();
        
         selectedItem = contentRect.GetChild(value);
        Debug.Log( "Selected Value: " + selectedItem);



        // Start the coroutine for smooth scrolling to the selected item


    }
    private void Update()
    {
    
        if (buttonPressed)
        {
            //do whatever you stupdi 

            HandleScroll();
            timer += Time.deltaTime;
            if (timer >= 2 && scrollSpeed > 200)
            {
                decelerationRate = (scrollInital / (timerDuration - 2 - 1));
                scrollSpeed -= decelerationRate * Time.deltaTime;
                //Debug.Log(idk + "value");
            }
            else if (scrollSpeed < 200)
            {
                
                scrollSpeed -= 20f * Time.deltaTime;
                scrollSpeed = Mathf.Clamp(scrollSpeed, 20, 1500);
                SmoothMoveToTarget(selectedItem.position);
            }
        }
        if (isDone)
        {
            //add item to inventory
            
               ContentObject data= selectedItem.GetComponent<ContentObject>();
            InventoryManager.Instance.Add(data.card);
            isDone = false;
            DoneButton.gameObject.SetActive(true);
            //so that it only adds it once
        }

    }
    public void start()
    {

         scrollSpeed=1500; 
        scrollInital = 1500;
        timerDuration = 7f;
        timer = 0f;
        buttonPressed = true;
        StartButton.gameObject.SetActive(false);



    }
    public void done()
    {
        Game.instance.setPacksorCardsOn();
        //destroy gameobject 
        Destroy(gameObject);

    }
    private IEnumerator WaitAndPrint()
    {
        //scroll three times than stop 
        int j = 0;
        while (  scrollSpeed>0 && isScrolling)
        {
            HandleScroll();

            timer += Time.deltaTime;
            if (timer >= 2 && scrollSpeed>200)
            {
                decelerationRate = (scrollInital / (timerDuration-2-1));
                scrollSpeed -= decelerationRate * Time.deltaTime;
                
                //Debug.Log(idk + "value");

                j++;

            }
            else if (scrollSpeed < 200)
            {
                decelerationRate = (scrollInital / (timerDuration - 2 - 1));
                scrollSpeed -= decelerationRate * Time.deltaTime;

                if (!isLeft)
                {
                    SmoothMoveToTarget(selectedItem.position);
                }
                
            }
            

            yield return null; // Wait until the next frame
        }
    }
    private void SmoothMoveToTarget(Vector3 targetPosition)
    {

      
        
        Vector2 targetAnchoredPosition = (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
                                         - (Vector2)scrollRect.transform.InverseTransformPoint(targetPosition);

        float distanceThreshold = 40f; // Adjust this threshold as needed
                                       // Determine the direction of movement
        bool isMovingLeft = contentPanel.anchoredPosition.x < targetAnchoredPosition.x;
        //Debug.Log(isMovingLeft);

        if (isMovingLeft == false)
        {
           // Debug.Log(Vector2.Distance(contentPanel.anchoredPosition, targetAnchoredPosition) + " Position " + distanceThreshold);


            // Check if the distance between current and target positions is greater than the threshold
            if (Vector2.Distance(contentPanel.anchoredPosition, targetAnchoredPosition) > distanceThreshold)
            {
                // Move forward towards the target position
                //contentPanel.anchoredPosition = Vector2.Lerp(contentPanel.anchoredPosition, targetAnchoredPosition, smoothSpeed * Time.deltaTime);       
                decelerationRate = (scrollInital / (timerDuration - 2 - 1));
            }
            else
            {
                // Set the contentPanel's anchoredPosition directly to the target position
                //contentPanel.anchoredPosition = targetAnchoredPosition;
               // Debug.Log("Found now stop");
                buttonPressed = false;
                isDone = true;
            }
        }
        


        

    }
    private void HandleScroll()
    {
        contentRect.localPosition += Vector3.left * scrollSpeed * Time.deltaTime;
        //Debug.Log(contentRect.GetChild(0).position.x + "Position");
        if (contentRect.GetChild(0).position.x < startPosition.x - 15)
        {
            Transform firstChild = contentRect.GetChild(0);
            // Adjust the position of the "Content" RectTransform to move the first child to the end.
            //get the last child position + item spacing + width of the object
            Vector3 lastChildPosition = contentRect.GetChild(contentRect.childCount - 1).localPosition;
            //Debug.Log(lastChildPosition.x + "last child before");
            float width = firstChild.GetComponent<RectTransform>().rect.width;
            //Debug.Log(width + "width");
            lastChildPosition.x = lastChildPosition.x + scrollInitializer.ItemSpacing + width;
            //Debug.Log(lastChildPosition.x + "last child after");
            firstChild.localPosition = new Vector3(lastChildPosition.x, firstChild.localPosition.y, firstChild.localPosition.z);
            firstChild.SetAsLastSibling();
        }
        else
        {
            //Debug.Log(contentRect.GetChild(0).position.x + "Position");
        }
    }
   }


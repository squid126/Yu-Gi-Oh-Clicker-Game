using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private  Animation anim;
    [SerializeField] private string shrink = "Click_animation";

    void Start()
    {
        //anim = gameObject.GetComponent<Animation>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void playClickAnimation()
    {
        anim.Play(shrink);
    }
}

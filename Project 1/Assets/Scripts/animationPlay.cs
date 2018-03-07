using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationPlay : MonoBehaviour
{
    public bool isCharacterMoving;
    private Animator myAnimator;
	// Use this for initialization
	void Start ()
    {
        myAnimator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(isCharacterMoving)
        {
            myAnimator.SetBool("isMoving", true);
        }
        else
        {
            myAnimator.SetBool("isMoving", false);
        }
	}


}

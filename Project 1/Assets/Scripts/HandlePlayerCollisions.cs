using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlayerCollisions : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        Collider otherTriggerCollider = collision.collider;
        playerMovementScript.OnDragonCollisionEnter(collision);
        playerMovementScript.OnDragonTriggerEnter(otherTriggerCollider);
    }
}

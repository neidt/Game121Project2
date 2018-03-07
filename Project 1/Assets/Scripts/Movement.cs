using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Transform playerModel;
    public Transform target;
    public float moveSpeed = 7.0f;
    public LayerMask raycastLayers;
    public LayerMask floorOnly;
    private Vector3 destination;
    public float rayDistance = 1.0f;
    //in case we hit
    private bool hitThisFrame = false;
    private Vector3 rayCollisionNormal;
    private Vector3 hitLocThisFrame = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        destination = playerModel.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hitInfo;
        hitThisFrame = false;
        float step = moveSpeed * Time.deltaTime;

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, rayDistance, raycastLayers.value))
        {
            print("raycast hit" + hitInfo.transform.name + " at" + hitInfo.point);
            hitLocThisFrame = hitInfo.point;
            rayCollisionNormal = hitInfo.normal;
            hitThisFrame = true;
        }

        //move based on mouse click
        if (Input.GetMouseButtonDown(0))
        {
            Ray intoScreen = Camera.main.ScreenPointToRay(Input.mousePosition); // hits whatever is under mouse at the time 
            if (Physics.Raycast(intoScreen, out hitInfo, 1000, floorOnly))
            {
                destination = hitInfo.point;
                playerModel.LookAt(destination);
                rayCollisionNormal = hitInfo.normal;
            }
        }
        //move the player toward the destination

        if (Vector3.Distance(playerModel.position, destination) > rayDistance)
        {
            transform.position = Vector3.MoveTowards(playerModel.transform.position, destination, step);
        }
        //else if (Vector3.Distance(playerModel.position, destination) < rayDistance)
        //{
        //    destination = rayCollisionNormal + hitInfo.point;
        //    transform.position = Vector3.MoveTowards(playerModel.transform.position, destination, step);
        //}
        //transform.position = Vector3.MoveTowards(playerModel.transform.position, destination, step);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayDistance);
    }
}

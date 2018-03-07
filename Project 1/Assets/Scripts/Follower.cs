using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour
{
    public Transform playerObject;
    public Transform follower;
    public Transform floor;

    Rigidbody rb;
    public float followerMoveSpeed = 3.0f;
    public float followerUpDown = .2f;

    public LayerMask raycastLayers;
    public LayerMask floorOnly;

    public float rayDistance = 5.0f;
    float rayDistDown = 1.0f;
    public float followDistance = 3f;
    public bool movingUp = false;
    public bool movingDown = true;

    private Vector3 rayCollisionNormal;
    private Vector3 hitLocThisFrame = Vector3.zero;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //idle thingy
        RaycastHit hitInfoDown;
        if (Physics.Raycast(follower.transform.position, -follower.transform.up, out hitInfoDown, rayDistDown, floorOnly.value))
        {
            print("into ryacast");
            if (Vector3.Distance(follower.position, hitInfoDown.point) < 2)
            {
                print("adding force");
                rb.AddForce(new Vector3(0, 75, 0));
            }
        }

        //hitPlayer = false;
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, rayDistance, raycastLayers.value))
        {
            print("raycast hit " + hitInfo.transform.name + " at " + hitInfo.point);
            hitLocThisFrame = hitInfo.point;
            rayCollisionNormal = hitInfo.normal;
            follower.transform.LookAt(playerObject);
            if (Vector3.Distance(follower.position, playerObject.position) > followDistance)
            {
                float enemyYPos = transform.position.y;
                Vector3 customPos = playerObject.transform.position;
                customPos.y = enemyYPos;
                follower.transform.LookAt(customPos);
                follower.transform.position += follower.transform.forward * followerMoveSpeed * Time.deltaTime;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayDistance);
        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * rayDistDown);
    }
}
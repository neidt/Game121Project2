using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    public float rayDistance = 10.0f;
    public float detectionDistance = 10.0f;
    private float nextRound = 0.0f;
    public float fireRate = 3.0f;
    public float moveSpeed = 2.0f;
    public float radiusOfSatisfaction = 2.0f;

    public Transform playerObject;
    public Transform shooterObject;
    public Transform bulletSpawn;
    public GameObject bullet;
    public GameObject shooterObj;

    //patrolling stuff
    bool isAtWaypoint1;
    bool isAtWaypoint2;
    public GameObject waypoint1;
    public GameObject waypoint2;

    //animation
    private Animator myAnimator;
    public bool isPatrolling;
    public bool isPursuing = false;
    bool isFiring;

    //raycast stuff
    public LayerMask raycastLayers;
    private Vector3 rayCollisionNormal;

    //health stuff
    public float shooterHealth = 5.0f;
    bool isDead = false;


    // Use this for initialization
    void Start ()
    {
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        ShooterScript movementScript = shooterObj.GetComponent<ShooterScript>();
        isPatrolling = true;
        RaycastHit hitInfo;
        Vector3 startPoint = shooterObject.transform.position + new Vector3(0, 1, 0);

        if ((!isAtWaypoint1 && !isAtWaypoint2) || (isAtWaypoint2 && !isAtWaypoint1) && isPatrolling == true && isPursuing == false)
        {
            shooterObject.transform.LookAt(waypoint1.transform);
            if (Vector3.Distance(shooterObject.position, waypoint1.transform.position) > radiusOfSatisfaction)
            {
                float shooterYPos = transform.position.y;
                Vector3 customPos = waypoint1.transform.position;
                customPos.y = shooterYPos;
                shooterObject.transform.LookAt(customPos);
                shooterObject.transform.position += shooterObject.transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                isAtWaypoint1 = true;
                isAtWaypoint2 = false;
            }
        }

        if ((isAtWaypoint1 && !isAtWaypoint2) && isPatrolling == true && isPursuing == false)
        {
            shooterObject.transform.LookAt(waypoint2.transform);
            if (Vector3.Distance(shooterObject.position, waypoint2.transform.position) > radiusOfSatisfaction)
            {
                float shooterYPos = transform.position.y;
                Vector3 customPos = waypoint2.transform.position;
                customPos.y = shooterYPos;
                shooterObject.transform.LookAt(customPos);
                shooterObject.transform.position += shooterObject.transform.forward * moveSpeed * Time.deltaTime;
            }
            else
            {
                isAtWaypoint2 = true;
                isAtWaypoint1 = false;
            }
        }

        if (Physics.Raycast(startPoint, transform.forward, out hitInfo, rayDistance, raycastLayers.value))
        {
            //shooterObject.transform.LookAt(playerObject);
            if (Vector3.Distance(shooterObject.position, playerObject.position) <= detectionDistance)
            {
                isPatrolling = false;
                isPursuing = true;
                float shooterYPos = transform.position.y;
                Vector3 customPos = playerObject.transform.position;
                customPos.y = shooterYPos;
                shooterObject.transform.LookAt(customPos);
                shooterObject.transform.position += shooterObject.transform.forward * moveSpeed * Time.deltaTime;
                if (Time.time > nextRound)
                {
                    nextRound = Time.time + fireRate;
                    myAnimator.SetBool("isFiring", true);
                    var projectileBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
                    projectileBullet.GetComponent<Rigidbody>().velocity = projectileBullet.transform.forward * 10;
                    Destroy(projectileBullet, 4f);
                }
            }
            else if(Vector3.Distance(shooterObject.position, playerObject.position) > detectionDistance)
            {
                isPatrolling = true;
                isPursuing = false;
            }

            myAnimator.SetBool("isFiring", false);

        }
        //health system
        if (shooterHealth <= 0 && !isDead)
        {
            isDead = true;
            isPatrolling = false;
            myAnimator.SetTrigger("isDeadTrigger");
            Destroy(shooterObj, 7f);
            movementScript.enabled = !movementScript.enabled;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 startPos = transform.position + new Vector3(0, 1, 0);
        Gizmos.DrawLine(startPos, transform.position + transform.forward * rayDistance);

    }
}

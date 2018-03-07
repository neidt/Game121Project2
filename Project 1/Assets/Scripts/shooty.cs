using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooty : MonoBehaviour
{
    //raycast stuff
    public LayerMask raycastLayers;
    private Vector3 rayCollisionNormal;
    public float rayDistance = 5.0f;
    public float detectionDistance = 5.0f;
    private float nextRound = 0.0f;
    public float fireRate = 3.0f;
    public float moveSpeed = 2.0f;
    public float radiusOfSatisfaction = 2.0f;

    public Transform playerObject;
    public Transform shooterObject;
    public Transform bulletSpawn;
    public GameObject bullet;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        RaycastHit hitInfo;
        if (Physics.Raycast(transform.position, transform.forward, out hitInfo, rayDistance, raycastLayers.value))
        {
            if (Vector3.Distance(shooterObject.position, playerObject.position) > detectionDistance && Time.time > nextRound)
            {
                float shooterYPos = transform.position.y;
                Vector3 customPos = playerObject.transform.position;
                customPos.y = shooterYPos;
                shooterObject.transform.LookAt(customPos);

                nextRound = Time.time + fireRate;
                var projectileBullet = Instantiate(bullet, bulletSpawn.position, bulletSpawn.rotation);
                projectileBullet.GetComponent<Rigidbody>().velocity = projectileBullet.transform.forward * 10;
                Destroy(projectileBullet, 4f);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * rayDistance);

    }
}

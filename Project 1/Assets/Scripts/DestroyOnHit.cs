using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnHit : MonoBehaviour
{
    public GameObject lazer;
    public GameObject winObject;
    public GameObject playerObject;
    public Transform winObjectPosition;

    private hasWon winnerObject;
    private PlayerMovement playerHealth;
    private PlayerMovement playerHealth2;
    private ShooterScript shooterHealth;

    private bool hasHit = false;

    // Use this for initialization
    void Start()
    {
        winnerObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<hasWon>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //playerHealth2 = GameObject.FindGameObjectWithTag("PlayerWScript").GetComponent<PlayerMovement>();
        shooterHealth = GameObject.FindGameObjectWithTag("Enemy").GetComponent<ShooterScript>();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        hasHit = true;

        Collider otherCollider = collision.collider;
        GameObject otherObject = collision.gameObject;
        Transform otherTransform = collision.transform;
        Rigidbody otherRigidbody = collision.rigidbody;

        Destroy(lazer);
        if (otherObject.tag != "Player")
        {
            shooterHealth.shooterHealth--;
        }
        //else if (otherObject.tag == "PlayerWScript" || otherObject.tag == "Player")
        //{
        //    playerHealth.playerHealth--;
        //}

        if (otherObject.tag == "Enemy")
        {
            winnerObject.objectsDestroyed++;
        }

        //if(otherCollider.tag == "Finish")
        //{
        //    winObject.gameObject.SetActive(false);
        //    playerObject.GetComponent<Camera>().transform.position = -playerObject.GetComponent<Camera>().transform.position;
        //}
    }


}
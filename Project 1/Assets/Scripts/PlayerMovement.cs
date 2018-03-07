using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2.0f; //speed of objects in unity units per second
    public float maxSpeed = 5.0f;
    public float rotationAngle = 2f;
    public float thrust = 2f;
    public float jumpCount = 0;
    public float maxJumps = 1;

    public Transform playerCamera;
    private GameObject endCamera;
    private GameObject mainCamera;
    public Transform playerModel;
    public Transform lazerSpawn;
    public GameObject lazer;
    public GameObject playerObject;
    public GameObject enemy;


    Rigidbody rb;
    public Vector3 jumpForce = new Vector3(0, .1f, 0);
    public Vector3 jumpVec = new Vector3(0, .5f, 0);
    bool airborne = false;

    public Collider bounds;
    public Collider winBounds;
    public Vector3 startPosition = new Vector3(0, 1.5f, -20f);

    private hasWon winnerObject;


    //animation stuff
    private Animator myAnimator;
    public bool isMoving;
    public bool isMovingBackThisFrame;
    public bool isMovingForwardThisFrame;
    public bool isJumping;
    public bool isFiringThisFrame;
    public bool isTurningLeft;
    public bool isTurningRight;
    public bool isRunning;
    bool isHit;

    //health stuff
    public float playerHealth = 5.0f;
    bool isDead = false;

    //sound stuff
    public AudioSource gotHit;
    public AudioSource fireball;

    // Use this for initialization
    void Start()
    {
        rb = transform.parent.GetComponent<Rigidbody>();
        myAnimator = transform.parent.GetComponent<Animator>();
        AudioSource[] audioList = GetComponentsInParent<AudioSource>();
        fireball = audioList[0];
        gotHit = audioList[1];
        winnerObject = GameObject.FindGameObjectWithTag("GameController").GetComponent<hasWon>();
        endCamera = GameObject.FindGameObjectWithTag("EndCamera");
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement movementScript = playerObject.GetComponentInChildren<PlayerMovement>();
        float xAxis = Input.GetAxis("Mouse X");
        float yAxis = Input.GetAxis("Mouse Y");
        float scrollWheel = Input.GetAxis("Mouse ScrollWheel");

        isMoving = false;
        isMovingForwardThisFrame = false;
        isMovingBackThisFrame = false;
        isJumping = false;
        isHit = false;

        //player movement
        if (Input.GetKey(KeyCode.W))
        {
            isMoving = true;
            if (playerModel.transform.forward != playerCamera.transform.forward)
            {
                Vector3 cameraForward = playerCamera.transform.forward;
                cameraForward.y = 0;
                playerModel.transform.forward = cameraForward;
                isMovingBackThisFrame = false;
                isMovingForwardThisFrame = true;

                if (Input.GetKey(KeyCode.LeftShift))
                {
                    isRunning = true;
                }
                else
                {
                    isRunning = false;
                }
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            isMoving = true;
            if (playerModel.transform.forward != playerCamera.transform.forward)
            {
                Vector3 cameraForward = playerCamera.transform.forward;
                cameraForward.y = 0;
                playerModel.transform.forward = cameraForward;
                isMovingForwardThisFrame = false;
                isMovingBackThisFrame = true;
            }
        }
        else if (Input.GetKey(KeyCode.D))
        {
            if (playerModel.transform.forward != playerCamera.transform.forward)
            {
                Vector3 cameraForward = playerCamera.transform.right;
                cameraForward.y = 0;
                playerModel.transform.forward = cameraForward;
                playerModel.transform.position = playerModel.transform.position + (cameraForward) * Time.deltaTime * moveSpeed;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            if (playerModel.transform.forward != playerCamera.transform.forward)
            {
                Vector3 cameraForward = playerCamera.transform.right;
                cameraForward.y = 0;
                playerModel.transform.forward = -cameraForward;
                playerModel.transform.position = playerModel.transform.position - (cameraForward) * Time.deltaTime * moveSpeed;
            }
        }

        myAnimator.SetBool("isMovingBack", isMovingBackThisFrame);
        myAnimator.SetBool("isMovingForward", isMovingForwardThisFrame);
        myAnimator.SetBool("isMoving", isMovingBackThisFrame || isMovingForwardThisFrame);
        myAnimator.SetBool("isRunning", isRunning);

        //jumping
        if (Input.GetKey(KeyCode.Space))
        {
            isJumping = true;
            jumpCount += 1;
        }
        else if (Input.GetKeyUp(KeyCode.Space)) //reset jumps
        {
            isJumping = false;
            jumpCount = 0;
        }

        myAnimator.SetBool("isJumping", isJumping);
       
        //camera rotation variables
        float eulerAngleLimit = playerCamera.transform.eulerAngles.x;
        float minview = -15f;
        float maxview = 60f;
        //float horizontalRot = xAxis * rotationAngle;

        //horizontal rotation?
        //playerObject.transform.Rotate(0, horizontalRot, 0);

        //vertical rotation
        if (eulerAngleLimit > 180)
        {
            eulerAngleLimit -= 360;
        }
        else if (eulerAngleLimit < -180)
        {
            eulerAngleLimit += 360;
        }

        float targetRotation = eulerAngleLimit + yAxis * -rotationAngle;

        if (xAxis > 0)
        {
            playerCamera.transform.Rotate(Vector3.up, rotationAngle, Space.World);
        }
        else if (xAxis < 0)
        {
            playerCamera.transform.Rotate(Vector3.up, -rotationAngle, Space.World);
        }

        if (targetRotation < maxview && targetRotation > minview)
        {
            playerCamera.transform.eulerAngles += new Vector3(yAxis * -rotationAngle, 0, 0);
        }

        //isFiringThisFrame = false;
        //projectiles
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isFiringThisFrame = true;
            fireball.Play();
            var projectileLazer = Instantiate(lazer, lazerSpawn.position, lazerSpawn.rotation);
            projectileLazer.GetComponent<Rigidbody>().velocity = projectileLazer.transform.forward * 30;
            Destroy(projectileLazer, 4f);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isFiringThisFrame = false;
        }

        myAnimator.SetBool("isFiring", isFiringThisFrame);


        //health system
        if (playerHealth <= 0 && !isDead)
        {
            isDead = true;
            myAnimator.SetTrigger("isDeadTrigger");
            isMoving = false;
            movementScript.enabled = !movementScript.enabled;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        float exitPointX = playerModel.transform.position.x;
        float exitPointZ = playerModel.transform.position.z;
        float playerY = playerModel.position.y;
        Vector3 entryPointX = new Vector3(-exitPointX, playerY, exitPointZ);
        Vector3 entryPointZ = new Vector3(exitPointX, playerY, -exitPointZ);

        //if player exits from the pos x, start them at the -x, but at the same z
        if (exitPointX >= (bounds.transform.localScale.x) / 2)
        {
            playerModel.transform.position = entryPointX;
        }
        else if (exitPointX <= -(bounds.transform.localScale.x) / 2)
        {
            playerModel.transform.position = entryPointX;
        }
        //if player exits from pos z, starty them at -z but with same x
        if (exitPointZ >= (bounds.transform.localScale.z) / 2)
        {
            playerModel.transform.position = entryPointZ;
        }
        else if (exitPointZ <= -(bounds.transform.localScale.z) / 2)
        {
            playerModel.transform.position = entryPointZ;
        }
    }

    public void OnDragonTriggerEnter(Collider other)
    {
        //ShooterScript shooty = enemy.GetComponent<ShooterScript>();
        //Collision otherCollision = Collision;
        PlayerMovement movementScript = playerObject.GetComponent<PlayerMovement>();
        print("colliding with" + other.tag);
        if (other.gameObject.tag == "Finish")
        {
            print("into winner winner chicken dinner");
            other.gameObject.SetActive(false);
            movementScript.enabled = !movementScript.enabled;
        }
    }

    public void OnDragonCollisionEnter(Collision collision)
    {
        Collider otherCollider = collision.collider;
        GameObject otherObject = collision.gameObject;
        Transform otherTransform = collision.transform;
        Rigidbody otherRigidbody = collision.rigidbody;

        print("colliding with" + otherObject.tag);
        if (otherObject.tag == "Finish")
        {
            otherObject.gameObject.SetActive(false);
            //this.gameObject.SetActive(false);
            endCamera.SetActive(true);
            //mainCamera.SetActive(false);
           
        }
        if (otherCollider.gameObject.tag == "Bullet")
        {
            gotHit.Play();
            playerHealth--;
            myAnimator.SetBool("isHit", true);
        }
    }
}


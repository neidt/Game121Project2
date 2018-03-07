using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hasWon : MonoBehaviour
 {
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject winObject;
    public Transform winObjectTransform;
    public int objectsDestroyed = 0;

    private bool isCreated = false;
    // Use this for initialization
    void Start ()
    {
       
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (objectsDestroyed == 1)
        {
            if (isCreated) return;
            isCreated = true;
            //winObject.SetActive(true);
            var winner = Instantiate(winObject, winObjectTransform.transform.position, winObjectTransform.transform.rotation);
        }
    }
}

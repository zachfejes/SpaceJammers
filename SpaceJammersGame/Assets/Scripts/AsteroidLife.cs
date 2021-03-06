﻿using UnityEngine;
using System.Collections;

public class AsteroidLife : MonoBehaviour {

    public Vector2 initialVector;
    public int asteroidClass = 1;
    public float initialSpeed = 0;
    public int status = 0;  // possible statuses: 0 = unscanned, 1 = rocket launched, 2 = rocket attached, 3 = science rocket launched, 4 = acquired and descending
    public bool inScience = false;
    public Vector2 mousePos1;
    public Vector2 mousePos2;
    public float asteroidThrust = 1;
    Animator animator;


	// Use this for initialization
	void Start () {
        // Get the animator component
        animator = GetComponent<Animator>();
        animator.SetInteger("status", status);
        animator.SetInteger("asteroidClass", asteroidClass);
        
        // Create a random initial velocity (also an apoapsis)
        initialVector.Normalize();
        transform.GetComponent<Rigidbody2D>().velocity = initialSpeed * initialVector;
	}

	// Update is called once per frame
	void Update () {
        animator.SetInteger("status", status);  //update the animation appropriately
        animator.SetInteger("asteroidClass", asteroidClass);

        if (transform.position.magnitude <= 3) {
            inScience = true;
        }
        else {
            inScience = false;
        }
        /*
        if (status == 0) {       // As yet unscanned

        }
        else if (status == 1) {  // Rocket Launched and Inbound

        }
        else if (status == 2) {  // Rocket Attached

        }
        else if (status == 3) {  // Science Rocket Launched

        }
        else if (status == 4) {  // Acquired and Descending
        } */
	}

    void OnMouseDown() {
        Debug.Log("Asteroid Clicked!");
        // Regardless of where we are, if we're in the Science Zone, clicking should launch a science rocket for capture
        if (inScience) {
            status = 3;
            GameObject scienceRocket = (GameObject)Instantiate(Resources.Load("ScienceRocket"));
            scienceRocket.GetComponent<ScienceRocketCatchup>().setTarget(transform);
        }
        // Send Rocket to Asteroid to attach Thruster, set status to 1
        if (status == 0) {
            status = 1;
            GameObject rocket = (GameObject)Instantiate(Resources.Load("Rocket"));
            rocket.GetComponent<RocketCatchup>().setTarget(transform);
        }
        else if (status == 2) {
            mousePos1 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }

        // If asteroid is in the science zone, clicks launch another rocket
    }

    void OnMouseUp() {
        if (status == 2) {
            mousePos2 = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 thrustVector = (mousePos2 - mousePos1).normalized;
            Debug.Log("THRUSTING. Thrust Vector: " + thrustVector);
            transform.GetComponent<Rigidbody2D>().AddForce(thrustVector * asteroidThrust);
        }
    }


}

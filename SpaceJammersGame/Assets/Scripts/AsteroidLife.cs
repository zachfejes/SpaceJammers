using UnityEngine;
using System.Collections;

public class AsteroidLife : MonoBehaviour {

    public Vector2 initialVector;
    public float initialSpeed = 0;
    public int status = 0;  // possible statuses: 0 = unscanned, 1 = rocket launched, 2 = rocket attached, 3 = in science zone, 4 = acquired


	// Use this for initialization
	void Start () {
        // Create a random initial velocity (also an apoapsis)
        initialVector.Normalize();
        transform.GetComponent<Rigidbody2D>().velocity = initialSpeed * initialVector;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnMouseDown() {
        Debug.Log("Asteroid Clicked!");
        // Send Rocket to Asteroid to attach Thruster, set status to 1
        if (status == 0) {
            status = 1;
            GameObject rocket = (GameObject)Instantiate(Resources.Load("Rocket"));
            rocket.GetComponent<RocketCatchup>().setTarget(transform);
        }

 
        // Once Rocket has attached, set Status to 2, and update sprite, now if user clicks and drags, accelerate asteroid

        // If asteroid is in the science zone, clicks launch another rocket
    }

}

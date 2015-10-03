using UnityEngine;
using System.Collections;

public class AsteroidLife : MonoBehaviour {

    public Vector2 initialVector;
    public float initialSpeed = 0;
    public int status = 0;  // possible statuses: 0 = unscanned, 1 = scanned, 2 = rocket attached, 3 = in science zone, 4 = acquired


	// Use this for initialization
	void Start () {
        // Create a random initial velocity (also an apoapsis)
        initialVector.Normalize();
        transform.GetComponent<Rigidbody2D>().velocity = initialSpeed * initialVector;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

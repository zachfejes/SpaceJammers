using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {

	public float targetGrav;
	public float asteroidGrav;
    public GameObject target;
    public float pullForce;

	// Use this for initialization
	void Start () {
	    // Create a random initial velocity (also an apoapsis)

	}
	
	// Update is called once per frame
	void Update () {
        //calculate direction to target
		Vector2 forceDirection = (Vector2)(target.transform.position - transform.position);

        //apply gravetic acceleration
        transform.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized*pullForce*Time.fixedDeltaTime);
	}
}

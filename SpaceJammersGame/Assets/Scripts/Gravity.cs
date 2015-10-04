using UnityEngine;
using System.Collections;

public class Gravity : MonoBehaviour {

	public float targetMass = 5.972e24f;
	public float asteroidMass = 4e10f;
    public GameObject target;
    public float gravForce;
    float G = 6.674e-11f; // N*(m/kg)^2
    float universeScale = 40000000/10;
    public float timeScale = 1;
    public bool inAtmosphere = false;
    public float atmosphereBoundary;
    public float atmoDrag = 0;

	// Use this for initialization
	void Start () {
        target = GameObject.FindGameObjectWithTag("Earth");
	}
	
	// Update is called once per frame
	void Update () {
        //calculate direction to target (not yet normalized)
		Vector2 forceDirection = universeScale*(Vector2)(target.transform.position - transform.position);

        //calculate scalar force component
        gravForce = G * targetMass * asteroidMass / Vector2.SqrMagnitude(forceDirection); //////// Working on this.

        //apply gravetic acceleration (vector)
        transform.GetComponent<Rigidbody2D>().AddForce(forceDirection.normalized*gravForce*(timeScale*Time.fixedDeltaTime));
        //Debug.Log(Time.fixedDeltaTime);

       // Debug.Log(forceDirection.magnitude/universeScale);
        if (forceDirection.magnitude/universeScale < atmosphereBoundary)
            inAtmosphere = true;
        else
            inAtmosphere = false;

        if (inAtmosphere)
            GetComponent<Rigidbody2D>().drag = atmoDrag;
        else
            GetComponent<Rigidbody2D>().drag = 0;

	}
}

using UnityEngine;
using System.Collections;

public class RocketCatchup : MonoBehaviour {

    public Vector3 initialPosition;
    public Vector3 targetPosition;
    public Transform target;
    public float speed = 1;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        if (target) {
            initialPosition = transform.position;
            targetPosition = target.position;
            transform.position = Vector3.Slerp(initialPosition, targetPosition, speed*Time.smoothDeltaTime);
        }
	}

    public void setTarget(Transform temp) {
        target = temp;
    }

    void OnCollisionEnter2D(Collision2D collider) {
        target.GetComponent<AsteroidLife>().status = 2;
        Destroy(gameObject);
    }
}

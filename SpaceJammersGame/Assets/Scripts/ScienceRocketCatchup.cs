using UnityEngine;
using System.Collections;

public class ScienceRocketCatchup : MonoBehaviour {

    public Vector3 initialPosition;
    public Vector3 targetPosition;
    public Transform target;
    public float speed = 1;
    float angle;
    Vector3 lookPosition;

    void Start()
    {

    }

    void Update()
    {
        if (target)
        {
            //Rotate the rocket to aim toward the asteroid
            lookPosition = target.position - transform.position;
            angle = Mathf.Atan2(lookPosition.y, lookPosition.x) * Mathf.Rad2Deg - 90;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            //Follow the asteroid
            initialPosition = transform.position;
            targetPosition = target.position;
            transform.position = Vector3.Slerp(initialPosition, targetPosition, speed * Time.smoothDeltaTime);
        }
    }

    public void setTarget(Transform temp)
    {
        target = temp;
    }

    void OnCollisionEnter2D(Collision2D collider)
    {
        target.GetComponent<AsteroidLife>().status = 4;
        GameObject.FindGameObjectWithTag("Earth").GetComponent<gameController>().SciencePoints += 100;
        Destroy(collider.gameObject);
        Destroy(gameObject);
    }
}

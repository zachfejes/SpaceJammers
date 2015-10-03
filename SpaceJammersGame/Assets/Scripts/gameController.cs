using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {

    // The scale factor we will be using (just as a standard) will be that 10 distance units = 40000km = 40000000m (appx. Geo-orbit). This assumes that we are working in [m].
    static float universeScale = 10/40000000; //[m]
    float atmoBoundary = 2 / universeScale;
    float scienceBoundary = 3 / universeScale;
    float zoomLevel = -7;
    int scienceLevel = 0;
    int score = 0;
    public int life = 5;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    //when a collision is detected, do things!
    void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log ("Asteroid Collision!");
        life = life - 1;
        DestroyObject(collision.gameObject);
    }


}

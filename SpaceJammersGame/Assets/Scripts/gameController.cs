using UnityEngine;
using System.Collections;

public class gameController : MonoBehaviour {
    public const int MAX_EARTH_HEALTH = 10;

    // USER VARIABLES
    public int EarthHealth;
    public int Score;
    public int SciencePoints;
    public float zoomLevel = -7;

    // EARTH
    // The scale factor we will be using (just as a standard) will be that 10 distance units = 40000km = 40000000m (appx. Geo-orbit). This assumes that we are working in [m].
    static float universeScale = 10/40000000; //[m]
    float atmoBoundary = 2 / universeScale;
    float scienceBoundary = 3 / universeScale;

	// Use this for initialization
	void Start () {
        // Init Base points
	    EarthHealth = MAX_EARTH_HEALTH;
        Score = 0;
        SciencePoints = 0;

        // Generate Asteroids
        // TODO: Currently generates every 5 seconds; want to randomly generate
        InvokeRepeating("CreateNewAsteroid", 0, 5);
	}
	
	// Update is called once per frame
	void Update () {
	}

    //when a collision is detected, do things!
    void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log ("Asteroid Collision!");
        EarthHealth = EarthHealth - 1;
        DestroyObject(collision.gameObject);
    }

    // Generate asteroids from prefab
    void CreateNewAsteroid() {
        Debug.Log ("New Asteroid!");

        // Instantiate
        GameObject newAsteroid = Instantiate(Resources.Load("Asteroid")) as GameObject;
        newAsteroid.transform.position = new Vector3(Random.Range(-10.0F, 10.0F), Random.Range(-10.0F, 10.0F), 0);
    }
}

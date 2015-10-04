using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class gameController : MonoBehaviour {
    public const string EARTH_HEALTH_TEXT = "Earth Health: ";
    public const string SCORE_TEXT = "Score: ";
    public const string SCIENCE_POINTS_TEXT = "Science Points: ";
    public const int MAX_EARTH_HEALTH = 100;
    public const float SMALL_MASS = 1000F;
    public const float MED_MASS = 4000F;
    public const float LARGE_MASS = 8000F;
    public static float[] masses = new float[3] {SMALL_MASS, MED_MASS, LARGE_MASS};
    public static Dictionary<float, int> impacts = new Dictionary<float, int>() {
        {SMALL_MASS, 5},
        {MED_MASS, 10},
        {LARGE_MASS, 20}
    };

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
        SetEarthHealthText();
        SetScoreText();
        SetSciencePointsText();
	}

    //when a collision is detected, do things!
    void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log ("Asteroid Collision!");
        float astMass = collision.gameObject.GetComponent<Gravity>().asteroidMass;
        if(astMass == SMALL_MASS) {
                EarthHealth = EarthHealth - impacts[SMALL_MASS];
        }
        else if(astMass == MED_MASS) {
                EarthHealth = EarthHealth - impacts[MED_MASS];
        }
        else if(astMass == LARGE_MASS) {
                EarthHealth = EarthHealth - impacts[LARGE_MASS];
        }
        DestroyObject(collision.gameObject);
    }

    // Generate asteroids from prefab
    void CreateNewAsteroid() {
        Debug.Log ("New Asteroid!");
        // Instantiate
        GameObject newAsteroid = Instantiate(Resources.Load("Asteroid")) as GameObject;
        newAsteroid.transform.position = new Vector3(Random.Range(-1.0F, 1.0F), Random.Range(-1.0F, 1.0F), 0)*10.0F;
        newAsteroid.GetComponent<Gravity>().asteroidMass = masses[ChooseMass()];
        newAsteroid.GetComponent<AsteroidLife>().initialVector = GetNormal2DVector(-newAsteroid.transform.position);
    }

    int ChooseMass() {
        return Random.Range(1, 100) % masses.Length;
    }

    Vector2 GetNormal2DVector(Vector3 v) {
        if(Random.Range(1,100) % 2 == 0) {
            return new Vector2(-v.x, v.y);
        }
        return new Vector2(v.x, -v.y);
    }

    void SetEarthHealthText() {
        GameObject.FindWithTag("EarthHealth").GetComponent<Text>().text = EARTH_HEALTH_TEXT + EarthHealth.ToString();
    }

    void SetScoreText() {
        GameObject.FindWithTag("Score").GetComponent<Text>().text = SCORE_TEXT + Score.ToString();
    }

    void SetSciencePointsText() {
        GameObject.FindWithTag("SciencePoints").GetComponent<Text>().text = SCIENCE_POINTS_TEXT + SciencePoints.ToString();
    }
}

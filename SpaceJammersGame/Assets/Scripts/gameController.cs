using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class gameController : MonoBehaviour {
    // CONSTANTS
    public const string EARTH_HEALTH_TEXT = "Earth Health: ";
    public const string SCORE_TEXT = "Score: ";
    public const string SCIENCE_POINTS_TEXT = "Science Points: ";

    public const int MAX_EARTH_HEALTH = 100;

    public struct Asteroid {
        public float mass, scale;
        public int impact;

        public Asteroid(float mass, int impact, float scale) {
            this.mass = mass;
            this.impact = impact;
            this.scale = scale;
        }
    }
    public const float SMALL_MASS = 1000F;
    public const float MED_MASS = 4000F;
    public const float LARGE_MASS = 8000F;
    public static Dictionary<int, Asteroid> asteroidClassInfo =
        new Dictionary<int, Asteroid> {
            {
                1,
                new Asteroid(SMALL_MASS, 5, 0.1F)
            },
            {
                2,
                new Asteroid(MED_MASS, 10, 0.2F)
            },
            {
                3,
                new Asteroid(LARGE_MASS, 15, 0.3F)
            },
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
        EarthHealth = EarthHealth - asteroidClassInfo[1].impact;
        // TODO: Update score
        // TODO: Update science
        DestroyObject(collision.gameObject);
    }

    // Generate asteroids from prefab
    void CreateNewAsteroid() {
        Debug.Log ("New Asteroid!");
        // Instantiate
        GameObject newAsteroid = Instantiate(Resources.Load("Asteroid")) as GameObject;
        newAsteroid.transform.position = new Vector3(Random.Range(-1.0F, 1.0F), Random.Range(-1.0F, 1.0F), 0)*10.0F;

        int asteroidClass = ChooseAsteroidClass();
        newAsteroid.GetComponent<AsteroidLife>().asteroidClass = asteroidClass;
        newAsteroid.GetComponent<AsteroidLife>().initialVector = GetNormal2DVector(-newAsteroid.transform.position);
        newAsteroid.GetComponent<Gravity>().asteroidMass = asteroidClassInfo[asteroidClass].mass;
        newAsteroid.transform.localScale = new Vector3(asteroidClassInfo[asteroidClass].scale, asteroidClassInfo[asteroidClass].scale, asteroidClassInfo[asteroidClass].scale);
    }

    int ChooseAsteroidClass() {
        return Random.Range(1, 100) % asteroidClassInfo.Count;
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

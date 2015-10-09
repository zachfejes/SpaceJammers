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
    public const int MAX_ASTEROIDS = 20;

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
                new Asteroid(MED_MASS, 20, 0.15F)
            },
            {
                3,
                new Asteroid(LARGE_MASS, 100, 0.2F)
            },
        };

    // USER VARIABLES
    public int EarthHealth;
    public int Score;
    public int SciencePoints;
    public float zoomLevel = -7;
    public bool gameOverFlag = false;

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

        // If we've run out of health, run the GameOver() function only once.
        if (EarthHealth <= 0 && !gameOverFlag) {
            gameOverFlag = true;
            GameOver();
        }

        if (SciencePoints == 0) {
            zoomLevel = -14;
           // transform.Find("Science Zone Boundary").transform.localScale.Set(0.47f, 0.47f, 0.47f);
        }
        else {
            zoomLevel = -14 - (float)SciencePoints/200;
           // transform.Find("Science Zone Boundary").transform.localScale.Set(0.47f * SciencePoints / 200, 0.47f * SciencePoints / 200, 0.47f * SciencePoints / 200);
        }
        //scienceBoundary = (3 / universeScale)*SciencePoints
        Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, new Vector3(0, 0, zoomLevel), Time.smoothDeltaTime);
	}

    //when a collision is detected, do things!
    void OnCollisionEnter2D(Collision2D collision) {
		Debug.Log ("Asteroid Collision!");
        float astMass = collision.gameObject.GetComponent<Gravity>().asteroidMass;
        EarthHealth = EarthHealth - asteroidClassInfo[collision.gameObject.GetComponent<AsteroidLife>().asteroidClass].impact;
        // TODO: Update score
        // TODO: Update science
        DestroyObject(collision.gameObject);
    }

    // Generate asteroids from prefab
    void CreateNewAsteroid() {
        if(GameObject.FindGameObjectsWithTag("Asteroid").Length == MAX_ASTEROIDS) { return; }
        Debug.Log ("New Asteroid!");
        // Instantiate
        GameObject newAsteroid = Instantiate(Resources.Load("Asteroid")) as GameObject;
        //we want to create the asteroids within more of a 'frame' around the square, where 10.0f points is the inner boundary, and 12.0f points is the outer boundary
        int rand1 = Random.Range(0,2);
        if (rand1 == 0) rand1 = -1;
        int rand2 = Random.Range(0, 2);
        if (rand2 == 0) rand2 = -1;
        float x_position = rand1*Random.Range(-1.8F, -2.0F) * 10.0F; //left frame
        float y_position = Random.Range(-1.0F, 1.0F) * 10.0F; //Anywhere in those frames
        newAsteroid.transform.position = new Vector3(x_position, y_position, 0); //This puts it 'within' the square

        int asteroidClass = ChooseAsteroidClass();
        newAsteroid.GetComponent<AsteroidLife>().asteroidClass = asteroidClass;
        newAsteroid.GetComponent<AsteroidLife>().initialVector = new Vector2(0-newAsteroid.transform.position.x, Random.Range(3.0F,8.0F)*rand2-newAsteroid.transform.position.y);
        newAsteroid.GetComponent<Gravity>().asteroidMass = asteroidClassInfo[asteroidClass].mass;
        newAsteroid.transform.localScale = new Vector3(asteroidClassInfo[asteroidClass].scale, asteroidClassInfo[asteroidClass].scale, asteroidClassInfo[asteroidClass].scale);
    }

    int ChooseAsteroidClass() {
        return Random.Range(1, 100) % asteroidClassInfo.Count + 1;
    }
    
    //depreciated
 /*   Vector2 GetNormal2DVector(Vector3 v) {
        if(Random.Range(1,100) % 2 == 0) {
            return new Vector2(-v.x, v.y);
        }
        return new Vector2(v.x, -v.y);
    }*/

    void SetEarthHealthText() {
        GameObject.FindWithTag("EarthHealth").GetComponent<Text>().text = EARTH_HEALTH_TEXT + EarthHealth.ToString();
    }

    void SetScoreText() {
        GameObject.FindWithTag("Score").GetComponent<Text>().text = SCORE_TEXT + Score.ToString();
    }

    void SetSciencePointsText() {
        GameObject.FindWithTag("SciencePoints").GetComponent<Text>().text = SCIENCE_POINTS_TEXT + SciencePoints.ToString();
    }

    void GameOver() {
        Debug.Log("GameOver");
        GameObject GameOverText = Instantiate(Resources.Load("GameOverUI")) as GameObject;
        GameObject MainMenuButton = Instantiate(Resources.Load("MainMenuButton")) as GameObject;
        GameObject TryAgainButton = Instantiate(Resources.Load("TryAgainButton")) as GameObject;
        GameOverText.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
        MainMenuButton.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
        TryAgainButton.transform.SetParent(GameObject.FindGameObjectWithTag("UI").transform);
    }
}

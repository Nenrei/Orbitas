using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnPlanet : MonoBehaviour
{
    public static SpawnPlanet Instance { get; private set; }

    [SerializeField] private GameObject planetPrefab;
    [SerializeField] private float gravityMultiplier = 20f;
    [SerializeField] private float speedMultiplier = 2000f;

    private List<GameObject> planets;
    private Camera mainCamera;
    PhysicsMaterial2D ball_bounciness;

    private void Awake()
    {
        Instance = this;
        planets = new List<GameObject>();
        ball_bounciness = new PhysicsMaterial2D("bounce");
        ball_bounciness.bounciness = ParametersHandler.Instance.GetBounciness();
        ball_bounciness.friction = 0;
    }

    private void Start()
    {
        ParametersHandler.Instance.SetGravityText(gravityMultiplier);
        ParametersHandler.Instance.SetSpeedText(speedMultiplier);
        ParametersHandler.Instance.SetTrailText(GetTrail());
        ParametersHandler.Instance.SetBounceText(0);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            GameObject newPlanet = GameObject.Instantiate(planetPrefab, GetWorldMousePosition(), Quaternion.identity);

            float size = Random.Range(0.5f, 2f);
            newPlanet.transform.localScale = new Vector3(size, size, 1);

            CircleCollider2D collider = newPlanet.GetComponent<CircleCollider2D>();
            collider.enabled = ParametersHandler.Instance.IsCollisionToggleOn();
            collider.radius = size * 5;

            newPlanet.GetComponent<Rigidbody2D>().mass = size;
            newPlanet.GetComponent<TrailRenderer>().time = GetTrail();

            newPlanet.GetComponent<CircleCollider2D>().sharedMaterial = ball_bounciness;

            planets.Add(newPlanet);
        }
    }


    private Vector3 GetWorldMousePosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        return mouseWorldPosition;
    }

    public float GetGravityMultiplier()
    {
        return gravityMultiplier;
    }

    public float GetSpeedMultiplier()
    {
        return speedMultiplier;
    }

    public float GetTrail()
    {
        if(planets.Count > 0)
        {
            return planets[0].GetComponent<TrailRenderer>().time;
        }

        return 10;
    }

    public void SetGravityMultiplier(float newGravity)
    {
        gravityMultiplier = newGravity;
    }

    public void SetSpeedMultiplier(float newSpeed)
    {
        speedMultiplier = newSpeed;
    }

    public void SetTrail(float newTrail)
    {
        foreach (GameObject planet in planets)
        {
            planet.GetComponent<TrailRenderer>().time = newTrail;
        }
    }

    public void SetBounciness(float newBounce)
    {
        ball_bounciness.bounciness = newBounce;

        foreach (GameObject planet in planets)
        {
            planet.GetComponent<CircleCollider2D>().sharedMaterial = ball_bounciness;
        }
    }

    public void EnableCollisions(bool enable)
    {
        foreach (GameObject planet in planets)
        {
            planet.GetComponent<CircleCollider2D>().enabled = enable;
        }
    }

    public void RemovePlanets()
    {
        foreach (GameObject planet in planets)
        {
            Destroy(planet);
        }
        planets.Clear();
    }


}

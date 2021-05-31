using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitsPlanet2d : MonoBehaviour
{

    [System.Serializable]
    public class PlanetSpriteSheet
    {
        public Sprite[] planetSpriteSheet;
    }

    private float initSpeed;

    [SerializeField] PlanetSpriteSheet[] planetSprites;

    private Vector3 parentPosition;
    private Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        parentPosition = Vector3.zero;
    }
    private void Start()
    {
        TrailRenderer tr = GetComponent<TrailRenderer>();
        tr.startColor = new Color(1, 1, 1, 0.5f);
        tr.endColor = new Color(0, 0, 0, 0);

        SetVelocity();

        StartCoroutine(AnimatePlanet());
    }

    private void FixedUpdate()
    {
        Vector3 difference = parentPosition - transform.position;
        Vector3 gravityDirection = difference.normalized;

        float forceG = 6.7f * (500 * rb.mass * SpawnPlanet.Instance.GetGravityMultiplier()) / (difference.magnitude * difference.magnitude);

        rb.AddForce(gravityDirection * forceG, ForceMode2D.Impulse);

    }

    private void SetVelocity()
    {
        Vector3 difference = parentPosition - transform.position;
        Vector3 initVelVector = Vector3.zero;

        initVelVector.x = difference.y;
        initVelVector.y = -difference.x;

        Vector3 gravityDirection = initVelVector.normalized;

        initSpeed = (6.7f * rb.mass * SpawnPlanet.Instance.GetSpeedMultiplier()) / difference.magnitude;

        rb.velocity = gravityDirection * initSpeed;
    }

    private IEnumerator AnimatePlanet()
    {
        Sprite planetSprite;
        int planet = Random.Range(0, planetSprites.Length);
        int index = 0;
        while (true)
        {
            planetSprite = planetSprites[planet].planetSpriteSheet[index];

            GetComponent<SpriteRenderer>().sprite = planetSprite;
            index++;
            if (index == planetSprites[planet].planetSpriteSheet.Length) index = 0;
            yield return new WaitForSeconds(0.1f);
        }
    }


}

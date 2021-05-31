using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePlanet : MonoBehaviour
{

    [System.Serializable]
    public class PlanetSpriteSheet
    {
        public Sprite[] planetSpriteSheet;
    }

    [SerializeField] PlanetSpriteSheet[] planetSprites;

    private void Start()
    {

        StartCoroutine(AnimatePlanetSprite());
    }

    private IEnumerator AnimatePlanetSprite()
    {
        Sprite planetSprite;
        int planet = Random.Range(0, planetSprites.Length);
        int index = 0;
        while (true)
        {
            planetSprite = planetSprites[planet].planetSpriteSheet[index];

            if (planetSprite.rect.width > 0)
            {
                Debug.Log("saturno");
            }

            GetComponent<SpriteRenderer>().sprite = planetSprite;
            index++;
            if (index == planetSprites[planet].planetSpriteSheet.Length) index = 0;
            yield return new WaitForSeconds(0.1f);
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class ParametersHandler : MonoBehaviour
{
    public static ParametersHandler Instance { get; private set; }

    [SerializeField] private Toggle collisionToggle;
    [SerializeField] private TMP_InputField gravityText;
    [SerializeField] private TMP_InputField speedText;
    [SerializeField] private TMP_InputField trailText;
    [SerializeField] private TMP_InputField bounceText;

    [SerializeField] private TextMeshProUGUI zoomText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        collisionToggle.onValueChanged.AddListener(delegate {
            OnCollisionToggleValueChange(collisionToggle);
        });

        gravityText.onValueChanged.AddListener(delegate {
            OnGravityValueChange();
        });
        speedText.onValueChanged.AddListener(delegate {
            OnSpeedValueChange();
        });
        trailText.onValueChanged.AddListener(delegate {
            OnTrailValueChange();
        });
        bounceText.onValueChanged.AddListener(delegate {
            OnBounceValueChange();
        });

    }

    private void OnCollisionToggleValueChange(Toggle toggle)
    {
        SpawnPlanet.Instance.EnableCollisions(toggle.isOn);
    }

    private void OnGravityValueChange()
    {
        if (gravityText.text.Length > 0)
        {
            SpawnPlanet.Instance.SetGravityMultiplier(float.Parse(gravityText.text));
        }
    }

    private void OnSpeedValueChange()
    {
        if (speedText.text.Length > 0)
        {
            SpawnPlanet.Instance.SetSpeedMultiplier(float.Parse(speedText.text));
        }
    }

    private void OnTrailValueChange()
    {
        if (trailText.text.Length > 0)
        {
            SpawnPlanet.Instance.SetTrail(float.Parse(trailText.text));
        }
    }

    private void OnBounceValueChange()
    {
        if (bounceText.text.Length > 0)
        {
            SpawnPlanet.Instance.SetBounciness(float.Parse(bounceText.text));
        }
        else
        {
            SpawnPlanet.Instance.SetBounciness(0);
        }
    }


    public void SetGravityText(float value)
    {
        gravityText.text = value.ToString();
    }

    public void SetSpeedText(float value)
    {
        speedText.text = value.ToString();
    }

    public void SetTrailText(float value)
    {
        trailText.text = value.ToString();
    }
    public void SetBounceText(float value)
    {
        bounceText.text = value.ToString();
    }

    public bool IsCollisionToggleOn()
    {
        return collisionToggle.isOn;
    }

    public float GetBounciness()
    {
        if (bounceText.text.Length == 0) return 0;

        return float.Parse(bounceText.text);
    }

    public void SetZoom(float zoomPercent)
    {
        zoomText.text = $"( {zoomPercent.ToString("F2")}% )";
    }


}

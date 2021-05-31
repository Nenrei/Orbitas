using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class CameraHandler : MonoBehaviour {

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D dragCursor;
    [SerializeField] private Texture2D arrowCursor;

    private float orthographicSize;
    private float targetOrthographicSize;
    private CinemachineFramingTransposer transposer;
    private Camera mainCamera;

    private void Start()
    {
        orthographicSize = cinemachineVirtualCamera.m_Lens.OrthographicSize;
        targetOrthographicSize = orthographicSize;
        transposer = cinemachineVirtualCamera.GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SpawnPlanet.Instance.RemovePlanets();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1;
            }
        }

    }

    private void HandleMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        if (x != 0 || y != 0)
        {
            Vector3 moveDir = new Vector3(x, y, 0).normalized;
            float moveSpeed = 400f;
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }
        else
        {
            if (Input.GetMouseButtonDown(2))
            {
                transposer.m_XDamping = 0;
                transposer.m_YDamping = 0;
                Cursor.SetCursor(dragCursor, Vector2.zero ,CursorMode.Auto);
            }

            if (Input.GetMouseButton(2))
            {
                transform.position += (new Vector3(-Input.GetAxis("Mouse X"), -Input.GetAxis("Mouse Y"), 0)) * 5f;
            }

            if (Input.GetMouseButtonUp(2))
            {
                transposer.m_XDamping = 1;
                transposer.m_YDamping = 1;
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            }
        }

        if (EventSystem.current.IsPointerOverGameObject())
        {
            Cursor.SetCursor(arrowCursor, Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
        }


    }

    private void HandleZoom()
    {
        float zoomAmount = 20f;
        targetOrthographicSize += -Input.mouseScrollDelta.y * zoomAmount;

        float minOrthographicSize = 30;
        float maxOrthographicSize = 1000;
        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, minOrthographicSize, maxOrthographicSize);

        float zoomSpeed = 5f;
        orthographicSize = Mathf.Lerp(orthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);

        cinemachineVirtualCamera.m_Lens.OrthographicSize = orthographicSize;

        ParametersHandler.Instance.SetZoom((orthographicSize / maxOrthographicSize) * 100);
    }

    private Vector3 GetWorldMousePosition()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;
        return mouseWorldPosition;
    }
}

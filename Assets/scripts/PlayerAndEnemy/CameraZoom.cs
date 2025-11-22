using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AllControl;

public class CameraZoom : MonoBehaviour
{
    public float minSize = 2f;
    public float maxSize = 10f;
    public float zoomSpeed = 2f;
    private Camera mainCamera;
    // Start is called before the first frame update
    void Start()
    {
        minSize = GameManager.Instance.CameraMinSize;
        maxSize = GameManager.Instance.CameraMaxSize;
        zoomSpeed = GameManager.Instance.CameraZoomSpeed;
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.mouseScrollDelta.y;
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - scroll * zoomSpeed, minSize, maxSize);
    }
}

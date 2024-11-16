using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    private bool isTouching = false;
    private Vector3 touchStartPosition;
    private Vector3 touchEndPosition;
    public float timerTouch = 0.5f;
    public float lastTouch;
    private bool isClicking = false;
    private float clickTime = 0f;
    private float requiredHoldTime = 0.25f; // in seconds

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnClicked, OnExit;

    private void Update()
    {
        if (Input.touchCount == 1 && !Input.mousePresent)
        {
            Debug.Log("touch");
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                touchStartPosition = touch.position;
                isTouching = true;
            }

            if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (isTouching)
                {
                    touchEndPosition = touch.position;
                    float touchDistance = Vector3.Distance(touchStartPosition, touchEndPosition);
                    float touchThreshold = 40f;
                    isTouching = false;
                    if (touchDistance < touchThreshold)
                    {
                        OnClicked?.Invoke();
                    }
                }
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            touchStartPosition = Input.mousePosition;
            isTouching = true;
        }
        if (Input.GetMouseButtonUp(0))
        {
            isClicking = false;
            Vector3 releasePosition = Input.mousePosition;
            float dragDistance = Vector3.Distance(touchStartPosition, releasePosition);
            if (dragDistance < 30f)
            {
                OnClicked?.Invoke();
            }
        }    

        
        //if (Input.GetKeyDown(KeyCode.Escape))
        //    OnExit?.Invoke();
    }

    public bool IsPointerOverUI()
        => EventSystem.current.IsPointerOverGameObject();

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition; 
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePan : MonoBehaviour
{
    private Vector3 touchStart;
    public Camera cam;
    public float groundZ = 5;
    public bool shopIsOpen = false;
    private float initialPinchDistance;
    private float zoomSpeed = 0.1f;
    // Update is called once per frame
    void Update()
    {
        if (!shopIsOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchStart = getWorldPosition(groundZ);
                //print(touchStart);
            }
            if (Input.GetMouseButton(0))
            {
                Vector3 direction = touchStart - getWorldPosition(groundZ);
                direction.y = 0;
                Camera.main.transform.position += direction;
            }
            if (Input.touchCount == 2)
            {
                Touch touch1 = Input.GetTouch(0);
                Touch touch2 = Input.GetTouch(1);

                if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
                {
                    
                    initialPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                }
                else if (touch1.phase == TouchPhase.Moved || touch2.phase == TouchPhase.Moved)
                {
                    float currentPinchDistance = Vector2.Distance(touch1.position, touch2.position);
                    float pinchDelta = currentPinchDistance - initialPinchDistance;

                    // Adjust the camera's field of view to zoom in/out
                    cam.fieldOfView = Mathf.Clamp(cam.fieldOfView - pinchDelta * zoomSpeed, 10, 60);
                    initialPinchDistance = currentPinchDistance;
                }
            }
            if (Input.mouseScrollDelta.y != 0)
            {
                cam.fieldOfView += Input.mouseScrollDelta.y;
                cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 19, 50);
            }
            cam.transform.position = new Vector3(
                    Mathf.Clamp(cam.transform.position.x, -10f, 10f),
                    Mathf.Clamp(cam.transform.position.y, 1f, 10f),
                    Mathf.Clamp(cam.transform.position.z, -17f, 7f));
        }
    }
    private Vector3 getWorldPosition(float z)
    {
        Ray mousePos = cam.ScreenPointToRay(Input.mousePosition);
        Plane ground = new Plane(Vector3.up, new Vector3(0, 0, 0));
        float distance;
        ground.Raycast(mousePos, out distance);
        return mousePos.GetPoint(distance);
    }
}
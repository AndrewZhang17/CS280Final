using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

[RequireComponent(typeof(ARRaycastManager))]
public class Interaction : MonoBehaviour
{
    public GameObject model;
    
    private GameObject currentObject;
    private ARRaycastManager _arRaycastManager;

    static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    static RaycastHit hit;

    private Vector2 inputPosition;
    private Vector2 direction;
    private bool rotating;
    private float holdDuration = 0;

    void Awake() 
    {
        _arRaycastManager = GetComponent<ARRaycastManager>();
    }

    int TryGetTouchPosition()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.position.x < 300 || touch.position.y < 300) {
                return -1;
            }
            switch (touch.phase)
            {
                //When a touch has first been detected, change the message and record the starting position
                case TouchPhase.Began:
                    // Record initial touch position.
                    inputPosition = touch.position;
                    holdDuration = 0;
                    return 0;
                //Determine if the touch is a moving touch
                case TouchPhase.Moved:
                    // Determine direction by comparing the current touch position with the initial one
                    direction = touch.position - inputPosition;
                    holdDuration += Time.deltaTime;
                    return 1;
                case TouchPhase.Ended:
                    // Report that the touch has ended when it ends
                    return 2;
                case TouchPhase.Stationary:
                    holdDuration += Time.deltaTime;
                    return -1;
            }
        }

        inputPosition = default;
        return -1;
    }

    // Update is called once per frame
    void Update()
    {
        var touchResult = TryGetTouchPosition();
        if (touchResult == -1)
        {
            return;
        }
        else if (touchResult == 1 && holdDuration > 0.2f) 
        {
            if (currentObject != null)
            {
                currentObject.transform.eulerAngles = new Vector3(0, -0.5f*direction.x, 0);
            }
        }
        else if (touchResult == 2 && holdDuration < 0.2f)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(inputPosition), out hit, 100) && hit.transform.gameObject.tag != "Plane")
            {
                currentObject = hit.transform.gameObject;
            }
            else if (_arRaycastManager.Raycast(inputPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                var hitPose = hits[0].pose;

                currentObject = Instantiate(model, hitPose.position, hitPose.rotation);
            }
        }

        
    }
}

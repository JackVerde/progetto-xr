using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR
{
    public class AircraftController : MonoBehaviour
    {
        [SerializeField] private ARRaycastManager arRaycastManager;
        private Transform aircraft;
        private Vector2 screenPoint;
        private bool placed;
        private bool rotating;
        public float rotationSpeed;

        private void Start()
        {
            Application.targetFrameRate = 60;
            placed = false;
            aircraft = transform.GetChild(0);
            screenPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
        }

        private void Update()
        {
            if (!placed)
            {
                var output = new List<ARRaycastHit>();
                arRaycastManager.Raycast(screenPoint, output, TrackableType.Planes);
                if (output.Count <= 0)
                {
                    aircraft.gameObject.SetActive(false);
                    return;
                }

                aircraft.gameObject.SetActive(true);
                transform.position = output[0].pose.position;
                if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
                    placed = true;
            }
            else
            {
                if (Input.touchCount > 0)
                {
                    var touch = Input.GetTouch(0);
                    if (touch.phase.Equals(TouchPhase.Moved))
                    {
                        var delta = touch.deltaPosition.x;
                        aircraft.transform.Rotate(Vector3.up,-delta*Time.deltaTime*rotationSpeed);
                    }
                }
            }
        }

        public void ResetPlaced()
        {
            placed = false;
        }
    }
}
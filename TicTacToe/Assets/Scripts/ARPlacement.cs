using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject boardPrefab;

    private bool placed = false;
    static List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (placed) return;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
                {
                    Pose pose = hits[0].pose;
                    Instantiate(boardPrefab, pose.position, pose.rotation);
                    placed = true;
                }
            }
        }
    }
}

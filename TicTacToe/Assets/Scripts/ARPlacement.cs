using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARPlacement : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject boardPrefab;

    private bool placed = false;
    private GameObject spawnedBoard;
    private ARAnchor spawnedAnchor;

    private static readonly List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        if (placed) return;

        // Mobile
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Began)
                TryPlace(t.position);
        }

#if UNITY_EDITOR
        // PC (XR Simulation)
        if (Input.GetMouseButtonDown(0))
        {
            TryPlace(Input.mousePosition);
        }
#endif
    }

    private void TryPlace(Vector2 screenPos)
    {
        if (!raycastManager.Raycast(screenPos, hits, TrackableType.PlaneWithinPolygon))
            return;

        Pose pose = hits[0].pose;

        GameObject anchorGO = new GameObject("BoardAnchor");
        anchorGO.transform.SetPositionAndRotation(pose.position, pose.rotation);

        spawnedAnchor = anchorGO.AddComponent<ARAnchor>();
        spawnedBoard = Instantiate(boardPrefab, anchorGO.transform);

        placed = true;
        Debug.Log("AR: Board placed");
    }

    public void ResetPlacement()
    {
        Debug.Log("AR: ResetPlacement");

        placed = false;

        // Détruire la grille
        if (spawnedBoard != null)
        {
            Destroy(spawnedBoard);
            spawnedBoard = null;
        }

        if (spawnedAnchor != null)
        {
            spawnedAnchor.enabled = false;
            Destroy(spawnedAnchor.gameObject);
            spawnedAnchor = null;
        }
    }

    void OnDisable()
    {
        ResetPlacement();
    }

    public bool IsPlaced() => placed;
}

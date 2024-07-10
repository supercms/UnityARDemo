using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARObjectController : MonoBehaviour
{
    private ARRaycastManager raycastManager;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    [SerializeField]
    Text txtStatus;

    [SerializeField]
    private List<GameObject> furniturePfbList = new List<GameObject>();

    // Start is called before the first frame update
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        txtStatus.text = "Awake";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        txtStatus.text = "TOUCHED";

        if (raycastManager.Raycast(Input.GetTouch(0).position, hits, TrackableType.PlaneWithinPolygon))
        {
            txtStatus.text = "TOUCHED, INSTANTIATE";
            var hitPose = hits[0].pose;
            Instantiate(raycastManager.raycastPrefab, hitPose.position, hitPose.rotation);
        }
    }
}

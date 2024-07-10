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
    private List<GameObject> spawnedObjects = new List<GameObject>();
    // private Dictionary<string, GameObject> spawnedObjectsDict = new Dictionary<string,GameObject>();
    int currentID = 0;

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

            // if (spawnedObjects.Count > 0)
            // {
            //     GameObject go = spawnedObjects[currentID];
            //     go.transform.position = hitPose.position;
            //     go.transform.rotation = hitPose.rotation;
            // }
            // else
            // {
            GameObject go = Instantiate(furniturePfbList[currentID], hitPose.position, hitPose.rotation);
            spawnedObjects.Add(go);
            // }

        }
    }

    public void changeFurnitureID(int id)
    {
        currentID = id;
    }
}

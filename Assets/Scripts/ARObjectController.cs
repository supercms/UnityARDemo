using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARObjectController : MonoBehaviour
{
    private ARRaycastManager raycastManager;

    private List<ARRaycastHit> arHits = new List<ARRaycastHit>();
    private RaycastHit physicsHit;
    private List<GameObject> spawnedObjects = new List<GameObject>();
    // private Dictionary<string, GameObject> spawnedObjectsDict = new Dictionary<string,GameObject>();
    int currentID = 0;
    ARObject selectedObject;

    [SerializeField]
    Text txtStatus;

    [SerializeField]
    private List<GameObject> furniturePfbList = new List<GameObject>();

    [SerializeField]
    private Camera arCamera;

    // Start is called before the first frame update
    private void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        //txtStatus.text = "Awake";
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 0)
            return;

        //txtStatus.text = "TOUCHED";

        Touch touch = Input.GetTouch(0);
        Vector2 touchPosition = touch.position;

        if (IsPointOverUIObject(touchPosition))
            return;

        if (touch.phase == TouchPhase.Began)
        {
            CheckHitObject(touchPosition);
        }
        else if (touch.phase == TouchPhase.Ended)
        {
            if (selectedObject)
            {
                selectedObject.Selected = false;
            }
        }

        CheckHitPlane(touchPosition);
    }

    bool CheckHitObject(Vector2 touchPos)
    {
        //txtStatus.text = "CheckHitObject";
        Ray ray = arCamera.ScreenPointToRay(touchPos);
        if (Physics.Raycast(ray, out physicsHit))
        {
            selectedObject = physicsHit.transform.GetComponent<ARObject>();
            if (selectedObject)
            {
                selectedObject.Selected = true;
                //txtStatus.text = "CheckHitObject >> TRUE";
                return true;
            }
        }

        return false;
    }

    void CheckHitPlane(Vector2 touchPos)
    {
        //txtStatus.text = "CheckHitPlane >>" + touchPos;
        if (raycastManager.Raycast(touchPos, arHits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = arHits[0].pose;

            if (selectedObject && selectedObject.Selected)
            {
                selectedObject.transform.position = hitPose.position;
                selectedObject.transform.rotation = hitPose.rotation;
                //txtStatus.text = "CheckHitPlane >> MOVE";
            }
            else
            {
                GameObject go = Instantiate(furniturePfbList[currentID], hitPose.position, hitPose.rotation);
                go.AddComponent<ARObject>();
                spawnedObjects.Add(go);
                //txtStatus.text = "CheckSelectPlane >> ADD";
            }
        }
    }

    bool IsPointOverUIObject(Vector2 pos)
    {
        PointerEventData eventDataCurPosition = new PointerEventData(EventSystem.current);
        eventDataCurPosition.position = pos;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurPosition, results);
        return results.Count > 0;
    }

    public void changeFurnitureID(int id)
    {
        currentID = id;
    }
}
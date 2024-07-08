using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class MultipleImageTracker : MonoBehaviour
{
    ARTrackedImageManager trackedImageManager;

    [SerializeField]
    Text txtStatus;

    [SerializeField]
    GameObject[] placeblePfbs;

    Dictionary<string, GameObject> spawnedObjects;

    void Awake()
    {
        trackedImageManager = GetComponent<ARTrackedImageManager>();
        spawnedObjects = new Dictionary<string, GameObject>();

        foreach (GameObject go in placeblePfbs)
        {
            GameObject newObject = Instantiate(go);
            newObject.name = go.name;
            newObject.SetActive(false);

            spawnedObjects.Add(newObject.name, newObject);
        }

        txtStatus.text = "Awake";
    }

    void OnEnable()
    {
        if (trackedImageManager)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
            txtStatus.text = "OnEnable";
        }
        else
        {
            txtStatus.text = "OnEnable, trackedImageManager null";
        }
    }

    void OnDisable()
    {
        if (trackedImageManager)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
            txtStatus.text = "OnDisable";
        }
        else
        {
            txtStatus.text = "OnDisable, trackedImageManager null";
        }
    }

    void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        txtStatus.text = "Changed, " + eventArgs.ToString();

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            UpdateSpawnObject(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            UpdateSpawnObject(trackedImage);
        }

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
        {
            if (spawnedObjects.ContainsKey(trackedImage.name))
            {
                spawnedObjects[trackedImage.referenceImage.name].SetActive(false);
            }
        }
    }

    void UpdateSpawnObject(ARTrackedImage trackedImage)
    {
        //txtStatus.text = "UpdateSpawnObject, " + trackedImage.referenceImage.name + "----" + placeblePfbs[0].name;
        txtStatus.text = "UpdateSpawnObject, " + placeblePfbs[1].name;

        string refImageName = trackedImage.referenceImage.name;
        if (spawnedObjects.ContainsKey(refImageName))
        {
            spawnedObjects[refImageName].transform.position = trackedImage.transform.position;
            spawnedObjects[refImageName].transform.rotation = trackedImage.transform.rotation;

            spawnedObjects[refImageName].SetActive(true);

            txtStatus.text = "UpdateSpawnObject(success), " + trackedImage.referenceImage.name;
        }
        else
        {
            Debug.LogWarning("The key is not in the dictionary >> " + refImageName);
        }
    }
}

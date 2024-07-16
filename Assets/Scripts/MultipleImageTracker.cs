using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

/*
 * 요구되는 클래스를 정의
 * ARTrackedImageManager가 반드시 필요
 */
[RequireComponent(typeof(ARTrackedImageManager))]

/*
 * @class
 * @classdesc 다중 미이지 트래킹에 사용되는 클래스
 */
public class MultipleImageTracker : MonoBehaviour
{
    /*
     * ARFaceManager를 연결하고 사용하기 위함
     */
    ARTrackedImageManager trackedImageManager;

    /*
     * 디버깅을 위한 테스트 출력
     */
    [SerializeField]
    Text txtStatus;

    /*
     * 각 이미지와 연동할 prefab
     */
    [SerializeField]
    GameObject[] placeblePfbs;

    /*
     * 스폰된 prefab을 관리하기 위함.
     */
    Dictionary<string, GameObject> spawnedObjects;

    /*
     * Unity 전용 함수 : 가장 먼저 실행
     */
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

        // txtStatus.text = "Awake";
    }

    /*
     * Unity 전용 함수 : 해당 객체가 활성화 될 때 호출
     */
    void OnEnable()
    {
        if (trackedImageManager)
        {
            // 이벤트 추가
            trackedImageManager.trackedImagesChanged += OnTrackedImageChanged;
            // txtStatus.text = "OnEnable";
        }
        else
        {
            // txtStatus.text = "OnEnable, trackedImageManager null";
        }
    }

    /*
     * Unity 전용 함수 : 해당 객체가 비활성화 될 때 호출
     */
    void OnDisable()
    {
        if (trackedImageManager)
        {
            // 이벤트 삭제
            trackedImageManager.trackedImagesChanged -= OnTrackedImageChanged;
            // txtStatus.text = "OnDisable";
        }
        else
        {
            // txtStatus.text = "OnDisable, trackedImageManager null";
        }
    }

    /*
     * Unity AR Foundation 전용 Event 함수에 연결
     * add, update, remove 3개를 지원
     */
    void OnTrackedImageChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // txtStatus.text = "Changed, " + eventArgs.ToString();

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

    /*
     * 이미지와 연동된 prefab 을 조정하는 함수
     */
    void UpdateSpawnObject(ARTrackedImage trackedImage)
    {
        //txtStatus.text = "UpdateSpawnObject, " + trackedImage.referenceImage.name + "----" + placeblePfbs[0].name;
        // txtStatus.text = "UpdateSpawnObject, " + placeblePfbs[1].name;

        string refImageName = trackedImage.referenceImage.name;
        if (spawnedObjects.ContainsKey(refImageName))
        {
            spawnedObjects[refImageName].transform.position = trackedImage.transform.position;
            spawnedObjects[refImageName].transform.rotation = trackedImage.transform.rotation;

            spawnedObjects[refImageName].SetActive(true);

            // txtStatus.text = "UpdateSpawnObject(success), " + trackedImage.referenceImage.name;
        }
        else
        {
            Debug.LogWarning("The key is not in the dictionary >> " + refImageName);
        }
    }
}
